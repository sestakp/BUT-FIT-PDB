# PDB project

## Requirements

- .NET 7 runtime
- dotnet ef global tool
- Docker deamon

## Recommended tooling

- PgAdmin4
- MongoDB Compass

## Startup guide

This project consists of 5 parts:

- .NET web API (WriteService)
- .NET web API (ReadService)
- RabbitMQ message broker
- PostgreSQL database
- MongoDB database

Use command below to run the project:

```shell
docker compose up
```

Web APIs are available at urls:

- http://localhost:5000/swagger/index.html
- http://localhost:5002/swagger/index.html

You can also comment out `write-service` and `read-service` in docker compose and run web APIs locally using `dotnet run` command in each service. This is the recommended approach during development and debugging.

### Database seeding

There is an option to run services with `--seed` flag (`dotnet run --seed`). In such a case, applications will seed some data into the database during application startup. These data are required in some scenarios (e.g. in product creation where categories are required).

Important notes:

- There is no validation if your database is empty before seeding.
- Database seeds are not applied when running services via docker compose.
