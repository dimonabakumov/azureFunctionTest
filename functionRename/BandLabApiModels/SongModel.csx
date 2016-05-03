#load "Entity.csx"

using System;

public class SongModel : Entity
{
    public string Name { get; set; }

    public Guid? BandId { get; set; }

    public bool Forkable { get; set; }

    public SongModel() { }

    public SongModel(string name)
    {
        Name = name;
        Forkable = true;
    }

    public SongModel(Guid bandId)
    {
        BandId = bandId;
        Forkable = true;
    }
}
