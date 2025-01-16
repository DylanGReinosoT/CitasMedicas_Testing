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
	public class ProcedimientoController : ControllerBase 
	{
		private readonly AppDbContext _context;

		public ProcedimientoController(AppDbContext context)
		{
			_context = context;
		}

		// Obtener todos los procedimientos
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Procedimiento>>> GetProcedimientos()
		{
			return await _context.Procedimientos.Include(p => p.Cita).ToListAsync();
		}

		// Obtener un procedimiento específico por su ID
		[HttpGet("{id}")]
		public async Task<ActionResult<Procedimiento>> GetProcedimiento(int id)
		{
			var procedimiento = await _context.Procedimientos.Include(p => p.Cita)
															 .FirstOrDefaultAsync(p => p.id_procedimiento == id);
			if (procedimiento == null) return NotFound();
			return procedimiento;
		}

		// Crear un nuevo procedimiento
		[HttpPost]
		public async Task<ActionResult<Procedimiento>> PostProcedimiento(Procedimiento procedimiento)
		{
			_context.Procedimientos.Add(procedimiento);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetProcedimiento), new { id = procedimiento.id_procedimiento }, procedimiento);
		}

		// Actualizar un procedimiento existente
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProcedimiento(int id, Procedimiento procedimiento)
		{
			if (id != procedimiento.id_procedimiento) return BadRequest();
			_context.Entry(procedimiento).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return NoContent();
		}

		// Eliminar un procedimiento por su ID
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProcedimiento(int id)
		{
			var procedimiento = await _context.Procedimientos.FindAsync(id);
			if (procedimiento == null) return NotFound();
			_context.Procedimientos.Remove(procedimiento);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
