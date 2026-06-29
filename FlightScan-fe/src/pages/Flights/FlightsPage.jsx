import { useState, useEffect } from 'react'
import { useAuth } from '../../context/AuthContext'
import FlightsTable from '../../components/FlightsTable/FlightsTable'
import AddFlightForm from '../../components/AddFlightForm/AddFlightForm'
import ConfirmModal from '../../components/ConfirmModal/ConfirmModal'
import { getAllFlights, cancelFlight, createFlight } from '../../api/flightsApi'
import styles from './FlightsPage.module.css'

export default function FlightsPage() {
  const { auth } = useAuth()
  const role = auth?.role?.toLowerCase()
  const isAdmin = role === 'administrator'
  const isAgent = role === 'agent'

  const [flights, setFlights]               = useState([])
  const [loading, setLoading]               = useState(true)
  const [error, setError]                   = useState(null)
  const [formError, setFormError]           = useState(null)
  const [flightToCancel, setFlightToCancel] = useState(null)
  const [pendingFlight, setPendingFlight]   = useState(null)

  useEffect(() => {
    getAllFlights({ includeCancelled: isAdmin })
      .then(setFlights)
      .catch(() => setError('Greška pri učitavanju letova.'))
      .finally(() => setLoading(false))
  }, [])

  async function handleConfirmCancel() {
    const flight = flightToCancel
    setFlightToCancel(null)
    try {
      await cancelFlight(flight.id)
      setFlights(prev => prev.map(f => f.id === flight.id ? { ...f, canceled: true } : f))
    } catch {
      setError('Otkazivanje leta nije uspelo.')
    }
  }

  async function handleConfirmCreate() {
    const data = pendingFlight
    setPendingFlight(null)
    setFormError(null)
    try {
      await createFlight(data)
      const updated = await getAllFlights({ includeCancelled: isAdmin })
      setFlights(updated)
    } catch (err) {
      const msg = err.response?.data?.error ?? err.response?.data?.message
      setFormError(typeof msg === 'string' ? msg : 'Kreiranje leta nije uspelo.')
    }
  }

  if (loading) return <div className={styles.state}>Učitavanje…</div>
  if (error)   return <div className={styles.stateError}>{error}</div>

  const table = (
    <FlightsTable
      flights={flights}
      onCancel={isAdmin ? setFlightToCancel : undefined}
    />
  )

  return (
    <>
      {isAgent ? (
        <div className={styles.layout}>
          <div className={styles.formCol}>
            <AddFlightForm onSubmit={setPendingFlight} />
            {formError && <div className={styles.formError}>{formError}</div>}
          </div>
          <div className={styles.tableCol}>{table}</div>
        </div>
      ) : (
        table
      )}

      <ConfirmModal
        title="Otkaži let?"
        confirmLabel="Otkaži"
        onConfirm={flightToCancel ? handleConfirmCancel : null}
        onClose={() => setFlightToCancel(null)}
      />
      <ConfirmModal
        title="Kreiraj let?"
        confirmLabel="Kreiraj"
        onConfirm={pendingFlight ? handleConfirmCreate : null}
        onClose={() => setPendingFlight(null)}
      />
    </>
  )
}
