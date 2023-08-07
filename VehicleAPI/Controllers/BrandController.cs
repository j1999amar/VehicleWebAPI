using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Mvc;
using VehicleInterface.Interface;
using VehicleTypeModel;

namespace VehicleAPI.Controllers
{
    [ApiController]
    public class BrandController : Controller
    {
        private readonly IBrandInterface _brand;
        private readonly IVehicleInterface _vehicle;
        private readonly IMapper _mapper;

        public BrandController(IBrandInterface brandInterface,IVehicleInterface vehicleInterface, IMapper mapper)
        {
            _brand = brandInterface;
            _vehicle = vehicleInterface;
            _mapper = mapper;
        }

        #region Get All Brands Of A Vehicle Type
        [HttpGet]
        [Route("[controller]/GetAllBrandsOfAVehicleType/{id}")]
        public ActionResult<ICollection<BrandDTO>> GetAllBrandsOfAVehicleType ( [FromRoute] int id)
        {
            try
            {
                if (_vehicle.IsExists(id))
                {
                    var brandsByVehicleType = _mapper.Map<ICollection<BrandDTO>>(_brand.GetAllBrandsOfAVehicleType(id));
                    if(brandsByVehicleType.Count>0)
                    {
                        return Ok(brandsByVehicleType);

                    }
                    else
                    {
                        return BadRequest("Data Not Found");
                    }
                }
                else
                {
                    return BadRequest("Id not found");
                }
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Brand Post Method
        [HttpPost]
        [Route("[controller]/AddBrand")]
        public async Task<ActionResult<BrandDTO>> AddBrand([FromBody] BrandDTO brandDTO)
        {
            try
            {
               
                if (brandDTO == null)
                {
                    return BadRequest();

                }
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                else
                {
                   if(_vehicle.IsExists(brandDTO.VehicleTypeId))
                    {
                        var brand = _mapper.Map<Brands>(brandDTO);
                        var brandIsAdded = await _brand.AddBrand(brand);
                        return Ok(brandDTO);
                    }
                    else
                    {
                        return BadRequest("Vehicle Type Id Not Exsits");
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        #endregion

        #region Brand Delete Method
        [HttpDelete]
        [Route("[controller]/DeleteBrand/{id}")]
        public ActionResult DeleteBrand([FromRoute]int id)
        {
            if(_brand.IsExists(id))
            {
                _brand.DeleteBrand(id);
                return Ok("Deleted");
            }
            else
            {
                return BadRequest("Something Went Wrong");
            }
        }
        #endregion

    }
}
