namespace programApi.Application.DTOs;

public record EmployeeCreateDto(
    string Document,
    string FirstName,
    string LastName,
    DateTime BirthDate,
    string Address,
    string Phone,
    string Email,
    string Position,
    decimal Salary,
    DateTime HireDate,
    string Status,
    string EducationLevel,
    string ProfessionalProfile,
    int DepartmentId);