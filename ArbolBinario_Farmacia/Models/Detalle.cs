using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArbolBinario_Farmacia.Models
{
    public class Detalle
    {
        public int Linea { get; set; }
        public string Nombre { get; set; }
        public string Descripcion {get; set;}
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public double Total_a_Cancelar {get; set;}
    }
}