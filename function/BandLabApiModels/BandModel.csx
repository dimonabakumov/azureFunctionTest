#load "Entity.csx"

using System;

public class BandModel : Entity
{
    public string Name { get; set; }

    public string Username { get; set; }

    public BandModel()
    {
        Name = "Azure function band";
    }
}
