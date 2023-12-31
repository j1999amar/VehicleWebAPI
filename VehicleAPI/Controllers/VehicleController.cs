﻿using AutoMapper;
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
        public async Task<ActionResult<VehicleTypeDTO>> AddVehicleType(VehicleTypeDTO vehicleTypeDTO)
        {
            try
            {
                var vehiclType =  _mapper.Map<VehicleType>(vehicleTypeDTO);
                var vehicleTypeIsAdded = await _vehicle.AddVehicleType(vehiclType);
                if (vehicleTypeIsAdded != null)
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

        #endregion

        #region Vehicle Put Method

        [HttpPut]
        [Route("[controller]/UpdateVehicleType")]
        public async Task<ActionResult<VehicleTypeDTO>> UpdateVehicleType(Guid id, VehicleTypeDTO vehicleTypeDTO)
        {
            try
            {
                if (id != vehicleTypeDTO.VehicleTypeId)
                {
                    return BadRequest("Id not match");
                }
                if (_vehicle.IsExists(id))
                {
                    var vehicleType = _mapper.Map<VehicleType>(vehicleTypeDTO);
                    await _vehicle.UpdateVehicleType(id, vehicleType);
                    return Ok(vehicleTypeDTO);
                }
                return null;
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
        public  ActionResult<ICollection<VehicleTypeDTO>> GetAllVehicleTypes()
        {
            try
            {
                var vehicleTypes = _mapper.Map<ICollection<VehicleTypeDTO>>(_vehicle.GetAllVehicleTypes());

                return Ok(vehicleTypes);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);            
            }
        }

        #endregion
    }

}
