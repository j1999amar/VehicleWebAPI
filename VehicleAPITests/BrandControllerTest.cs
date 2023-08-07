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
        public void GetAllBrandsByVehicleType_ShouldReturnOkResponse_WhenDataFound()
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
        public void GetAllBrandsByVehicleType_ShouldReturnBadResponse_WhenDataNotFound()
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
            var getResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            _brandMock.Verify(x => x.GetAllBrandsOfAVehicleType(id), Times.Once());
            _vehicleMock.Verify(x => x.IsExists(id), Times.Once());
        }

        [Fact]
        public void GetAllBrandsByVehicleType_ShouldReturnBadResponse_WhenVehicleIdIsNotExists()
        {
            //Arrange
            int id = 1;
            var brandByVehicleTypeList = _mapper.Map<ICollection<Brands>>(null);
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
        public async void AddBrand_ShouldReturnOkResponse_WhenDataIsVaild()
        {
            //Arange
            var brandDTOMock = _fixture.Create<BrandDTO>();
            var brandMock = _mapper.Map<Brands>(brandDTOMock);
            _brandMock.Setup(x => x.AddBrand(brandMock)).ReturnsAsync(brandMock);
            _vehicleMock.Setup(x=>x.IsExists(brandMock.VehicleTypeId.Value)).Returns(true);

            //Act
            var result = await _sut.AddBrand(brandDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<BrandDTO>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _brandMock.Verify(x => x.AddBrand(brandMock), Times.Never());
            _vehicleMock.Verify(x => x.IsExists(brandMock.VehicleTypeId.Value),Times.Once);


        }

        [Fact]
        public async void AddBrand_ShouldReturnBadRequest_WhenDataIsNotVaild()
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
        #endregion


    }
}
