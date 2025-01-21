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
	public class DoctorController: ControllerBase
	{
		private readonly AppDbContext _context;

		public DoctorController(AppDbContext context) { 
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctores()
		{
			return await _context.Doctores.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Doctor>> GetDoctor(int id)
		{
			var doctor = await _context.Doctores.FindAsync(id);
			if (doctor == null) return NotFound();
			return doctor;
		}

		[HttpPost]
		public async Task<ActionResult<Doctor>> PostDoctor([FromBody] Doctor doctor)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Doctores.Add(doctor);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetDoctor), new { id = doctor.id_doctor }, doctor);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutDoctor(int id, [FromBody] Doctor doctor)
		{
			if (id != doctor.id_doctor)
			{
				return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Entry(doctor).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!DoctorExists(id))
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

		private bool DoctorExists(int id)
		{
			return _context.Doctores.Any(e => e.id_doctor == id);
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteDoctor(int id)
		{
			var doctor = await _context.Doctores.FindAsync(id);
			if (doctor == null) return NotFound();
			_context.Doctores.Remove(doctor);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
