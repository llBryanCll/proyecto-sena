using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ProyectoSenaLmour.Models;
using ProyectoSenaLmour.Datos;
using ProyectoSenaLmour.servicios;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProyectoSenaLmour.Controllers
{
    public class InicioController : Controller
    {

        //Get inicio
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(string correo, string contraseña)
        {
            Usuario usuario = DBUsuario.validar(correo, UtilidadServicio.ConvertirSHA256(contraseña));
            if (usuario != null) 
            {
                if(usuario.Confirmado)
                {
                    ViewBag.Mensaje = $"Falta confirmar su cuenta. Se le envio un correo a {correo}";

                }else if (usuario.Restablecer)
                {
                    ViewBag.Mensaje = $"Se ha solicitado restablecer su cuenta, favor revise su bandeja del correo {correo}";

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias";  
            }

            return View();
        }
      


          
        }
    }

