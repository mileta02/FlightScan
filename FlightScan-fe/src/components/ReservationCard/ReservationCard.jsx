import styles from './ReservationCard.module.css'

const CITY_CODES = { Beograd: 'BEG', Niš: 'NIS', Kraljevo: 'KRA' }
const code = (c) => CITY_CODES[c] || c
const formatDate = (d) => {
  if (!d) return ''
  const [y, m, day] = d.split('-')
  return `${day}.${m}.${y}.`
}

export default function ReservationCard({ reservation }) {
  const { num, from, to, date, seats, status } = reservation
  const approved = status === 'accepted'

  return (
    <div className={styles.card}>
      <div className={styles.cardTop}>
        <span className={styles.flightNum}>{num}</span>
        <span className={approved ? styles.badgeApproved : styles.badgePending}>
          {approved ? 'Odobrena' : 'Na čekanju'}
        </span>
      </div>

      <div className={styles.route}>
        {code(from)} → {code(to)}
      </div>
      <div className={styles.cities}>{from} – {to}</div>

      <div className={styles.meta}>
        <span>{formatDate(date)}</span>
        <span>Mesta: <strong className={styles.seats}>{seats}</strong></span>
      </div>
    </div>
  )
}
