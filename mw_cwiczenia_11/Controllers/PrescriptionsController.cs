using Microsoft.AspNetCore.Mvc;
using mw_cwiczenia_11.DTOs;
using mw_cwiczenia_11.Exceptions;
using mw_cwiczenia_11.Services;

namespace mw_cwiczenia_11.Controllers;


[Route("api/[controller]")]
[ApiController]
public class PrescriptionsController : ControllerBase
{
    private readonly IDbService _dbService;

    public PrescriptionsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription(CreatePrescriptionRequestDto request)
    {
        try
        {
            await _dbService.AddPrescriptionAsync(request);
            return Created();
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal Server Error occured");
        }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetPatientPrescriptions(int id)
    {
        try
        {
            var patientPrescriptions = await _dbService.GetPatientPrescriptionsAsync(id);
            return Ok(patientPrescriptions);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal Server Error occured");
        }
    }
    
    
    
}