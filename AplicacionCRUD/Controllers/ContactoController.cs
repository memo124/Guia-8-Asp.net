using AplicacionCRUD.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Diagnostics.Contracts;

namespace AplicacionCRUD.Controllers
{
    public class ContactoController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();
        private static List<Contacto> olista = new List<Contacto>();
        // GET: Contacto
        public ActionResult Inicio()
        {
            olista = new List<Contacto>();
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("Select * from contacto", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Contacto contacto = new Contacto();
                        contacto.idContacto = Convert.ToInt32(dr["IdContacto"]);
                        contacto.Nombres = dr["Nombres"].ToString();
                        contacto.Apellidos = dr["Apellidos"].ToString();
                        contacto.Telefono = dr["Telefono"].ToString();
                        contacto.Correo = dr["Correo"].ToString();
                        olista.Add(contacto);
                    }
                }
            }
            return View(olista);
        }
        [HttpGet]
        public ActionResult Registrar()
        {
            return View();
        }
            [HttpPost]
        public ActionResult Registrar(Contacto contacto)
        {
            using (SqlConnection oconexion =  new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Registrar", oconexion);
                cmd.Parameters.AddWithValue("Nombres", contacto.Nombres);
                cmd.Parameters.AddWithValue("Apellidos", contacto.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", contacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", contacto.Correo);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
                return RedirectToAction("Inicio","Contacto");
        }
        [HttpGet]
        public ActionResult Editar(int? idcontacto)
        {
            if (idcontacto == null)
            {
                return RedirectToAction("Inicio", "Contacto");
            }
            Contacto ocontacto = olista.Where(c=>c.idContacto == idcontacto).FirstOrDefault();
            return View(ocontacto);
        }
        [HttpPost]
        public ActionResult Editar(Contacto contacto)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Editar", oconexion);
                cmd.Parameters.AddWithValue("IdContacto", contacto.idContacto);
                cmd.Parameters.AddWithValue("Nombres", contacto.Nombres);
                cmd.Parameters.AddWithValue("Apellidos", contacto.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", contacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", contacto.Correo);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Contacto");
        }
        [HttpGet]
        public ActionResult Eliminar(int? idcontacto)
        {
            if (idcontacto == null)
            {
                return RedirectToAction("Inicio", "Contacto");
            }
            Contacto ocontacto = olista.Where(c => c.idContacto == idcontacto).FirstOrDefault();
            return View(ocontacto);
        }
        [HttpPost]
        public ActionResult Eliminar(string idcontacto)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Eliminar", oconexion);
                cmd.Parameters.AddWithValue("IdContacto", idcontacto);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Contacto");
        }
    }

   
}