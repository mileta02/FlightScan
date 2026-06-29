import styles from './ApprovalsTable.module.css'

const CITY_CODES = { Beograd: 'BEG', Niš: 'NIS', Kraljevo: 'KRA' }
const code = (c) => CITY_CODES[c] || c

function formatDate(d) {
  if (!d) return ''
  const [y, m, day] = d.split('-')
  return `${day}.${m}.${y}.`
}

export default function ApprovalsTable({ reservations = [], onApprove = () => {} }) {
  const empty = reservations.length === 0

  return (
    <div className={styles.wrapper}>
      <div className={styles.tableHeader}>
        <h3 className={styles.title}>Zahtevi za odobravanje</h3>
      </div>

      {empty ? (
        <div className={styles.empty}>
          <div className={styles.emptyTitle}>Nema rezervacija na čekanju</div>
        </div>
      ) : (
        <div className={styles.scroll}>
          <table className={styles.table}>
            <thead>
              <tr>
                <th className={styles.th}>LET</th>
                <th className={styles.th}>RUTA</th>
                <th className={`${styles.th} ${styles.hideOnMobile}`}>KORISNIK</th>
                <th className={`${styles.th} ${styles.hideOnMobile}`}>DATUM</th>
                <th className={`${styles.th} ${styles.hideOnMobile}`}>MESTA</th>
                <th className={`${styles.th} ${styles.thRight}`}></th>
              </tr>
            </thead>
            <tbody>
              {reservations.map((r) => (
                <tr key={r.id}>
                  <td className={`${styles.td} ${styles.tdMono}`}>{r.num}</td>
                  <td className={styles.td}>
                    <div className={styles.route}>
                      <span className={styles.routeCode}>{code(r.from)}</span>
                      <span className={styles.routeArrow}>→</span>
                      <span className={styles.routeCode}>{code(r.to)}</span>
                    </div>
                    <div className={styles.routeSub}>{r.from} → {r.to}</div>
                  </td>
                  <td className={`${styles.td} ${styles.hideOnMobile}`}>{r.username}</td>
                  <td className={`${styles.td} ${styles.hideOnMobile}`}>{formatDate(r.date)}</td>
                  <td className={`${styles.td} ${styles.hideOnMobile}`}>{r.seats}</td>
                  <td className={`${styles.td} ${styles.tdRight}`}>
                    <button className={styles.approveBtn} onClick={() => onApprove(r)}>
                      Odobri
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  )
}
