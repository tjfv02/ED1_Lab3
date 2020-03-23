using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArbolBinario_Farmacia.Models
{
    public class Nodo
    {
        //Datos
        public int Linea { get; set; }
        public string Nombre { get; set; }
        public string Descripcion {get; set;}
        public string Casa_productora {get; set;}
        public double Precio{get; set;}
        public int Existencia{get; set;}
        //hijos
        public Nodo Padre;
        public Nodo Izquierdo;
        public Nodo Derecho;

        //Control
        public int Altura { get; set; }
    }
}