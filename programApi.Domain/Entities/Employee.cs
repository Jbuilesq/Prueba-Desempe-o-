using System;
using programApi.Domain.Enums;

namespace programApi.Domain.Entities;

public class Employee
{
    // ---------- Identity ----------
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.User;

    // ---------- Excel data ----------
    public string Document { get; set; } = string.Empty;        // Documento
    public string FirstName { get; set; } = string.Empty;       // Nombres
    public string LastName { get; set; } = string.Empty;        // Apellidos
    public DateTime BirthDate { get; set; }                    // FechaNacimiento
    public string Address { get; set; } = string.Empty;         // Direccion
    public string Phone { get; set; } = string.Empty;           // Telefono
    public string Position { get; set; } = string.Empty;        // Cargo
    public decimal Salary { get; set; }                        // Salario
    public DateTime HireDate { get; set; }                     // FechaIngreso
    public string Status { get; set; } = "Active";              // Estado
    public string EducationLevel { get; set; } = string.Empty;  // NivelEducativo
    public string ProfessionalProfile { get; set; } = string.Empty; // PerfilProfesional

    // ---------- Navigation ----------
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    // ---------- Helper ----------
    public string FullName => $"{FirstName} {LastName}".Trim();
}