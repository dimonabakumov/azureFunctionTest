#load "Entity.csx"
#load "RegionModel.csx"

using System;
using System.Collections.Generic;

public class TrackModel : Entity
{
    public int Order { get; set; }

    public string Name { get; set; }

    public ICollection<RegionModel> Regions { get; set; }
}
