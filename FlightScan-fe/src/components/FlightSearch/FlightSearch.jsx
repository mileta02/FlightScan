import { useState } from 'react'
import styles from './FlightSearch.module.css'

const DEFAULT_CITIES = ['Beograd', 'Niš', 'Kraljevo']

function validate(value) {
  if (!value.from || !value.to) return 'Grad polaska i dolaska su obavezni.'
  if (value.from === value.to) return 'Grad polaska i dolaska ne mogu biti isti.'
  if (value.date) {
    const selected = new Date(value.date)
    const today = new Date()
    today.setHours(0, 0, 0, 0)
    if (selected < today) return 'Datum ne može biti u prošlosti.'
  }
  return null
}

export default function FlightSearch({
  value = { from: '', to: '', date: '', directOnly: false },
  onChange = () => {},
  onSearch = () => {},
  cities = DEFAULT_CITIES,
}) {
  const [error, setError] = useState(null)

  function handleSearch() {
    const err = validate(value)
    if (err) { setError(err); return }
    setError(null)
    onSearch()
  }

  return (
    <div className={styles.card}>
      <div className={styles.row}>
        <div className={styles.field}>
          <label className={styles.label}>Od</label>
          <select
            className={styles.select}
            value={value.from}
            onChange={(e) => onChange({ from: e.target.value })}
          >
            <option value="">Izaberi grad</option>
            {cities.map((c) => <option key={c}>{c}</option>)}
          </select>
        </div>

        <div className={styles.field}>
          <label className={styles.label}>Do</label>
          <select
            className={styles.select}
            value={value.to}
            onChange={(e) => onChange({ to: e.target.value })}
          >
            <option value="">Izaberi grad</option>
            {cities.map((c) => <option key={c}>{c}</option>)}
          </select>
        </div>

        <div className={styles.field}>
          <label className={styles.label}>Datum (opciono)</label>
          <input
            type="date"
            className={styles.dateInput}
            value={value.date}
            onChange={(e) => onChange({ date: e.target.value })}
          />
        </div>

        <button className={styles.searchBtn} onClick={handleSearch}>
          Pretraži
        </button>
      </div>

      {error && <div className={styles.errorBox}>{error}</div>}

      <label className={styles.checkLabel}>
        <input
          type="checkbox"
          checked={value.directOnly}
          onChange={(e) => onChange({ directOnly: e.target.checked })}
        />
        Samo letovi bez presedanja
      </label>
    </div>
  )
}
