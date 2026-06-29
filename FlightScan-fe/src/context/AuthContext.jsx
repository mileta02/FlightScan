import { createContext, useContext, useState } from 'react'

const AuthContext = createContext(null)

export function AuthProvider({ children }) {
  const [auth, setAuth] = useState(() => {
    const saved = localStorage.getItem('flightscan_auth')
    return saved ? JSON.parse(saved) : null
  })

  function saveAuth(data) {
    setAuth(data)
    localStorage.setItem('flightscan_auth', JSON.stringify(data))
  }

  function logout() {
    setAuth(null)
    localStorage.removeItem('flightscan_auth')
  }

  return (
    <AuthContext.Provider value={{ auth, saveAuth, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  return useContext(AuthContext)
}
