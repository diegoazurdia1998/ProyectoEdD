using System;
using System.Collections.Generic;
namespace Salud_para_tu_sonrisa.Models
{
    public class Citas
    {
        public DateTime Fecha;
        public int Espacios;

        public Citas(DateTime fecha, int espacio)
        {
            this.Fecha = fecha;
            Espacios++;

        }
        
        
    }
}
