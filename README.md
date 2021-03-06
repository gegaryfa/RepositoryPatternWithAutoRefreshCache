# Repository Pattern With Auto-Refresh Cache

[![license](https://img.shields.io/github/license/gegaryfa/DockerComposeMakefile)](https://github.com/gegaryfa/DockerComposeMakefile/blob/master/LICENSE)
![GitHub stars](https://img.shields.io/github/stars/gegaryfa/RepositoryPatternWithAutoRefreshCache)

.Net Core API - Repository pattern with auto refresh cache. This is a simple API using the repository design pattern to get data from a database. The data from the database are being cached either InMemory or in Redis(for scaled systems).

## Getting Started

Clone the repo, open a terminal and run `make` to see all the possible actions.
![makefile](img/make.PNG)

The project setup is based on Domain Driven Design:
![makefile](img/DDD.png)

### Prerequisites

* [`GNU Make`](https://www.gnu.org/software/make/)
* [`.Net Core v5.0`](https://dotnet.microsoft.com/download/dotnet-core/5.0)
* [`Docker`](https://www.docker.com/get-started)
* Your favorite IDE/editor


### Installing

I created a makefile to make it as easy as possible to get the docker containers up and runnig 😎. The only thing you need to do is run:

```
make up
```
After that you need to wait for the project to build and you are all set!
The make command will run the docker-compose file and download all the images required for the porject. This will take some time so be patient ☕.
When the `make up` command is finished, you should have 3 containers running:
- The web API @: http://localhost:8000/ 
- A SQL server with some dummy data already seeded in a database.
- A redis cache which is being used when you set redis as the cache provider.

Alternatively, you can run the `RepositoryWithCaching.WebApi` project which will use In-memory(by default) data instead of a database on a sql server.

## Built With

* [.Net Core v5.0](https://dotnet.microsoft.com/download/dotnet-core/5.0)
* [AutoMapper](https://automapper.org/) - A convention-based objet-objet mapper.
* [Hangfire](https://hangfire.io/) - Easy and reliable library to perform fire-and-forget tasks.
* [MediatR](https://github.com/jbogard/MediatR) - Simple mediator implementation in .NET.
* [Swagger](https://swagger.io/) - Tools for documenting APIs.

## Sources
A good read on distributed caching for .Net Core: [Distributed caching in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-3.1).


## Authors

* **George Garyfallou** - *Initial work* - [gegaryfa](https://github.com/gegaryfa)

## Acknowledgments

* Hat tip to anyone whose code was used

