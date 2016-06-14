#r "Newtonsoft.Json"

#load "..\Configuration\ApiUrls.csx"
#load "..\Configuration\JsonConfiguration.csx"
#load "..\BandLabApiModels\RevisionModel.csx"
#load "..\BandLabApiModels\CommentModel.csx"
#load "..\BandLabApiModels\MediaCaption.csx"

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Shouts
{
    public Guid Post(string accessToken)
    {
        var fileName = "azuref.png";
        var contentType = "image/jpg";
        var mediaUrl = "images";


        //Make request message
        var request = new HttpRequestMessage(HttpMethod.Post, ApiUrls.Api + mediaUrl);

        //Make multipartFormData content
        var multiPartContent = new MultipartFormDataContent("----MediaContent");

        //Read currebt file into ByteArray
        var byteArrayContent = new ByteArrayContent(File.ReadAllBytes(fileName));
        byteArrayContent.Headers.Add("Content-Type", contentType);
        byteArrayContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file" };

        //Make json part
        var caption = new MediaCaption { Caption = "Some text here", AutoPost = false };
        //if (type == Media.Video)
        //    caption.Clip = new Clip(startPosition, 15);
        var jsonContent = new StringContent(JsonConvert.SerializeObject(caption));
        jsonContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "json" };
        jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //Add file and json to multipartFormData
        multiPartContent.Add(byteArrayContent, "name=file", fileName);
        multiPartContent.Add(jsonContent, "name=metadata");
        request.Content = multiPartContent;

        //Upload this info to the server
        var upload = new HttpClient();
        upload.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var load = upload.SendAsync(request).Result;

        var mediaId = JsonConvert.DeserializeObject<Entity>(load.Content.ReadAsStringAsync().Result).Id;
        var post = upload.PostAsync(ApiUrls.Api + $"images({mediaId})/posts", null).Result;

        return JsonConvert.DeserializeObject<Entity>(post.Content.ReadAsStringAsync().Result).Id;

        //if (type == Media.Image)
        //return JsonConvert.DeserializeObject<ImageMediaModel>(load.Content.ReadAsStringAsync().Result);

        //return JsonConvert.DeserializeObject<VideoMediaModel>(load.Content.ReadAsStringAsync().Result);
    }

    public int Like(Guid postId, string accessToken)
    {
        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var like = toApi.PostAsync(ApiUrls.Api + $"posts({postId})/likes", null).Result;

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
                    return await toApi.DeleteAsync(ApiUrls.Api + $"posts({liker.PostId})/likes");
                }
            });

            tasks.Add(t);
        }

        Task.WaitAll(tasks.ToArray());

        if (tasks.FirstOrDefault(x => x.Result.StatusCode != HttpStatusCode.Accepted) != null)
            return 400;

        return 202;
    }

    public long LeftComment(Guid shoutId, string accessToken, string myUsername = null)
    {
        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var postContent = new StringContent(JsonConvert.SerializeObject(new CommentModel(myUsername)));
        postContent.Headers.ContentType = new MediaTypeHeaderValue("Application/Json");

        var comment = toApi.PostAsync(ApiUrls.Api + $"posts({shoutId})/comments", postContent).Result;
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
                    return await toApi.DeleteAsync(ApiUrls.Api + $"posts/comments({commenter.CommentId})");
                }
            });

            tasks.Add(t);
        }

        Task.WaitAll(tasks.ToArray());

        if (tasks.FirstOrDefault(x => x.Result.StatusCode != HttpStatusCode.Accepted) != null)
            return 400;

        return 202;
    }
}
