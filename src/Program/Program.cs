using System;
using CompAndDel.Pipes;
using CompAndDel.Filters;
using Ucu.Poo.Cognitive;
using Ucu.Poo.Twitter;

namespace CompAndDel
{
    class Program
    {
        static void Main(string[] args)
        {
            //Carga de imagen
            PictureProvider provider = new PictureProvider();
            IPicture picture = null;
            try
            {
                // Carga de la imagen
                picture = provider.GetPicture(@"C:\repos\PipesFilters\src\Program\luke.jpg");
                Console.WriteLine("Imagen cargada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar la imagen: " + ex.Message);
                return;
            }
            
            
            //instanciamos el Cognitive y Facefilter
            CognitiveFace cognitiveFace = new CognitiveFace(markFaces: true);
            FaceFilter faceFilter = new FaceFilter(cognitiveFace);
            
            //Inverso a la secuencia (Ejercicio 1)
            //Creamos PipeNull (Ejercicio 1)
            PipeNull pipeNull = new PipeNull();
            
            //Se crea el PipeSerial que contiene el filtro negativo y va hacia el PipeNull (Ejercicio 1)
            IFilter filterNegative = new FilterNegative();
            
            //Se creo una clase SaveFilter para guardar la imagen por cada transformacion (Ejercicio2)
            IFilter saveStep2 = new SaveFilter(@"C:\repos\PipesFilters\src\Program\output_result2.jpg");
            //Se creo una clase TwitterPost para publicar la imagen que se comunica con la API de Twitter(Ejercicio 3)
            IFilter tweetStep2 = new TwitterPost(@"C:\repos\PipesFilters\src\Program\output_result2.jpg", "4GRAM. Negative");
            PipeSerial pipeNegative = new PipeSerial(filterNegative, new PipeSerial(saveStep2, new PipeSerial(tweetStep2, pipeNull)));
            
            //Creamos el PipeSerial pero contiene el filtro escala de grises (Ejercicio 1)
            IFilter filterGreyscale = new FilterGreyscale();
            
            //Se creo una clase SaveFilter para guardar la imagen por cada transformacion (Ejercicio2)
            IFilter saveStep1 = new SaveFilter(@"C:\repos\PipesFilters\src\Program\output_result1.jpg");
            //Se creo una clase TwitterPost para publicar la imagen que se comunica con la API de Twitter(Ejercicio 3)
            IFilter tweetStep1 = new TwitterPost(@"C:\repos\PipesFilters\src\Program\output_result.jpg", "4GRAM. Grayscale");
            PipeSerial pipeGreyscale = new PipeSerial(filterGreyscale, new PipeSerial(saveStep1, new PipeSerial(tweetStep1, pipeNegative)));

            //Bifurcacion creada segun el resultado del facefilter
            PipeFork pipeFork = new PipeFork(faceFilter, pipeNegative, pipeGreyscale);
            IPicture resultImage = null;
            
            try
            {
                // Enviar imagen a través del PipeFork
                resultImage = pipeFork.Send(picture);
                Console.WriteLine("Imagen procesada y filtrada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al aplicar el filtro y bifurcación: " + ex.Message);
            }

            try
            {
                // Guardar la imagen final
                string outputPath = @"C:\repos\PipesFilters\src\Program\output_result.jpg";
                provider.SavePicture(resultImage, outputPath);
                Console.WriteLine("Imagen final guardada en: " + outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar la imagen final: " + ex.Message);
            }

            // Publicación en Twitter solo si FaceFilter detecta una cara
            if (faceFilter.HasFace)
            {
                try
                {
                    TwitterPost finalTweet = new TwitterPost(@"C:\repos\PipesFilters\src\Program\output_result2.jpg", "4GRAM: Cara detectada,filtro aplicado.");
                    finalTweet.Filter(resultImage);
                    Console.WriteLine("Imagen publicada en Twitter.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al publicar en Twitter: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("No se detectó una cara, solo se guardó el resultado final.");
            }
        }
    }
}