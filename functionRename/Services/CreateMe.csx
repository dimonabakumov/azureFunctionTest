﻿#r "System.Net.Http"
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
	public UserStorageModel Auth(string userId)
	{
        var userQuery = new UserModelQuery();
        userQuery.TryGet(userId, Actions.Me);

        var generedUser = new AuthorizationModel
        {
            Username = "dimon",
            Password = "password",
            Provider = "password",
            ClientId = "Angular",
            RememberMe = true
        };

        var registeredUser = new Authorisation().GetUser(generedUser, userId, Actions.Me);
        userQuery.Add(registeredUser);

        return registeredUser;
    }
}
