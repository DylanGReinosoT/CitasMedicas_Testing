using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitasMedicas.AppDbContext;
namespace CitasMedicas.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CitasController : ControllerBase
	{
		private readonly AppDbContext _context;

		public CitasController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Cita>>> GetCitas()
		{
			return await _context.Citas.Include(c => c.Paciente)
									   .Include(c => c.Doctor)
									   .ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Cita>> GetCita(int id)
		{
			var cita = await _context.Citas.Include(c => c.Paciente)
										   .Include(c => c.Doctor)
										   .FirstOrDefaultAsync(c => c.id_cita == id);
			if (cita == null) return NotFound();
			return cita;
		}

		[HttpPost]
		public async Task<ActionResult<Cita>> PostCita([FromBody] Cita cita)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Validamos que el doctor y el paciente existen antes de agregar la cita
			var doctor = await _context.Doctores.FindAsync(cita.id_doctor);
			var paciente = await _context.Pacientes.FindAsync(cita.id_paciente);

			if (doctor == null || paciente == null)
			{
				return BadRequest("Doctor o Paciente no existentes.");
			}

			DateTime fechaDeHoy = DateTime.Today;

			// Verificar si la fecha de la cita es válida
			if (cita.fecha_hora < fechaDeHoy)
			{
				return BadRequest("Fecha inválida.");
			}

			// Verificar si ya existe una cita en la misma fecha y hora para el doctor
			bool citaExistente = await _context.Citas
											 .AnyAsync(c => c.fecha_hora == cita.fecha_hora && c.id_doctor == cita.id_doctor);

			if (citaExistente)
			{
				return Conflict("Ya existe una cita en esa fecha y hora para el doctor.");
			}

			_context.Citas.Add(cita);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetCita), new { id = cita.id_cita }, cita);
		}



		[HttpPut("{id}")]
		public async Task<IActionResult> PutCita(int id, [FromBody] Cita cita)
		{
			if (id != cita.id_cita)
			{
				return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Entry(cita).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CitaExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		private bool CitaExists(int id)
		{
			return _context.Citas.Any(e => e.id_cita == id);
		}




		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCita(int id)
		{
			var cita = await _context.Citas.FindAsync(id);
			if (cita == null) return NotFound();
			_context.Citas.Remove(cita);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
