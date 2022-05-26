using Rest_Api.Models;
using Rest_Api.Models.DTOs.Requests;
using Rest_Api.Models.DTOs.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly _2019SBDContext _context;
        public TestRepository(_2019SBDContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<GetTripsWithClientsAndCountriesResponseDto>> GetTripsWithClientsAndCountriesAsync()
        {

            var output = await _context.Trips
                .Select(el=>new GetTripsWithClientsAndCountriesResponseDto
   
            {
                Name = el.Name,
                Description = el.Description,
                DateFrom = el.DateFrom,
                DateTo = el.DateTo,
                MaxPeople = el.MaxPeople,

                Countries = el.CountryTrips
                .Select(el => new { el.IdCountryNavigation.Name })
                .ToList(),
                
                Clients = el.ClientTrips
                .Select(el => new {firstName= el.IdClientNavigation.FirstName,lastName= el.IdClientNavigation.LastName })
                

            }).OrderByDescending(el => el.DateFrom).ToListAsync();
       


            return output;
        }
        public async Task<bool> CheckIfClientWithIdExistsAsync(int idClient)
        {
            return await _context.Clients.Where(e => e.IdClient == idClient).AnyAsync();
        }

        public async Task<bool> CheckClientsTripsAsync(int idClient)
        {
            return await _context.Clients.Where(el => el.ClientTrips.Any()).Where(el => el.IdClient == idClient).AnyAsync();
        }
        public async Task DeleteClientAsync(int idClient)
        {
             _context.Remove(await _context.Clients.SingleAsync(el => el.IdClient == idClient));
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfClientWithPersonalIdNumExistsAsync( string persIdNum)
        {
            return await _context.Clients.Where(e => e.Pesel.Equals(persIdNum)).AnyAsync();
        }

        public async Task<bool> CheckIfClientAlreadyAssignedToTripAsync(string idTrip, string persIdNum)
        {
            return await _context.Clients.Include(el=>el.ClientTrips)
                .Where(el => el.Pesel.Equals(persIdNum)).Where(el=>el.ClientTrips.Where(el=>el.IdTrip==int.Parse(idTrip)).Any())
                .AnyAsync();
               
        }

        public async Task<bool> CheckIfTripExistsAsync(string idTrip)
        {
            return await _context.Trips
                .Where(el => el.IdTrip==int.Parse(idTrip))
                .AnyAsync();
        }

        public async Task AssignClientToTripAsync(string idTrip, AssigningClientStructureRequestDto dataStructure)
        {
            int newId;
            if (!await CheckIfClientWithPersonalIdNumExistsAsync(dataStructure.Pesel))
            {
                newId = _context.Clients.Max(el => el.IdClient) + 1;
                await _context.Clients.AddAsync(new Client
                {
                    IdClient = newId,
                    FirstName = dataStructure.FirstName,
                    LastName = dataStructure.LastName,
                    Email = dataStructure.Email,
                    Telephone = dataStructure.Telephone,
                    Pesel = dataStructure.Pesel
                });
            } else
                newId =await _context.Clients.Where(el => el.Pesel.Equals(dataStructure.Pesel)).Select(el => el.IdClient).FirstAsync();

            await _context.ClientTrips.AddAsync(new ClientTrip
            {
                IdClient = newId,
                IdTrip = int.Parse(idTrip),
                RegisteredAt=DateTime.Now,
                PaymentDate=dataStructure.PaymentDate
            });
            await _context.SaveChangesAsync();
        }
    }
}
