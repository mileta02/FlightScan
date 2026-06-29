import { useState, useEffect } from 'react'
import { useOutletContext } from 'react-router-dom'
import ApprovalsTable from '../../components/ApprovalsTable/ApprovalsTable'
import ConfirmModal from '../../components/ConfirmModal/ConfirmModal'
import { getAllReservations, approveReservation } from '../../api/reservationsApi'
import styles from './ReservationsPage.module.css'

export default function ReservationsPage() {
  const { onPendingChange } = useOutletContext() ?? {}

  const [reservations, setReservations] = useState([])
  const [loading, setLoading]           = useState(true)
  const [loadError, setLoadError]       = useState(null)
  const [actionError, setActionError]   = useState(null)
  const [toApprove, setToApprove]       = useState(null)

  useEffect(() => {
    getAllReservations()
      .then(data => {
        setReservations(data)
        onPendingChange?.(data.length)
      })
      .catch(() => setLoadError('Greška pri učitavanju rezervacija.'))
      .finally(() => setLoading(false))
  }, [])

  async function handleConfirmApprove() {
    const r = toApprove
    setToApprove(null)
    setActionError(null)
    try {
      await approveReservation(r.id)
      setReservations(prev => {
        const updated = prev.filter(x => x.id !== r.id)
        onPendingChange?.(updated.length)
        return updated
      })
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
