using AutoFixture;
using AutoMapper;
using DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using VehicleAPI.Controllers;
using VehicleContext;
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
        private readonly VehicleController _sut;
        public VehicleTypeControllerTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _serviceMock = _fixture.Freeze<Mock<IVehicleInterface>>();
            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _mapper = mapConfig.CreateMapper();
            _sut = new VehicleController(_serviceMock.Object, _mapper);
        }

        #region GetAllVehicleTypes Test
        [Fact]
        public async void GetAllVehicleTypes_ShouldReturnOkResponse_WhenVehicleTypeListIsFound()
        {
            //Arange
            var vehicleDTOMock = _fixture.Create<ICollection<VehicleTypeDTO>>();
            var vehicleMock = _mapper.Map<ICollection<VehicleTypes>>(vehicleDTOMock);
            var service = _serviceMock.Setup(x => x.GetAllVehicleTypes()).Returns(vehicleMock);
            //Act
            var result =  _sut.GetAllVehicleTypes();
            var check = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo <ActionResult<ICollection<VehicleTypeDTO>>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _serviceMock.Verify(x=>x.GetAllVehicleTypes(), Times.Once());

        }
        
        [Fact]
        public void GetAllVehicleTypes_ShouldReturnNotFound_WhenVehicleTypeListIsEmpty()
        {

            //Arange
            ICollection<VehicleTypes> response = null;
            _serviceMock.Setup(x => x.GetAllVehicleTypes()).Returns(response);
            //Act
            var result = _sut.GetAllVehicleTypes();
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<VehicleTypeDTO>>>();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            _serviceMock.Verify(x => x.GetAllVehicleTypes(), Times.Once());

        }

        [Fact]
        public void GetAllVehicleTypes_ShouldReturnOkResponse_WhenBrandListHavingCountMoreThanZero()
        {
            //Arrang
            var vehicleTypeDTOList = _fixture.CreateMany<VehicleTypeDTO>(5).ToList();
            var vehicleTypeList = _mapper.Map<ICollection<VehicleTypes>>(vehicleTypeDTOList);
            _serviceMock.Setup(x => x.GetAllVehicleTypes()).Returns(vehicleTypeList);

            //Act
            var result = _sut.GetAllVehicleTypes();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<VehicleTypeDTO>>>();
            var getResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var getResultValue = getResult.Value as ICollection<VehicleTypeDTO>;
            getResultValue.Count.Should().Be(5);
            _serviceMock.Verify(x => x.GetAllVehicleTypes(), Times.Once());
        }

        [Fact]
        public void GetAllVehicleTypes_ShouldReturnNotFoundResponse_WhenBrandListHavingCountIsZero()
        {
            //Arrang
            var vehicleTypeList = _mapper.Map<ICollection<VehicleTypes>>(null);
            _serviceMock.Setup(x => x.GetAllVehicleTypes()).Returns(vehicleTypeList);

            //Act
            var result = _sut.GetAllVehicleTypes();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<VehicleTypeDTO>>>();
            var getResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            var getResultValue = getResult.Value as ICollection<VehicleTypeDTO>;
            _serviceMock.Verify(x => x.GetAllVehicleTypes(), Times.Once());
        }

        #endregion

        #region AddVehicleType Test
        [Fact]
        public async void AddVehicleType_ShouldReturnBadRequest_WhenVehicleTypeIdIsAlreadyExists()
        {
            //Arange
            var vehicleTypeDTOMock = _fixture.Create<VehicleTypeDTO>();
            var vehicleTypeMock = _mapper.Map<VehicleTypes>(vehicleTypeDTOMock);
            _serviceMock.Setup(x => x.AddVehicleType(vehicleTypeMock)).ReturnsAsync(vehicleTypeMock);
            _serviceMock.Setup(x => x.IsExists(vehicleTypeDTOMock.VehicleTypeId)).Returns(true);
            //Act
            var result = await _sut.AddVehicleType(vehicleTypeDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<VehicleTypeDTO>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _serviceMock.Verify(x => x.AddVehicleType(vehicleTypeMock), Times.Never());
        }
        [Fact]
        public async void AddVehicleType_ShouldReturnOkResponse_WhenVehicleTypeIdIsNotExists()
        {
            //Arange
            var vehicleTypeDTOMock = _fixture.Create<VehicleTypeDTO>();
            var vehicleTypeMock = _mapper.Map<VehicleTypes>(vehicleTypeDTOMock);
            _serviceMock.Setup(x => x.AddVehicleType(vehicleTypeMock)).ReturnsAsync(vehicleTypeMock);
            _serviceMock.Setup(x => x.IsExists(vehicleTypeDTOMock.VehicleTypeId)).Returns(false);
            //Act
            var result = await _sut.AddVehicleType(vehicleTypeDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<VehicleTypeDTO>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _serviceMock.Verify(x => x.AddVehicleType(vehicleTypeMock), Times.Never());
        }

       

        [Fact]
        public async  void AddVehicleType_ShouldReturnOkResponse_WhenVehicleTypeDataIsNotNull()
        {
            //Arange
            var vehicleTypeDTOMock=_fixture.Create<VehicleTypeDTO>();
            var vehicleTypeMock=_mapper.Map<VehicleTypes>(vehicleTypeDTOMock);
            _serviceMock.Setup(x =>  x.AddVehicleType(vehicleTypeMock)).ReturnsAsync(vehicleTypeMock);
            //Act
            var result = await _sut.AddVehicleType(vehicleTypeDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<VehicleTypeDTO>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _serviceMock.Verify(x => x.AddVehicleType(vehicleTypeMock), Times.Never());


        }

        [Fact]
        public async void AddVehicleType_ShouldReturnBadRequest_WhenVehicleTypeDataIsNull()
        {
            //Arange
            VehicleTypeDTO vehicleTypeDTOMock = null;
            var vehicleTypeMock = _mapper.Map<VehicleTypes>(vehicleTypeDTOMock);
            _serviceMock.Setup(x => x.AddVehicleType(vehicleTypeMock)).ReturnsAsync(vehicleTypeMock);
            //Act
            var result = await _sut.AddVehicleType(vehicleTypeDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<VehicleTypeDTO>>();
            result.Result.Should().BeAssignableTo<BadRequestResult>();
            _serviceMock.Verify(x => x.AddVehicleType(vehicleTypeMock), Times.Never());
        }
        #endregion

        #region Vehicle Update Test

        [Fact]
        public async void UpdateVehicleType_ShouldReturnOkResponse_WhenVehicleTypeIdAndDataIsVaild()
        {
            //Arrange
            var vehicleTypesDTOMock = _fixture.Create<VehicleTypeDTO>();
            var vehicleTypesMock=_mapper.Map<VehicleTypes>(vehicleTypesDTOMock);
            int id = vehicleTypesDTOMock.VehicleTypeId;
            _serviceMock.Setup(x=>x.UpdateVehicleType(id,vehicleTypesMock)).ReturnsAsync(vehicleTypesMock);
            _serviceMock.Setup(x => x.IsExists(id)).Returns(true);

            //Act
            var result =await _sut.UpdateVehicleType(id, vehicleTypesDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<VehicleTypeDTO>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _serviceMock.Verify(x => x.UpdateVehicleType(id, vehicleTypesMock), Times.Never());
            _serviceMock.Verify(x=>x.IsExists(id), Times.Once());

        }

        [Fact]
        public async void UpdateVehicleType_ShouldReturnBadRequestResponse_WhenVehicleTypeIdNotInDataBase()
        {
            //Arrange
            var vehicleTypesDTOMock = _fixture.Create<VehicleTypeDTO>();
            var vehicleTypesMock = _mapper.Map<VehicleTypes>(vehicleTypesDTOMock);
            int id=vehicleTypesDTOMock.VehicleTypeId;
            _serviceMock.Setup(x => x.UpdateVehicleType(id, vehicleTypesMock)).ReturnsAsync(vehicleTypesMock);
            _serviceMock.Setup(x => x.IsExists(vehicleTypesDTOMock.VehicleTypeId)).Returns(false);

            //Act
            var result = await _sut.UpdateVehicleType(id, vehicleTypesDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<VehicleTypeDTO>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _serviceMock.Verify(x => x.UpdateVehicleType(id, vehicleTypesMock), Times.Never());

        }


        [Fact]
        public async void UpdateVehicleType_ShouldReturnBadRequestResponse_WhenVehicleTypeIdIsNotMatchedWithUpadateData()
        {
            //Arrange
            var vehicleTypesDTOMock = _fixture.Create<VehicleTypeDTO>();
            var vehicleTypesMock = _mapper.Map<VehicleTypes>(vehicleTypesDTOMock);
            int id=vehicleTypesDTOMock.VehicleTypeId+1;
            _serviceMock.Setup(x => x.UpdateVehicleType(vehicleTypesDTOMock.VehicleTypeId, vehicleTypesMock)).ReturnsAsync(vehicleTypesMock);
            _serviceMock.Setup(x => x.IsExists(vehicleTypesDTOMock.VehicleTypeId)).Returns(true);
            //Act
                var result = await _sut.UpdateVehicleType(id, vehicleTypesDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<VehicleTypeDTO>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _serviceMock.Verify(x => x.UpdateVehicleType(vehicleTypesDTOMock.VehicleTypeId, vehicleTypesMock), Times.Never());

        }

        #endregion


       
    }
}
