using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTypeModel
{
    public class Brands
    {
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set;}

        public Guid VehicleTypeId { get; set; }

        public VehicleType VehicleType { get; set; }
    }
}
