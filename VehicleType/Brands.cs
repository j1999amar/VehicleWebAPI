using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTypeModel
{
    [Table("brand")]
    public class Brands
    {
        public int BrandId { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public int? SortOrder { get; set; }

        public bool? IsActive { get; set; }

        public int? VehicleTypeId { get; set; }
        public VehicleTypes VehicleType { get; set; }
    }
}
