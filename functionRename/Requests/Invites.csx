#load "..\Configuration\ApiUrls.csx"
#load "..\Configuration\JsonConfiguration.csx"
#load "..\BandLabApiModels\RevisionModel.csx"
#load "..\BandLabApiModels\InviteModel.csx"

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Invites
{
    public Guid PostInvite(Guid entityId, string inviterAccessToken, string userId, bool toBand = true)
    {
        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", inviterAccessToken);
        var postContent = new StringContent(JsonConvert.SerializeObject(new InviteModel(Guid.Parse(userId))));
        postContent.Headers.ContentType = new MediaTypeHeaderValue("Application/Json");
        var invUrl = toBand ? $"bands({entityId})/invites" : $"songs({entityId})/invites";
        var invite = toApi.PostAsync(ApiUrls.Api + invUrl, postContent).Result;

        //Extract server response

        var postedInvite = JsonConvert.DeserializeObject<List<InviteModel>>(invite.Content.ReadAsStringAsync().Result);

        return postedInvite[0].InviteId;
    }

    public void AcceptAnInvite(Guid inviteId, string accessToken)
    {
        var toApi = new HttpClient();
        toApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var invite = toApi.PutAsync(ApiUrls.Api + $"invites({inviteId})", null).Result;
        var acceptedInvite = invite.Content.ReadAsStringAsync().Result;
    }
}
