using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Dto;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStrore.villaList);
        }
        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVillas(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStrore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO viLLADTO) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (VillaStrore.villaList.FirstOrDefault(u => u.Name.ToLower() == viLLADTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("customError", "Villay already exists!");
                return BadRequest(ModelState);
            }
            if (viLLADTO == null)
            {
                return BadRequest(viLLADTO);
            } if (viLLADTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            viLLADTO.Id = VillaStrore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStrore.villaList.Add(viLLADTO);
            return CreatedAtRoute("GetVilla", new { id = viLLADTO.Id }, viLLADTO);
        }
        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}", Name ="DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStrore.villaList.FirstOrDefault(u=>u.Id ==id);
            if(villa == null)
            {
                return NotFound();
            }
            VillaStrore.villaList.Remove(villa);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}", Name = "UpdateVilla")]
        public IActionResult updateVilla(int id, [FromBody] VillaDTO viLLADTO)
        {

            if (viLLADTO == null || id!= viLLADTO.Id )
            {
                return BadRequest();
            }
            var villa = VillaStrore.villaList.FirstOrDefault(u => u.Id == id);
            villa.Name= viLLADTO.Name;
            villa.Sqft = viLLADTO.Sqft;
            villa.Occupancy = viLLADTO.Occupancy;
            return NoContent();

        }
        
        //[HttpPut("id", Name = "UpdatePartialVilla")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        //{

        //    if (patchDTO == null || id == 0)
        //    {
        //        return BadRequest();
        //    }
        //    var villa = VillaStrore.villaList.FirstOrDefault(u => u.Id == id);
        //    if(villa == null)
        //    {
        //        return BadRequest();
        //    }
        //    patchDTO.ApplyTo(villa, ModelState);
        //    if(!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    return NoContent() ;
        //}

    }
    }
