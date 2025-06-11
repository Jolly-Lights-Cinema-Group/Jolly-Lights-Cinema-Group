using Xunit;
using Moq;
using Jolly_Lights_Cinema_Group.BusinessLogic;
using JollyLightsCinemaGroup.DataAccess;
using Jolly_Lights_Cinema_Group.Common;
using Jolly_Lights_Cinema_Group.Models;

public class AuthenticationServiceTests
{
    [Fact]
    public void TestLoginValidCredentials()
    {
        Mock<AuthenticationRepository> mockRepo = new Mock<AuthenticationRepository>();
        User expectedUser = new User { ValidLogin = true };
        mockRepo.Setup(repo => repo.Login("testlogin", "ilovejolly")).Returns(expectedUser);

        AuthenticationService authenticationService = new AuthenticationService(mockRepo.Object);

        bool result = authenticationService.Login("testlogin", "ilovejolly");

        Xunit.Assert.True(result);
        Xunit.Assert.NotNull(Globals.CurrentUser);
        Xunit.Assert.True(Globals.CurrentUser!.IsAuthenticated);
    }

    [Fact]
    public void TestLoginInvalidCredentials()
    {
        Mock<AuthenticationRepository> mockRepo = new Mock<AuthenticationRepository>();
        User invalidUser = new User { ValidLogin = false };
        mockRepo.Setup(repo => repo.Login("testwrongpassword", "wrongpassword")).Returns(invalidUser);

        AuthenticationService authenticationService = new AuthenticationService(mockRepo.Object);

        bool result = authenticationService.Login("testwrongpassword", "wrongpassword");

        Xunit.Assert.False(result);
        Xunit.Assert.Null(Globals.CurrentUser);
    }

    [Theory]
    [InlineData("", "password")]
    [InlineData("username", "")]
    [InlineData("", "")]
    public void TestLoginWithEmptyUsernameOrPassword(string username, string password)
    {
        AuthenticationService authenticationService = new AuthenticationService();

        bool result = authenticationService.Login(username, password);

        Xunit.Assert.False(result);
    }

    [Fact]
    public void TestLogout()
    {
        Globals.CurrentUser = new User { ValidLogin = true, IsAuthenticated = true };
        AuthenticationService authenticationService = new AuthenticationService();

        authenticationService.Logout();

        Xunit.Assert.Null(Globals.CurrentUser);
    }
}