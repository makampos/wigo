# Wigo

Wigo is a backend API solution for managing top-up beneficiaries, exploring available top-up options, and executing top-up transactions for desired phone numbers. It is built using C# and .NET.

## Table of Contents

- [Wigo](#wigo)
  - [Table of Contents](#table-of-contents)
  - [Project Description](#project-description)
  - [Features](#features)
  - [Architecture](#architecture)
  - [Technologies Used](#technologies-used)
  - [Setup and Installation](#setup-and-installation)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
    - [Database Setup](#database-setup)
  - [Running the Application](#running-the-application)
  - [Nero Project Description](#nero-project-description)
  - [Nero Features](#nero-features)
  - [Technologies Used in Nero Project](#technologies-used-in-nero-project)
  - [Running Nero the Application](#running-nero-the-application)

## Project Description

Wigo aims to provide financial inclusion services for underbanked users.

## Features

- Add and manage top-up beneficiaries.
- View available top-up options.
- Execute top-up transactions.
- Apply different top-up limits based on user verification status.

## Architecture

The project follows Clean Architecture principles and is structured into different layers:

- `Wigo.API` - The API layer that handles HTTP requests.
- `Wigo.Domain` - The domain layer containing entities and interfaces.
- `Wigo.Service` - The service layer containing business logic and handlers.
- `Wigo.Infrastructure` - The infrastructure layer containing data access and external service integrations.
- `Wigo.Tests` - The testing layer containing comprehensive tests for various implementations and functionalities within the project.


## Technologies Used

- C#
- .NET 8
- Microsoft.EntityFrameworkCore
- Npgsql.EntityFrameworkCore.PostgreSQL
- MediatR
- Flurl.Http
- FluentValidation.AspNetCore
- xunit
- FluentAssertions
- Faker.Net
- Microsoft.EntityFrameworkCore.InMemory
- NSubstitute

## Setup and Installation

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop)

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/your-username/wigo.git
    cd wigo
    ```

2. Install dependencies:
    ```sh
    dotnet restore
    ```

### Database Setup

1. Start PostgreSQL using Docker:
   
   Navigate to the `Wigo.Infrastructure/Docker` directory and run:
   
   ```sh
   docker-compose up

2. Apply database migrations:
    Navigate to the src directory and run:

    ```sh
    dotnet ef migrations add InitialCreate --project Wigo.Infrastructure --startup-project Wigo.API
    ```

    ```sh
    dotnet ef database update --project Wigo.API
    ```

## Running the Application

1. Start the application:
    ```sh
    dotnet run --project Wigo.API
    ```

The API will be available at `http://localhost:5295/swagger/v1/swagger.json`.

## Nero Project Description

Nero is an external HTTP service responsible for providing real-time balance information and debit/credit functionality.

## Nero Features

- Add account balance
- Get data from the account balance
- Make a debit to the account balance
- Add credit to the account balance

## Technologies Used in Nero Project

- C#
- .NET 8
- Faker.Net
- FluentAssertions
- Microsoft.AspNetCore.Mvc.Testing
- Microsoft.EntityFrameworkCore.InMemory
- Microsoft.EntityFrameworkCore.Sqlite
- xunit

## Running Nero the Application

In the root folder (`src`):

1. Start the application:
    ```sh
    dotnet run --project Nero.API
    ```
The API will be available at `http://localhost:5127/swagger/index.html`.


It will create a SQLite database and populate the tables automatically during the initialization.



# Manual Test Description

Follow these steps to manually test and interact with the Wigo and Nero APIs:

## Table of Contents

- [Step 1: Run Wigo.API and Create a User](#step-1-run-wigoapi-and-create-a-user)
- [Step 2: Run Nero.API and Create an Account Balance](#step-2-run-neroapi-and-create-an-account-balance)
- [Step 3: Add Credit to the Account Balance](#step-3-add-credit-to-the-account-balance)
- [Step 4: Interact with Wigo.API Endpoints](#step-4-interact-with-wigoapi-endpoints)
  - [Example: Consult Available Top-Ups](#example-consult-available-top-ups)
  - [Example: Add Beneficiaries for a User](#example-add-beneficiaries-for-a-user)

## Step 1: Run Wigo.API and Create a User

1. Start the Wigo.API project.
2. Create a new user by making a `POST` request to the `/api/user` endpoint.
3. Note the `userId` returned in the response, as you will need it for subsequent steps.

## Step 2: Run Nero.API and Create an Account Balance

1. Start the Nero.API project.
2. Create an account balance for the user by making a `POST` request to the `/api/balance/create` endpoint.
3. Provide the `userId` and a name in the request body.
4. Note the `userAccountBalanceNumber` returned in the response, as you will need it for subsequent steps.

## Step 3: Add Credit to the Account Balance

1. Make a `POST` request to the `/api/account-balance/credit` endpoint on Nero.API.
2. Provide the `userId`, `userAccountBalanceNumber`, and the amount to be credited in the request body.

## Step 4: Interact with Wigo.API Endpoints

Now that you have a user and an account balance set up, you can interact with the Wigo.API through its available endpoints. Here are some examples:

### Example: Consult Available Top-Ups

1. Make a `GET` request to the `/api/topups` endpoint to retrieve the available top-up options.

### Example: Add Beneficiaries for a User

1. Make a `POST` request to the `/api/beneficiary` endpoint.
2. Provide the `userId`, a nickname, and a phone number in the request body to add a beneficiary for the user.

Feel free to explore other endpoints and functionalities provided by the Wigo.API and Nero.API to fully understand and test the system's capabilities.


# Future Improvements

- **Resilient Communication**: Integrate the Polly package to enhance the resilience of communication between Wigo.API and Nero.API by adding retry policies. This will help ensure that transient faults do not disrupt the flow of data and operations between the services.

- **Message Broker Integration**: Introduce a message broker to facilitate efficient message exchange between Wigo.API and Nero.API using MassTransit. This will enable the system to handle asynchronous operations more effectively and improve overall performance.

- **Expand Test Coverage**: Both Wigo.API and Nero.API currently has almost 50 tests each. However, there is room for improvement by adding more tests, such as integration tests using TestContainers.

- **Dockerization**: Dockerize the application to simplify the setup process and make it more portable and ready to start quicker.

- **Additional Enhancements**: Continuously explore and implement additional enhancements and functionalities.







