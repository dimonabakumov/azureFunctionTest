#r "System.Net.Http"
#r "Newtonsoft.Json"

#load "..\ModelsGenerator\Registration.csx"
#load "..\BandLabApiModels\AuthorizationModel.csx"
#load "..\BandLabApiModels\WasRegisteredModel.csx"

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public class CreateMe
{
	public object Auth()
	{
        var generedUser = new AuthorizationModel
        {
            Username = "dimon",
            Password = "password",
            Provider = "password",
            ClientId = "Angular"
        };

        string Identity = "https://identity-test.bandlab.com/v1.0/";

        //Register new user

        var toApi = new HttpClient();
        //var postContent = new StringContent(generedUser.ToJson());
        var postContent = new StringContent(JsonConvert.SerializeObject(generedUser));
        postContent.Headers.ContentType = new MediaTypeHeaderValue("Application/Json");
        var post = toApi.PostAsync(Identity + "authorizations", postContent).Result;


        //Extract auth token

        return JsonConvert.DeserializeObject<WasRegisteredModel>(post.Content.ReadAsStringAsync().Result);
    }
}
