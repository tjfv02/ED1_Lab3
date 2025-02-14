﻿using ArbolBinario_Farmacia.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ArbolBinario_Farmacia.Controllers
{
    public class ArbolController : Controller
    {
        //LIsta de pedidos 
        public static List<Pedido> Pedidos = new List<Pedido>(); // Todos los pedidos 
        public static List<Nodo> ListaRecorridos = new List<Nodo>(); // Lista Recorridos

        public static List<Nodo> Reabastecido = new List<Nodo>(); // Lista Reabastecer

        public static Nodo Raiz;
        public static Nodo nodo;

        //GET
        public ActionResult PaginaPrincipal()
        {
            return View();
        }

        //Post
        [HttpPost]
        public ActionResult PaginaPrincipal(FormCollection collection)
        {
            return View();
        }

        public ActionResult dummy()
        {
            Pedido auxPedido;
            

            auxPedido = Pedidos[Pedidos.Count - 1];
            foreach (Detalle auxDetalle in auxPedido.detalle)
            {
                Nodo auxFarmaco;
                auxFarmaco = BuscarNodo(auxDetalle.Linea, Raiz);
                auxFarmaco.Existencia -= auxDetalle.Cantidad;
               

                ////REABASTECER 
                //Random rnd = new Random();
                //int Reabastecer = rnd.Next(1, 15);
                
                //auxFarmaco.Existencia = Reabastecer;

            }
            // return View(Pedidos[Pedidos.Count - 1].detalle);

            return View(Pedidos[Pedidos.Count-1]);
        }

        //GET
        public ActionResult OverStock()
        {
            return View();
        }

        //Llena una nueva lista con los datos del farmaco a eliminar, reabasteciendo
        //GET
        public ActionResult Reabastecer()
        {
            Pedido auxPedido;
            Random rnd = new Random();
            int Reabastecer = rnd.Next(1, 15);

            auxPedido = Pedidos[Pedidos.Count-1];
            foreach (Detalle auxDetalle in auxPedido.detalle)
            {
                Nodo auxFarmaco;
                auxFarmaco = BuscarNodo(auxDetalle.Linea, Raiz);
                auxFarmaco.Existencia = Reabastecer;


                ////REABASTECER 
                

                //auxFarmaco.Existencia = Reabastecer;

            }
            // return View(Pedidos[Pedidos.Count - 1].detalle);

            return View(Pedidos[Pedidos.Count-1]);
        }
        //Post
        [HttpPost]
        public ActionResult Reabastecer(FormCollection collection)
        {
            //Random rnd = new Random();
            //int ReabastecerRandom = rnd.Next(1, 15);

            //Nodo NuevoReabastecer = new Nodo()
            //{
            //    Linea = Convert.ToInt32(collection["Linea"]),
            //    Nombre = collection["Nombre"],
            //    Descripcion = collection["Descripcion"],
            //    Casa_productora = collection["Casa_productora"],
            //    Precio = Convert.ToDouble(collection["Precio"]),
            //    Existencia = ReabastecerRandom
                

            //};
            //Reabastecido.Add(NuevoReabastecer);
            return View();
        }

        //GET
        public ActionResult NuevoDetalle()
        {
            //Detalle detalleNuevo = new Detalle();
            //Pedidos[Pedidos.Count - 1].detalle.Add(detalleNuevo);
            return View(Pedidos[Pedidos.Count-1]);
        }
        //GET
        public ActionResult AgregarDetalle()
        {
            Detalle datos = new Detalle
            {
                Linea = nodo.Linea,
                Nombre = nodo.Nombre,
                Descripcion=nodo.Descripcion,
                Precio=nodo.Precio
            };
            return View(datos);
        }

        //Post 
        [HttpPost]
        public ActionResult AgregarDetalle(FormCollection collection)
        {
            //Lista
            Detalle NuevoDetalle = new Detalle
            {
                //Campos
                Linea = nodo.Linea,
                Nombre=nodo.Nombre,
                Descripcion = nodo.Descripcion,
                 Cantidad = Convert.ToInt32(collection["Cantidad"]),
                Precio = nodo.Precio
            
            };
            Nodo auxBusqueda;
            auxBusqueda = BuscarNodo(NuevoDetalle.Linea, Raiz);
            if (NuevoDetalle.Cantidad>auxBusqueda.Existencia)
            {
                return RedirectToAction("OverStock");
                //No hay suficientes
            }
            else
            {

                NuevoDetalle.Total_a_Cancelar = NuevoDetalle.Cantidad * NuevoDetalle.Precio;
                Pedidos[Pedidos.Count - 1].detalle.Add(NuevoDetalle);
                //Suma los totales
                Pedidos[Pedidos.Count - 1].Total += NuevoDetalle.Total_a_Cancelar;


                return RedirectToAction("NuevoDetalle");
            }
           

             

           
        }

        //Post
        [HttpPost]
        public ActionResult NuevoDetalle(FormCollection collection)
        {
            //Cargando los datos agregados al 
            nodo = BuscarNodo(Convert.ToInt32(collection["Linea"]), Raiz);
            return RedirectToAction("AgregarDetalle");
        }
        // Datos del cliente 

        // GET
        public ActionResult NuevoPedido()
        {
            return View();
        }
        //Post
        [HttpPost]
        public ActionResult NuevoPedido(FormCollection collection)
        {
            Pedido NuevoPedido = new Pedido()
            {
                Id = Pedidos.Count,
                Nombre = collection["Nombre"],
                Nit = collection["Nit"],
                Direccion = collection["Direccion"],


            };
            Pedidos.Add(NuevoPedido);

            return RedirectToAction("NuevoDetalle");
        }
        //Carga de Archivo
        //GET
        public ActionResult CargaArch()
        {
            ViewBag.Message = "Elección de archivo";
            return View();
        }
        //Post
        [HttpPost]
        public ActionResult Carga(HttpPostedFileBase postedFile)
        {

            string directarchivo = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Cargas/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                directarchivo = path + Path.GetFileName(postedFile.FileName);
                postedFile.SaveAs(directarchivo);
                //Caja_arbol.Instance.direccion_archivo_arbol = directarchivo;
            }
            //Modificación de los digitos de la exitencia
            using (var archivo = new FileStream(directarchivo, FileMode.Open))
            {
                using (var archivolec = new StreamReader(archivo))
                {
                    string lector = archivolec.ReadLine();
                   lector= archivolec.ReadLine();
                    while (lector != null)
                    {
                        Regex regx = new Regex("," + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        string[] infor_separada = regx.Split(lector);
                        if (infor_separada[infor_separada.Length - 1].Length < 2)
                        {
                            infor_separada[infor_separada.Length - 1] = "0" + infor_separada[infor_separada.Length - 1];

                        }
                        if (infor_separada.Length==6)
                        {
                            Nodo Insertado = NuevoHijo(Convert.ToInt32(infor_separada[0]), ref Raiz, ref Raiz);
                            Insertado.Nombre = infor_separada[1];
                            Insertado.Descripcion = infor_separada[2];
                            Insertado.Casa_productora = infor_separada[3];
                            Insertado.Precio = Convert.ToDouble(infor_separada[4].Replace("$", ""));
                            Insertado.Existencia = Convert.ToInt32(infor_separada[5]);
                            lector = archivolec.ReadLine();
                        }
                        lector = archivolec.ReadLine();

                    }
                    Raiz = LlamarBalanceo(Raiz);
                }
            }
            


            return RedirectToAction("PaginaPrincipal") ;
        }





        // GET: Arbol
        public ActionResult Index()
        {
            return View();
        }

        // GET: Arbol/Details/5
        public ActionResult Details(int id)
        {
            Nodo Visualizar =BuscarNodo(id,Raiz);


            if (Visualizar == null)
            {
                return View();
            }
            else
            {
                return View(Visualizar);
            }

        }

        // GET: Arbol/Create
        public ActionResult Insertar()
        {
            return View();
        }

        // POST: Arbol/Create
        [HttpPost]
        public ActionResult Insertar(FormCollection collection)
        {
            try
            {
                int linea = Convert.ToInt32(collection["Linea"]);
                string nombre = collection["Nombre"];
                Nodo insertado = NuevoHijo(linea,ref Raiz, ref Raiz);

                insertado.Nombre = nombre;

                return RedirectToAction("Insertar");
            }
            catch
            {
                return View();
            }
        }
        // GET: Arbol/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Arbol/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                int linea = Convert.ToInt32(collection["Linea"]);
                ELiminar(linea);

                


                return RedirectToAction("NuevoDetalle");
            }
            catch
            {
                return View();
            }
        }

        // GET: Arbol/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Arbol/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        void QuitarProductos(int Linea, Nodo nodoQuitar)
        {
            Nodo NodoSinExistencia;
            NodoSinExistencia=BuscarNodo(Linea, nodoQuitar);
            if (NodoSinExistencia.Existencia==0)
            {
                //mandarlos a otra lista
                Reabastecer();
                ELiminar(NodoSinExistencia.Linea);
            }
        }

        

        //----------------------------------Metodos Arbol------------------------------------------------
        Nodo NuevoHijo(int linea,ref  Nodo hoja,ref Nodo padre, int Altura=0)
        {

            if (hoja==null)
            {
                hoja = new Nodo();
                hoja.Linea = linea;
                hoja.Padre = padre;
                hoja.Altura = Altura;
                return hoja;
            }
            // si es igual
            else if (linea==hoja.Linea)
            {
                return hoja;
            }
            //Si es mayor
            else if (linea>hoja.Linea)
            {
               return NuevoHijo(linea,ref  hoja.Derecho,ref hoja,Altura+1);
            }
            else
            {
                return NuevoHijo(linea, ref hoja.Izquierdo, ref hoja,Altura+1);
            }

        }

        Nodo BuscarNodo(int linea,Nodo hoja)
        {
            if (hoja==null)
            {
                return null;
            }
            else if (hoja.Linea==linea)
            {
                return hoja; // Valor encontrado
            }
            else if (linea>hoja.Linea)
            {
                return BuscarNodo(linea, hoja.Derecho);
            }
            else
            {
                return BuscarNodo(linea, hoja.Izquierdo);
            }
        }
       
        void ELiminar(int linea)
        {
            Nodo NodoELiminado;
            Nodo auxDerecho;
            Nodo auxIzquierdo;
            NodoELiminado = BuscarNodo(linea, Raiz); // Encontrado

            auxDerecho = NodoELiminado.Derecho;
            auxIzquierdo = NodoELiminado.Izquierdo;
            if (auxDerecho==null)
            {
                auxDerecho = auxIzquierdo;
                auxIzquierdo = null;
            }
            NodoELiminado = auxDerecho;

            if (auxIzquierdo!=null)
            {
                Nodo auxInferior;
                auxInferior = auxDerecho;
                while (auxInferior.Izquierdo != null)
                {
                    auxInferior = auxInferior.Izquierdo;
                }
                auxInferior.Izquierdo = auxIzquierdo;
            }
            Raiz = LlamarBalanceo(Raiz); //Despues de eliminar el nodo, se debe balancear el arbol
        }
        Nodo NecesitaReabastecimiento(int linea)
        {

            return Raiz;
        }
        //                                      Recorridos
        //---------------------------------------------------------------------------------------------------
        void listarPreOrden( )
        {
            PreOrden(Raiz, ListaRecorridos);
        }
        void listarInOrden()
        {
            InOrden(Raiz, ListaRecorridos);
        }
        void listarPostOrden()
        {
            PostOrden(Raiz, ListaRecorridos);
        }


        void PreOrden(Nodo NodoRecorrido, List<Nodo> Lista)
        {
            //PreOrden
            Lista.Add(NodoRecorrido);
            if (NodoRecorrido.Derecho!=null)
            {
                PreOrden(NodoRecorrido.Derecho, Lista);
            }
            if (NodoRecorrido.Izquierdo != null)
            {
                PreOrden(NodoRecorrido.Izquierdo, Lista);
            }
        }
        void InOrden(Nodo NodoRecorrido, List<Nodo> Lista)
        {
            if (NodoRecorrido.Derecho != null)
            {
                InOrden(NodoRecorrido.Derecho, Lista);
            }
            Lista.Add(NodoRecorrido);
            if (NodoRecorrido.Izquierdo != null)
            {
                InOrden(NodoRecorrido.Izquierdo, Lista);
            }
        }
        void PostOrden(Nodo NodoRecorrido, List<Nodo> Lista)
        {
            if (NodoRecorrido.Derecho != null)
            {
                PostOrden(NodoRecorrido.Derecho, Lista);
            }
            if (NodoRecorrido.Izquierdo != null)
            {
                PostOrden(NodoRecorrido.Izquierdo, Lista);
            }
            Lista.Add(NodoRecorrido);
        }


        //                                      Funciones AVL
        //---------------------------------------------------------------------------------------------------
        //Función para obtener que rama es mayor
        int max(int lhs, int rhs)
        {
            return lhs > rhs ? lhs : rhs;
        }
        int Alturas(Nodo Raiz)
        {
            return Raiz == null ? -1 : Raiz.Altura;
        }

        //Funcion para obtener la Altura del arbol
        int getAltura(Nodo nodoActual)
        {
            if (nodoActual == null)
                return 0;
            else
                return 1 + Math.Max(getAltura(nodoActual.Izquierdo), getAltura(nodoActual.Derecho));
        }

        //Balanceo de arbol AVL
        public Nodo LlamarBalanceo(Nodo inicio)
        {


            if (inicio.Izquierdo != null)
            {
                inicio.Izquierdo = LlamarBalanceo(inicio.Izquierdo);
            }

            if (inicio.Derecho != null)
            {
                inicio.Derecho = LlamarBalanceo(inicio.Derecho);
            }
            int a = NecesitoBalanceo(inicio);
            if (a >= -1 && a <= 1)
            {
                //Nodo balanceado
            }
            else
            {
                if (a > 1)
                {
                    int b = NecesitoBalanceo(inicio.Izquierdo);
                    if (b < 0)
                    {

                        inicio = RotacionDerechoDoble(inicio);
                    }
                    else
                    {
                        inicio = RotacionDerechoSimple(inicio);
                    }

                }
                else
                {
                    int c = NecesitoBalanceo(inicio.Derecho);
                    if (c > 0)
                    {
                        inicio = RotacionIzquierdoDoble(inicio);
                    }
                    else
                    {
                        inicio = RotacionIzquierdoSimple(inicio);
                    }
                }

            }

            return inicio;
        }
        public int NecesitoBalanceo(Nodo padre)
        {
            padre.calcularAltura();
            int a = 0;
            if (padre.Izquierdo == null && padre.Derecho == null)
            {
                a = 0;
            }
            else if (padre.Izquierdo == null && padre.Derecho != null)
            {
                a = 0 - padre.Derecho.Altura;
            }
            else if (padre.Izquierdo != null && padre.Derecho == null)
            {
                a = padre.Izquierdo.Altura;
            }
            else if (padre.Izquierdo != null && padre.Derecho != null)
            {
                a = padre.Izquierdo.Altura - padre.Derecho.Altura;
            }

            return a;
        }

        //Rotacion Izquierdo Simple
        Nodo RotacionDerechoSimple(Nodo k2)
        {
            Nodo k1 = k2.Izquierdo;
            k2.Izquierdo = k1.Derecho;
            k1.Derecho = k2;
            k2.Altura = max(Alturas(k2.Izquierdo), Alturas(k2.Derecho)) + 1;
            k1.Altura = max(Alturas(k1.Izquierdo), k2.Altura) + 1;
            return k1;
        }
        //Rotacion Derecho Simple
        Nodo RotacionIzquierdoSimple(Nodo k1)
        {
            Nodo k2 = k1.Derecho;
            k1.Derecho = k2.Izquierdo;
            k2.Izquierdo = k1;
            k1.Altura = max(Alturas(k1.Izquierdo), Alturas(k1.Derecho)) + 1;
            k2.Altura = max(Alturas(k2.Derecho), k1.Altura) + 1;
            return k2;
        }
        //Doble Rotacion Izquierdo
        Nodo RotacionDerechoDoble(Nodo k3)
        {
            k3.Izquierdo = RotacionIzquierdoSimple(k3.Izquierdo);
            return RotacionDerechoSimple(k3);
        }
        //Doble Rotacion Derecho
        Nodo RotacionIzquierdoDoble(Nodo k1)
        {
            k1.Derecho = RotacionDerechoSimple(k1.Derecho);
            return RotacionIzquierdoSimple(k1);
        }

    }
}
