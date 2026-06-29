import styles from './UsersTable.module.css'

const ROLE_LABEL = { administrator: 'Administrator', agent: 'Agent', visitor: 'Posetilac' }

export default function UsersTable({ users = [] }) {
  return (
    <div className={styles.wrapper}>
      <div className={styles.tableHeader}>
        <h3 className={styles.title}>Korisnici</h3>
      </div>

      <div className={styles.scroll}>
        <table className={styles.table}>
          <thead>
            <tr>
              <th className={styles.th}>KORISNIČKO IME</th>
              <th className={`${styles.th} ${styles.hideOnMobile}`}>ULOGA</th>
            </tr>
          </thead>
          <tbody>
            {users.map((u) => (
              <tr key={u.username}>
                <td className={`${styles.td} ${styles.tdName}`}>{u.username}</td>
                <td className={`${styles.td} ${styles.hideOnMobile}`}>
                  <span className={styles.rolePill}>{ROLE_LABEL[u.role] ?? u.role}</span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}
