import { useState, useEffect } from 'react'
import { useAuth } from '../../context/AuthContext'
import FlightsTable from '../../components/FlightsTable/FlightsTable'
import ConfirmModal from '../../components/ConfirmModal/ConfirmModal'
import { getAllFlights, cancelFlight } from '../../api/flightsApi'
import styles from './FlightsPage.module.css'

export default function FlightsPage() {
  const { auth } = useAuth()
  const isAdmin = auth?.role?.toLowerCase() === 'administrator'

  const [flights, setFlights] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [flightToCancel, setFlightToCancel] = useState(null)

  useEffect(() => {
    getAllFlights()
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

  if (loading) return <div className={styles.state}>Učitavanje…</div>
  if (error)   return <div className={styles.stateError}>{error}</div>

  return (
    <>
      <FlightsTable
        flights={flights}
        onCancel={isAdmin ? setFlightToCancel : undefined}
      />
      <ConfirmModal
        title="Otkaži let?"
        confirmLabel="Otkaži"
        onConfirm={flightToCancel ? handleConfirmCancel : null}
        onClose={() => setFlightToCancel(null)}
      />
    </>
  )
}
