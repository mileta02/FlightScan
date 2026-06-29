import { useState } from 'react'
import styles from './Sidebar.module.css'

const NAV_BY_ROLE = {
  agent: [
    { key: 'flights', label: 'Letovi' },
    { key: 'reservations', label: 'Rezervacije', badgeKey: 'pending' },
  ],
  visitor: [
    { key: 'search', label: 'Pretraga letova' },
    { key: 'myreservations', label: 'Moje rezervacije' },
  ],
  administrator: [
    { key: 'flights', label: 'Letovi' },
    { key: 'users', label: 'Korisnici' },
  ],
}

const ROLE_LABEL = {
  agent: 'Agent',
  visitor: 'Posetilac',
  administrator: 'Administrator',
}

function initials(name) {
  if (!name) return '?'
  const p = name.trim().split(/\s+/)
  return (p[0][0] + (p[1] ? p[1][0] : '')).toUpperCase()
}

export default function Sidebar({
  role = 'agent',
  activeKey,
  onNavigate = () => {},
  onLogout = () => {},
  userName = '',
  pendingCount = 0,
}) {
  const [open, setOpen] = useState(false)
  const items = NAV_BY_ROLE[role] || []

  return (
    <>
      <button className={styles.burgerBtn} onClick={() => setOpen(true)}>
        <svg width="20" height="14" viewBox="0 0 20 14" fill="none">
          <rect y="0" width="20" height="2" rx="1" fill="#fff" />
          <rect y="6" width="20" height="2" rx="1" fill="#fff" />
          <rect y="12" width="20" height="2" rx="1" fill="#fff" />
        </svg>
      </button>

      {open && <div className={styles.overlay} onClick={() => setOpen(false)} />}

      <aside className={`${styles.sidebar}${open ? ` ${styles.sidebarOpen}` : ''}`}>
        <button className={styles.closeBtn} onClick={() => setOpen(false)}>
          <svg width="14" height="14" viewBox="0 0 14 14" fill="none">
            <path d="M1 1l12 12M13 1L1 13" stroke="#fff" strokeWidth="2" strokeLinecap="round" />
          </svg>
        </button>

        <div className={styles.logoArea}>
          <div className={styles.logoIcon}>
            <svg width="18" height="18" viewBox="0 0 24 24">
              <path d="M3 13.2 L21 4 L14.4 21 L11.3 13.4 Z" fill="#fff" />
            </svg>
          </div>
          <span className={styles.logoText}>FlightScan</span>
        </div>

        <nav className={styles.nav}>
          {items.map((item) => {
            const active = activeKey === item.key
            const badge = item.badgeKey === 'pending' ? pendingCount : 0
            return (
              <button
                key={item.key}
                onClick={() => { onNavigate(item.key); setOpen(false) }}
                className={`${styles.navItem}${active ? ` ${styles.navItemActive}` : ''}`}
              >
                <span>{item.label}</span>
                {badge > 0 && <span className={styles.badge}>{badge}</span>}
              </button>
            )
          })}
        </nav>

        <div className={styles.spacer} />

        <div className={styles.footer}>
          <div className={styles.userRow}>
            <div className={styles.avatar}>{initials(userName)}</div>
            <div className={styles.userInfo}>
              <div className={styles.userName}>{userName}</div>
              <div className={styles.roleLabel}>{ROLE_LABEL[role]}</div>
            </div>
          </div>
          <button className={styles.logoutBtn} onClick={onLogout}>
            Odjava
          </button>
        </div>
      </aside>
    </>
  )
}
