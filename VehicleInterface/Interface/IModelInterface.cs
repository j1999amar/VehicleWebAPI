using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTypeModel;

namespace VehicleInterface.Interface
{
    public interface IModelInterface
    {
        public bool IsExists(int id);

        public Task<Models> AddModelForBrand(Models model);
        public Task<ICollection<Models>> GetAllModelByBrand(int id);


    }
}
