#load "Services\CreateMe.csx"
#load "Services\RevisionService.csx"
#load "Services\FollowService.csx"
#load "TableStorage\Queries\SlackModelQuery.csx"

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
                return new { text = new FollowService().Follow(sessionId) };
                break;

            // Unfollow me

            case 3:
                return new { text = new FollowService().Unfollow(sessionId) };
                break;

            // Like my revision

            case 4:
                return new { text = new RevisionService().Like(sessionId) };
                break;

            // Dislike my revision

            case 5:
                return new { text = new RevisionService().Dislike(sessionId) };
                break;

            // Unknown command

            default:
                return new {text = "Unknown command"};
                break;
        }
    }
}
