using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleAPI.Controllers;
using Xunit;

namespace VehicleAPITests.Controller
{
    public class VehicleControllerTests
      
    {
        private readonly VehicleController _vehicle;

        public VehicleControllerTests(VehicleController vehicleController)
        {
            _vehicle = vehicleController;
        }
        [Fact]
        public void getAllVehicleTypes_returnCollectionOfVehicleTypes()
        {
            var result=_vehicle.GetAllVehicleTypes();
            Assert.NotNull(result);
        }
    }
}
