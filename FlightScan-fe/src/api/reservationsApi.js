import axios from 'axios'

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

export async function createReservation(flightId, seatsCount) {
  const { data } = await client.post('/api/reservations', { FlightId: flightId, SeatsCount: seatsCount })
  return data
}

const FROM_API = { 'Nis': 'Niš' }
const fromApiCity = (c) => FROM_API[c] ?? c

export function mapReservation(r) {
  return {
    id:       r.id,
    num:      `FS${String(r.flightId).padStart(3, '0')}`,
    from:     fromApiCity(r.whereFrom),
    to:       fromApiCity(r.whereTo),
    date:     r.departureDate.split('T')[0],
    seats:    r.reservedSeats,
    status:          r.status?.toLowerCase() ?? 'pending',
    username:        r.username ?? null,
    flightCancelled: r.isFlightCancelled ?? false,
  }
}

export async function getMyReservations() {
  const { data } = await client.get('/api/reservations/my')
  return data.data.map(mapReservation)
}

export async function getAllReservations() {
  const { data } = await client.get('/api/reservations/pending')
  return data.data.map(mapReservation)
}

export async function approveReservation(id) {
  await client.put(`/api/reservations/${id}/approve`)
}
