using System;

public class CommentModel
{
    public long Id { get; set; }

    public string Content { get; set; }

    public float Timestamp { get; set; }

    public CommentModel(string username = null)
    {
        if(username != null)
            Content = "Azure function content of comment with mention " + username;
        else
            Content = "Azure function content of comment";
    }
}
