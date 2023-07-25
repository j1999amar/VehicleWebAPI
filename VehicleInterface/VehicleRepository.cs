using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleContext;
using VehicleInterface.Interface;
using VehicleTypeModel;

namespace VehicleInterface
{
    public class VehicleRepository : IVehicleInterface
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<VehicleType> AddVehicleType(VehicleType vehicleType)
        {
            await _context.VehicleTypes.AddAsync(vehicleType);
            var vehicleIsAdded=await _context.SaveChangesAsync();
            return vehicleIsAdded > 0 ? vehicleType : null;
        }

        public async Task<VehicleType> UpdateVehicleType(Guid id, VehicleType vehicleType)
        {
            _context.VehicleTypes.Entry(vehicleType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return vehicleType;
        }

        public ICollection<VehicleType> GetAllVehicleTypes()
        {
            return _context.VehicleTypes.ToList();
             
        }

        public bool IsExists(Guid id)
        {
           return _context.VehicleTypes.Any(vehicleType => vehicleType.VehicleTypeId == id);
        }
    }
}
