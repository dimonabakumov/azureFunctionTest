#load "Services\CreateMe.csx"
#load "Services\RevisionService.csx"
#load "TableStorage\Queries\SlackModelQuery.csx"
#load "ModelsGenerator\Registration.csx"

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
            var slackUserId = coll.Get("user_id").Trim('\'');
            return ExecCommand(int.Parse(text[1]), slackUserId);
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
            case 1:
                var reg = new CreateMe().Auth(sessionId);
                return new { text = "Registered! Username : " + reg.UserName + " Password : " + reg.Password };
                break;

            case 2:
                var revision = new RevisionService().CreateIfNotExists(sessionId);
                var newLiker = new Authorisation().GetUser(new Registration().Generate(), sessionId, Actions.LikeYourRevision);
                var like = new Revisions().Like(revision, newLiker);
                if (like == 202)
                    return new { text = "Liked" };
                else
                    return new { text = "Something went wrong" };
                break;

            //case 3:
            //    return 3;
            //    break;

            default:
                return new {text = "Unknown command"};
                break;
        }
    }
}
