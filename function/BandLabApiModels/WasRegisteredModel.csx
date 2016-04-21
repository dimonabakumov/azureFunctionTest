using System;

public class WasRegisteredModel
{
    public string SessionKey { get; set; }

    public string RefreshToken { get; set; }

    public DateTimeOffset ExpiryDate { get; set; }

    public string Provider { get; set; }

    public bool WasRegistered { get; set; }
}
