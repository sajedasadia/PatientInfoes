using Microsoft.AspNetCore.Mvc;
using PatientInfoes.Models;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
	private readonly IPatientRepository _repository;

	public PatientsController(IPatientRepository repository)
	{
		_repository = repository;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var patients = await _repository.GetAllPatients();
		return Ok(patients);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(int id)
	{
		var patient = await _repository.GetPatientById(id);
		if (patient == null) return NotFound();
		return Ok(patient);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] Patient patient)
	{
		await _repository.AddPatient(patient);
		return CreatedAtAction(nameof(Get), new { id = patient.PatientId }, patient);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(int id, [FromBody] Patient patient)
	{
		patient.PatientId = id;
		await _repository.UpdatePatient(patient);
		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _repository.DeletePatient(id);
		return NoContent();
	}
}