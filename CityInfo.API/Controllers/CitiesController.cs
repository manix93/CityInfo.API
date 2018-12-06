using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        [HttpGet]
        public IActionResult GetCities() => Json(CitiesDataStore.Current.Cities);

        [HttpGet("{id}")]
        public IActionResult GetCity(int id) => Json(CitiesDataStore.Current.Cities.SingleOrDefault(c => c.Id == id));
    }
}
