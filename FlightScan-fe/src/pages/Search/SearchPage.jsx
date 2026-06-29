import { useState } from 'react'
import FlightSearch from '../../components/FlightSearch/FlightSearch'
import FlightCard from '../../components/FlightCard/FlightCard'
import ReserveModal from '../../components/ReserveModal/ReserveModal'
import { searchFlights } from '../../api/flightsApi'
import { createReservation } from '../../api/reservationsApi'
import styles from './SearchPage.module.css'

export default function SearchPage() {
  const [search, setSearch] = useState({ from: '', to: '' })
  const [directOnly, setDirectOnly] = useState(false)
  const [activeFlight, setActiveFlight] = useState(null)
  const [reserving, setReserving] = useState(false)
  const [reserveError, setReserveError] = useState(null)
  const [allResults, setAllResults] = useState([])
  const [searched, setSearched] = useState(false)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)

  const results = directOnly ? allResults.filter(f => f.stops === 0) : allResults

  async function handleSearch() {
    setDirectOnly(false)
    setLoading(true)
    setError(null)
    try {
      const flights = await searchFlights(search)
      setAllResults(flights)
      setSearched(true)
    } catch {
      setError('Došlo je do greške pri učitavanju letova. Pokušaj ponovo.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <>
    <div>
      <FlightSearch
        value={search}
        onChange={(patch) => setSearch(s => ({ ...s, ...patch }))}
        onSearch={handleSearch}
      />

      {loading && (
        <div className={styles.emptyState}>Učitavanje letova…</div>
      )}

      {error && (
        <div className={styles.errorState}>{error}</div>
      )}

      {searched && !loading && !error && allResults.length === 0 && (
        <div className={styles.emptyState}>
          Nema dostupnih letova za izabranu rutu.
        </div>
      )}

      {searched && !loading && !error && allResults.length > 0 && (
        <>
          <label className={styles.filterCheck}>
            <input
              type="checkbox"
              checked={directOnly}
              onChange={(e) => setDirectOnly(e.target.checked)}
            />
            Bez presedanja
          </label>

          {results.length === 0 ? (
            <div className={styles.emptyState}>
              Nema direktnih letova za izabranu rutu.
            </div>
          ) : (
            <div className={styles.grid}>
              {results.map(f => (
                <FlightCard key={f.id} flight={f} onReserve={setActiveFlight} />
              ))}
            </div>
          )}
        </>
      )}
    </div>

    <ReserveModal
      flight={activeFlight}
      onClose={() => { setActiveFlight(null); setReserveError(null) }}
      onConfirm={async (flight, seats) => {
        setReserving(true)
        setReserveError(null)
        try {
          await createReservation(flight.id, seats)
          setActiveFlight(null)
          const refreshed = await searchFlights(search)
          setAllResults(refreshed)
        } catch (err) {
          const msg = err.response?.data?.error ?? err.response?.data?.message
          setReserveError(typeof msg === 'string' ? msg : 'Rezervacija nije uspela. Pokušaj ponovo.')
        } finally {
          setReserving(false)
        }
      }}
      reserving={reserving}
      error={reserveError}
    />
    </>
  )
}
