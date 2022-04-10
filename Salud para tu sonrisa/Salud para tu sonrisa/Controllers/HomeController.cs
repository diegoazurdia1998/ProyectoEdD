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
        private static AVLTree<String, Paciente> PacientesClinica = new AVLTree<string, Paciente>(new Models.Comparadores.KeyComparer().Comparer);
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
            ViewData["pacientes"] = PacientesClinica.TreeToList();
            return View();
        }

    }
}
