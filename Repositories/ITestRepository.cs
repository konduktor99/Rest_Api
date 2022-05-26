using Rest_Api.Models.DTOs.Requests;
using Rest_Api.Models.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api.Repositories
{
    public interface ITestRepository
    {
        public  Task<IEnumerable<GetTripsWithClientsAndCountriesResponseDto>> GetTripsWithClientsAndCountriesAsync();
        public Task<bool> CheckClientsTripsAsync(int idClient);
        public Task DeleteClientAsync(int idClient);
        public Task<bool> CheckIfClientWithIdExistsAsync(int idClient);
        //public Task<bool> CheckIfClientWithPersonalIdNumExistsAsync(string persIdNum);
        public Task<bool> CheckIfClientAlreadyAssignedToTripAsync(string idTrip, string persIdNum);
        public Task<bool> CheckIfTripExistsAsync(string idTrip);
        public Task AssignClientToTripAsync(string idTrip, AssigningClientStructureRequestDto dataStructure);
    }
}
