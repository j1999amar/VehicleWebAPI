using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTypeModel
{
    [Table("model")]

    public class Models
    {
        public int ModelId { get; set; }
        public int BrandId { get; set; }
        public string? modelname { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public Brands Brands { get; set; }
    }
}
