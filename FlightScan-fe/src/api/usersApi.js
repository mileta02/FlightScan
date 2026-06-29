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

function mapUser(u) {
  return {
    username: u.username,
    role:     u.role?.toLowerCase() ?? 'visitor',
  }
}

export async function getUsers() {
  const { data } = await client.get('/api/users')
  return data.data.map(mapUser)
}

export async function createUser({ username, password, role }) {
  const { data } = await client.post('/api/users', { Username: username, Password: password, Role: role })
  return data
}
