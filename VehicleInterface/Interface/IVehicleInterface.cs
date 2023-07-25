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
        public   Task<VehicleType> AddVehicleType(VehicleType vehicleType);
        public Task<ICollection<VehicleType>> GetAllVehicleTypes();
        public Task<VehicleType> UpdateVehicleType(Guid id,VehicleType vehicleType);
        public bool IsExists(Guid id);
    }
}
