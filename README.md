# Simple C# Application for YugabyteDB

This application connects to your YugabyteDB instance via the 
[Npgsql](https://docs.yugabyte.com/latest/reference/drivers/ysql-client-drivers/#npgsql) driver for PostgreSQL and performs basic SQL operations. The instructions below are provided for [YugabyteDB Managed](https://cloud.yugabyte.com/) deployments. 
If you use a different type of deployment, then update the `sample-app.cs` file with proper connection parameters.

## Prerequisites

* [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download) or later version.
* Command line tool or your favourite IDE, such as Visual Studio Code.

## Start YugabyteDB Managed Cluster

* [Start YugabyteDB Managed](https://docs.yugabyte.com/latest/yugabyte-cloud/cloud-quickstart/qs-add/) instance. You can use
the free tier at no cost.
* Add an IP address of your machine/laptop to the [IP allow list](https://docs.yugabyte.com/latest/yugabyte-cloud/cloud-secure-clusters/add-connections/#manage-ip-allow-lists)

## Clone Application Repository

Clone the repository and change dirs into it:

```bash
git clone https://github.com/yugabyte/yugabyte-simple-csharp-app && cd yugabyte-simple-csharp-app
```

## Provide Cluster Connection Parameters

Locate and define the following connection parameters in the `sample-app.cs` file:
* `urlBuilder.Host` - the hostname of your instance.
* `urlBuilder.Username` - the username for your instance.
* `urlBuilder.Password` - the database password.
* `urlBuilder.SslMode`  - make sure it's set to `SslMode.VerifyFull`.
* `urlBuilder.RootCertificate` - a full path to your CA root cert (for example, `/Users/dmagda/certificates/root.crt`). 

Note, you can easily find all the settings on the YugabyteDB Managed dashboard:

![image](resources/cloud_app_settings.png)

## Run the Application
 
Build and run the application:
```bash
dotnet run
```

Upon successful execution, you will see output similar to the following:

```bash
>>>> Connecting to YugabyteDB!
>>>> Successfully connected to YugabyteDB!
>>>> Successfully created table DemoAccount.
>>>> Selecting accounts:
name = Jessica, age = 28, country = USA, balance = 10000
name = John, age = 28, country = Canada, balance = 9000
>>>> Transferred 800 between accounts
>>>> Selecting accounts:
name = Jessica, age = 28, country = USA, balance = 9200
name = John, age = 28, country = Canada, balance = 9800
```

## Explore Application Logic

Congrats! You've successfully executed a simple C# app that works with YugabyteDB.

Now, explore the source code of `sample-app.cs`:
1. `connect` function - establishes a connection with your cloud instance via the Npgsql driver.
2. `createDatabase` function - creates a table and populates it with sample data.
3. `selectAccounts` function - queries the data with SQL `SELECT` statements.
4. `transferMoneyBetweenAccounts` function - updates records consistently with distributed transactions.

## Questions or Issues?

Having issues running this application or want to learn more from Yugabyte experts?

Join [our Slack channel](https://communityinviter.com/apps/yugabyte-db/register),
or raise a question on StackOverflow and tag the question with `yugabytedb`!
