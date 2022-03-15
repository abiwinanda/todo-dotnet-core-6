# TodoApp

Todo app created using dotnet core 6 that demonstrate IoC, unit testing, data layer, and generic repository pattern.

## Project Set Up

For local development purposes, you might want to run the server or unit test locally. You could follow the following guideline to set up or run the project locally.

### Prerequisites

Assuming you already have dotnet code 6 and docker installed, you could skip prerequisites steps. Otherwise, you could follow the following steps:

1. First you would need to have dotnet core version 6 installed on your local machine. You could download dotnet core 6 from the [Official Download Page](https://dotnet.microsoft.com/en-us/download). If you have installed dotnet core version 6, you could double check it using the cli

```sh
dotnet --info

.NET SDK (reflecting any global.json):
 Version:   6.x.xxx
 Commit:    ...

...
```

2. Next, you would also need docker to set up dependency services such as `MS SQL Server`. To install docker, it is recommended that you follow the official [Get Docker](https://docs.docker.com/get-docker/) guideline.

### Start the Application

1. Clone this repository into your local computer and cd into the directory.

```sh
git clone <https/ssh-url>
cd todo-dotnet-core-6
```

2. Run `dotnet restore` to restore all project dependencies.

3. Start `MS SQL Server` by running `docker-compose up -d`. This will run a mssqlserver container which you could check by running `docker ps`. By default, mssqlserver will run on port `1433` and you could connect to it with `sa` as the username and `Password@123` as the password. If you want to change the port, you could change the host port in the [docker-compose](https://github.com/abiwinanda/todo-dotnet-core-6/blob/master/docker-compose.yaml#L9). If you prefer to use your locally installed mssqlserver, you could skip this step.

4. Once you have `MS SQL Server` running in you local, you could start the db migration by running

```sh
dotnet ef database update --project TodoApp.Data --startup-project TodoApp.Api
```

5. Once the migration is done you could run the app API server by running

```sh
dotnet run --project TodoApp.Api
```

### Run Unit Test

To run unit test simply run the following command

```sh
dotnet test
```

You could run the command in the root sln directory or inside `TodoApp.App` project directory.


## Usage

Once you have the application running you could use postman to play around with the API or you could access `https://https://localhost:7216/swagger/index.html` to play around with the API using swagger (recommended).

### Issues

If you found any issues with the project, whether it is a bug or you are unable to run the application locally, do not hesitate to post the issues in the [Issues](https://github.com/abiwinanda/todo-dotnet-core-6/issues) tab.

## Contribution

For developers who will work on this project, you could first try to understand the project structure before starting the development.

### Project Structure

The structure of this project is as follows:
* `TodoApp.Api` => contain the Rest API interface.
* `TodoApp.Core` => contain all the business process or logic of the app.
* `TodoApp.Data` => contain all the data definition of the app such as entities, dtos, and dbcontext.
* `TodoApp.Test` => contain the unit test of the project.
