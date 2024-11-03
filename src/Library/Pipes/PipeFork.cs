using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompAndDel;

namespace CompAndDel.Pipes
{
    public class PipeFork : IPipe
    {
        private IPipe next2Pipe;
        private IPipe nextPipe;
        private FaceFilter facefilter;

        /// <summary>
        /// La cañería recibe una imagen, la clona y envía la original por una cañería y la clonada por otra.
        /// </summary>
        /// <param name="faceFilter">Filtro condicional para determinar la bifurcación</param>
        /// <param name="nextPipe">Cañería a la que se enviará la imagen si el filtro condicional es verdadero</param>
        /// <param name="next2Pipe">Cañería a la que se enviará la imagen si el filtro condicional es falso</param>
        public PipeFork(FaceFilter faceFilter, IPipe nextPipe, IPipe next2Pipe)
        {
            this.facefilter = faceFilter;
            this.next2Pipe = next2Pipe;
            this.nextPipe = nextPipe;
        }

        /// <summary>
        /// La cañería recibe una imagen, la filtra y decide a qué cañería enviarla basándose en el resultado del filtro condicional.
        /// </summary>
        /// <param name="picture">Imagen a filtrar y enviar a las siguientes cañerías</param>
        public IPicture Send(IPicture picture)
        {
            
            // Aplicar el filtro condicional
            picture = facefilter.Filter(picture);
        // Bifurcar en base a si se encontró una cara o no
            if (facefilter.HasFace)
            {
                return nextPipe.Send(picture);
            }
            else
            {
                return next2Pipe.Send(picture);
            }
        }
    }
}