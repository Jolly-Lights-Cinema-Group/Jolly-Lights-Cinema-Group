using Jolly_Lights_Cinema_Group.Common;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group.BusinessLogic;

public class AuthenticationService(AuthenticationRepository authenticationRepository)
{
    public bool Login(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            return false;
        
        var currentUser = authenticationRepository.Login(userName, password);

        if (!currentUser.ValidLogin)
            return false;
        
        Globals.CurrentUser = currentUser;
        return true;
    }

    public bool Logout()
    {
        Globals.CurrentUser = null;
        return true;
    }
}