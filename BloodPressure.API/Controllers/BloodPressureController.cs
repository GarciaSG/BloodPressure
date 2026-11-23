using BloodPressureApp.API.Services;
using BloodPressureApp.Core.DTOs;
using BloodPressureApp.Core.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BloodPressureApp.API.Controllers;

[ApiController]
[Route("api/bloodpressure")]
public class BloodPressureController : ControllerBase
{

    private readonly BloodPressureService _service;
    private readonly BloodPressureCreateValidator _createValidator;
    private readonly BloodPressureUpdateValidator _updateValidator;

    public BloodPressureController(
        BloodPressureService service,
        BloodPressureCreateValidator createValidator,
        BloodPressureUpdateValidator updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    private int GetUserId()
    {
        var idClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        int.TryParse(idClaim?.Value, out int userId);

        return userId;

    }

    [HttpGet]
    public async Task<ActionResult<List<BloodPressureDTO>>> GetAll()
    {
        var data = await _service.GetAllAsync(GetUserId());
        return Ok(data);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BloodPressureDTO>> GetById(int id)
    {
        var data = await _service.GetByIdAsync(id, GetUserId());

        if (data == null)
        {
            return NotFound();
        }

        return Ok(data);
    }

    [HttpPost]
    public async Task<ActionResult<BloodPressureDTO>> Create(BloodPressureCreateDTO dto)
    {

        var validation = await _createValidator.ValidateAsync(dto);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
        }

        var nuevo = await _service.CreateAsync(dto, GetUserId());

        return Ok(nuevo);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BloodPressureDTO>> Update(int id, BloodPressureUpdateDTO dto)
    {

        var validation = await _updateValidator.ValidateAsync(dto);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var data = await _service.UpdateAsync(id, dto, GetUserId());

        if (data == null)
        {
            return NotFound();
        }

        return Ok(data);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var data = await _service.DeleteAsync(id, GetUserId());

        if (!data)
        {
            return NotFound();
        }

        return Ok();
    }
}
