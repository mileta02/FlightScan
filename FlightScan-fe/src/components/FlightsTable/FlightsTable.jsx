import styles from './FlightsTable.module.css'

const CITY_CODES = { Beograd: 'BEG', Niš: 'NIS', Kraljevo: 'KRA' }
const code = (c) => CITY_CODES[c] || c
const stopsLabel = (n) => n === 0 ? 'Direktan' : n === 1 ? '1' : `${n}`
const formatDate = (d) => {
  if (!d) return ''
  const [y, m, day] = d.split('-')
  return `${day}.${m}.${y}.`
}

export default function FlightsTable({ flights = [], onCancel, lowSeatThreshold = 5 }) {
  const isAdmin = typeof onCancel === 'function'

  return (
    <div className={styles.wrapper}>
      <div className={styles.tableHeader}>
        <h3 className={styles.title}>Svi letovi</h3>
        <div className={styles.legend}>
        </div>
      </div>

      <div className={styles.scroll}>
        <table className={styles.table}>
          <thead>
            <tr>
              <th className={styles.th}>LET</th>
              <th className={styles.th}>RUTA</th>
              <th className={styles.th}>DATUM</th>
              <th className={`${styles.th} ${styles.hideOnMobile}`}>PRESEDANJA</th>
              <th className={styles.th}>SLOBODNO</th>
              {isAdmin && <th className={`${styles.th} ${styles.thRight}`}></th>}
            </tr>
          </thead>
          <tbody>
            {flights.map((f) => {
              const low = !f.canceled && f.free < lowSeatThreshold
              return (
                <tr key={f.id} className={isAdmin ? '' : f.canceled ? styles.rowCanceled : low ? styles.rowLow : ''}>
                  <td className={`${styles.td} ${styles.tdMono}`}>{f.num}</td>
                  <td className={styles.td}>
                    <div className={styles.routeCodes}>
                      <span className={styles.routeCode}>{code(f.from)}</span>
                      <span className={styles.routeArrow}>→</span>
                      <span className={styles.routeCode}>{code(f.to)}</span>
                    </div>
                    <div className={styles.routeSub}>{f.from} – {f.to}</div>
                  </td>
                  <td className={styles.td}>{formatDate(f.date)}</td>
                  <td className={`${styles.td} ${styles.hideOnMobile}`}>{stopsLabel(f.stops)}</td>
                  <td className={styles.td}>
                    <span className={`${styles.seats} ${isAdmin ? '' : f.canceled ? styles.seatsCanceled : low ? styles.seatsLow : ''}`}>
                      {f.free}
                    </span>
                    <span className={styles.seatsTotal}> / {f.total}</span>
                  </td>
                  {isAdmin && (
                    <td className={`${styles.td} ${styles.tdRight}`}>
                      <button
                        className={styles.cancelBtn}
                        onClick={() => !f.canceled && onCancel(f)}
                        disabled={f.canceled}
                      >
                        Otkaži let
                      </button>
                    </td>
                  )}
                </tr>
              )
            })}
          </tbody>
        </table>
      </div>
    </div>
  )
}
