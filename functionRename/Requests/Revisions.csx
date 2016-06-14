#r "Newtonsoft.Json"

#load "..\Configuration\ApiUrls.csx"
#load "..\Configuration\JsonConfiguration.csx"
#load "..\BandLabApiModels\RevisionModel.csx"
#load "..\BandLabApiModels\SongModel.csx"
#load "..\BandLabApiModels\CommentModel.csx"
#load "..\BandLabApiModels\ForkSongModel.csx"

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Revisions
{
	public Revisions() { }

    public RevisionModel Post(RevisionModel revision, string accessToken)
    {
        //Post revision on the api.bandlab.com side

        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var postContent = new StringContent(JsonConvert.SerializeObject(revision));
        postContent.Headers.ContentType = new MediaTypeHeaderValue("Application/Json");
        var post = toApi.PostAsync(ApiUrls.Api + "revisions", postContent).Result;

        //Extract server response

        return JsonConvert.DeserializeObject<RevisionModel>(post.Content.ReadAsStringAsync().Result);
        //return createdRevision.Id;
    }

    public int Like(Guid revisionId, string accessToken)
    {
        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var like = toApi.PostAsync(ApiUrls.Api + $"revisions({revisionId})/likes", null).Result;

        return (int)like.StatusCode;
    }

    public int Dislike(List<UserStorageModel> likers)
    {
        List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();
        foreach (var liker in likers)
        {
            var t = Task.Run(async () =>
            {
                using (var toApi = new HttpClient { Timeout = TimeSpan.FromSeconds(180) })
                {
                    toApi.DefaultRequestHeaders.Add("Authorization", "Bearer " + liker.AccessToken);
                    return await toApi.DeleteAsync(ApiUrls.Api + $"revisions({liker.RevisionId})/likes");
                }
            });

            tasks.Add(t);
        }

        Task.WaitAll(tasks.ToArray());

        if (tasks.FirstOrDefault(x => x.Result.StatusCode != HttpStatusCode.Accepted) != null)
            return 400;

        return 202;
    }

    public long LeftComment(Guid revisionId, string accessToken, string myUsername = null)
    {
        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var postContent = new StringContent(JsonConvert.SerializeObject(new CommentModel(myUsername)));
        postContent.Headers.ContentType = new MediaTypeHeaderValue("Application/Json");

        var comment = toApi.PostAsync(ApiUrls.Api + $"revisions({revisionId})/comments", postContent).Result;
        var postedComment = JsonConvert.DeserializeObject<CommentModel>(comment.Content.ReadAsStringAsync().Result);

        return postedComment.Id;
    }

    public int DeleteComment(List<UserStorageModel> commenters)
    {
        List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();
        foreach (var commenter in commenters)
        {
            var t = Task.Run(async () =>
            {
                using (var toApi = new HttpClient { Timeout = TimeSpan.FromSeconds(180) })
                {
                    toApi.DefaultRequestHeaders.Add("Authorization", "Bearer " + commenter.AccessToken);
                    return await toApi.DeleteAsync(ApiUrls.Api + $"revisions/comments({commenter.CommentId})");
                }
            });

            tasks.Add(t);
        }

        Task.WaitAll(tasks.ToArray());

        if (tasks.FirstOrDefault(x => x.Result.StatusCode != HttpStatusCode.Accepted) != null)
            return 400;

        return 202;
    }

    public Guid ForkSong(Guid revisionId, string accessToken)
    {
        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var postContent = new StringContent(JsonConvert.SerializeObject(new ForkSongModel(revisionId)));
        postContent.Headers.ContentType = new MediaTypeHeaderValue("Application/Json");
        var post = toApi.PostAsync(ApiUrls.Api + "songs/forks", postContent).Result;

        var forkedRevision = JsonConvert.DeserializeObject<SongModel>(post.Content.ReadAsStringAsync().Result);

        return forkedRevision.Revision.Id;
    }
}
