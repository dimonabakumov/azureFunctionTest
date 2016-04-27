#load "..\BandLabApiModels\RevisionModel.csx"
#load "..\BandLabApiModels\TrackModel.csx"
#load "..\BandLabApiModels\RegionModel.csx"
#load "..\BandLabApiModels\SongModel.csx"

using System;
using System.Text;
using System.Collections.ObjectModel;

public class Revision 
{
    public RevisionModel Create(Guid sampleId, bool isPublic = false, Guid? parentId = null, int tracks = 1, string partOfName = "Azure function song")
    {
        var revision = new RevisionModel
        {
            SampleId = sampleId,
            IsPublic = isPublic,
            Title = "Azure function revision",
            Description = "Azure function description @dimon",
            Tracks = new List<TrackModel>(),
            ParentId = parentId,
            Song = (!parentId.HasValue) ? new SongModel(partOfName) : null,
        };

        for (var i = 0; i < tracks; i++)
        {
            revision.Tracks.Add
                (
                    new TrackModel
                    {
                        Order = i,
                        Name = "Azure function track name",
                        Regions = new Collection<RegionModel>
                        {
                                new RegionModel
                                {
                                    StartPosition = 10,
                                    EndPosition = 20,
                                    SampleStartPosition = 10,
                                    SampleId = sampleId
                                }
                        }
                    }
                );
        }

        return revision;
    }

    public RevisionModel Update(bool isPublic)
    {
        return new RevisionModel
        {
            IsPublic = isPublic
        };
    }
}