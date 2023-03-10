using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Utilities
{
    public class EmailSender :IEmailSender
    {
        private string _apiKey;
        private string _emailAccountSengrid;

        public EmailSender(IConfiguration configuration)
        {
            _apiKey = configuration.GetSection("SendGrid")["ApiKey"];
            _emailAccountSengrid = configuration.GetValue<string>("SendGrid:EmailAccountSengrid");
        }


        public Task SendEmailAsync(string email,string subject,string htmlMessage)
        {

            return Execute(email,subject,htmlMessage);
        }
        private async Task Execute(string email,string subject,string htmlMessage)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_emailAccountSengrid,"Tecno produtos Carhué");
            var to = new EmailAddress(email);
            var plainTextContent = "Gracias por registrarce en Nuestra pagina";
            var msg = MailHelper.CreateSingleEmail(from,to,subject,plainTextContent,htmlMessage);
            var response = await client.SendEmailAsync(msg);

        }
    }
}
