using System.Text.Json.Serialization;

namespace VehicleTypeModel
{
    public class VehicleType
    {
        public Guid VehicleTypeId { get; set; }
        public string  VehicleTypeName { get; set; }
        public string Description { get; set; }
        public ICollection<Brands> Brands { get; set; }



    }
}