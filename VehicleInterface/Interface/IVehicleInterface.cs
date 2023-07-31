using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTypeModel;

namespace VehicleInterface.Interface
{
    public interface IVehicleInterface
    {
        public   Task<VehicleTypes> AddVehicleType(VehicleTypes vehicleType);
        public ICollection<VehicleTypes> GetAllVehicleTypes();
        public Task<VehicleTypes> UpdateVehicleType(int id,VehicleTypes vehicleType);
        public bool IsExists(int id);
    }
}
