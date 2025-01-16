using CitasMedicas;
using CitasMedicas.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static CitasMedicas.AppDbContext;

namespace TestProjectPacientes
{
	public class PacientesControllerTest
	{
		/* Prueba de creación de paciente */
		[Fact]
		public async Task CreatePaciente_ShouldReturnPaciente()
		{
			// Arrange
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			using var context = new AppDbContext(options);
			var controller = new PacientesController(context);
			var paciente = new Paciente
			{
				nombre = "Juan",
				apellido = "Pérez",
				fecha_nacimiento = new System.DateTime(1985, 1, 15),
				telefono = "555-1234",
				direccion = "Calle Falsa 123",
				correo = "juan.perez@example.com"
			};

			// Act
			var result = await controller.PostPaciente(paciente);

			// Assert
			var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
			var returnValue = Assert.IsType<Paciente>(createdAtActionResult.Value);
			Assert.Equal("Juan", returnValue.nombre);
		}

		/* Prueba de actualización de paciente */
		[Fact]
		public async Task UpdatePaciente_ShouldReturnNoContent()
		{
			// Arrange
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			using var context = new AppDbContext(options);
			var controller = new PacientesController(context);

			var paciente = new Paciente
			{
				id_paciente = 1,
				nombre = "Maria",
				apellido = "González",
				fecha_nacimiento = new System.DateTime(1990, 3, 22),
				telefono = "555-5678",
				direccion = "Avenida Siempre Viva 456",
				correo = "maria.gonzalez@example.com"
			};
			context.Pacientes.Add(paciente);
			context.SaveChanges();

			var updatedPaciente = new Paciente
			{
				id_paciente = 1,
				nombre = "Maria Actualizada",
				apellido = "González",
				fecha_nacimiento = new System.DateTime(1990, 3, 22),
				telefono = "555-5678",
				direccion = "Avenida Siempre Viva 456",
				correo = "maria.gonzalez@example.com"
			};

			// Act
			var result = await controller.PutPaciente(1, updatedPaciente);

			// Assert
			var noContentResult = Assert.IsType<NoContentResult>(result);
		}

		/* Prueba de eliminación de paciente */
		[Fact]
		public async Task DeletePaciente_ShouldReturnNoContent()
		{
			// Arrange
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			using var context = new AppDbContext(options);
			var controller = new PacientesController(context);

			var paciente = new Paciente
			{
				id_paciente = 1,
				nombre = "Carlos",
				apellido = "Ramírez",
				fecha_nacimiento = new System.DateTime(1978, 7, 30),
				telefono = "555-9012",
				direccion = "Boulevard Central 789",
				correo = "carlos.ramirez@example.com"
			};
			context.Pacientes.Add(paciente);
			context.SaveChanges();

			// Act
			var result = await controller.DeletePaciente(1);

			// Assert
			var noContentResult = Assert.IsType<NoContentResult>(result);
		}
	}
}
