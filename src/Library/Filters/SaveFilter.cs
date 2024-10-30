using System;

namespace CompAndDel.Filters;

public class SaveFilter : IFilter
{
    private string Path;

    public SaveFilter(string path)
    {
        Path = path;
    }

    public IPicture Filter(IPicture image)
    {
        PictureProvider provider = new PictureProvider();
        provider.SavePicture(image, Path);
        Console.WriteLine($"Imagen guardada en: {Path}");
        return image;
    }
}