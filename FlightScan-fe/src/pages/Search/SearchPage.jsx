import { useState } from 'react'
import FlightSearch from '../../components/FlightSearch/FlightSearch'
import styles from './SearchPage.module.css'

export default function SearchPage() {
  const [search, setSearch] = useState({ from: '', to: '', date: '', directOnly: false })
  const [searched, setSearched] = useState(false)

  function handleSearch() {
    setSearched(true)
  }

  return (
    <div>
      <FlightSearch
        value={search}
        onChange={(patch) => setSearch(s => ({ ...s, ...patch }))}
        onSearch={handleSearch}
      />
      {!searched && (
        <div className={styles.emptyState}>
          Izaberi rute i pritisni <strong>Pretraži</strong> da vidiš dostupne letove.
        </div>
      )}
    </div>
  )
}
