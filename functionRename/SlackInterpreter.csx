#load "Services\CreateMe.csx"
#load "Services\RevisionService.csx"
#load "TableStorage\Queries\SlackModelQuery.csx"
#load "TableStorage\Queries\UserModelQuery.csx"
#load "ModelsGenerator\Registration.csx"
#load "Requests\Following.csx"
#load "Requests\Revisions.csx"

using System;
using System.Web;

public class SlackInterpreter
{
    public object ExtractCommand(string request)
    {
        var coll = System.Web.HttpUtility.ParseQueryString(request);
        var noted = new SlackModelQuery(coll);

        try
        {
            var text = coll.Get("text").Trim('\'').Split(' ');
            var sessionId = coll.Get("user_id").Trim('\'');
            return ExecCommand(int.Parse(text[1]), sessionId);
        }
        catch (Exception ex)
        {
            return new {exeprion = ex};
        }
    }

    public object ExecCommand(int digitCommand, string sessionId)
    {
        switch (digitCommand)
        {
            // Help

            case 0:
                return new { text = "Help is here..."};
                break;

            // Reg/Auth user

            case 1:
                var reg = new CreateMe().Auth(sessionId);
                return new { text = "Registered! Username : " + reg.UserName + " Password : " + reg.Password };
                break;

            // Follow me

            case 2:
                var me = new UserModelQuery().Get(sessionId, Actions.Me);
                var newFollower = new Authorisation().GetUser(new Registration().Generate(), sessionId, Actions.FollowYou);
                var follow = new Following().Follow(me.Id, newFollower);
                if (follow == 204)
                    return new { text = "Followed" };
                else
                    return new { text = "Something went wrong" };
                break;

            // Unfollow me

            case 3:
                return new { text = "Not ready yet" };
                break;

            // Like my revision

            case 4:
                var revision = new RevisionService().CreateIfNotExists(sessionId);
                var newLiker = new Authorisation().GetUser(new Registration().Generate(), sessionId, Actions.LikeYourRevision);
                var like = new Revisions().Like(revision, newLiker);
                if (like == 202)
                    return new { text = "Liked" };
                else
                    return new { text = "Something went wrong" };
                break;

            // Unlike my revision

            case 5:
                return new { text = "Not ready yet" };
                break;

            // Unknown command

            default:
                return new {text = "Unknown command"};
                break;
        }
    }
}
