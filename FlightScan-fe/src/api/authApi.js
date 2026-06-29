import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
})

export async function login(username, password) {
  const { data } = await api.post('/api/auth/login', { username, password })
  return data 
}
