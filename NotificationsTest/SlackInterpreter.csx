#load "Services\CreateMe.csx"
#load "Services\LikeCommentService.csx"
#load "Services\FollowService.csx"
#load "Services\InviteService.csx"
#load "Services\ShoutService.csx"
#load "TableStorage\Queries\SlackModelQuery.csx"

#load "Requests\Shouts.csx"

using System;
using System.Web;

public class SlackInterpreter
{
    protected string Username { get; set; }

    protected string Password { get; set; }

    public object ExtractCommand(string request)
    {
        var coll = System.Web.HttpUtility.ParseQueryString(request);
        var noted = new SlackModelQuery(coll);

        try
        {
            var text = coll.Get("text").Trim('\'').Split(' ');
            var sessionId = coll.Get("user_id").Trim('\'');
            if (int.Parse(text[1]) == 1)
            {
                Username = text[2].Split('/')[0];
                Password = text[2].Split('/')[1];
            }
            return ExecCommand(int.Parse(text[1]), sessionId);
        }
        catch (Exception ex)
        {
            return new { text = "Can't parse your command" };
            //return new {exeprion = ex};
        }
    }

    public object ExecCommand(int digitCommand, string sessionId)
    {
        switch (digitCommand)
        {
            // Help

            case 0:
                return new
                {
                    text = "Existing commands(triggered word is *go*) : \r\n"
                    + "0 - Help \r\n "
                    + "1 - Authorise a user(go 1 username/password) \r\n "
                    + "2 - Follow me \r\n "
                    + "3 - Unfollow me \r\n "
                    + "4 - Like my revision \r\n "
                    + "5 - Dislike my revision \r\n "
                    + "6 - Comment my revision \r\n "
                    + "7 - Comment my revision with mintion me \r\n "
                    + "8 - Delete all comments from revision \r\n "
                    + "9 - New song in my band \r\n "
                    + "10 - New revision in any song \r\n "
                    + "11 - New revision based on my revision \r\n "
                    + "12 - Like my shout \r\n "
                    + "13 - Dislike my shout \r\n "
                    + "14 - Comment my shout \r\n "
                    + "15 - Comment my shout with mintion me \r\n "
                    + "16 - Delete all comments from shout \r\n "
                    + "17 - User sends request to join my band \r\n "
                    + "18 - User invites me in their band \r\n "
                    + "19 - User invites me to collaborate \r\n "
                    + "20 - User joined my band \r\n "
                    + "21 - User joined my song \r\n "
                    + "22 - User fork my song, create new revision and publish it \r\n "
                };
                break;

            // Authorise a user

            case 1:
                var reg = new CreateMe().Auth(sessionId, Username, Password);
                return reg ? new { text = "User " + Username + " with Password : " + Password + " authorised" } : new { text = "Unsuccessful authorisation" };
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
                return new { text = new LikeCommentService().Like(sessionId) };
                break;

            // Dislike my revision

            case 5:
                return new { text = new LikeCommentService().Dislike(sessionId) };
                break;

            // Comment my revision

            case 6:
                return new { text = new LikeCommentService().LeftComment(sessionId) };
                break;

            // Comment my revision with mintion me

            case 7:
                return new { text = new LikeCommentService().LeftComment(sessionId, true) };
                break;

            // Delete all comments from revision

            case 8:
                return new { text = new LikeCommentService().DeleteComment(sessionId) };
                break;

            // New song in my band

            case 9:
                return new { text = new RevisionService().NewSongInTheBand(sessionId) };
                break;

            // New revision in any song

            case 10:
                return new { text = new RevisionService().NewRevisionInAnySong(sessionId) };
                break;

            // New revision based on my revision

            case 11:
                return new { text = new RevisionService().NewRevisionBasedOnMy(sessionId) };
                break;

            // Like my shout

            case 12:
                return new { text = new ShoutService().Like(sessionId) };
                break;

            // Dislike my shout

            case 13:
                return new { text = new ShoutService().Dislike(sessionId) };
                break;

            // Comment my shout

            case 14:
                return new { text = new ShoutService().LeftComment(sessionId) };
                break;

            // Comment my shout with mintion me

            case 15:
                return new { text = new ShoutService().LeftComment(sessionId, true) };
                break;

            // Delete comment from shout

            case 16:
                return new { text = new ShoutService().DeleteComment(sessionId) };
                break;

            // User sends request to join my band

            case 17:
                return new { text = new InviteService().RequestToJoinBand(sessionId, withAccept: true) };
                break;

            // User invites me in their band

            case 18:
                return new { text = new InviteService().InviteTo(sessionId) };
                break;

            // User invites me to collaborate

            case 19:
                return new { text = new InviteService().InviteTo(sessionId, Actions.InviteToSong) };
                break;

            // User joined my band

            case 20:
                return new { text = new InviteService().InviteTo(sessionId, Actions.JoinedYourBand) };
                break;

            // User joined my song

            case 21:
                return new { text = new InviteService().InviteTo(sessionId, Actions.JoinedYourSong) };
                break;

            // User fork my revision, create new and publish it

            case 22:
                return new { text = new RevisionService().PublishForkedRevision(sessionId) };
                break;

            // Unknown command

            default:
                return new {text = "Unknown command"};
                break;
        }
    }
}
