using API.Controllers;
using API.Models;
using API.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NSubstitute;

namespace Test.ControllerTests
{
    public class UserControllerTest
    {
        // Dependencias para os testes ultilizando Moq
        private readonly Mock<IUserInterface> MockUserInterface;
        private readonly UserController _userController;

        // Dependencias para os testes ultilizando NSubstitute
        private readonly IUserInterface SubstituteUserInterface;
        private readonly UserController _UserController;

        public UserControllerTest()
        {
            MockUserInterface = new Mock<IUserInterface>();
            _userController = new UserController(MockUserInterface.Object);

            SubstituteUserInterface = Substitute.For<IUserInterface>();
            _UserController = new UserController(SubstituteUserInterface);

        }

        private static User CreateMockUser()
        {
            return new User
            {
                Id = 1,
                Name = "Test User",
                Email = "testuser@email.com"
            };
        }


        // Testes ultilizando Moq e FluentAssertions
        [Fact]
        public async void UserController_Create_ReturnsCreateded()
        {
            // Arrange
            var user = CreateMockUser();

            // Act
            MockUserInterface.Setup(u => u.CreateAsync(user)).ReturnsAsync(true);
            var result = (CreatedAtActionResult)await _userController.Create(user);

            // Assert
            result.StatusCode.Should().Be(201);
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(1)]
        public async void UserController_GetUserById_ReturnsOk(int userId)
        {
            // Arrange
            var user = CreateMockUser();

            // Act
            MockUserInterface.Setup(u => u.GetByIdAsync(userId)).ReturnsAsync(user);
            var result = (OkObjectResult)await _userController.GetUserById(userId);

            // Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async void UserController_GetUsers_ReturnsOk()
        {
            // Arrange
            var users = new List<User> { CreateMockUser() };
            MockUserInterface.Setup(u => u.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = (OkObjectResult)await _userController.GetUsers();

            // Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async void UserController_UpdateUser_ReturnsOk()
        {
            // Arrange
            var user = CreateMockUser();
            MockUserInterface.Setup(u => u.UpdateAsync(user)).ReturnsAsync(true);

            // Act
            var result = (OkResult)await _userController.UpdateUser(user);

            // Assert
            result.StatusCode.Should().Be(200);
            result.Should().NotBeNull();
        }

        [Fact]
        public async void UserController_DeleteUser_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            MockUserInterface.Setup(u => u.DeleteAsync(userId)).ReturnsAsync(true);

            // Act
            var result = (NoContentResult)await _userController.DeleteUser(userId);

            // Assert
            result.StatusCode.Should().Be(204);
            result.Should().NotBeNull();
        }

        // Testes ultilizando NSubstitute sem FluentAssertions
        [Fact]
        public async Task UserController_Create_ReturnsBadRequest()
        {
            // Arrange
            var user = CreateMockUser();
            SubstituteUserInterface.CreateAsync(user).Returns(Task.FromResult(false)); // CreateAsync retorna um objeto Task<bool>, por isso usamos Task.FromResult(false) ao invés de só false

            // Act
            var result = await _UserController.Create(user);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task UserController_GetUserById_ReturnsNotFound(int userId)
        {
            // Arrange
            SubstituteUserInterface.GetByIdAsync(userId).Returns(Task.FromResult<User?>(null)); // GetByIdAsync retorna um objeto Task<User>, por isso usamos Task.FromResult<User?>(null) ao invés de só null

            // Act
            var result = await _UserController.GetUserById(userId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UserController_GetUsers_ReturnsNotFound()
        {
            // Arrange
            SubstituteUserInterface.GetAllAsync().Returns(Task.FromResult(Enumerable.Empty<User>())); // GetAllAsync retorna um objeto Task<IEnumerable<User>>, por isso retornamos uma lista vazia com Task.FromResult(Enumerable.Empty<User>())

            // Act
            var result = await _UserController.GetUsers();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UserController_UpdateUser_ReturnsNotFound()
        {
            // Arrange
            var user = CreateMockUser();
            SubstituteUserInterface.UpdateAsync(user).Returns(Task.FromResult(false));

            // Act
            var result = await _UserController.UpdateUser(user);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UserController_DeleteUser_ReturnsNotFound()
        {
            // Arrange
            int userId = 1;
            SubstituteUserInterface.DeleteAsync(userId).Returns(Task.FromResult(false));

            // Act
            var result = await _UserController.DeleteUser(userId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
