using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Path2CodeDemo.Application.IService;
using Path2CodeDemo.Application.RequestModels;
using Path2CodeDemo.Domain;

namespace Path2CodeDemo.Api.Controllers.Tests
{
    public class CandidateControllerTest
    {
        private readonly Mock<ICandidateService> _mockService;
        private readonly Mock<ILogger<CandidateController>> _logger;
        private readonly CandidateController _controller;
        private readonly Fixture _fixture;

        public CandidateControllerTest()
        {
            _mockService = new Mock<ICandidateService>();
            _logger = new Mock<ILogger<CandidateController>>();
            _controller = new CandidateController(_mockService.Object, _logger.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenCandidateExists()
        {
            // Arrange
            var candidate = _fixture.Create<Candidate>();

            _mockService.Setup(s => s.GetCandidateByIdAsync(It.IsAny<Guid>()))
                        .ReturnsAsync(candidate);

            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(candidate, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenCandidateDoesNotExist()
        {
            // Arrange
            var candidateId = Guid.NewGuid();
            _mockService.Setup(s => s.GetCandidateByIdAsync(candidateId))
                        .ReturnsAsync((Candidate?)null);

            // Act
            var result = await _controller.GetById(candidateId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WithCorrectRouteValues()
        {
            // Arrange
            var request = _fixture.Create<CreateCandidateRequest>();
            var newCandidateId = Guid.NewGuid();

            _mockService.Setup(s => s.AddCandidateAsync(request)).ReturnsAsync(newCandidateId);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(CandidateController.GetById), createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues);
            Assert.True(createdResult.RouteValues.ContainsKey("id"));
            Assert.Equal(newCandidateId, createdResult.RouteValues["id"]);
            Assert.Null(createdResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsError_WhenRequestIsInvalid()
        {
            // Arrange
            var request = _fixture.Build<CreateCandidateRequest>()
                                                            .Without(r => r.Name)
                                                            .Create();
            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            //Act
            var result = await _controller.Create(It.IsAny<CreateCandidateRequest>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }
    }
}
