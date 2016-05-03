#load "Services\CreateMe.csx"
#load "Services\RevisionService.csx"
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
            var slackUserId = coll.Get("user_id").Trim('\'');
            return ExecCommand(int.Parse(text[1]), slackUserId);
        }
        catch (Exception ex)
        {
            return new {exeprion = ex};
        }
    }

    public object ExecCommand(int digitCommand, string userId)
    {
        switch (digitCommand)
        {
            case 1:
                return new CreateMe().Auth(userId);
                break;

            case 2:
                return new RevisionService().CreateIfNotExists(userId);
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
