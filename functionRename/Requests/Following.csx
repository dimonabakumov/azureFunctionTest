#load "..\Configuration\ApiUrls.csx"
#load "..\Configuration\JsonConfiguration.csx"
#load "..\TableStorage\Models\PostStorageModel.csx"
#load "..\BandLabApiModels\RevisionModel.csx"

using System;
using System.Net.Http;
using System.Net.Http.Headers;

public class Following
{
	public Following() { }

    public int Follow(Guid userId, UserStorageModel follower)
    {
        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.AccessToken);
        var like = toApi.PostAsync(ApiUrls.Api + $"users({userId})/followers", null).Result;

        return (int)like.StatusCode;
    }

    public int Unfollow(Guid userId, UserStorageModel follower)
    {
        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.AccessToken);
        var like = toApi.DeleteAsync(ApiUrls.Api + $"users({userId})/followers").Result;

        return (int)like.StatusCode;
    }
}
