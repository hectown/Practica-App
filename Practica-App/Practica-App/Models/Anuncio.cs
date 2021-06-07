using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_App.Models
{
    public class Anuncio
    {
        public Int32 Id { get; set; }
        public string Titulo { get; set; }
        public string Tipo { get; set; }
        public Int32 Precio { get; set; }

        public string Imagen { get; set; }
    }
}
