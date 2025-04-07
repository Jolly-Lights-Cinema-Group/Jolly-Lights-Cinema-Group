using Jolly_Lights_Cinema_Group.BusinessLogic;

namespace JollyLightsCinemaGroup.BusinessLogic;

public static class TestLogin
{
    public static void ExecuteTestLogin()
    {
        var userName = "admin";
        var password = "admin";
            
        var result = AuthenticationService.Login(userName, password);

        Console.WriteLine(result ? "Login Ok!" : "Login Failed!");
    }
}