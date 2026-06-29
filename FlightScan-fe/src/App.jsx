import { BrowserRouter, Routes, Route, Navigate, Outlet, useOutletContext } from 'react-router-dom'
import { AuthProvider, useAuth } from './context/AuthContext'
import LoginPage from './pages/Login/LoginPage'
import DashboardLayout from './layouts/DashboardLayout'
import SearchPage from './pages/Search/SearchPage'
import MyReservationsPage from './pages/MyReservations/MyReservationsPage'
import FlightsPage from './pages/Flights/FlightsPage'
import UsersPage from './pages/Users/UsersPage'
import ReservationsPage from './pages/Reservations/ReservationsPage'

const DEFAULT_BY_ROLE = { agent: 'flights', visitor: 'search', administrator: 'flights' }

function RequireAuth() {
  const { auth } = useAuth()
  return auth ? <Outlet /> : <Navigate to="/login" replace />
}

function RequireRole({ allowed }) {
  const { auth } = useAuth()
  const role = auth?.role?.toLowerCase() ?? 'visitor'
  const ctx = useOutletContext()
  return allowed.includes(role)
    ? <Outlet context={ctx} />
    : <Navigate to={`/dashboard/${DEFAULT_BY_ROLE[role] ?? 'search'}`} replace />
}

function DashboardIndex() {
  const { auth } = useAuth()
  const role = auth?.role?.toLowerCase() ?? 'visitor'
  return <Navigate to={DEFAULT_BY_ROLE[role] ?? 'search'} replace />
}

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path="/login" element={<LoginPage />} />

          <Route element={<RequireAuth />}>
            <Route path="/dashboard" element={<DashboardLayout />}>
              <Route index element={<DashboardIndex />} />

              <Route element={<RequireRole allowed={['administrator', 'agent']} />}>
                <Route path="flights" element={<FlightsPage />} />
              </Route>

              <Route element={<RequireRole allowed={['agent']} />}>
                <Route path="reservations" element={<ReservationsPage />} />
              </Route>

              <Route element={<RequireRole allowed={['visitor']} />}>
                <Route path="search" element={<SearchPage />} />
                <Route path="myreservations" element={<MyReservationsPage />} />
              </Route>

              <Route element={<RequireRole allowed={['administrator']} />}>
                <Route path="users" element={<UsersPage />} />
              </Route>
            </Route>
          </Route>

          <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  )
}
