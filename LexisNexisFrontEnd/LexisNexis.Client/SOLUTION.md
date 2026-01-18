# Solution Architecture

## Overview

This document describes the architecture of the LexisNexis solution, which consists of an Angular frontend and a .NET 8 backend API.

# Design Decisions

## Architectural Philosophy

The solution prioritizes **auditability** and **compile-time safety**, trading development velocity for long-term maintainability and observability.

## Frontend State Management

**Specification**: RxJS for state management

**Choice**: NgRx (which uses RxJS under the hood)

**Rationale**: Demonstrates senior-level Angular expertise while fulfilling the specification. Provides TypeScript type safety, predictable state flow, time-travel debugging, and centralized error handling at the cost of additional boilerplate (actions, reducers, effects, selectors).

## Backend Event Sourcing (CQRS)

**Choice**: Hand-rolled CQRS with EventPlayerService

**Trade-off**: More boilerplate and stricter patterns vs. complete audit trail and compile-time safety

**Benefits**:
- EventPlayerService provides centralized logging of all business operations
- Event replay capabilities for debugging and testing
- .NET compile-time safety through pattern enforcement
- Clear separation of concerns

## Result Pattern

**Choice**: Discriminated unions for error handling

**Benefit**: Compile-time enforcement prevents accessing data in failure paths (and vice versa), making error handling explicit and self-documenting.

## Summary

These decisions create a system with strong auditability, type safety, and predictability. The trade-off is clear: **accept more boilerplate in exchange for safety and maintainability**.

## Project Structure

The solution is organized into the following projects:

### Backend Projects

- **FrontEnd** - Angular stand alone application using NGRX
- **API** - Main API entry point, controllers, and HTTP endpoints
- **BLL (Business Logic Layer)** - Business logic and domain operations
- **Common** - Shared utilities, helpers, and cross-cutting concerns
- **DAL (Data Access Layer)** - Database access, repositories, and data models
- **Test** - Automated testing suite
  - **Caching Tests** - Unit tests for cache service functionality
  - **SearchEngine Tests** - Tests for fuzzy search algorithm and scoring
  - **API Integration Tests** - In-memory API tests that start the API and call endpoints

## Technology Stack

### Frontend
- **Framework**: Angular 20.3.14
- **Runtime**: Node.js 24.3.0
- **Package Manager**: npm 11.7.0
- **Location**: `LexisNexis\LexisNexisFrontEnd\LexisNexis.Client`
- **Architecture**: Standalone components with NgRx state management
- **UI Library**: Angular Material for styling and components

### Backend
- **Framework**: .NET 8
- **API Project**: LexisNexis.API
- **Package Management**: NuGet with Central Package Management (`Directory.Packages.props`)
- **Location**: `stunning-LN\LexisNexisServer\LexisNexis.slnx`

## Frontend Architecture

### Project Organization

The Angular application uses a **hybrid organizational structure**:

1. **N-Tier Layer Grouping** (top level):
   - **Components** - Reusable UI components
   - **Models** - TypeScript interfaces and data models
   - **Pages** - Route-level page components
   - **Services** - HTTP services for API communication
   - **Store** - NgRx state management (actions, reducers, effects, selectors)

2. **Feature Grouping** (within each layer):
   - Related functionality is co-located (e.g., product-related components, services, and store slices grouped together)

### State Management (NgRx)

The Angular application uses **NgRx** for centralized state management with a unidirectional data flow:

- **Standalone Components**: All components are standalone (not using NgModules)
- **Action-Based Communication**: Components emit actions rather than calling services directly
- **Effects**: NgRx Effects use dependency injection to resolve services and handle side effects (API calls, etc.)
  - Effects handle async operations (HTTP requests) using RxJS operators (`switchMap`, `mergeMap`, `concatMap`)
  - `switchMap`: Cancels previous requests for operations like search/load (prevents race conditions)
  - `mergeMap`: Allows concurrent operations for CRUD actions
  - `concatMap`: Ensures sequential execution for related actions (e.g., reload after save)
- **Store**: Single source of truth for application state
- **Error Handling**: Effects display user feedback via toast notifications for errors or server messages
- **Success Flows**: Effects can chain actions (e.g., after successful create/update, reset pagination and reload data)

**Data Flow:**
```
[Component] → Dispatch Action → [Store] → [Effect] → [Service] → [API]
                                    ↓
[Component] ← Select State ← [Reducer] ← Action ← [Effect]
                                                      ↓
                                              Toast Notifications
```

