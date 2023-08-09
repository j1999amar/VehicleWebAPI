using AutoFixture;
using AutoMapper;
using DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleAPI.Controllers;
using VehicleInterface.Interface;
using VehicleTypeModel;
using Xunit;

namespace VehicleAPITests
{
    
    public class BrandControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IBrandInterface> _brandMock;
        private readonly Mock<IVehicleInterface> _vehicleMock;
        private readonly IMapper _mapper;
        private readonly BrandController _sut;
        public BrandControllerTest()
        {
            _fixture = new Fixture();
            _brandMock = _fixture.Freeze<Mock<IBrandInterface>>();
            _vehicleMock=_fixture.Freeze<Mock<IVehicleInterface>>();
            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _mapper = mapConfig.CreateMapper();
            _sut = new BrandController(_brandMock.Object,_vehicleMock.Object, _mapper);
        }

        #region Get All Brands By Vehicle Type
        [Fact]
        public void GetAllBrandsByVehicleType_ShouldReturnOkResponse_WhenBrandHavingVehicleTypeId()
        {
            //Arrange
            int id = _fixture.Create<int>();
            var brandDataWithSameVehicleId = _fixture.Build<BrandDTO>()
                .With(o => o.VehicleTypeId, id)
                .CreateMany(5)
                .ToList();
            var brandByVehicleTypeList = _mapper.Map<ICollection<Brands>>(brandDataWithSameVehicleId);
            _brandMock.Setup(x=>x.GetAllBrandsOfAVehicleType(id)).Returns(brandByVehicleTypeList);
            _vehicleMock.Setup(x=>x.IsExists(id)).Returns(true);

            //Act
            var result = _sut.GetAllBrandsOfAVehicleType(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<BrandDTO>>>();
            var getResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var getResultValue = getResult.Value as ICollection<BrandDTO>;
            getResultValue.Count.Should().Be(5);
            getResultValue.First().VehicleTypeId.Should().Be(id);
            _brandMock.Verify(x => x.GetAllBrandsOfAVehicleType(id), Times.Once());
            _vehicleMock.Verify(x=>x.IsExists(id), Times.Once());
        }

        [Fact]
        public void GetAllBrandsByVehicleType_ShouldReturnNotFoundResponse_WhenBrandHasNoRecordForVehiclTypeId()
        {
            //Arrange
            int id = 1;
            ICollection<Brands> brandByVehicleTypeList = null;
            _brandMock.Setup(x => x.GetAllBrandsOfAVehicleType(id)).Returns(brandByVehicleTypeList);
            _vehicleMock.Setup(x => x.IsExists(id)).Returns(true);

            //Act
            var result = _sut.GetAllBrandsOfAVehicleType(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<BrandDTO>>>();
            var getResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            _brandMock.Verify(x => x.GetAllBrandsOfAVehicleType(id), Times.Once());
            _vehicleMock.Verify(x => x.IsExists(id), Times.Once());
        }

        [Fact]
        public void GetAllBrandsByVehicleType_ShouldReturnBadRequestResponse_WhenVehicleIdIsNotExists()
        {
            //Arrange
            int id = 1;
            ICollection<Brands> brandByVehicleTypeList = null;
            _brandMock.Setup(x => x.GetAllBrandsOfAVehicleType(id)).Returns(brandByVehicleTypeList);
            _vehicleMock.Setup(x => x.IsExists(id)).Returns(false);

            //Act
            var result = _sut.GetAllBrandsOfAVehicleType(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<BrandDTO>>>();
            var getResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            _brandMock.Verify(x => x.GetAllBrandsOfAVehicleType(id), Times.Never());
            _vehicleMock.Verify(x => x.IsExists(id), Times.Once());
        }

        #endregion

        #region Brand Post
        [Fact]
        public async void AddBrand_ShouldReturnOkResponse_WhenBrandIdIsNotExists()
        {
            //Arange
            var brandDTOMock = _fixture.Create<BrandDTO>();
            var brandMock = _mapper.Map<Brands>(brandDTOMock);
            _brandMock.Setup(x => x.AddBrand(brandMock)).ReturnsAsync(brandMock);
            _brandMock.Setup(x => x.IsExists(brandMock.BrandId)).Returns(false);
            _vehicleMock.Setup(x => x.IsExists(brandMock.VehicleTypeId.Value)).Returns(true);

            //Act
            var result = await _sut.AddBrand(brandDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<BrandDTO>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _brandMock.Verify(x => x.AddBrand(brandMock), Times.Never());
            _vehicleMock.Verify(x => x.IsExists(brandMock.VehicleTypeId.Value), Times.Once);


        }
        [Fact]
        public async void AddBrand_ShouldReturnBadRequestResponse_WhenBrandIdIsExists()
        {
            //Arange
            var brandDTOMock = _fixture.Create<BrandDTO>();
            var brandMock = _mapper.Map<Brands>(brandDTOMock);
            _brandMock.Setup(x => x.AddBrand(brandMock)).ReturnsAsync(brandMock);
            _brandMock.Setup(x=>x.IsExists(brandMock.BrandId)).Returns(true);
            _vehicleMock.Setup(x => x.IsExists(brandMock.VehicleTypeId.Value)).Returns(true);

            //Act
            var result = await _sut.AddBrand(brandDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<BrandDTO>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _brandMock.Verify(x => x.AddBrand(brandMock), Times.Never());
            _vehicleMock.Verify(x => x.IsExists(brandMock.VehicleTypeId.Value), Times.Never);


        }
        

        [Fact]
        public async void AddBrand_ShouldReturnBadRequestResponse_WhenBrandIsNull()
        {
            //Arange
            BrandDTO brandDTO = null;
            Brands brandMock = null;
            _brandMock.Setup(x => x.AddBrand(brandMock)).ReturnsAsync(brandMock);
            //Act
            var result = await _sut.AddBrand(brandDTO);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<BrandDTO>>();
            result.Result.Should().BeAssignableTo<BadRequestResult>();
            _brandMock.Verify(x => x.AddBrand(brandMock), Times.Never());
        }

        public async void AddBrand_ShouldReturnOkResponse_WhenBrandIsNotNull()
        {
            //Arange
            var brandDTOMock = _fixture.Create<BrandDTO>();
            var brandMock = _mapper.Map<Brands>(brandDTOMock);
            _brandMock.Setup(x => x.AddBrand(brandMock)).ReturnsAsync(brandMock);
            _brandMock.Setup(x => x.IsExists(brandMock.BrandId)).Returns(false);
            _vehicleMock.Setup(x => x.IsExists(brandMock.VehicleTypeId.Value)).Returns(true);

            //Act
            var result = await _sut.AddBrand(brandDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<BrandDTO>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _brandMock.Verify(x => x.AddBrand(brandMock), Times.Never());
            _vehicleMock.Verify(x => x.IsExists(brandMock.VehicleTypeId.Value), Times.Once);


        }

        public async void AddBrand_ShouldReturnOkResponse_WhenBrandHavingVehicleTypeId()
        {
            //Arange
            var brandDTOMock = _fixture.Create<BrandDTO>();
            var brandMock = _mapper.Map<Brands>(brandDTOMock);
            _brandMock.Setup(x => x.AddBrand(brandMock)).ReturnsAsync(brandMock);
            _brandMock.Setup(x => x.IsExists(brandMock.BrandId)).Returns(false);
            _vehicleMock.Setup(x => x.IsExists(brandMock.VehicleTypeId.Value)).Returns(true);

            //Act
            var result = await _sut.AddBrand(brandDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<BrandDTO>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _brandMock.Verify(x => x.AddBrand(brandMock), Times.Never());
            _vehicleMock.Verify(x => x.IsExists(brandMock.VehicleTypeId.Value), Times.Once);


        }

        public async void AddBrand_ShouldReturnBadRequestResponse_WhenBrandIsNotHavingVehicleTypeId()
        {
            //Arange
            var brandDTOMock = _fixture.Create<BrandDTO>();
            var brandMock = _mapper.Map<Brands>(brandDTOMock);
            _brandMock.Setup(x => x.AddBrand(brandMock)).ReturnsAsync(brandMock);
            _brandMock.Setup(x => x.IsExists(brandMock.BrandId)).Returns(false);
            _vehicleMock.Setup(x => x.IsExists(brandMock.VehicleTypeId.Value)).Returns(false);

            //Act
            var result = await _sut.AddBrand(brandDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<BrandDTO>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _brandMock.Verify(x => x.AddBrand(brandMock), Times.Never());
            _vehicleMock.Verify(x => x.IsExists(brandMock.VehicleTypeId.Value), Times.Once);


        }
        #endregion

        #region Delete Brand

        [Fact]
         public void DeleteBrand_ShouldReturnOkResponse_WhenBrandIdIsVaildForDelete()
        {
            //Arrange
            var brandDTO = _fixture.Create<BrandDTO>();
            var brand = _mapper.Map<Brands>(brandDTO);
            _brandMock.Setup(x =>x.DeleteBrand(brandDTO.BrandId)).Returns(true);
            _brandMock.Setup(x=>x.IsExists (brandDTO.BrandId)).Returns(true);
            //Act
            var result = _sut.DeleteBrand(brandDTO.BrandId);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            _brandMock.Verify(x => x.DeleteBrand(brandDTO.BrandId), Times.Once);
            _brandMock.Verify(x => x.IsExists(brandDTO.BrandId), Times.Once);

        }

        [Fact]
        public void DeleteBrand_ShouldReturnBadRequestResponse_WhenBrandIdIsNotVaildForDelete()
        {
            //Arrange
            var brandDTO = _fixture.Create<BrandDTO>();
            var brand = _mapper.Map<Brands>(brandDTO);
            _brandMock.Setup(x => x.DeleteBrand(brandDTO.BrandId)).Returns(false);
            _brandMock.Setup(x => x.IsExists(brandDTO.BrandId)).Returns(false);
            //Act
            var result = _sut.DeleteBrand(brandDTO.BrandId);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            _brandMock.Verify(x => x.DeleteBrand(brandDTO.BrandId), Times.Never);
            _brandMock.Verify(x => x.IsExists(brandDTO.BrandId), Times.Once);
        }

        #endregion


    }
}
