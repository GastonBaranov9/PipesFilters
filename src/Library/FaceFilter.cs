using System;
using Ucu.Poo.Cognitive;

namespace CompAndDel;

public class FaceFilter : IFilter
{
    private readonly CognitiveFace cognitiveFace;
    
    public bool HasFace { get;private set; }

    public FaceFilter(CognitiveFace cognitiveFace)
    {
        this.cognitiveFace = cognitiveFace;
        
    }

    public IPicture Filter(IPicture image)
    {
        if (image is null) throw new ArgumentNullException(nameof(image));
        
        string TempImagePath = "C:\\repos\\PipesFilters\\src\\Program\\Temporal.jpg";
        
        cognitiveFace.Recognize(TempImagePath);
        HasFace = cognitiveFace.FaceFound;
        return image;
    }
}