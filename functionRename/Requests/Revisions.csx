#r "Newtonsoft.Json"

#load "..\Configuration\ApiUrls.csx"
#load "..\Configuration\JsonConfiguration.csx"
#load "..\TableStorage\Models\PostStorageModel.csx"
#load "..\BandLabApiModels\RevisionModel.csx"

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public class Revisions
{
	public Revisions() { }

    public PostStorageModel Post(RevisionModel revision, UserStorageModel user)
    {
        //Post revision on the api.bandlab.com side

        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.AccessToken);
        var postContent = new StringContent(JsonConvert.SerializeObject(revision));
        postContent.Headers.ContentType = new MediaTypeHeaderValue("Application/Json");
        var post = toApi.PostAsync(ApiUrls.Api + "revisions", postContent).Result;

        //Extract server response

        var createdRevision = JsonConvert.DeserializeObject<RevisionModel>(post.Content.ReadAsStringAsync().Result);

        //Make and return new Post model

        return new PostStorageModel
        {
            PartitionKey = DateTimeOffset.Now.Ticks.ToString(),
            RowKey = user.RowKey,
            Id = createdRevision.Id,
            CreatorId = user.Id,
        };
    }
}
