using Jolly_Lights_Cinema_Group.Common;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group.BusinessLogic;

public class AuthenticationService
{
    private readonly AuthenticationRepository _authenticationRepository;

    public AuthenticationService(AuthenticationRepository? authenticationRepository = null)
    {
        _authenticationRepository = authenticationRepository ?? new AuthenticationRepository();
    }
    public bool Login(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            return false;

        var currentUser = _authenticationRepository.Login(userName, password);

        if (!currentUser.ValidLogin)
            return false;

        currentUser.IsAuthenticated = true;
        Globals.CurrentUser = currentUser;
        return true;
    }

    public void Logout()
    {
        Globals.CurrentUser = null;
    }
}