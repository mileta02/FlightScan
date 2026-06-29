import { useState, useEffect } from 'react'
import AddUserForm from '../../components/AddUserForm/AddUserForm'
import UsersTable from '../../components/UsersTable/UsersTable'
import ConfirmModal from '../../components/ConfirmModal/ConfirmModal'
import { getUsers, createUser } from '../../api/usersApi'
import styles from './UsersPage.module.css'

export default function UsersPage() {
  const [users, setUsers]         = useState([])
  const [loading, setLoading]     = useState(true)
  const [error, setError]         = useState(null)
  const [formError, setFormError] = useState(null)
  const [pending, setPending]     = useState(null)

  useEffect(() => {
    getUsers()
      .then(setUsers)
      .catch(() => setError('Greška pri učitavanju korisnika.'))
      .finally(() => setLoading(false))
  }, [])

  async function handleConfirm() {
    const data = pending
    setPending(null)
    setFormError(null)
    try {
      await createUser(data)
      const updated = await getUsers()
      setUsers(updated)
    } catch (err) {
      const msg = err.response?.data?.error ?? err.response?.data?.message
      setFormError(typeof msg === 'string' ? msg : 'Dodavanje korisnika nije uspelo.')
    }
  }

  return (
    <>
    <div className={styles.layout}>
      <div className={styles.formCol}>
        <AddUserForm onSubmit={setPending} />
        {formError && <div className={styles.formError}>{formError}</div>}
      </div>

      <div className={styles.tableCol}>
        {loading && <div className={styles.state}>Učitavanje…</div>}
        {error   && <div className={styles.stateError}>{error}</div>}
        {!loading && !error && <UsersTable users={users} />}
      </div>
    </div>

    <ConfirmModal
      title="Dodaj korisnika?"
      confirmLabel="Dodaj"
      onConfirm={pending ? handleConfirm : null}
      onClose={() => setPending(null)}
    />
    </>
  )
}
