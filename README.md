# NeonArcade

NeonArcade is a fast-paced, neon-themed arcade game collection with retro visuals and modern mechanics. This repository contains a .NET solution with a backend server and a separate client folder for the game front-end. It includes source, configuration templates, and setup instructions to run the application locally and prepare a development environment.

> Fast. Bright. Addictive.

---

## Table of contents

- [What this repo contains](#what-this-repo-contains)
- [Tech stack](#tech-stack)
- [Requirements](#requirements)
- [Quickstart (development)](#quickstart-development)
- [Detailed setup](#detailed-setup)
- [Running the application](#running-the-application)
- [Project structure](#project-structure)
- [Controls](#controls)
- [Contributing](#contributing)
- [Roadmap](#roadmap)
- [License](#license)
- [Contact](#contact)

---

## What this repo contains

- A .NET solution file (NeonArcade.sln) that ties together the server and other projects.
- A backend project/area in `NeonArcade.Server` (server-side code, config templates and database migrations).
- A client project in `neonarcade.client` (frontend; check this folder for exact stack and available README).
- A `SETUP.md` with development setup instructions and configuration templates.

---

## Tech stack (inferred from repository)

- .NET SDK (solution and server)
- Entity Framework Core (database migrations / `dotnet ef`)
- A separate client frontend (check `neonarcade.client` for exact framework: e.g., React/Angular/Phaser/other)

---

## Requirements

- .NET SDK (version compatible with the solution — check global.json or project files if present)
- EF Core tools for migrations: `dotnet tool install --global dotnet-ef` (if not already installed)
- A supported database (the server may be configured to use SQLite, SQL Server, or another provider — check `NeonArcade.Server/appsettings*.json.template`)
- Node.js + npm/yarn if the client is a JavaScript/TypeScript web frontend (check `neonarcade.client/package.json`)

---

## Quickstart (development)

1. Clone the repository
   git clone https://github.com/ANDYGAB04/NeonArcade.git
   cd NeonArcade

2. Configure the server (development)
   - Copy the template config and edit credentials:
     cp NeonArcade.Server/appsettings.Development.json.template NeonArcade.Server/appsettings.Development.json
   - Edit NeonArcade.Server/appsettings.Development.json and set:
     - AdminUser:Email
     - AdminUser:Password (must meet password requirements — see Detailed setup)

3. Database initialization
   cd NeonArcade.Server
   dotnet ef database update

4. Run the server
   dotnet run

5. Run the client (if applicable)
   - If the client is a Node-based frontend, from the repository root or neonarcade.client:
     cd neonarcade.client
     npm install
     npm run start
   - If the client is a different type, check the client folder for instructions.

---

## Detailed setup

Follow the repository's SETUP.md for step-by-step instructions. Key items from that file are reproduced here for convenience:

1. Configure development settings
   - Copy the template:
     cp NeonArcade.Server/appsettings.Development.json.template NeonArcade.Server/appsettings.Development.json
   - Edit `appsettings.Development.json` and update:
     - `AdminUser:Email`
     - `AdminUser:Password` (secure password meeting requirements)

   Important: `appsettings.Development.json` is git-ignored. Do not commit secrets — use templates as references.

2. Password requirements (admin password)
   - At least 6 characters
   - At least one uppercase letter
   - At least one lowercase letter
   - At least one digit
   - Non-alphanumeric characters optional

3. Database setup
   - Ensure EF Core tools are available:
     dotnet tool install --global dotnet-ef
   - From `NeonArcade.Server`:
     dotnet ef database update

4. Run the application
   - From `NeonArcade.Server`:
     dotnet run
   - On first run the admin user will be created automatically using credentials from the config.

Security notes
- Never commit `appsettings.Development.json` or production secrets.
- For production deploys, use environment variables or secret stores (e.g., Azure Key Vault).

Refer to SETUP.md in the repo for the authoritative version:
https://github.com/ANDYGAB04/NeonArcade/blob/master/SETUP.md

---

## Running the application

Backend (server)
- From the server folder:
  cd NeonArcade.Server
  dotnet run
- The server exposes the API and any admin UI or endpoints configured in the project. Check `NeonArcade.Server` for launch settings, ports, and logging configuration.

Client (frontend)
- The client is located at `neonarcade.client`. Open that folder and check for:
  - `package.json` (Node web frontend / game)
  - `README.md` inside the client
  - Build & start scripts (e.g. `npm run start`, `npm run build`)

If the client is a web frontend, typical steps:
  cd neonarcade.client
  npm install
  npm run start
Open the indicated URL (usually http://localhost:3000 or http://localhost:8080) and ensure it is configured to target the backend server’s API URL.

---

## Project structure (items discovered)

- NeonArcade.sln — .NET solution file
- NeonArcade.Server/ — server project (configuration templates and EF migrations likely here)
- neonarcade.client/ — client/frontend project (inspect to discover exact tech)
- SETUP.md — repository setup guide (contains important config and database setup steps)
- .gitattributes
- .gitignore
- README.md (this file)

---

## Controls

Default controls depend on the client implementation. Common mappings:
- Move: Arrow keys / WASD
- Action: Space / Enter
- Shoot / Interact: Ctrl / Left Click
- Pause/Menu: Esc

Update controls when the client code (or client README) clarifies exact bindings.


---

## Roadmap (examples)

- Polish particle and shader effects
- Implement online leaderboards
- Add additional mini-games
- Mobile/touch support
- Publish builds to itch.io / Steam

---

## License

This project currently has no license file in the repository root (or license text was not detected). Add a LICENSE file (e.g., MIT) to clarify usage and contribution terms.

---

## Contact

Maintainer: ANDYGAB04  
Repository: https://github.com/ANDYGAB04/NeonArcade

---

