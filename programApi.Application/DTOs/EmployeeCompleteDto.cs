namespace programApi.Application.DTOs;

public record EmployeeCompleteDto(
    DateTime BirthDate,
    string Address,
    string Position,
    decimal Salary,
    DateTime HireDate,
    string EducationLevel,
    string ProfessionalProfile,
    int DepartmentId);
    
public record EmployeeUpdateDto(string Phone, string Address, string ProfessionalProfile);