-add mysql support to each project-
dotnet add package Pomelo.EntityFrameworkCore.MySql -v 2.2.0

-make migrations-
dotnet ef migrations add YourMigrationName

-apply migrations to database-
dotnet ef database update