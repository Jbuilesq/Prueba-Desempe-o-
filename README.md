Sistema de Gestión de Empleados – TalentoPlus S.A.S

This is an API project developed using ASP.NET following a Layered Architecture (DDD).
🚀 Quick Start
1. Clone the Repository

Open your terminal, navigate to your desired location, and clone the repository:
Bash

git clone https://aws.amazon.com/es/what-is/repo/
cd [Nombre de la Carpeta del Proyecto]

2. Database Setup (MySQL)

This project uses MySQL via Pomelo.EntityFrameworkCore.

IMPORTANT: Before running migrations, you must update the database credentials (user and password) in the DefaultConnection string inside the appsettings.json file located in the projCSharp.Api/ folder.
Execute Migrations

If you have changes to the data model, run these commands (ensure you have dotnet-ef installed globally):
Bash

# 1. Install dotnet EF (if not installed)
dotnet tool install --global dotnet-ef

# 2. Add a new migration (Example: NewMigrationName)
dotnet ef migrations add NewMigrationName --project projCSharp.Infrastructure/ --startup-project projCSharp.Api/

# 3. Apply changes to the database
dotnet ef database update --project projCSharp.Infrastructure/ --startup-project projCSharp.Api/

3. Run the Project

Execute the API using the following command:
Bash

dotnet run --project projCSharp.Api/

The API will start running, and the endpoints will be accessible, usually through Swagger at http://localhost:[Port]/swagger.
⚙️ Architecture and Security

    Architecture: The project uses a layered architecture based on Domain-Driven Design (DDD), clearly separating Domain, Infrastructure, Application, and Api concerns.

    Security: Access to protected endpoints is managed using JWT (JSON Web Tokens) according to assigned roles.

    Documentation: API documentation is provided by Swashbuckle.AspNetCore.

    UML Diagrams: You can find design diagrams (UML and Use Cases) in the Documentation/ folder.

📦 Key Packages

The main dependencies include AutoMapper, JWTBearer for authentication, BCrypt.Net-Next for password hashing, and Pomelo.EntityFrameworkCore.MySql for database access.
🔗 Repository Link

Present by Juan David Builes Quiroz

https://github.com/Jbuilesq/Prueba-Desempe-o-