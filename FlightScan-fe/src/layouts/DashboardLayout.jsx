import { useNavigate, useLocation, Outlet } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import Sidebar from '../components/Sidebar'

export default function DashboardLayout() {
  const { auth, logout } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()

  const role = auth.role?.toLowerCase() ?? 'visitor'
  const userName = auth.username ?? ''

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
    <div style={{ display: 'flex', minHeight: '100vh' }}>
      <Sidebar
        role={role}
        activeKey={activeKey}
        onNavigate={handleNavigate}
        onLogout={handleLogout}
        userName={userName}
      />
      <main style={{ flex: 1, background: '#F4F7FB', overflow: 'auto' }}>
        <Outlet />
      </main>
    </div>
  )
}
