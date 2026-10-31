# Real-time Analytics Dashboard

A real-time analytics dashboard built with ASP.NET Core. When someone adds an event,
every open browser updates instantly, with no page refresh.

<img width="1920" height="1097" alt="image" src="https://github.com/user-attachments/assets/35dfe9e2-eb92-4285-9867-c7216cb463e8" />


## Tech Stack
- ASP.NET Core Web API (.NET)
- Clean Architecture (Api, Application, Domain, Infrastructure)
- CQRS with MediatR
- Dapper + SQL Server (LocalDB)
- SignalR for real-time updates
- Hangfire for background jobs
- Chart.js for the live chart

## Features
- Live dashboard with total events, total revenue, and a revenue chart
- Add an event from the page and see every open browser update at once
- REST API: `GET /api/Analytics` and `POST /api/Analytics/events`
- Swagger UI for testing the API

## How to run
1. Install Visual Studio with the ASP.NET workload (it includes SQL Server LocalDB).
2. Clone this repo and open the solution file.
3. Make sure the connection string in `appsettings.json` points to your SQL Server.
4. Press the green **HTTPS** button.
5. Open `https://localhost:7270` in two browser windows.
6. Click **Add Event** in one window and watch both update.

## Author
Md. Abdullah Al Mamun
Email: mstudent251@gmail.com 
LinkedIn link: https://www.linkedin.com/in/md-abdullah-al-mamun-a97651246/
