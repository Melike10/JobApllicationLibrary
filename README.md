# Job Application Evaluation System - Unit Test Project

This repository demonstrates a **Job Application Evaluation System** written in **C#**, showcasing unit testing with **NUnit**, **Moq**, and **FluentAssertions**. The project is designed to teach the fundamentals of unit testing for developers new to the concept.

## 🚀 Overview

The system evaluates job applications based on various criteria such as:
- Applicant's age
- Years of experience
- Technology stack compatibility
- Office location
- Identity validation

This repository includes a library project and a corresponding unit test project to validate the business logic.

---

## 📂 Project Structure

### **Library Project: `JobApplicationLibrary`**
- **Models**
  - `Applicant`: Represents an applicant with properties like `IdentyNumber` and `Age`.
  - `JobApplication`: Represents a job application form with properties for experience, tech stack, etc.
  - `ValidationMode`: Enum specifying validation modes (`Quick` or `Detail`).

- **Services**
  - `IIdentityValidator`: Interface for validating identity numbers and retrieving country data.
  - `ICountryProvider` and `ICountryData`: Interfaces for hierarchical country data.

- **ApplicationEvaluator**
  - The main business logic class that evaluates job applications using criteria and external dependencies.

### **Unit Test Project: `JobApplicationLibrary.UnitTest`**
- Contains test cases for the `ApplicationEvaluator` class.
- Demonstrates mocking external dependencies using **Moq**.
- Uses **FluentAssertions** for readable and expressive assertions.

---

## 🔧 Prerequisites

Before running the project or tests, ensure you have:
1. .NET SDK installed (version 6.0 or later recommended).
2. A C# IDE such as Visual Studio or Visual Studio Code.

---



## 🧪 Unit Testing Explained
What is Unit Testing?
Unit testing is the process of testing individual units or components of a software system in isolation to ensure they work as expected. A "unit" is typically the smallest piece of testable code, such as a method or a class.

### Why Unit Testing?
Early Bug Detection: Identify issues before integrating components.
Code Refactoring: Make changes confidently, knowing tests will catch regressions.
Documentation: Tests serve as examples of how the code is expected to behave.
Tools Used
NUnit: Test framework for writing and running tests.
Moq: Mocking library to simulate dependencies and isolate the unit under test.
FluentAssertions: Provides a readable syntax for writing test assertions.
## 📝 Example Tests
Here are some key tests included in the project:

### Test Case 1: Reject Underage Applicants
Input: Applicant age is below 18.
Expected Output: Application is automatically rejected.
Test Code:
```csharp

[Test]
public void Applicant_ShouldTransferredToAutoReject_WithUnderAge()
{
    var evaluator = new ApplicationEvaluator(null);
    var form = new JobApplication
    {
        Applicant = new Applicant { Age = 17 }
    };

    var result = evaluator.Evaluate(form);

    result.Should().Be(ApplicationResult.AutoReject);
}
```
### Test Case 2: Auto-Accept with Experience and Tech Stack
Input: Valid identity, matching tech stack, and sufficient experience.
Expected Output: Application is automatically accepted.
Test Code:
```csharp

[Test]
public void Applicant_ShouldTransferredToAutoAccepted_WithStackListAndExperience()
{
    var mockValidator = new Mock<IIdentityValidator>();
    mockValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);
    mockValidator.Setup(v => v.CountryProvider.CountryData.Country).Returns("TURKEY");
    var evaluator = new ApplicationEvaluator(mockValidator.Object);

    var form = new JobApplication
    {
        Applicant = new Applicant { Age = 30, IdentyNumber = "123" },
        TechStackList = new List<string> { "C#", "RabbitMq", "Ms Sql" },
        YearsOfExperience = 16,
        OfficeLocation = "ISTANBUL"
    };

    var result = evaluator.Evaluate(form);

    result.Should().Be(ApplicationResult.AutoAccept);
}
```

### 📚 Key Learnings
Dependency Injection: ApplicationEvaluator depends on IIdentityValidator, which is mocked during testing.
Mocking: Using Moq, external dependencies like IIdentityValidator are simulated for isolated testing.
Assertions: Using FluentAssertions for better readability and clarity in tests.
Validation Modes: Conditional business logic with ValidationMode based on applicant's age.
### 🛠️ Future Enhancements
Add integration tests to test the entire workflow.
Include validation for additional applicant details.
Enhance the tech stack comparison logic with weighted matching.
## 🤝 Contributing
Feel free to fork the repository, submit pull requests, or open issues to suggest improvements.

## 📜 License
This project is licensed under the MIT License - see the LICENSE file for details.














