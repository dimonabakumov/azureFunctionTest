#load "..\BandLabApiModels\AuthorizationModel.csx"

using System;
using System.Text;

public class Registration
{
    private static readonly Random Rand = new Random();
    private const string EnglishAlphabet = "abcdefghijklmnopqrstuvwxyz";
    private const string AllowedUserChars = "'.-";

    public AuthorizationModel Generate(bool register = true)
    {
        var newUser = new AuthorizationModel
        {
            Password = "password",
            Name = RandomName()
        };

        newUser.Email = string.Format("{0}@bandlab.com", newUser.Name);
        newUser.Register = register;
        newUser.RememberMe = true;
        newUser.ClientId = "Angular";
        newUser.Provider = "Password";

        return newUser;
    }

    private string RandomName()
    {
        var randomName = new StringBuilder();

        for (int i = 0; i < 15; i++)
        {
            if (i == 10)
                randomName.Append(AllowedUserChars[Rand.Next(AllowedUserChars.Count() - 1)]);
            randomName.Append(EnglishAlphabet[Rand.Next(EnglishAlphabet.Length)].ToString());
        }

        return randomName.ToString();
    }
}