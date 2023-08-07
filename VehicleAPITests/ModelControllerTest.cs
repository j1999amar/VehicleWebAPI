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
    public class ModelControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IBrandInterface> _brandMock;
        private readonly Mock<IModelInterface> _modelMock;
        private readonly IMapper _mapper;
        private readonly ModelController _sut;
        public ModelControllerTest()
        {
            _fixture = new Fixture();
            _brandMock = _fixture.Freeze<Mock<IBrandInterface>>();
            _modelMock = _fixture.Freeze<Mock<IModelInterface>>();
            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _mapper = mapConfig.CreateMapper();
            _sut = new ModelController( _modelMock.Object,_brandMock.Object, _mapper);
        }
        [Fact]
        public async void AddModelForBrand_ShouldReturnOkResponse_WhenDataIsValid()
        {
            //Arrange
            var modelDTOMock = _fixture.Create<ModelDTO>();
            var modelMock=_mapper.Map<Models>(modelDTOMock);
            _modelMock.Setup(x => x.AddModelForBrand(modelMock)).ReturnsAsync(modelMock);
            _brandMock.Setup(x=> x.IsExists(modelDTOMock.BrandId)).Returns(true);
            //Act
            var result = await _sut.AddModelForBrand(modelDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ModelDTO>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _modelMock.Verify(x => x.AddModelForBrand(modelMock), Times.Never);
            _brandMock.Verify(x => x.IsExists(modelDTOMock.BrandId), Times.Once);
        }
        [Fact]
        public async void AddModelForBrand_ShouldReturnBadRequestResponse_WhenDataIsNotValid()
        {
            //Arrange
            ModelDTO modelDTOMock = null;
            Models modelMock = null;
            _modelMock.Setup(x => x.AddModelForBrand(modelMock)).ReturnsAsync(modelMock);
            _brandMock.Setup(x => x.IsExists(1)).Returns(true);
            //Act
            var result = await _sut.AddModelForBrand(modelDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ModelDTO>>();
            result.Result.Should().BeAssignableTo<BadRequestResult>();
            _modelMock.Verify(x => x.AddModelForBrand(modelMock), Times.Never);
            _brandMock.Verify(x => x.IsExists(1), Times.Never);

        }

        [Fact]
        public async void AddModelForBrand_ShouldReturnBadRequestResponse_WhenBrandIdIsNotVaild()
        {
            //Arrange
            var modelDTOMock = _fixture.Create<ModelDTO>();
            var modelMock = _mapper.Map<Models>(modelDTOMock);
            _modelMock.Setup(x => x.AddModelForBrand(modelMock)).ReturnsAsync(modelMock);
            _brandMock.Setup(x => x.IsExists(modelDTOMock.BrandId)).Returns(false);
            //Act
            var result = await _sut.AddModelForBrand(modelDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ModelDTO>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _modelMock.Verify(x => x.AddModelForBrand(modelMock), Times.Never);
            _brandMock.Verify(x => x.IsExists(modelDTOMock.BrandId), Times.Once);
        }
    }
}
