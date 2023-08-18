# MoneyManager

## Purpose of the project

The main purpose of the project is a helps to track personal finances. The project was made for a purpose to developing my programming skills and gain the practical experience. Additionally I got to know new technologies in practice.

## Technologies

Project based on .NET 6.

The frontend part is made using Blazor WebAssembly.<br>
Backend part is in ASP.NET Core.

In this project I also use such technologies and libraries as:
- Entity Framework
- MediatR
- xUnit
- Blazorise?

### Architecture of project

I tried to implement the project using the onion architecture.
So the entire project consist of some layers:
- API
- Domain
- Aplication
- Infrasctructure
- UI

#### API

In the API project there are controllers that handle HTTP requests and assign requests to appropriate actions. Using the IMediatr interface from the MediatR library, we send a request with the appropriate class (implementing the IRequest interface), which is then handled in the Application project by the appropriate class (implementing IRequestHandler).

#### Core

In Application Core, there are interfaces (which define the responsibilities of repositories and services), entities (used by EF), and business logic for handling API queries (implemented using MediatR).

The CQRS approach is implemented here. So the models of reading and writing are separated. Classes that are responsible for creating, modifying, and deleting have *Command* in their name, and classes that only read data have *Query* in their name.

#### Infrasctructure

The Infrastructure project includes the implementation of communication with the database using Entity Framework, communication with external APIs (currently only with CoinGecko API), sending emails, and authentication of users (JWT tokens).
