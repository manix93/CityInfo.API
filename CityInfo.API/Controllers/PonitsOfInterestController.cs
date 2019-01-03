using System.Linq;
using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{cityId}/poi/{id}", Name = "GetPointOfInterest")]
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

        [HttpPost("{cityId}/poi")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto poi)
        {
            if (poi == null)
            {
                return BadRequest();
            }

            if(poi.Description == poi.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }
      

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var currentMaxPoiId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointOfInterests).Max(p => p.Id);

            var newPoi = new PointOfInterestDto()
            {
                Id = ++currentMaxPoiId,
                Name = poi.Name,
                Description = poi.Description
            };

            city.PointOfInterests.Add(newPoi);

            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId = cityId,
                id = newPoi.Id
            },
            newPoi);
        }

        [HttpPut("{cityId}/poi/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForUpdateDto poi)
        {
            if (poi == null)
            {
                return BadRequest();
            }

            if (poi.Description == poi.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.SingleOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound();

            var pointOfInterestFromStore = city.PointOfInterests.FirstOrDefault(p => p.Id == id);

            if(pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = poi.Name;
            pointOfInterestFromStore.Description = poi.Description;

            return NoContent();
        }

        [HttpPatch("{cityId}/poi/{id}")]
        public IActionResult PartiallyUpdatePointOfInteresent(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if(city == null)
            {
                return NotFound();
            }

            var pointInterestFromStore = city.PointOfInterests.FirstOrDefault(c => c.Id == id);
            if(pointInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointInterestFromStore.Name,
                Description = pointInterestFromStore.Description
            };

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pointInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{cityId}/poi/{id}")]
        public IActionResult DeletePointOfInteresent(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointInterestFromStore = city.PointOfInterests.FirstOrDefault(c => c.Id == id);
            if (pointInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointOfInterests.Remove(pointInterestFromStore);

            return NoContent();
        }

    }
}