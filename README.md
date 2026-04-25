**Game of Life (WPF, .NET)**

A clean and modular implementation of Game of Life built with C#, .NET, and WPF, following principles from Clean Architecture and MVVM.

**Overview**

The solution is structured into clear layers:

Domain → Core game logic (rules, grid, cells)
Application → Use cases and orchestration
Presentation → WPF UI using MVVM

**Architecture**

**Presentation (WPF / MVVM)**
        ↓
**Application (Use Cases)**
        ↓
**Domain (Core Logic)**

**Key design principles:**
Separation of concerns
Testability
Immutability in domain logic
No exceptions for flow control (Result pattern)

**Features**
Start / Stop simulation
Step-by-step execution
Random grid initialization

**Technologies**
.NET 10
WPF
C#
MVVM pattern
LINQ
xUnit + FluentAssertions (for tests)

**Getting Started**
Prerequisites
.NET 10 SDK
Visual Studio 2022+ (with WPF support)
Run the application

**Testing**
Domain logic (rules, grid behavior)
Application logic (use cases)

**Design Decisions**
Immutable domain logic
Result pattern instead of exceptions
Injected dependencies (Random, Timer) for testability
MVVM pattern and UI separation
