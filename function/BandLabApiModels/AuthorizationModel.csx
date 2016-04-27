#load "Entity.csx"

using System;

public class AuthorizationModel : Entity
{
    public string Login { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string RefreshToken { get; set; }

    public bool RememberMe { get; set; }

    public string Provider { get; set; }

    public bool Register { get; set; }

    public bool IsPrimary { get; set; }

    public string Type { get; set; }

    public string ClientId { get; set; }

    public string AccessToken { get; set; }
}
