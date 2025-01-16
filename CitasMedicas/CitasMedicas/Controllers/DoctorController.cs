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
		public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
		{
			_context.Doctores.Add(doctor);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetDoctor), new { id = doctor.id_doctor }, doctor);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
		{
			if (id != doctor.id_doctor) return BadRequest();
			_context.Entry(doctor).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return NoContent();
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
