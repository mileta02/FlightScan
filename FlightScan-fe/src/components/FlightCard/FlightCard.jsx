import styles from './FlightCard.module.css'

const CITY_CODES = { Beograd: 'BEG', Niš: 'NIS', Kraljevo: 'KRA' }

function code(city) {
  return CITY_CODES[city] || city
}

function stopsLabel(n) {
  if (n === 0) return 'Direktan'
  if (n === 1) return '1 presedanje'
  return `${n} presedanja`
}

function formatDate(d) {
  if (!d) return ''
  const [y, m, day] = d.split('-')
  return `${day}.${m}.${y}.`
}

function parseDate(s) {
  const [y, m, d] = s.split('-').map(Number)
  return new Date(y, m - 1, d)
}

function daysUntil(date, today) {
  const base = today ? parseDate(today) : new Date()
  base.setHours(0, 0, 0, 0)
  return Math.round((parseDate(date) - base) / 86400000)
}

export default function FlightCard({
  flight,
  onReserve = () => {},
  lowSeatThreshold = 5,
  today,
}) {
  const { num, from, to, date, stops, free } = flight
  const low = free < lowSeatThreshold
  const canReserve = daysUntil(date, today) >= 3

  return (
    <div className={styles.card}>
      <div className={styles.cardTop}>
        <span className={styles.flightNum}>{num}</span>
        <span className={styles.badge}>{stopsLabel(stops)}</span>
      </div>

      <div className={styles.route}>
        <div className={styles.city}>
          <span className={styles.cityCode}>{code(from)}</span>
          <span className={styles.cityName}>{from}</span>
        </div>
        <div className={styles.arrow}>
          <div className={styles.line} />
          <svg className={styles.plane} width="14" height="14" viewBox="0 0 24 24">
            <path d="M3 13.2 L21 4 L14.4 21 L11.3 13.4 Z" fill="#FF6A3D" />
          </svg>
          <div className={styles.line} />
        </div>
        <div className={styles.city}>
          <span className={styles.cityCode}>{code(to)}</span>
          <span className={styles.cityName}>{to}</span>
        </div>
      </div>

      <div className={styles.meta}>
        <span>{formatDate(date)}</span>
        <span>
          Slobodno:{' '}
          <span className={low ? styles.seatsLow : styles.seatsOk}>{free}</span>
        </span>
      </div>

      {canReserve ? (
        <button className={styles.reserveBtn} onClick={() => onReserve(flight)}>
          Rezerviši mesto
        </button>
      ) : (
        <div className={styles.blocked}>
          Polazak za manje od 3 dana
        </div>
      )}
    </div>
  )
}
