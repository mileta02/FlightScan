import { useState, useEffect } from 'react'
import { useNavigate, useLocation, Outlet } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import Sidebar from '../components/Sidebar/Sidebar'
import { createConnection } from '../api/signalrApi'
import styles from './DashboardLayout.module.css'

const PAGE_TITLE = {
  flights: 'Letovi',
  reservations: 'Rezervacije',
  search: 'Pretraga letova',
  myreservations: 'Moje rezervacije',
  users: 'Korisnici',
}

export default function DashboardLayout() {
  const { auth, logout } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()

  const role = auth.role?.toLowerCase() ?? 'visitor'
  const userName = auth.username ?? ''

  const [pendingCount, setPendingCount] = useState(0)
  const [connection, setConnection] = useState(null)

  useEffect(() => {
    const conn = createConnection()
    setConnection(conn)
    conn.start().catch(console.error)
    return () => {
      conn.stop()
    }
  }, [])

  const segments = location.pathname.split('/')
  const activeKey = segments[segments.length - 1] || role

  function handleNavigate(key) {
    navigate(`/dashboard/${key}`)
  }

  function handleLogout() {
    logout()
    navigate('/login', { replace: true })
  }

  return (
    <div className={styles.shell}>
      <Sidebar
        role={role}
        activeKey={activeKey}
        onNavigate={handleNavigate}
        onLogout={handleLogout}
        userName={userName}
        pendingCount={pendingCount}
      />
      <div className={styles.content}>
        <header className={styles.header}>
          <h1 className={styles.pageTitle}>{PAGE_TITLE[activeKey] ?? ''}</h1>
        </header>
        <main className={styles.main}>
          <Outlet context={{ onPendingChange: setPendingCount, connection }} />
        </main>
      </div>
    </div>
  )
}
