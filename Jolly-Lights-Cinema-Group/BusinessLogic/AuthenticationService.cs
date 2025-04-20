using Jolly_Lights_Cinema_Group.Common;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group.BusinessLogic;

public class AuthenticationService()
{
    public static bool Login(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            return false;

        var currentUser = AuthenticationRepository.Login(userName, password);

        if (!currentUser.ValidLogin)
            return false;

        currentUser.IsAuthenticated = true;
        Globals.CurrentUser = currentUser;
        return true;
    }

    public static void Logout()
    {
        Globals.CurrentUser = null;
    }
}