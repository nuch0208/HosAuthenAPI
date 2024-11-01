using Microsoft.AspNetCore.Mvc;
using HosAuthenAPI.Services;

namespace HosAuthenAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class GetSerialController : ControllerBase
{
    private readonly GetSerial _service;

    public GetSerialController(GetSerial service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<string>> GetSerialNumber()
    {
        var serialNumber = await _service.GetSerialNumber();
        if (serialNumber == null)
        {
            return NotFound();
        }
        return Ok(serialNumber);
    }

    [HttpGet("get-serial-number")]
    public async Task<ActionResult<string>> GetSerialNumberByParam(string param)
    {
        var serialNumber = await _service.GetSerialNumberByParam(param);
        if (serialNumber == null)
        {
            return NotFound();
        }
        return Ok(serialNumber);
    }
}