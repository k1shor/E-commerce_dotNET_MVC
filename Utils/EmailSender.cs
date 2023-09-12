using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Utils
{
    public class EmailSender : IEmailSender
    {
        private SmtpSettings smtpSettings { get; set; }
        public EmailSender(IConfiguration _config)
        {
            smtpSettings = new()
            {
                Server = _config.GetValue<string>("SmtpSettings:Server"),
                Port = _config.GetValue<int>("SmtpSetting:Port"),
                Username = _config.GetValue<string>("SmtpSettings:Username"),
                Password = _config.GetValue<string>("SmtpSettings:Password")

            };
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // logic for sending email
            //            var smtpSettings = GetSmtpSettingsFromConfig();
            var client = new SmtpClient(smtpSettings.Server, smtpSettings.Port);
            using (client)
            {
                client.Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password);
                client.EnableSsl = true; // Enable SSL if necessary

                var message = new MailMessage
                {
                    From = new MailAddress("noreply@ecommerce-app.com"),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,

                };

                message.To.Add(email);

                client.Send(message);
            }
            return Task.CompletedTask;
        }
        static SmtpSettings GetSmtpSettingsFromConfig()
        {
            // Load SMTP settings from your configuration file
            // Replace this with your own configuration loading logic
            // (e.g., from appsettings.json)
            return new SmtpSettings
            {
                Server = "smtp.mailtrap.io",
                Port = 2525,
                Username = "98d01484d2ca26",
                Password = "e75a30a02b5212"
            };
        }
    }
}
