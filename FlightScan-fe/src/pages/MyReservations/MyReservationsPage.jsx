import { useState, useEffect } from 'react'
import { useOutletContext } from 'react-router-dom'
import ReservationCard from '../../components/ReservationCard/ReservationCard'
import { getMyReservations } from '../../api/reservationsApi'
import styles from './MyReservationsPage.module.css'

export default function MyReservationsPage() {
  const { connection } = useOutletContext() ?? {}

  const [reservations, setReservations] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  useEffect(() => {
    getMyReservations()
      .then(setReservations)
      .catch(() => setError('Greška pri učitavanju rezervacija.'))
      .finally(() => setLoading(false))
  }, [])

  useEffect(() => {
    if (!connection) return

    function handleReservationApproved(data) {
      setReservations(prev => {
        return prev.map(r => {
          if (r.id === data.id) {
            return { ...r, status: 'accepted' }
          }
          return r
        })
      })
    }

    connection.on('ReservationApproved', handleReservationApproved)
    return () => {
      connection.off('ReservationApproved', handleReservationApproved)
    }
  }, [connection])

  if (loading) return <div className={styles.state}>Učitavanje…</div>
  if (error)   return <div className={styles.stateError}>{error}</div>

  if (reservations.length === 0) {
    return (
      <div className={styles.state}>
        Nemate rezervacija.
      </div>
    )
  }

  return (
    <div className={styles.grid}>
      {reservations.map(r => (
        <ReservationCard key={r.id} reservation={r} />
      ))}
    </div>
  )
}