This architecture ensures:
- Predictable state changes
- Separation of concerns (components don't directly interact with services)
- Testable side effects through Effects
- Time-travel debugging capabilities

### Forms and Validation

- **Angular Material Forms**: Used for all user input
- **Validators**: Material form validators for input validation
- **Server Feedback**: Error messages and user feedback from the server are displayed via toast notifications triggered by Effects

### Testing

- **Unit Tests**: Includes basic unit tests demonstrating HTTP service mocking (e.g., ProductService test)

### Backend

## Package Management

### Central Package Management

The solution uses **Directory.Packages.props** for centralized package management. This approach provides:

- **Centralized Version Control**: All NuGet package versions are defined in a single `Directory.Packages.props` file
- **Consistency**: Ensures all projects use the same version of shared dependencies
- **Simplified Updates**: Package versions can be updated in one location
- **Reduced Conflicts**: Eliminates version mismatches across projects

All project files reference packages without specifying versions, which are instead managed centrally in `Directory.Packages.props`.

## Architecture Layers

### Presentation Layer (API)

The **API** project uses **vertical slice architecture** with minimal APIs, organizing endpoints by feature:

- **Categories** - Category-related endpoints (one endpoint per file)
- **Products** - Product-related endpoints (one endpoint per file)
- **Helpers** - Mapping utilities for registering minimal API endpoints
- **Middleware** - Custom error handling middleware that catches exceptions and returns generic error messages with trace IDs for debugging

**Design Pattern:**
- Each endpoint file defines a single minimal API route
- Endpoints use dependency injection to access `EventPlayerService`
- Business operations are executed by emitting commands/queries through the EventPlayerService
- This approach maintains feature cohesion and separates HTTP concerns from business logic

### Business Logic Layer (BLL)

The **BLL** project implements business logic using **vertical slice architecture**, organizing code by feature rather than technical layer:

- **Categories** - All commands, queries, and handlers related to category operations
- **Products** - All commands, queries, and handlers related to product operations  
- **SearchEngine** - Generic fuzzy search implementation with type constraint `where T : EntityBase<TKey>`
  - Discovers searchable string properties via reflection on initialization
  - Applies `SearchWeightAttribute` to adjust field relevance in scoring
  - **Scoring Algorithm**:
    - Prioritizes exact matches
    - Rewards substring matches
    - Uses Levenshtein distance algorithm for fuzzy matching with configurable similarity thresholds
  - Combines field scores with weights to produce final relevance ranking
  - Returns results ordered by descending score

Each feature slice contains its own events and event handlers, keeping related logic co-located and reducing cross-cutting dependencies.

### Data Access Layer (DAL)

The **DAL** project handles data persistence and retrieval:

- **Mappers** - Simple mapping utilities for converting between domain models and DTOs (e.g., `ProductModel` to `ProductDto`)
- **Models** - Domain models that inherit from `EntityBase<T>` for shared Id properties
- **Seed** - Generates seed data for testing and development
- **Storage** - Data access implementations with generic repositories using `<TModel, TKey>` type arguments (typically `TKey` is `int`)
  - **Entity Framework**:
    - **Read Repository**: Optimized for query operations
    - **Write Repository**: Handles create, update, and delete operations
    - *Design Note*: Designed to work with separate DbContexts for read/write segregation, but currently both use a single `ApplicationDbContext`
  - **In-Memory**:
    - **ID Generator**: `IIdGenerator` with int-based implementation using `Interlocked` for atomic ID generation
    - **InMemoryRepository**: Single implementation that implements both `IReadRepository` and `IWriteRepository` interfaces
    - Consumers reference either `IReadRepository` or `IWriteRepository` to establish read or write context semantically

### Common Layer
The **Common** project contains shared infrastructure code referenced by all other projects:

- **Cache** - Caching infrastructure and utilities
- **CQRS** - Command Query Responsibility Segregation implementation
- **DTO** - Data Transfer Objects using record types for immutability and value equality
- **Filters** - Predicate composition utilities and search weighting attributes
  - **PredicateExtensions**: Expression tree combinators for building dynamic LINQ queries (e.g., `And()` method for combining predicates)
  - **SearchWeightAttribute**: Attribute for defining search relevance weights on properties, used by SearchEngineService to adjust Levenshtein algorithm scores
- **Result** - Result pattern implementation for operation outcomes

#### Result Pattern

The solution uses the **Result Pattern** implemented as discriminated unions for explicit error handling without exceptions. Pattern matching prevents bugs by making it impossible to access data in a failure code path (and vice versa), providing compile-time safety guarantees.

#### CQRS Implementation

The solution implements a **hand-rolled CQRS** pattern using marker interfaces and event handlers:

- **Marker Interfaces**: `ICommand<T>` and `IQuery<T>` distinguish between operations that modify state (commands) and operations that read state (queries)
- **Dispatchers**: `ICommandDispatcher` and `IQueryDispatcher` route commands and queries to their respective handlers using reflection to dynamically resolve and invoke handlers at runtime
  - *Note: While the current implementation uses reflection, this could be optimized using source code generation to create compile-time mappings in a generated .cs file*
- **EventPlayerService**: Central service that coordinates command/query execution and maintains an event log for replay capabilities
- **Auto-Registration**: The `AddCqrs()` extension method scans provided assemblies to automatically register all `ICommandHandler<,>` and `IQueryHandler<,>` implementations with the DI container

**Event Replay:**
- Commands are logged with timestamp and type information in a `ReplayEntry` collection
- The `EventReplayBehaviorAttribute` with `EventReplayOptions` flags controls which events can be replayed
- Only commands marked as `Replayable` are included during replay operations
- Queries are not logged (read-only operations don't need replay)

This approach provides event sourcing capabilities while maintaining separation between read and write operations.

#### Caching

The solution uses **Microsoft's IMemoryCache** for in-memory caching with custom enhancements:

- **CacheService**: Wrapper around IMemoryCache providing key generation, eviction, and container management
- **CacheContainer<T>**: Thread-safe cache wrapper using `SemaphoreSlim` for lock-based synchronization on cached items
- **Key Generation**: Composite cache keys are hashed using `XxHash3` algorithm, combining multiple key parts (strings, GUIDs, primitives) into a single Guid identifier
  - Uses `ArrayPool<byte>` for efficient memory allocation during hashing
- **Cache Eviction**: Support for manual cache eviction via `EvictCache()` method
- **Expiration Control**: Configurable cache duration with absolute expiration

This provides efficient, thread-safe caching with deterministic key generation for complex composite keys.

## Communication Flow

```
[Angular App] (http://localhost:4200)
      ↓
[Proxy /api requests]
      ↓
[API Layer] (https://localhost:7059)
      ↓
[Business Logic Layer]
      ↓
[Data Access Layer]
      ↓
[Database]
```
