#load "..\Configuration\ApiUrls.csx"
#load "..\Configuration\JsonConfiguration.csx"
#load "..\BandLabApiModels\BandModel.csx"
#load "..\BandLabApiModels\SongModel.csx"

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Bands
{
    public Guid PostBand(UserStorageModel owner)
    {
        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", owner.AccessToken);
        var postContent = new StringContent(JsonConvert.SerializeObject(new BandModel()));
        postContent.Headers.ContentType = new MediaTypeHeaderValue("Application/Json");
        var band = toApi.PostAsync(ApiUrls.Api + $"bands", postContent).Result;

        //Extract server response

        var postedBand = JsonConvert.DeserializeObject<BandModel>(band.Content.ReadAsStringAsync().Result);

        return postedBand.Id;
    }

    public int MoveSongtoTheBand(string accessToken, Guid bandId, Guid songId)
    {
        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var postContent = new StringContent(JsonConvert.SerializeObject(new SongModel(bandId)));
        postContent.Headers.ContentType = new MediaTypeHeaderValue("Application/Json");

        var request = new HttpRequestMessage(new HttpMethod("PATCH"), ApiUrls.Api + $"songs({songId})") { Content = postContent };
        var band = toApi.SendAsync(request).Result;

        return (int)band.StatusCode;
    }
}
