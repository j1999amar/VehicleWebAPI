using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Mvc;
using VehicleInterface.Interface;
using VehicleTypeModel;

namespace VehicleAPI.Controllers
{
    [ApiController]
    public class VehicleController : Controller
    {
        private readonly IVehicleInterface _vehicle;
        private readonly IMapper _mapper;

        public VehicleController(IVehicleInterface vehicleInterface, IMapper mapper)
        {
            _vehicle = vehicleInterface;
            _mapper = mapper;
        }

        #region Vehicle Post Method
        [HttpPost]
        [Route("[controller]/AddVehicleType")]
        public async Task<ActionResult<VehicleTypeDTO>> AddVehicleType([FromBody] VehicleTypeDTO vehicleTypeDTO)
        {
            try
            {
                if (vehicleTypeDTO == null)
                {
                    return BadRequest();

                }
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                else
                {
                    var vehiclType = _mapper.Map<VehicleTypes>(vehicleTypeDTO);
                    var vehicleTypeIsAdded = await _vehicle.AddVehicleType(vehiclType);
                    return Ok(vehicleTypeDTO);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        #endregion

        #region Vehicle Put Method

        [HttpPut]
        [Route("[controller]/UpdateVehicleType/{id}")]
        public async Task<ActionResult<VehicleTypeDTO>> UpdateVehicleType([FromRoute] int id, [FromBody] VehicleTypeDTO vehicleTypeDTO)
        {
            try
            {
                if (id != vehicleTypeDTO.VehicleTypeId)
                {
                    return BadRequest("Id not match with update id");
                }
                if (_vehicle.IsExists(id))
                {
                    var vehicleType = _mapper.Map<VehicleTypes>(vehicleTypeDTO);
                    await _vehicle.UpdateVehicleType(id, vehicleType);
                    return Ok(vehicleTypeDTO);
                }
                return BadRequest("Id not found");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region Get All Vehicle Type
        [HttpGet]
        [Route("[controller]/GetAllVehicleTypes")]
        public   ActionResult<ICollection<VehicleTypeDTO>> GetAllVehicleTypes()
        {
            try
            {
                var vehicleTypes = _mapper.Map<ICollection<VehicleTypeDTO>>(_vehicle.GetAllVehicleTypes());

                if (vehicleTypes.Count==0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(vehicleTypes);

                }
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);            
            }
        }

        #endregion
    }

}
