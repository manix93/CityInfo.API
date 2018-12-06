using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PonitsOfInterestController : Controller
    {
        [HttpGet("{cityId}/poi")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var cityToReturn = CitiesDataStore.Current.Cities.SingleOrDefault(c => c.Id == cityId);

            if (cityToReturn == null)
                return NotFound();

            else
                return Ok(cityToReturn.PointOfInterests);
        }

        [HttpGet("{cityId}/poi/{id}")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var cityToReturn = CitiesDataStore.Current.Cities.SingleOrDefault(c => c.Id == cityId);

            if (cityToReturn == null)
                return NotFound();

            var poiToReturn = cityToReturn.PointOfInterests.FirstOrDefault(p => p.Id == id);

            if (poiToReturn == null)
                return NotFound();

            return Ok(poiToReturn);


        }
    }
}
