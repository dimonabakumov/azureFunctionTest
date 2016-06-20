#load "..\TableStorage\Models\UserStorageModel.csx"
#load "..\BandLabApiModels\AuthorizationModel.csx"
#load "..\BandLabApiModels\WasRegisteredModel.csx"
#load "..\BandLabApiModels\UserModel.csx"
#load "..\Configuration\ApiUrls.csx"
#load "..\Configuration\JsonConfiguration.csx"

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public class Authorisation
{
	public Authorisation() { }

    public UserStorageModel GetUser(AuthorizationModel authUser, string sessionId, Actions action)
    {
        //Register new user

        var toApi = new HttpClient();
        var postContent = new StringContent(JsonConvert.SerializeObject(authUser));
        postContent.Headers.ContentType = new MediaTypeHeaderValue("Application/Json");
        var post = toApi.PostAsync(ApiUrls.Identity + "authorizations", postContent).Result;
        if (post.StatusCode != System.Net.HttpStatusCode.Created)
            return null;

        //Extract auth token

        var wasRegister = JsonConvert.DeserializeObject<WasRegisteredModel>(post.Content.ReadAsStringAsync().Result);


        //Get Me

        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", wasRegister.SessionKey);

        var get = toApi.GetAsync(ApiUrls.Api + "me").Result;
        var me = JsonConvert.DeserializeObject<UserModel>(get.Content.ReadAsStringAsync().Result);

        return new UserStorageModel
        {
            PartitionKey = sessionId,
            RowKey = me.Id.ToString(),
            Password = authUser.Password,
            UserName = me.UserName,
            AccessToken = wasRegister.SessionKey,
            Actions = action.ToString(),
            ExpiryDate = wasRegister.ExpiryDate,
            RefreshToken = wasRegister.RefreshToken,
        };
    }

    public string RefreshToken(string username, string password)
    {
        var authUser = new AuthorizationModel
        {
            Login = username,
            Password = password,
            Provider = "password",
            ClientId = "Angular",
            Register = false
        };

        var toApi = new HttpClient();
        var postContent = new StringContent(JsonConvert.SerializeObject(authUser));
        postContent.Headers.ContentType = new MediaTypeHeaderValue("Application/Json");
        var post = toApi.PostAsync(ApiUrls.Identity + "authorizations", postContent).Result;
        if (post.StatusCode != System.Net.HttpStatusCode.Created)
            return null;

        //Extract auth token

        var wasRegister = JsonConvert.DeserializeObject<WasRegisteredModel>(post.Content.ReadAsStringAsync().Result);

        return wasRegister.SessionKey;
    }
}
