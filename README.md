# Inno-Shop

## Overview

The Inno-Shop project comprises of three primary services:

1. **Product Management Service**: Responsible for managing products, including their creation, validation, and search functionalities.
2. **User Management Service**: Handles user-related operations such as login, password reset, account verification, user creation, deletion, retrieval and more.
3. **Ocelot API Gateway**: Acts as a gateway to route requests to the appropriate microservices.

## How to Get Started

1. Clone the repository.
2. Ensure Docker and Docker Compose are installed on your machine.
3. Navigate to the project directory and run:
   ```bash
   docker-compose up --build
   ```
4. Access the services via the Ocelot API Gateway:
   - The API Gateway can be accessed using `http://localhost:5000/gateway/{appropriate_api}`.
   - Use tools like Postman or Swagger to test the APIs. Swagger is available at `http://localhost:5000/swagger`.
5. Use the following seeded users to log in:
   - **Admin User**: Email: `admin@admin.com`, Password: `password`
   - **Basic User**: Email: `user@user.com`, Password: `password`
6. Use generated token to access both services without any problem.
7. To access e-mails sent from the application check out the section below about `Email Testing`.
8. Both services are available at `http://localhost:5001` - `User Management Service` and `http://localhost:5002` - `Product Management Service`, to use them properly without gateway you also need to generate and provide token first.

## Tools and Technologies

### Docker

- Docker is used to containerize the services and their dependencies, ensuring consistency across different environments.
- The `docker-compose.yml` file orchestrates the services and their dependencies, simplifying setup and deployment.

## Services

### Databases

1. **PostgreSQL**
   - Used as the database for the Product Management Service.
2. **SQL Server**
   - Used as the database for the User Management Service.

### Message Communication

- **RabbitMQ**
  - Used for message-based communication between User Management Service and Product Management Service.

### Email Testing

- **MailHog**
  - Used for capturing and testing emails sent by the User Management Service. This ensures that email-related features like account verification and password reset are functioning as intended without sending real emails.
  - MailHog is accessible at `http://localhost:8025`.
  - To test the password recovery or account verification options, navigate to MailHog and retrieve the required token from the captured emails.

## Authorization

The system includes two roles:

1. **Admin**:
   - Has access to both Product Management and User Management services.
   - Can view all products currently available in the system but can only manage their own products.
2. **User**:
   - Has access only to the Product Management Service.
   - Can manage only their own products.

### User Deletion and Deactivation
- When a user is **deleted** by an admin, all their products are permanently deleted from the system.
- When a user is **deactivated**, their products are soft-deleted (i.e., they still exist in the database but are marked as inactive).
  - Deactivated users cannot log in until they are reactivated by an admin.
