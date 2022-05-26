using Rest_Api.Models;
using Rest_Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {


        private readonly ITestRepository _testRepository;
        private readonly _2019SBDContext _context;
        public ClientsController(ITestRepository testRepository, _2019SBDContext context)
        {
            _testRepository = testRepository;
            _context = context;
        }

        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient([FromRoute] int idClient)
        {
            if (await _testRepository.CheckIfClientWithIdExistsAsync(idClient))
                return NotFound("No client with such id");
                
            if (await _testRepository.CheckClientsTripsAsync(idClient))
                return BadRequest("Client already assigned to a trip");
            else
            await _testRepository.DeleteClientAsync(idClient);

            return Ok();

        }
    } 
}
