#load "..\BandLabApiModels\AuthorizationModel.csx"

using System;
using System.Text;

public class Registration
{
    public AuthorizationModel Generate(bool register = true)
    {
        var now = DateTimeOffset.UtcNow.Ticks % 100000000;
        var newUser = new AuthorizationModel
        {
            Password = "password",
            Name = "Azure func user " + now,
            Email = string.Format("{0}@bandlab.com", "azureFuncUser" + now),
            Register = register,
            RememberMe = true,
            ClientId = "Angular",
            Provider = "Password"
        };

        return newUser;
    }
}