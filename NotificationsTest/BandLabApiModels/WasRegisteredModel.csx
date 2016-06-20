#load "Entity.csx"

using System;

public class WasRegisteredModel : Entity
{
    public string SessionKey { get; set; }

    public string RefreshToken { get; set; }

    public DateTimeOffset ExpiryDate { get; set; }

    public string Provider { get; set; }

    public bool WasRegistered { get; set; }
}
