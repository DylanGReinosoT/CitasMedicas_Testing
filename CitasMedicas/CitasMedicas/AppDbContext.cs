using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
namespace CitasMedicas
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		public DbSet<Paciente> Pacientes { get; set; }
		public DbSet<Doctor> Doctores { get; set; }
		public DbSet<Cita> Citas { get; set; }
		public DbSet<Procedimiento> Procedimientos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Cita>()
				.HasOne(p => p.Paciente)
				.WithMany(c => c.Citas)
				.HasForeignKey(p => p.id_paciente);

			modelBuilder.Entity<Cita>()
				.HasOne(d => d.Doctor)
				.WithMany(c => c.Citas)
				.HasForeignKey(d => d.id_doctor);

			modelBuilder.Entity<Procedimiento>()
				.HasOne(c => c.Cita)
				.WithMany(p => p.Procedimientos)
				.HasForeignKey(c => c.id_cita)
				.OnDelete(DeleteBehavior.Cascade);
		}

		public class Paciente
		{
			[Key]
			public int? id_paciente { get; set; }

			[Required]
			public string? nombre { get; set; }

			[Required]
			public string? apellido { get; set; }

			[Required]
			public DateTime? fecha_nacimiento { get; set; }

			public string? telefono { get; set; }
			public string? direccion { get; set; }

			[EmailAddress]
			public string? correo { get; set; }

			// Inicializa la colección de Citas para evitar problemas de validación
			public ICollection<Cita> Citas { get; set; } = new List<Cita>();
		}


		public class Doctor
		{
			[Key]
			public int? id_doctor { get; set; }

			[Required]
			public string? nombre { get; set; }

			[Required]
			public string? apellido { get; set; }

			[Required]
			public string? especialidad { get; set; }

			public string? telefono { get; set; }
			public string? correo { get; set; }

			// Inicializa la colección de Citas para evitar problemas de validación
			public ICollection<Cita> Citas { get; set; } = new List<Cita>();
		}

		public class Cita
		{
			[Key]
	
			public int id_cita { get; set; }

			[Required]
			public int id_paciente { get; set; }

			[Required]
			public int id_doctor { get; set; }

			[Required]
			public DateTime? fecha_hora { get; set; }

			public string? estado { get; set; } // "Pendiente", "Confirmada", "Cancelada", "Realizada"
			public string? descripcion { get; set; }

			// No hacemos las propiedades de navegación requeridas
			public virtual Paciente? Paciente { get; set; }
			public virtual Doctor? Doctor { get; set; }
			public virtual ICollection<Procedimiento>? Procedimientos { get; set; }
		}



		public class Procedimiento
		{
			[Key]
			[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
			public int id_procedimiento { get; set; }

			[Required]
			public string? descripcion { get; set; }

			[Required]
			public decimal costo { get; set; }

			[Required]
			public int id_cita { get; set; }

			// No hacemos la propiedad de navegación requerida
			public virtual Cita? Cita { get; set; }
		}

	}
}

