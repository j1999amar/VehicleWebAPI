using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VehicleTypeModel
{

    [Table("vehicletype")]

    public class VehicleTypes
    {
        public int VehicleTypeId { get; set; }

        public string VehicleType { get; set; }

        public string Description { get; set; }

        public bool? IsActive { get; set; }

        public ICollection<Brands> Brands { get; set; }



    }
}