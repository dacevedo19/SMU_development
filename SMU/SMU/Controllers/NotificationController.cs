using SMU.Models;
using System.Net.Mail;

namespace SMU.Controllers
{
    public static class NotificationController
    {        
        public static bool SendEmail(Notification noti)
        {
            if (noti != null)
            {
                try
                {
                    // Configuracion de mail
                    MailMessage mm = new MailMessage
                    {
                        From = new MailAddress("SMUsolicitudes@gmail.com"),
                        Subject = noti.Subject,
                        Body = noti.Body,
                        IsBodyHtml = false
                    };
                    mm.To.Add(noti.To);


                    // Configuracion de SMTP
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        UseDefaultCredentials = true,
                        EnableSsl = true,
                        Credentials = new System.Net.NetworkCredential("SMUsolicitudes@gmail.com", "Sol11Ici22Tud33Es44")
                    };

                    smtp.Send(mm);
                    return true;
                } catch { return false; }                
                
            } else { return false; }
            
        }

        public static string GetBodyForEmail(bool result, AppUser user, Request req)
        {
            string resultado;
            if (result) { resultado = "aceptada"; } else { resultado = "rechazada"; }

            return "Estimado/a " + user.Name + " " + user.Lastname + ":  \n"
                        + " \n"
                        + "Le notificamos que su solicitud con ID: " + req.Id + " ha sido " + resultado + ". \n"
                        + " \n"
                        + "Si desea obtener mas información, comuníquese con el departamento de Recursos Humanos indicando el ID de su solicitud. \n"
                        + " \n"
                        + "Este es un email autogenerado, por favor no responda a esta casilla.\n "
                        + " \n"
                        + "SOCIEDAD MÉDICA UNIVERSAL";
        }


    }
}
