using Rest_Api.Models;
using Rest_Api.Models.DTOs.Requests;
using Rest_Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITestRepository _testRepository;
        private readonly _2019SBDContext _context;
        public TripsController(ITestRepository testRepository, _2019SBDContext context)
        {
            _testRepository = testRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTripsInfo()
        {

            //return await _testRepositry.GetTripsAsync() != null ? Ok(await _testRepositry.GetTripsAsync()) : NotFound("There is no registered trips.");

            if (await _testRepository.GetTripsWithClientsAndCountriesAsync() == null)
                return NotFound("No trips in DB.");
            return Ok(await _testRepository.GetTripsWithClientsAndCountriesAsync());
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip([FromRoute] string idTrip, [FromBody] AssigningClientStructureRequestDto dataStructure)
        {
           // if (! await _testRepository.CheckIfClientWithPersonalIdNumExistsAsync(dataStructure.Pesel))
           //     return BadRequest("No client with such personal identity number");

            if (await _testRepository.CheckIfClientAlreadyAssignedToTripAsync(idTrip,dataStructure.Pesel))
                return BadRequest("Client already assigned to the trip");

            if (!await _testRepository.CheckIfTripExistsAsync(idTrip))
                return BadRequest("No trip with such Id");

            await _testRepository.AssignClientToTripAsync(idTrip,dataStructure);
            return Ok();

        }
    }
}
