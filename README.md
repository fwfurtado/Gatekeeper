# Gatekeeper
Gatekeeper is a condo management system that allows residents to book facilities, make payments, and communicate with the management.

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=fwfurtado_Gatekeeper)](https://sonarcloud.io/summary/new_code?id=fwfurtado_Gatekeeper)

[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=fwfurtado_Gatekeeper&metric=coverage)](https://sonarcloud.io/summary/new_code?id=fwfurtado_Gatekeeper)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=fwfurtado_Gatekeeper&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=fwfurtado_Gatekeeper)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=fwfurtado_Gatekeeper&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=fwfurtado_Gatekeeper)

[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=fwfurtado_Gatekeeper&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=fwfurtado_Gatekeeper)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=fwfurtado_Gatekeeper&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=fwfurtado_Gatekeeper)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=fwfurtado_Gatekeeper&metric=bugs)](https://sonarcloud.io/summary/new_code?id=fwfurtado_Gatekeeper)

[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=fwfurtado_Gatekeeper&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=fwfurtado_Gatekeeper)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=fwfurtado_Gatekeeper&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=fwfurtado_Gatekeeper)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=fwfurtado_Gatekeeper&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=fwfurtado_Gatekeeper)


## Getting Started

### Prerequisites
* [.Net 7](https://dotnet.microsoft.com/download/dotnet/7.0)
* [Docker](https://www.docker.com/products/docker-desktop)
* [Cake](https://cakebuild.net/docs/getting-started/setting-up-a-new-scripting-project)

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
dotnet cake docker.cake --target=up
```

4. Create a user at Keycloak IDP [see more](Docs/Create-Keycloak-User.md)

5. Run the application
    
   * Run the application
      ```bash
      dotnet run --project Rest/Gatekeeper.Rest
      ```

   * Run in IDE

6. Stop database
```bash
dotnet cake docker.cake --target=down
```