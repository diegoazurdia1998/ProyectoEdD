using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Salud_para_tu_sonrisa.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataStructurs.TreeStructurs;

namespace Salud_para_tu_sonrisa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static AVLTree<String, Paciente> PacientesClinica = new AVLTree<string, Paciente>(new Models.Comparadores.KeyComparer().Comparer, "log.txt");
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        /// <summary>
        /// Recibe los datos del nuevo paciente
        /// </summary>
        /// <returns>View con la recepción de datos</returns>
        public IActionResult recibirDatos()
        {
            return View();
        }
        /// <summary>
        /// Guarda al nuevo paceinte en el árbol
        /// </summary>
        /// <param name="name">Nombre del paceinte</param>
        /// <param name="id">DPI o partida de nacimiento  del paceinte</param>
        /// <param name="age">Edad  del paceinte</param>
        /// <param name="phone">Teléfono de contacto  del paceinte</param>
        /// <param name="last">Última vicita  del paceinte</param>
        /// <param name="next">Próxima cita  del paceinte</param>
        /// <param name="desc">Descripción del diagnóstico o tratamiento del paceinte</param>
        /// <returns>View con la confirmación de guardado</returns>
        public IActionResult guardarDatos(String name, String id, int age, String phone, DateTime last, DateTime next = default, String desc = default)
        {
            Paciente nuevoIngreso = new Paciente(name, id, age, phone, last, next, desc);
            PacientesClinica.Add(id, nuevoIngreso);
            ViewBag.PacienteIngresado = name + ", con ID " + id + ", contacto " + phone;
            
            return View();
        }
        /// <summary>
        /// Muestra la lista de pacientes
        /// </summary>
        /// <returns>View con la lista de pacientes</returns>
        public IActionResult mostrarPacientes()
        {
            if (PacientesClinica.getCount() == 0)
            {
                Random rand = new Random();
                String desc;
                int descRand;
                int randString;
                DateTime prev = new DateTime(2021, 06, 25, 11, 50, 50);
                for (int i = 10; i < 40; i++)
                {
                    randString = rand.Next(10, 99);
                    descRand = rand.Next(0,4);
                    if (descRand == 0)
                        desc = default;
                    else if (descRand == 1)
                        desc = "ortodoncia";
                    else if (descRand == 2)
                        desc = "caries";
                    else
                        desc = "tratamiento especifico";
                    Paciente nuevo = new Paciente(Convert.ToString(randString), Convert.ToString(randString), i, Convert.ToString(randString), prev ,desc);
                    PacientesClinica.Add(nuevo.ID, nuevo);
                }
            }
            ViewData["pacientes"] = PacientesClinica.TreeToList();
            return View();
        }
        /// <summary>
        /// Busca un pacinete y muestra su información
        /// </summary>
        /// <param name="search">ID o nombre del paciente</param>
        /// <returns>Vista con la información del paciente</returns>
        public IActionResult buscarPaciente(String search)
        {
            //busca al paciente por su id
            Paciente buscarPaciente = PacientesClinica.Find(search);
            //si no encuentra al paciente por su id lo busca por su nombre
            if (buscarPaciente == default)
            {
                //crea un arbol con el nombre como llave
                AVLTree<String, Paciente> nameTree = new AVLTree<string, Paciente>(new Models.Comparadores.KeyComparer().Comparer);
                foreach (var item in PacientesClinica.TreeToList())
                {
                    nameTree.Add(item.Name, item);
                }
                buscarPaciente = nameTree.Find(search);
            }
            if (buscarPaciente != default)
            {
                ViewBag.SerchedPatient = buscarPaciente;
            }
            else 
            {
                ViewBag.SerchedPatientFalse = "No se encontró el paciente";
            }
            return View();
        }
        /// <summary>
        /// Muestra la lista de paceintes con el seguimeinto requerido
        /// </summary>
        /// <param name="tipo">tipo de seguimiento deseado</param>
        /// <returns>Vista con el seguimeinto de pacientes</returns>
        public IActionResult Seguimiento(int tipo)
        {
            //crea un arbol para guardar a los pacientes con el tipo de seguimiento deseado
            AVLTree<String, Paciente> arbolSeguimiento = new AVLTree<string, Paciente>(new Models.Comparadores.KeyComparer().Comparer);
            int MonthDiff;
            DateTime Today = DateTime.Today;
            DateTime defaultDate = new DateTime(2022, 03, 24, 12, 00, 00);
            //guarda a los pacientes con almenos 6 meses sin cita y sin descripción
            if (tipo == 0)
            {
                foreach (var item in PacientesClinica.TreeToList())
                {
                    if (item.Description == default || item.Description == null)
                    {
                        if (item.NextDate == default || item.NextDate == defaultDate)
                        {
                            MonthDiff = Math.Abs(12 * (item.LastDate.Year - Today.Year) + (item.LastDate.Month - Today.Month));
                            if (MonthDiff >= 6)
                            {
                                arbolSeguimiento.Add(item.ID, item);
                            }
                        }
                    }
                }
                ViewBag.Follow = "Pacientes con tratamiento de limpieza dental";
            }
            //guarda a los pacientes con almenos 2 meses sin cita y con "ortodoncia" en su descripción
            else if (tipo == 1)
            {
                ViewBag.Follow = "Pacientes con tratamiento de ortodoncia";
                foreach (var item in PacientesClinica.TreeToList())
                {
                    if (item.Description != null && new Models.Comparadores.DescriptionComparer().Find(item.Description.ToLower(), "ortodoncia"))
                    {
                        if (item.NextDate == default || item.NextDate == defaultDate)
                        {
                            MonthDiff = Math.Abs(12 * (item.LastDate.Year - Today.Year) + (item.LastDate.Month - Today.Month));
                            if (MonthDiff >= 2)
                            {
                                arbolSeguimiento.Add(item.ID, item);
                            }
                        }
                    } 
                }
            }
            //guarda a los pacientes con almenos 4 meses sin cita y con "caries" en su descripción
            else if (tipo == 2)
            {
                foreach (var item in PacientesClinica.TreeToList())
                {
                    ViewBag.Follow = "Pacientes con tratamiento para las caries";
                    if (item.Description != null &&  new Models.Comparadores.DescriptionComparer().Find(item.Description.ToLower(), "caries"))
                    {
                        if (item.NextDate == default || item.NextDate == defaultDate)
                        {
                            MonthDiff = Math.Abs(12 * (item.LastDate.Year - Today.Year) + (item.LastDate.Month - Today.Month));
                            if (MonthDiff >= 4)
                            {
                                arbolSeguimiento.Add(item.ID, item);
                            }
                        }
                    }
                }
                
            }
            //guarda a los pacientes con almenos 6 meses sin cita y con un tratamiento específico en su descripción
            else
            {
                ViewBag.Follow = "Pacientes con tratamiento específico";
                foreach (var item in PacientesClinica.TreeToList())
                {
                    if (item.Description != null && !(new Models.Comparadores.DescriptionComparer().Find(item.Description.ToLower(), "caries")) && !(new Models.Comparadores.DescriptionComparer().Find(item.Description.ToLower(), "ortodoncia")))
                    {
                        if (item.NextDate == default || item.NextDate == defaultDate)
                        {
                            MonthDiff = Math.Abs(12 * (item.LastDate.Year - Today.Year) + (item.LastDate.Month - Today.Month));
                            if (MonthDiff >= 6)
                            {
                                arbolSeguimiento.Add(item.ID, item);
                            }
                        }
                    }
                }
                
            }

            ViewData["seguimiento"] = arbolSeguimiento.TreeToList();
            return View();
        }
        /// <summary>
        /// Cambia la siguiente cita del paciente
        /// </summary>
        /// <param name="pacientID">ID del paciente</param>
        /// <param name="next">Cita a guardar</param>
        /// <returns>Vista con el paciente y su nueva cita</returns>
        public IActionResult registrarCita(String pacientID, DateTime next)
        {
            Paciente update = PacientesClinica.Find(pacientID);
            update.NextDate = next;
            ViewBag.NewDate = update;
            return View();
        }
        public IActionResult extraerLog()
        {
            PacientesClinica._log.Flush();
            return View();
        }
    }
}
