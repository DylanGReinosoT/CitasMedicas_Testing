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
	public class PacientesController : ControllerBase
	{
		private readonly AppDbContext _context;

		public PacientesController(AppDbContext context) { 
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientes()
		{
			return await _context.Pacientes.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Paciente>> GetPaciente(int id)
		{
			var paciente = await _context.Pacientes.FindAsync(id);
			if (paciente == null) return NotFound();
			return paciente;
		}

		[HttpPost]
		public async Task<ActionResult<Paciente>> PostPaciente([FromBody] Paciente paciente)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			DateTime fechaDeHoy = DateTime.Today;
			//Validad la fecha
			if (paciente.fecha_nacimiento > fechaDeHoy)
			{
				return BadRequest("Fecha invalida");
			}

			_context.Pacientes.Add(paciente);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetPaciente), new { id = paciente.id_paciente }, paciente);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> PutPaciente(int id, [FromBody] Paciente paciente)
		{
			if (id != paciente.id_paciente)
			{
				return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Entry(paciente).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PacienteExists(id))
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

		private bool PacienteExists(int id)
		{
			return _context.Pacientes.Any(e => e.id_paciente == id);
		}



		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePaciente(int id)
		{
			var paciente = await _context.Pacientes.FindAsync(id);
			if (paciente == null) return NotFound();
			_context.Pacientes.Remove(paciente);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
