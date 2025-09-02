# OrbitPass API - Enterprise Architecture

## Folder Structure

### 🏗️ **Core** (Domain Layer)
- **Entities/**: Domain entities (User, Event, Order, Ticket)
- **Interfaces/**: Repository and service contracts
- **DTOs/**: Data Transfer Objects for API communication
- **Enums/**: Domain enumerations (UserRole, TicketType, OrderStatus)
- **Exceptions/**: Custom domain exceptions
- **Specifications/**: Query specifications pattern

### 🔧 **Infrastructure** (Data & External Services)
- **Data/**: DbContext, entity configurations
- **Repositories/**: Repository implementations
- **Services/**: External service implementations (email, payment)
- **Configurations/**: Entity Framework configurations
- **Migrations/**: Database migrations

### 💼 **Application** (Business Logic)
- **Services/**: Business logic services
- **Interfaces/**: Application service contracts
- **Validators/**: FluentValidation validators
- **Mappings/**: AutoMapper profiles
- **Features/**: CQRS commands/queries (optional)

### 🌐 **Presentation** (API Layer)
- **Controllers/**: API controllers
- **Middleware/**: Custom middleware (error handling, logging)
- **Filters/**: Action filters and attributes
- **Extensions/**: Service registration extensions

### 🔄 **Shared** (Cross-Cutting)
- **Constants/**: Application constants
- **Helpers/**: Utility classes
- **Extensions/**: Extension methods
- **Models/**: Shared models and responses

### 🧪 **Tests**
- **UnitTests/**: Unit tests for business logic
- **IntegrationTests/**: API integration tests
- **TestHelpers/**: Test utilities and fixtures

## Dependency Flow
```
Presentation → Application → Infrastructure
     ↓              ↓            ↓
   Core ←────── Core ←────── Core
```

## Benefits
- **Separation of Concerns**: Each layer has distinct responsibilities
- **Testability**: Easy to mock and test individual components
- **Maintainability**: Changes in one layer don't affect others
- **Scalability**: Easy to add new features following established patterns
- **Enterprise Ready**: Follows industry best practices