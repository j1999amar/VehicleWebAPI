using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTypeModel;

namespace VehicleInterface.Interface
{
    public interface IBrandInterface
    {
        public Task<Brands> AddBrand(Brands brand);
        public ICollection<Brands> GetAllBrands();
        public Task<Brands> UpdateBrands(Guid id,Brands brand);

        public bool DeleteBrand(Guid id);
        public bool IsExists(Guid id);

        public ICollection<Brands> GetAllBrandsOfAVehicleType(Guid id);


    }
}
