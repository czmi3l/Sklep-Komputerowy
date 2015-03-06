using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace WebUI.Infrastructure
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store) { }

        public static AppUserManager Create(
            IdentityFactoryOptions<AppUserManager> options,
            IOwinContext context)
        {
            AppIdentityDbContext db = context.Get<AppIdentityDbContext>();
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(db));

            manager.EmailService = new EmailService();
            manager.UserTokenProvider = new DataProtectorTokenProvider<AppUser>(options.DataProtectionProvider.Create("ASP.NET Identity")){TokenLifespan = TimeSpan.FromHours(3)};
            manager.UserValidator = new UserValidator<AppUser>(manager){RequireUniqueEmail = true};
            return manager;
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            string fromAddress = "czmiel24@gmail.com";
            string fromPassword = "rickardsson1";

            MailMessage mail = new MailMessage(fromAddress, message.Destination);
            mail.Body = message.Body;
            mail.Subject = message.Subject;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            smtp.Port = 587;
            NetworkCredential networkCredential = new NetworkCredential(fromAddress, fromPassword);
            smtp.Credentials = networkCredential;

            return smtp.SendMailAsync(mail);
        }
    }
}