using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class ModelRepository : IModelInterface
    {
        private readonly ApplicationDbContext _context;

        public ModelRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Models> AddModelForBrand(Models model)
        {
            await _context.Models.AddAsync(model);
            var modelAdded=await _context.SaveChangesAsync();
            return modelAdded>0?model:null;
        }

        public async Task<ICollection<Models>> GetAllModelByBrand(int id)
        {
            var modelByBrandList = await _context.Models.Where(model => model.BrandId == id).ToListAsync();

            return modelByBrandList;
        }

        public bool IsExists(int id)
        {
            return _context.Models.Any(Model => Model.BrandId == id);
        }
    }
}
