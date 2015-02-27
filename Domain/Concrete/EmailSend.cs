using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;

namespace Domain.Concrete
{
    public class EmailSend : IEmailSend
    {

        public void SendEmail(Entities.OrderDetails orderDetails, Entities.Cart cart)
        {
            string fromAddress = "czmiel24@gmail.com";
            string fromPassword = "rickardsson1";
            string subject = "Sklep komputerowy - zamównienie";
            StringBuilder body = new StringBuilder("");
            body.AppendLine("Ilość      Nazwa                 Cena");
            foreach (var cartLine in cart.Lines)
            {
                body.AppendLine(cartLine.Quantity + "    " + cartLine.Product.Name + "   " +
                                cartLine.Quantity*cartLine.Product.Price);
            }
            body.AppendLine("===================================");
            body.AppendLine("Suma: " + cart.Lines.Sum(m => m.Quantity*m.Product.Price));
            body.AppendLine("Suma z przesyłką: " +
                            (cart.Lines.Sum(m => m.Quantity*m.Product.Price) + (int) orderDetails.DeliveryType));
            body.AppendLine("===================================");
            body.AppendLine("Dane zamawiającego:");
            body.AppendLine("Imię: " + orderDetails.FirstName);
            body.AppendLine("Nazwisko: " + orderDetails.LastName);
            body.AppendLine("Ulica: " + orderDetails.Street);
            body.AppendLine("Kod pocztowy: " + orderDetails.ZipCode);
            body.AppendLine("Miasto: " + orderDetails.City);
            body.AppendLine("Adres E-Mail: " + orderDetails.EmailAdress);
            body.AppendLine("===================================");

            using (MailMessage mail = new MailMessage(fromAddress, orderDetails.EmailAdress))
            {
                mail.Subject = subject;
                mail.Body = body.ToString();
                mail.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential(fromAddress, fromPassword);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                
                smtp.Send(mail);
            }
        }
    }
}
