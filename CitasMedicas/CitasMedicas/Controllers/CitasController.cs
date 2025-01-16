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
		public async Task<ActionResult<Cita>> PostCita(Cita cita)
		{
			_context.Citas.Add(cita);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetCita), new { id = cita.id_cita }, cita);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCita(int id, Cita cita)
		{
			if (id != cita.id_cita) return BadRequest();
			_context.Entry(cita).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return NoContent();
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
