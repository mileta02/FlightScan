import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { login } from '../api/authApi'
import { useAuth } from '../context/AuthContext'
import styles from './LoginPage.module.css'

export default function LoginPage() {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const { saveAuth } = useAuth()
  const navigate = useNavigate()

  async function handleSubmit(e) {
    e.preventDefault()
    setError('')
    setLoading(true)

    try {
      const data = await login(username, password)
      saveAuth(data)
      navigate('/welcome-page')
    } catch (err) {
      const msg = err.response?.data?.message ?? err.response?.data
      setError(typeof msg === 'string' ? msg : 'Pogrešno korisničko ime ili lozinka.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className={styles.wrapper}>
      <div className={styles.leftPanel}>
        <div className={styles.circle1} />
        <div className={styles.circle2} />
        <div className={styles.circle3} />
        <div className={styles.logoWrap}>
          <div className={styles.logoIcon}>
            <svg width="34" height="34" viewBox="0 0 24 24" fill="none">
              <path d="M21 3L3 10.53v.98l6.84 2.65L12.48 21h.98L21 3z" fill="#fff" />
            </svg>
          </div>
          <span className={styles.logoText}>FlightScan</span>
        </div>
      </div>

      <div className={styles.rightPanel}>
        <form className={styles.formCard} onSubmit={handleSubmit} noValidate>
          <h1 className={styles.heading}>Prijava</h1>

          <div className={styles.fieldGroup}>
            <label className={styles.label}>Korisničko ime</label>
            <input
              className={styles.input}
              type="text"
              placeholder="Unesite korisničko ime"
              value={username}
              onChange={e => setUsername(e.target.value)}
              autoComplete="username"
              required
            />
          </div>

          <div className={styles.fieldGroupLast}>
            <label className={styles.label}>Lozinka</label>
            <input
              className={styles.input}
              type="password"
              placeholder="••••••••"
              value={password}
              onChange={e => setPassword(e.target.value)}
              autoComplete="current-password"
              required
            />
          </div>

          {error && <div className={styles.errorBox}>{error}</div>}

          <button className={styles.button} type="submit" disabled={loading}>
            {loading ? 'Prijava...' : 'Prijavi se'}
          </button>
        </form>
      </div>
    </div>
  )
}
