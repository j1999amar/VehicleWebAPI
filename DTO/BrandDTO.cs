using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class BrandDTO
    {
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public Guid VehicleTypeId { get; set; }
    }
}
