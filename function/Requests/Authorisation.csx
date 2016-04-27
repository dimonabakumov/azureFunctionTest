#load "..\TableStorage\Models\UserStorageModel.csx"
#load "..\BandLabApiModels\AuthorizationModel.csx"
#load "..\BandLabApiModels\WasRegisteredModel.csx"
#load "..\BandLabApiModels\UserModel.csx"
#load "..\Configuration\ApiUrls.csx"
#load "..\Configuration\JsonConfiguration.csx"

using System;

public class Authorisation
{
	public Authorisation()
	{
	}

    //public UserStorageModel GetUser()
    //{
    //    return new UserStorageModel();
    //}

    public UserStorageModel GetUser(AuthorizationModel authUser, string sessionId, Actions action)
    {
        //Register new user

        var toApi = new HttpClient();
        var postContent = new StringContent(JsonConvert.SerializeObject(authUser));
        postContent.Headers.ContentType = new MediaTypeHeaderValue("Application/Json");
        var post = toApi.PostAsync(ApiUrls.Identity + "authorizations", postContent).Result;


        //Extract auth token

        var wasRegister = JsonConvert.DeserializeObject<WasRegisteredModel>(post.Content.ReadAsStringAsync().Result);


        //Get Me

        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", wasRegister.SessionKey);

        var get = toApi.GetAsync(ApiUrls.Api + "me").Result;
        var me = JsonConvert.DeserializeObject<UserModel>(get.Content.ReadAsStringAsync().Result);

        return new UserStorageModel
        {
            PartitionKey = DateTimeOffset.Now.Ticks.ToString(),
            RowKey = sessionId,
            Id = me.Id,
            Password = authUser.Password,
            UserName = me.UserName,
            AccessToken = wasRegister.SessionKey,
            Actions = action.ToString(),
            ExpiryDate = wasRegister.ExpiryDate,
            RefreshToken = wasRegister.RefreshToken,
        };
    }
}
