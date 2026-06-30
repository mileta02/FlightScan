# FlightScan 

FlightScan is a mini airline ticket reservation application built with ASP.NET Core and React.
The application supports three user roles: Administrator, Agent, and Visitor.

**Administrator** can create new agents and visitors, view all flights, and cancel flights.

**Agent** can create new flights, view all flights, and approve visitor reservations.

**Visitor** can search for available flights by origin and destination, filter by direct flights, make reservations, and view their reservation history with real-time status updates.

Real-time updates for new reservations and approvals are powered by SignalR.

## Prerequisites

- Docker
- Docker Compose

## Setup

1. Clone the repository
```bash
git clone https://github.com/mileta02/FlightScan.git
cd FlightScan
```

2. Create a `.env` file in the root directory based on `.env.example`
```bash
cp .env.example .env
```

> Fill in the required values in the `.env` file before starting the application. See the **Default .env values** section below for reference.

3. Start the application
```bash
docker-compose up --build
```

4. Open your browser at `http://localhost:3000`

## First time setup

When the application starts for the first time, only the **Administrator** account is created automatically using the credentials defined in your `.env` file.

To get started:
1. Log in as Administrator using the credentials from your `.env` file
2. Navigate to the **Users** section and create at least one Agent and one Visitor
3. Log out and log in with the newly created Agent or Visitor account

## Default .env values

| Variable | Default value | Note |
|---|---|---|
| ADMIN_USERNAME | administrator | *(min 8 chars)* |
| ADMIN_PASSWORD | administrator | *(min 8 chars)* |
| JWT_KEY | *(set a secure key, e.g. flightscanflightscanflightscanflightscanflightscanflightscanflightscanflightscanflightscan)* ||
| DB_PASSWORD | root ||

## Resetting the application

If you need to reset the application and start fresh (e.g. to change admin credentials), run:

```bash
docker-compose down -v
docker-compose up --build
```

> **Warning:** This will delete all data including users, flights and reservations.

> **Note:** The `.env` file is not included in the repository. Copy `.env.example` to `.env` and adjust the values before starting the application.
