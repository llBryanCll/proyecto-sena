using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using ProyectoSenaLmour.Models;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System.Linq.Expressions;

namespace ProyectoSenaLmour.servicios
{
    public class CorreoServicio
    {
        private static string _Host = "smtp.gmail.com";
        private static int  _Puerto = 587;
        private static string _NombreEnvia = "SENA";
        private static string _Correo = "bejaranokevin355@gmail.com";
        //VER VIDEO Y CAMBIAR ESTA CONTRASEÑA
        private static string _Contraseña = "2palaciosK";




        private static bool Enviar( correoDTO correoDTO )
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(_NombreEnvia, _Correo));    
                email.To.Add(MailboxAddress.Parse(correoDTO.Para));
                email.Subject = correoDTO.Asunto;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = correoDTO.Contenido
                };


                var smtp = new SmtpClient();
                smtp.Connect(_Host, _Puerto, SecureSocketOptions.StartTls);
                smtp.Authenticate(_Correo, _Contraseña);
                smtp.Send(email);
                smtp.Disconnect(true);
                return true;
            }
            
            catch
            {
                    return false;


            
            }
        }

    }
}
