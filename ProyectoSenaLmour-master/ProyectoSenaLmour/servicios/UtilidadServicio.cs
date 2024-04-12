

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Cryptography;
using System.Text;




namespace ProyectoSenaLmour.servicios
{
    public static class UtilidadServicio
    {
        public static string ConvertirSHA256(string texto)
        {
            string hash=string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                //OBTENER HASH DEL TEXTO RECIBIDO
                byte[] hashValue =sha256.ComputeHash (Encoding.UTF8.GetBytes (texto));
                // convertir el array byte en cadena texto
                foreach (byte b in hashValue)
                 hash += $"{b:x2}";
            }

            return hash;
        }

          public static string GenerarToken()
          {

            string token = Guid.NewGuid().ToString("N");
            return token;
          }

    }
}

