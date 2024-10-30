using System;
using System.IO;
using Ucu.Poo.Twitter;

namespace CompAndDel.Filters;

public class TwitterPost : IFilter
{
    private string _imagePath;
    private string _message;
    private TwitterImage _twitter;

    public TwitterPost(string imagePath, string message)
    {
        _imagePath = imagePath;
        _message = message;

        // Inicializar la clase TwitterImage
        _twitter = new TwitterImage();
    }

    public IPicture Filter(IPicture image)
    {
        if (_twitter != null)
        {
            Console.WriteLine("Publicando en Twitter...");
            string result = _twitter.PublishToTwitter(_message, _imagePath);
            Console.WriteLine(result == "Ok" ? $"Imagen publicada en Twitter con mensaje: {_message}" : $"Error al publicar en Twitter: {result}");
        }
        else
        {
            Console.WriteLine("No se pudo inicializar la clase de Twitter.");
        }
        return image;
    }
}