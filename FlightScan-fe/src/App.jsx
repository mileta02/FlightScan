import { BrowserRouter, Routes, Route, Navigate, Outlet } from 'react-router-dom'
import { AuthProvider, useAuth } from './context/AuthContext'
import LoginPage from './pages/Login/LoginPage'
import DashboardLayout from './layouts/DashboardLayout'
import SearchPage from './pages/Search/SearchPage'
import MyReservationsPage from './pages/MyReservations/MyReservationsPage'
import FlightsPage from './pages/Flights/FlightsPage'
import UsersPage from './pages/Users/UsersPage'

const DEFAULT_BY_ROLE = { agent: 'flights', visitor: 'search', administrator: 'flights' }

function RequireAuth() {
  const { auth } = useAuth()
  return auth ? <Outlet /> : <Navigate to="/login" replace />
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
              <Route path="flights" element={<FlightsPage />} />
              <Route path="reservations" element={<div></div>} />
              <Route path="search" element={<SearchPage />} />
              <Route path="myreservations" element={<MyReservationsPage />} />
              <Route path="users" element={<UsersPage />} />
            </Route>
          </Route>

          <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  )
}
