import { useState, useEffect } from 'react'
import styles from './ReserveModal.module.css'

const CITY_CODES = { Beograd: 'BEG', Niš: 'NIS', Kraljevo: 'KRA' }

const code       = (c) => CITY_CODES[c] || c
const stopsLabel = (n) => n === 0 ? 'Direktan' : n === 1 ? '1 presedanje' : `${n} presedanja`
const formatDate = (d) => {
  if (!d) return ''
  const [y, m, day] = d.split('-')
  return `${day}.${m}.${y}.`
}

export default function ReserveModal({ flight, onConfirm = () => {}, onClose = () => {}, reserving = false, error = null }) {
  const [seats, setSeats] = useState(1)

  useEffect(() => { if (flight) setSeats(1) }, [flight])

  if (!flight) return null

  const max = flight.free
  const dec = () => setSeats((n) => Math.max(1, n - 1))
  const inc = () => setSeats((n) => Math.min(max, n + 1))

  return (
    <div className={styles.overlay} onClick={onClose}>
      <div className={styles.modal} onClick={(e) => e.stopPropagation()}>

        <div className={styles.header}>
          <div>
            <div className={styles.title}>
              {code(flight.from)} → {code(flight.to)}
            </div>
            <div className={styles.subtitle}>
              {/* {flight.from} – {flight.to} · {flight.num} */}
            </div>
          </div>
          <button className={styles.closeBtn} onClick={onClose}>×</button>
        </div>

        <div className={styles.body}>
          <div className={styles.row}>
            <span>Datum</span>
            <span className={styles.rowValue}>{formatDate(flight.date)}</span>
          </div>
          <div className={`${styles.row} ${styles.rowLast}`}>
            <span>Presedanja</span>
            <span className={styles.rowValue}>{stopsLabel(flight.stops)}</span>
          </div>

          <div className={styles.stepperLabel}></div>
          <div className={styles.stepper}>
            <button className={styles.stepBtn} onClick={dec}>−</button>
            <span className={styles.stepCount}>{seats}</span>
            <button className={styles.stepBtn} onClick={inc}>+</button>
          </div>
          <div className={styles.stepHint}></div>

          {error && <div className={styles.errorBox}>{error}</div>}

          <button className={styles.confirmBtn} onClick={() => onConfirm(flight, seats)} disabled={reserving}>
            {reserving ? 'Rezervacija u toku…' : 'Potvrdi rezervaciju'}
          </button>
        </div>

      </div>
    </div>
  )
}
