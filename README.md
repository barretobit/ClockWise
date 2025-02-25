# ClockWise API

ClockWise is a .NET 8 API designed to help companies manage employee work hours. Employees can log in, start, and stop their working times, which are recorded and stored for tracking purposes.

## Features
- Company and employee management
- Employee types classification
- Tick logs for tracking work sessions
- RESTful API with authentication and authorization

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server
- Entity Framework Core

### Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/barretobit/ClockWise.git
   ```
2. Navigate to the API directory:
   ```sh
   cd ClockWise.Api
   ```
3. Install dependencies:
   ```sh
   dotnet restore
   ```
4. Apply database migrations:
   ```sh
   dotnet ef database update
   ```
5. Run the application:
   ```sh
   dotnet run
   ```

## API Endpoints

### Companies
- `GET /api/companies` - Retrieve all companies
- `GET /api/companies/{id}` - Retrieve a company by ID
- `POST /api/companies` - Create a new company
- `PUT /api/companies/{id}` - Update a company
- `DELETE /api/companies/{id}` - Delete a company

### Employees
- `GET /api/employees` - Retrieve all employees
- `GET /api/employees/{id}` - Retrieve an employee by ID
- `POST /api/employees` - Create a new employee
- `PUT /api/employees/{id}` - Update an employee
- `DELETE /api/employees/{id}` - Delete an employee

### Employee Types
- `GET /api/employeeTypes` - Retrieve all employee types
- `GET /api/employeeTypes/{id}` - Retrieve an employee type by ID
- `POST /api/employeeTypes` - Create a new employee type
- `PUT /api/employeeTypes/{id}` - Update an employee type
- `DELETE /api/employeeTypes/{id}` - Delete an employee type

### Tick Logs
- `GET /api/tickLogs/{employeeId}` - Retrieve tick logs by employee ID
- `GET /api/tickLogs/{employeeId}/today` - Retrieve today's tick logs for an employee
- `GET /api/tickLogs/{employeeId}/range?dateFrom=YYYY-MM-DD&dateTo=YYYY-MM-DD` - Retrieve tick logs within a date range

## Running Tests
To run unit tests, navigate to the test project directory and execute:
```sh
 dotnet test
```

## Contribution
Feel free to fork the repository, create a new branch, and submit a pull request for review.

## License
This project is licensed under the MIT License - see the LICENSE file for details.

## Contact
For inquiries, reach out to `barretobit@gmail.com` or visit the [GitHub Issues](https://github.com/barretobit/ClockWise/issues) page.

