﻿using System;
using CompAndDel.Pipes;
using CompAndDel.Filters;
using Ucu.Poo.Twitter;

namespace CompAndDel
{
    class Program
    {
        static void Main(string[] args)
        {
            //Carga de imagen
            PictureProvider provider = new PictureProvider();
            IPicture picture = provider.GetPicture(@"C:\Users\gasto\OneDrive\Escritorio\repos\PII_Pipes_Filters\src\Program\luke.jpg");
            
            //Inverso a la secuencia (Ejercicio 1)
            //Creamos PipeNull (Ejercicio 1)
            PipeNull pipeNull = new PipeNull();
            
            //Se crea el PipeSerial que contiene el filtro negativo y va hacia el PipeNull (Ejercicio 1)
            IFilter filterNegative = new FilterNegative();
            
            //Se creo una clase SaveFilter para guardar la imagen por cada transformacion (Ejercicio2)
            IFilter saveStep2 = new SaveFilter(@"C:\Users\gasto\OneDrive\Escritorio\repos\PII_Pipes_Filters\src\Program\output_result2.jpg");
            
            //Se creo una clase TwitterPost para publicar la imagen que se comunica con la API de Twitter(Ejercicio 3)
            IFilter tweetStep2 = new TwitterPost(@"C:\Users\gasto\OneDrive\Escritorio\repos\PII_Pipes_Filters\src\Program\output_result2.jpg", "4GRAM.");
            PipeSerial pipeNegative = new PipeSerial(filterNegative, new PipeSerial(saveStep2, new PipeSerial(tweetStep2, pipeNull)));
            
            //Creamos el PipeSerial pero contiene el filtro escala de grises (Ejercicio 1)
            IFilter filterGreyscale = new FilterGreyscale();
            
            //Se creo una clase SaveFilter para guardar la imagen por cada transformacion (Ejercicio2)
            IFilter saveStep1 = new SaveFilter(@"C:\Users\gasto\OneDrive\Escritorio\repos\PII_Pipes_Filters\src\Program\output_result1.jpg");
            //Se creo una clase TwitterPost para publicar la imagen que se comunica con la API de Twitter(Ejercicio 3)
            IFilter tweetStep1 = new TwitterPost(@"C:\Users\gasto\OneDrive\Escritorio\repos\PII_Pipes_Filters\src\Program\output_result1.jpg", "4GRAM.");
            PipeSerial pipeGreyscale = new PipeSerial(filterGreyscale, new PipeSerial(saveStep1, new PipeSerial(tweetStep1, pipeNegative)));
            
            //Enviamos la imagen (Ejercicio 1)
            IPicture resultImage = pipeGreyscale.Send(picture);
            
            //Guardamos la imagen (Ejercicio 1)
            provider.SavePicture(resultImage, @"C:\Users\gasto\OneDrive\Escritorio\repos\PII_Pipes_Filters\src\Program\output_result.jpg");
            
            //Guardamos la imagen final 
            string outputPath = @"C:\Users\gasto\OneDrive\Escritorio\repos\PII_Pipes_Filters\src\Program\output_result.jpg";
            provider.SavePicture(resultImage, outputPath);
            
            // Publicar en Twitter el resultado final
            TwitterPost finalTweet = new TwitterPost(outputPath, "4GRAM (G aston,R odrigo,A lejandro,M auricio");
            finalTweet.Filter(resultImage);
        }
    }
}
