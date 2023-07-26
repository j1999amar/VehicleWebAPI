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
        #region Get All Brands
        [HttpGet]
        [Route("[controller]/GetAllBrands")]
        public ActionResult<ICollection<BrandDTO>> GetAllVehicleTypes()
        {
            try
            {
                var brands = _mapper.Map<ICollection<BrandDTO>>(_brand.GetAllBrands());

                return Ok(brands);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get All Brands Of A Vehicle Type
        [HttpGet]
        [Route("[controller]/GetAllBrandsOfAVehicleType/{id}")]
        public ActionResult<ICollection<BrandDTO>> GetAllBrandsOfAVehicleType ( [FromRoute] Guid id)
        {
            if (_vehicle.IsExists(id))
            {
                var brandsByVehicleType =  _mapper.Map<ICollection<BrandDTO>>(_brand.GetAllBrandsOfAVehicleType(id));
                return Ok(brandsByVehicleType);
            }
            else
            {
                return BadRequest("Id not found");
            }
        }
        #endregion

        #region Brand Post Method
        [HttpPost]
        [Route("[controller]/AddBrand")]
        public async Task<ActionResult<BrandDTO>> AddVehicleType([FromBody] BrandDTO brandDTO)
        {
            try
            {
                var brand =  _mapper.Map<Brands>(brandDTO);
                var brandIsAdded = await _brand.AddBrand(brand);
                if (brandIsAdded != null)
                {
                    return Ok(brandDTO);
                }
                else
                {
                    return BadRequest(null);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        #endregion

        #region Brand Put Method
        [HttpPut]
        [Route("[controller]/UpdateBrand/{id}")]
        public async Task<ActionResult<BrandDTO>> UpdateBrand([FromRoute] Guid id,[FromBody] BrandDTO brandDTO)
        {
            try
            {
                if (id != brandDTO.BrandId)
                {
                    return BadRequest("Id not match");
                }
                if (_brand.IsExists(id)&&_vehicle.IsExists(brandDTO.VehicleTypeId))
                {
                    var brand = _mapper.Map<Brands>(brandDTO);
                    await _brand.UpdateBrands(id, brand);
                    return Ok(brandDTO);
                }
                else
                {
                    return BadRequest("Id not exists");
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
        public ActionResult DeleteBrand([FromRoute]Guid id)
        {
            if(_brand.IsExists(id))
            {
                _brand.DeleteBrand(id);
                return Ok("Deleted");
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }
        #endregion

    }
}
