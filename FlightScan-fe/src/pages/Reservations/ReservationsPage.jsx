import { useState, useEffect } from 'react'
import { useOutletContext } from 'react-router-dom'
import ApprovalsTable from '../../components/ApprovalsTable/ApprovalsTable'
import ConfirmModal from '../../components/ConfirmModal/ConfirmModal'
import { getAllReservations, approveReservation, mapReservation } from '../../api/reservationsApi'
import styles from './ReservationsPage.module.css'

export default function ReservationsPage() {
  const { onPendingChange, connection } = useOutletContext() ?? {}

  const [reservations, setReservations] = useState([])
  const [loading, setLoading]           = useState(true)
  const [loadError, setLoadError]       = useState(null)
  const [actionError, setActionError]   = useState(null)
  const [toApprove, setToApprove]       = useState(null)

  useEffect(() => {
    getAllReservations()
      .then(setReservations)
      .catch(() => setLoadError('Greška pri učitavanju rezervacija.'))
      .finally(() => setLoading(false))
  }, [])

  useEffect(() => {
    onPendingChange?.(reservations.length)
  }, [reservations.length])

  useEffect(() => {
    if (!connection) return

    function handleNewReservation(data) {
      const mapped = mapReservation(data)
      setReservations(prev => [mapped, ...prev])
    }

    connection.on('NewReservation', handleNewReservation)
    return () => {
      connection.off('NewReservation', handleNewReservation)
    }
  }, [connection])

  async function handleConfirmApprove() {
    const r = toApprove
    setToApprove(null)
    setActionError(null)
    try {
      await approveReservation(r.id)
      setReservations(prev => prev.filter(x => x.id !== r.id))
    } catch {
      setActionError('Odobravanje rezervacije nije uspelo.')
    }
  }

  if (loading)   return <div className={styles.state}>Učitavanje…</div>
  if (loadError) return <div className={styles.stateError}>{loadError}</div>

  return (
    <>
      <ApprovalsTable reservations={reservations} onApprove={setToApprove} />
      {actionError && <div className={styles.actionError}>{actionError}</div>}

      <ConfirmModal
        title="Odobri rezervaciju?"
        confirmLabel="Odobri"
        onConfirm={toApprove ? handleConfirmApprove : null}
        onClose={() => setToApprove(null)}
      />
    </>
  )
}
