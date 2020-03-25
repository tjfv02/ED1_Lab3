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

        //Función para obtener la altura del nodo
        public int calcularAltura()
        {

            if (Izquierdo == null && Derecho == null)
            {
                Altura = 1;
            }
            else
            {

                if (Izquierdo == null)
                {
                    Derecho.calcularAltura();
                    Altura = Derecho.Altura + 1;
                }
                else
                {
                    if (Derecho == null)
                    {
                        Izquierdo.calcularAltura();
                        Altura = Izquierdo.Altura + 1;
                    }

                    else
                    {
                        Izquierdo.calcularAltura();
                        Derecho.calcularAltura();
                        if (Derecho.Altura >= Izquierdo.Altura)
                        {
                            Altura = Derecho.Altura + 1;
                        }
                        else
                        {
                            Altura = Izquierdo.Altura + 1;
                        }
                    }

                }


            }

            return Altura;
        }
    }
}