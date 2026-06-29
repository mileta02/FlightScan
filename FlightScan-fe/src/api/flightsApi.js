import axios from 'axios'

const TO_API   = { 'Niš': 'Nis' }
const FROM_API = { 'Nis': 'Niš' }

function toApiCity(city)   { return TO_API[city]   ?? city }
function fromApiCity(city) { return FROM_API[city]  ?? city }

function getToken() {
  try { return JSON.parse(localStorage.getItem('flightscan_auth'))?.token ?? null }
  catch { return null }
}

const client = axios.create({ baseURL: import.meta.env.VITE_API_URL })

client.interceptors.request.use(cfg => {
  const token = getToken()
  if (token) cfg.headers.Authorization = `Bearer ${token}`
  return cfg
})

function mapFlight(f) {
  return {
    id:                f.id,
    num:               `FS${String(f.id).padStart(3, '0')}`,
    from:              fromApiCity(f.whereFrom),
    to:                fromApiCity(f.whereTo),
    date:              f.departureDate.split('T')[0],
    stops:             f.stops,
    free:              f.availableSeats,
    total:             f.totalSeats,
    canceled:          f.isCancelled,
    isLowAvailability: f.isLowAvailability,
  }
}

export async function searchFlights({ from, to }) {
  const params = { IsCancelled: false, HasAvailableSeats: true }
  if (from) params.WhereFrom = toApiCity(from)
  if (to)   params.WhereTo   = toApiCity(to)

  const { data } = await client.get('/api/flights', { params })
  return data.data.map(mapFlight)
}

export async function getAllFlights() {
  const { data } = await client.get('/api/flights')
  return data.data.map(mapFlight)
}

export async function cancelFlight(id) {
  await client.put(`/api/flights/${id}/cancel`)
}
