# Gatekeeper

Gatekeeper is a condo management system that allows residents to book facilities, make payments, and communicate with the management.

## Getting Started

### Prerequisites
* [.Net 7](https://dotnet.microsoft.com/download/dotnet/7.0)
* [Docker](https://www.docker.com/products/docker-desktop)

### Dev Tools

#### IDE
* [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
* [Visual Studio Code](https://code.visualstudio.com/download)
* [Rider](https://www.jetbrains.com/rider/download)


#### Http Requests
* [Postman](https://www.postman.com/downloads/)
* [Insomnia](https://insomnia.rest/download)

#### Database Access
* [DBeaver](https://dbeaver.io/download/)
* [DataGrip](https://www.jetbrains.com/datagrip/download/)

### Running the Application
Before running the application, make sure that you have the following those steps:

1. Run the database using docker-compose
```bash
docker compose up -d
```

2. Ensure that you have following environment variables set
* `DATABASE_CONNECTION_STRING` - The connection string to the database [see more](https://www.npgsql.org/doc/connection-string-parameters.html)

3. Run migrations
```bash
 dotnet run --project Database/Gatekeeper.Migration $DATABASE_CONNECTION_STRING <optinally you can use an absolute path for migrations folder>
```
4. Run the application
    
   * Run the application
      ```bash
      dotnet run --project Rest/Gatekeeper.Rest
      ```

   * Run in IDE