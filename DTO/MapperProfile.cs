using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTypeModel;

namespace DTO
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<VehicleTypes,VehicleTypeDTO>().ReverseMap();
            CreateMap<Brands, BrandDTO>().ReverseMap();
            CreateMap<Models, ModelDTO>().ReverseMap();


        }
    }
}
