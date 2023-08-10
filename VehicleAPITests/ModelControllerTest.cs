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
        #region Model Post 

        [Fact]
        public async void AddModelForBrand_ShouldReturnOkResponse_WhenModelIdIsNotExists()
        {
            //Arrange
            var modelDTOMock = _fixture.Create<ModelDTO>();
            var modelMock = _mapper.Map<Models>(modelDTOMock);
            _modelMock.Setup(x => x.AddModelForBrand(modelMock)).ReturnsAsync(modelMock);
            _modelMock.Setup(x=> x.IsExists(modelDTOMock.ModelId)).Returns(false);
            _brandMock.Setup(x => x.IsExists(modelDTOMock.BrandId)).Returns(true);
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
        public async void AddModelForBrand_ShouldReturnBadRequestResponse_WhenModelIdExists()
        {
            //Arrange
            var modelDTOMock = _fixture.Create<ModelDTO>();
            var modelMock = _mapper.Map<Models>(modelDTOMock);
            _modelMock.Setup(x => x.AddModelForBrand(modelMock)).ReturnsAsync(modelMock);
            _modelMock.Setup(x => x.IsExists(modelDTOMock.ModelId)).Returns(true);
            _brandMock.Setup(x => x.IsExists(modelDTOMock.BrandId)).Returns(true);
            //Act
            var result = await _sut.AddModelForBrand(modelDTOMock);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ModelDTO>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _modelMock.Verify(x => x.AddModelForBrand(modelMock), Times.Never);
            _brandMock.Verify(x => x.IsExists(modelDTOMock.BrandId), Times.Never);
        }
        [Fact]
        public async void AddModelForBrand_ShouldReturnOkResponse_WhenModelIsValid()
        {
            //Arrange
            var modelDTOMock = _fixture.Create<ModelDTO>();
            var modelMock = _mapper.Map<Models>(modelDTOMock);
            _modelMock.Setup(x => x.AddModelForBrand(modelMock)).ReturnsAsync(modelMock);
            _brandMock.Setup(x => x.IsExists(modelDTOMock.BrandId)).Returns(true);
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
        public async void AddModelForBrand_ShouldReturnBadRequestResponse_WhenModelIsNull()
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
        #endregion

        #region Get All Model By Brand 
        [Fact]
        public async void GetAllModelByBrand_ShouldReturnOkResponse_WhenModelFoundForBrand()
        {
            //Arrange
            int id = _fixture.Create<int>();
            var modelDataWithBrandId = _fixture.Build<ModelDTO>()
                .With(o => o.BrandId, id)
                .CreateMany(5)
                .ToList();
            var modelByBrand = _mapper.Map<ICollection<Models>>(modelDataWithBrandId);
            _modelMock.Setup(x => x.GetAllModelByBrand(id)).ReturnsAsync(modelByBrand);
            _brandMock.Setup(x => x.IsExists(id)).Returns(true);

            //Act
            var result = await _sut.GetAllModelByBrand(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<ModelDTO>>>();
            var getResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var getResultValue = getResult.Value as ICollection<ModelDTO>;
            getResultValue.Count.Should().Be(5);
            _modelMock.Verify(x => x.GetAllModelByBrand(id),Times.Once);
            _brandMock.Verify(x => x.IsExists(id), Times.Once);
        }

        [Fact]
        public async void GetAllModelByBrand_ShouldReturnNotFoundResponse_WhenModelIsNotFoundForBrand()
        {
            //Arrange
           
            int id = _fixture.Create<int>();
            ICollection < Models > modelMock= null;
            _modelMock.Setup(x => x.GetAllModelByBrand(id)).ReturnsAsync(modelMock);
            _brandMock.Setup(x => x.IsExists(id)).Returns(true);

            //Act
            var result =await _sut.GetAllModelByBrand(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<ModelDTO>>>();
            result.Result.Should().BeOfType<NotFoundObjectResult>();
            _modelMock.Verify(x => x.GetAllModelByBrand(id), Times.Once());
            _brandMock.Verify(x => x.IsExists(id), Times.Once());
        }

        [Fact]
        public async void GetAllModelByBrand_ShouldReturnBadResponse_WhenBrandIdIsNotExists()
        {
            //Arrange
            int id = 1;
            ICollection<Models> modelMock = null;
            _modelMock.Setup(x => x.GetAllModelByBrand(id)).ReturnsAsync(modelMock);
            _brandMock.Setup(x => x.IsExists(id)).Returns(false);

            //Act
            var result = await _sut.GetAllModelByBrand(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<ModelDTO>>>();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            _modelMock.Verify(x => x.GetAllModelByBrand(id), Times.Never());
            _brandMock.Verify(x => x.IsExists(id), Times.Once());
        }

        [Fact]
        public async void GetAllModelByBrand_ShouldReturnOkResponse_WhenBrandListHavingCountMoreThanZero()
        {
            //Arrange
            int id = _fixture.Create<int>();
            var modelDTODataWithSameBrandId = _fixture.Build<ModelDTO>()
                .With(o => o.BrandId, id)
                .CreateMany(5)
                .ToList();
            var modelDataWithSameBrandId = _mapper.Map<ICollection<Models>>(modelDTODataWithSameBrandId);
            _modelMock.Setup(x => x.GetAllModelByBrand(id)).ReturnsAsync(modelDataWithSameBrandId);
            _brandMock.Setup(x => x.IsExists(id)).Returns(true);

            //Act
            var result =await  _sut.GetAllModelByBrand(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<ModelDTO>>>();
            var getResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var getResultValue = getResult.Value as ICollection<ModelDTO>;
            getResultValue.Count.Should().Be(5);
            getResultValue.First().BrandId.Should().Be(id);
            _modelMock.Verify(x => x.GetAllModelByBrand(id), Times.Once());
            _brandMock.Verify(x => x.IsExists(id), Times.Once());
        }

        [Fact]
        public async void GetAllModelByBrand_ShouldReturnNotFoundResponse_WhenBrandListHavingCountIsZero()
        {
            //Arrange
            int id = _fixture.Create<int>();
          
            var modelDataWithSameBrandId = _mapper.Map<ICollection<Models>>(null);
            _modelMock.Setup(x => x.GetAllModelByBrand(id)).ReturnsAsync(modelDataWithSameBrandId);
            _brandMock.Setup(x => x.IsExists(id)).Returns(true);

            //Act
            var result = await _sut.GetAllModelByBrand(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<ModelDTO>>>();
            var getResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            var getResultValue = getResult.Value as ICollection<ModelDTO>;
            _modelMock.Verify(x => x.GetAllModelByBrand(id), Times.Once());
            _brandMock.Verify(x => x.IsExists(id), Times.Once());
        }
        #endregion
    }
}
