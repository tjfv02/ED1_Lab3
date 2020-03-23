using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArbolBinario_Farmacia.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        //Cliente
        public string Nombre        {get; set;}
        public string Nit           {get; set;}
        public string Direccion     {get; set;}

        public List<Detalle> detalle= new List<Detalle>();
        public int Linea  { get; set; }
        public double Total { get; set; }
    }
}