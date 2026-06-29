import * as signalR from '@microsoft/signalr'

function getToken() {
  try { return JSON.parse(localStorage.getItem('flightscan_auth'))?.token ?? null }
  catch { return null }
}

export function createConnection() {
  return new signalR.HubConnectionBuilder()
    .withUrl(`${import.meta.env.VITE_API_URL}/hubs/flight`, {
      accessTokenFactory: () => getToken()
    })
    .withAutomaticReconnect()
    .build()
}
