#r "System.Net.Http"
#r "Newtonsoft.Json"
#r "Microsoft.WindowsAzure.Storage"

#load "..\BandLabApiModels\AuthorizationModel.csx"
#load "..\Requests\Authorisation.csx"
#load "..\TableStorage\Queries\UserModelQuery.csx"

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public class CreateMe
{
	public bool Auth(string sessionId, string username, string password)
	{
        var userQuery = new UserModelQuery();
        var userExist = userQuery.TryGet(sessionId, Actions.Me, username, password);
	    if (userExist) return true;
	    else
	    {
	        var generedUser = new AuthorizationModel
	        {
	            Username = username,
	            Password = password,
	            Provider = "password",
	            ClientId = "Angular",
	            RememberMe = true
	        };

	        var registeredUser = new Authorisation().GetUser(generedUser, sessionId, Actions.Me);
	        if (registeredUser == null)
	            return false;

            userQuery.Add(registeredUser);
	        return true;
	    }
    }
}
