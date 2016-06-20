#load "Entity.csx"

using System;

public class SongModel : Entity
{
    public string Name { get; set; }

    public Guid? BandId { get; set; }

    public bool IsForkable { get; set; }

    public RevisionModel Revision { get; set; }

    public SongModel() { }

    public SongModel(string name)
    {
        Name = name;
        IsForkable = true;
    }

    public SongModel(Guid bandId)
    {
        Name = "PATCHED Azure song name";
        BandId = bandId;
        IsForkable = true;
    }
}
