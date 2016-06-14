#load "Entity.csx"

using System;

public class InviteModel : Entity
{
    public List<Guid> UserIds { get; set; }

    public string Message { get; set; }

    public Guid InviteId { get; set; }

    public InviteModel(Guid userId)
    {
        UserIds = new List<Guid> { userId };
        Message = "Azure message for invitation";
    }
}
