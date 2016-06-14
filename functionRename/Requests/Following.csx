#load "..\Configuration\ApiUrls.csx"
#load "..\Configuration\JsonConfiguration.csx"
#load "..\BandLabApiModels\RevisionModel.csx"

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class Following
{
	public Following() { }

    public int Follow(Guid userId, UserStorageModel follower)
    {
        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", follower.AccessToken);
        var follow = toApi.PostAsync(ApiUrls.Api + $"users({userId})/followers", null).Result;

        return (int)follow.StatusCode;
    }

    public int Unfollow(Guid userId, List<UserStorageModel> followers)
    {
        List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();
        foreach (var follower in followers)
        {
            var t = Task.Run(async () =>
            {
                using (var toApi = new HttpClient { Timeout = TimeSpan.FromSeconds(180) })
                {
                    toApi.DefaultRequestHeaders.Add("Authorization", "Bearer " + follower.AccessToken);
                    return await toApi.DeleteAsync(ApiUrls.Api + $"users({userId})/followers");
                }
            });

            tasks.Add(t);
        }

        Task.WaitAll(tasks.ToArray());

        if (tasks.FirstOrDefault(x => x.Result.StatusCode != HttpStatusCode.NoContent) != null)
            return 400;

        return 204;
    }
}
