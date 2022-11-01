# A brief example of working with DotNet Core and OData
This is a DotNet Core web api repo that uses OData to prevent over-fetching and under-fetching.

[OData](https://www.odata.org), short for Open Data Protocol, is an open protocol to allow the creation and consumption of queryable and interoperable RESTful APIs in a simple and standard way.

In this example, we are going to create a Web API to request for some data. OData will be in charge of **loading only the required data**.
The data model is represented in the next image:
![Database diagram](/docs/img/database-diagram-1.png?raw=true "Database diagram")

### Technologies in this repo:
* DotNet Core 6
* OData
* Entity Framework 6 (Code First)
* SQL Server (Docker Container)
* xUnit (Integration Tests)

### Database
We are using SQL Server as the default database, but you can use any Entity Framework supported database.

#### Setup Database
Run this command to create the database container (you need to have Docker installed on your system):

```sh
docker run -d --name my-sqlserver -p 1433:1433 -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=My@Passw0rd" mcr.microsoft.com/mssql/server:2019-latest
```

Stop and remove the container when needed:

```sh
docker stop my-sqlserver && docker rm my-sqlserver
```

#### Create Database

Apply the existing migration. Run this command in the application root folder:

```sh
dotnet ef database update --project ODataExample.Web
```