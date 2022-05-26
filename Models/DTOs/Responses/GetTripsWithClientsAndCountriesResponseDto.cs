using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api.Models.DTOs.Responses
{
    public class GetTripsWithClientsAndCountriesResponseDto
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public IEnumerable Countries { get; set; }
        public IEnumerable Clients { get; set; }
    }
}
