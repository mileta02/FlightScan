import { useState, useEffect } from 'react'
import styles from './AddFlightForm.module.css'

const DEFAULT_CITIES = ['Beograd', 'Niš', 'Kraljevo']

const todayStr = () => new Date().toISOString().slice(0, 10)

function validate(from, to, date, seats) {
  if (from === to) return 'Polazak i dolazak moraju biti u različitim gradovima.'
  if (!date) return 'Izaberi datum leta.'
  if (date <= todayStr()) return 'Datum mora biti u budućnosti.'
  const s = parseInt(seats, 10)
  if (!s || s < 1) return 'Unesi broj mesta (najmanje 1).'
  return null
}

export default function AddFlightForm({ onSubmit = () => {}, cities = DEFAULT_CITIES, resetTrigger = 0 }) {
  const [from,  setFrom]  = useState(cities[0])
  const [to,    setTo]    = useState(cities[1])
  const [date,  setDate]  = useState('')
  const [stops, setStops] = useState('0')
  const [seats, setSeats] = useState('')
  const [error, setError] = useState(null)

  useEffect(() => {
    if (resetTrigger === 0) return
    setFrom(cities[0])
    setTo(cities[1])
    setDate('')
    setStops('0')
    setSeats('')
    setError(null)
  }, [resetTrigger])

  function handleSubmit() {
    const err = validate(from, to, date, seats)
    if (err) { setError(err); return }
    setError(null)
    onSubmit({ from, to, date, stops: parseInt(stops, 10), seats: parseInt(seats, 10) })
  }

  return (
    <div className={styles.card}>
      <h3 className={styles.title}>Novi let</h3>

      <div className={styles.row}>
        <div className={styles.field}>
          <label className={styles.label}>Polazak</label>
          <select className={styles.input} value={from} onChange={(e) => setFrom(e.target.value)}>
            {cities.map((c) => <option key={c}>{c}</option>)}
          </select>
        </div>
        <div className={styles.field}>
          <label className={styles.label}>Dolazak</label>
          <select className={styles.input} value={to} onChange={(e) => setTo(e.target.value)}>
            {cities.map((c) => <option key={c}>{c}</option>)}
          </select>
        </div>
      </div>

      <div className={styles.fieldFull}>
        <label className={styles.label}>Datum leta</label>
        <input type="date" className={styles.input} value={date} onChange={(e) => setDate(e.target.value)} />
      </div>

      <div className={styles.row}>
        <div className={styles.field}>
          <label className={styles.label}>Presedanja</label>
          <select className={styles.input} value={stops} onChange={(e) => setStops(e.target.value)}>
            {[0, 1, 2, 3, 4, 5].map(n => <option key={n} value={n}>{n}</option>)}
          </select>
        </div>
        <div className={styles.field}>
          <label className={styles.label}>Broj mesta</label>
          <input
            type="number"
            min="1"
            className={styles.input}
            value={seats}
            onChange={(e) => setSeats(e.target.value)}
          />
        </div>
      </div>

      {error && <div className={styles.errorBox}>{error}</div>}

      <button className={styles.submitBtn} onClick={handleSubmit}>
        Kreiraj let
      </button>
    </div>
  )
}
