# Candidates Management API

This is a .NET 8 Web API application for managing job candidate information. The API provides endpoints for adding or updating candidate information. It is built with scalability and maintainability in mind, using best practices for .NET development.

## Getting Started

To get started with the Candidates Management API, follow these steps:

1. Clone this repository to your local machine:


2. Open the solution in Visual Studio.

3. Build the solution.

4. Run the application.

## Usage

### Endpoints

#### POST /api/candidates/AddOrUpdateCandidate

Creates or updates a candidate in the system. The endpoint expects the following information in the request body:

- First name (required)
- Last name (required)
- Email (required)
- Phone number
- Time interval when it's better to call
- LinkedIn profile URL
- GitHub profile URL
- Free text comment (required)

If the candidate profile already exists in the system, it will be updated. If not, a new candidate will be created.

### Error Handling

The API returns well-structured responses for various scenarios, including successful operations and errors. Errors are returned with appropriate status codes and error messages.

## Dependencies

The Candidates Management API relies on the following dependencies:

- .NET 8
- Entity Framework Core
- FluentValidation
- Moq (for testing)
- xUnit (for testing)

## Testing

Unit tests are included in the `CandidatesManagement.Tests` project. These tests cover both the controller and repository functionalities. To run the tests, use the test runner in Visual Studio.

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
