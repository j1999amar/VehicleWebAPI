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
        [HttpPost]
        [Route("[controller]/AddVehicleType")]
        public async Task<ActionResult<VehicleTypeDTO>> AddVehicleType(VehicleTypeDTO vehicleTypeDTO)
        {
            try
            {
                var vehiclType = _mapper.Map<VehicleType>(vehicleTypeDTO);
                var vehicleTypeIsAdded=_vehicle.AddVehicleType(vehiclType);
                if(vehicleTypeIsAdded != null)
                {
                    return Ok(vehicleTypeDTO);
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
    }
}
