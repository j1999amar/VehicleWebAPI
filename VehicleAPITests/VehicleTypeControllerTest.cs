using AutoFixture;
using AutoMapper;
using DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VehicleAPI.Controllers;
using VehicleInterface.Interface;
using VehicleTypeModel;
using Xunit;

namespace VehicleAPITests
{
    public class VehicleTypeControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IVehicleInterface> _serviceMock;
        private readonly IMapper _mapper;
        private readonly VehicleController _controller;

        public VehicleTypeControllerTest()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IVehicleInterface>>();
            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _mapper = mapConfig.CreateMapper();
            _controller = new VehicleController(_serviceMock.Object, _mapper);
        }


        [Fact]
        public void GetAllVehicleTypes_ShouldReturnOkResponse_WhenDataFound()
        {

            //Arange
            var vehicleDTOMock = _fixture.Create<ICollection<VehicleTypeDTO>>();
            var vehicleMock = _mapper.Map<ICollection<VehicleTypes>>(vehicleDTOMock);
            var service = _serviceMock.Setup(x => x.GetAllVehicleTypes()).Returns(vehicleMock);
            //Act
            var result = _controller.GetAllVehicleTypes();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo <ActionResult<ICollection<VehicleTypeDTO>>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(vehicleDTOMock.GetType());
            _serviceMock.Verify(x=>x.GetAllVehicleTypes(), Times.Once());

        }


        [Fact]
        public void GetAllVehicleTypes_ShouldReturnNotFound_WhenDataNotFound()
        {

            //Arange
            ICollection<VehicleTypes> response = null;
            _serviceMock.Setup(x => x.GetAllVehicleTypes()).Returns(response);
            //Act
            var result = _controller.GetAllVehicleTypes();
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<VehicleTypeDTO>>>();
            result.Result.Should().BeAssignableTo<NotFoundResult>();

        }
    }
}
