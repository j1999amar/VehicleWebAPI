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

        public bool DeleteBrand(int id);
        public bool IsExists(int id);

        public ICollection<Brands> GetAllBrandsOfAVehicleType(int id);


    }
}
