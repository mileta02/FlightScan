import { useState } from 'react'
import styles from './AddUserForm.module.css'

function validate(username, password) {
  if (!username) return 'Korisničko ime je obavezno.'
  if (username.length < 8)  return 'Korisničko ime mora imati najmanje 8 karaktera.'
  if (username.length > 50) return 'Korisničko ime može imati najviše 50 karaktera.'
  if (!password) return 'Lozinka je obavezna.'
  if (password.length < 8)  return 'Lozinka mora imati najmanje 8 karaktera.'
  if (password.length > 50) return 'Lozinka može imati najviše 50 karaktera.'
  return null
}

export default function AddUserForm({ onSubmit = () => {} }) {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [role, setRole]         = useState('Visitor')
  const [error, setError]       = useState(null)

  function handleSubmit() {
    const err = validate(username, password)
    if (err) { setError(err); return }
    setError(null)
    onSubmit({ username: username.trim(), password, role })
    setUsername('')
    setPassword('')
    setRole('Visitor')
  }

  return (
    <div className={styles.card}>
      <h3 className={styles.title}>Novi korisnik</h3>

      <div className={styles.field}>
        <label className={styles.label}>Korisničko ime</label>
        <input
          className={styles.input}
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />
      </div>

      <div className={styles.field}>
        <label className={styles.label}>Lozinka</label>
        <input
          type="password"
          className={styles.input}
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </div>

      <div className={styles.field}>
        <label className={styles.label}>Uloga</label>
        <select className={styles.input} value={role} onChange={(e) => setRole(e.target.value)}>
          <option value="Visitor">Posetilac</option>
          <option value="Agent">Agent</option>
        </select>
      </div>

      {error && <div className={styles.errorBox}>{error}</div>}

      <button className={styles.submitBtn} onClick={handleSubmit}>
        Dodaj korisnika
      </button>
    </div>
  )
}
