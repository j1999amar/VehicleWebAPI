using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Mvc;
using VehicleInterface.Interface;
using VehicleTypeModel;

namespace VehicleAPI.Controllers
{
    [ApiController]
    public class ModelController : Controller
    {
        private readonly IModelInterface _model;
        private readonly IBrandInterface _brand;
        private readonly IMapper _mapper;


        public ModelController(IModelInterface model,IBrandInterface brand ,IMapper mapper)
        {
            _model = model;
            _brand = brand;
            _mapper = mapper;
        }
        #region Model Post 
        [HttpPost]
        [Route("[controller]/addModelForBrand")]
        public async Task<ActionResult<ModelDTO>> AddModelForBrand([FromBody] ModelDTO modelDTO)
        {
            try
            {
                if (modelDTO == null)
                {
                    return BadRequest();

                }
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                else
                {
                    var model = _mapper.Map<Models>(modelDTO);
                    if (_brand.IsExists(modelDTO.BrandId))
                    {
                        var modelIsAdded = await _model.AddModelForBrand(model);                        
                         return Ok(modelDTO);
                    }
                    else
                    {
                        return BadRequest("Brand Id not exists");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        #endregion

        #region Get All Model By Brand

        [HttpGet]
        [Route("[controller]/GetAllModelByBrand/{id}")]
        public async Task<ActionResult<ICollection<ModelDTO>>> GetAllModelByBrand([FromRoute]int id)
        {
            try
            {
                if (_brand.IsExists(id))
                {
                    var getAllModelByBrandList = await _model.GetAllModelByBrand(id);
                    var getAllModelByBrandDTOList = _mapper.Map<ICollection<ModelDTO>>(getAllModelByBrandList);
                    return Ok(getAllModelByBrandDTOList);
                }
                else
                {
                    return BadRequest("Brand Id is not exists");
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        #endregion

    }
}
