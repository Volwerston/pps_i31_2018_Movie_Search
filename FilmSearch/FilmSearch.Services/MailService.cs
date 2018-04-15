using FilmSearch.Models;
using FilmSearch.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using FilmSearch.Controllers;

namespace FilmSearch.Services
{
    public class MailService
    {

        public void SendNewsletter(NewsletterDTO newsletter, IEnumerable<AppUser> users, MailParams parameters)
        {
            if (string.IsNullOrWhiteSpace(newsletter.Text)) return;

            foreach(var idHolder in newsletter.UserIds)
            {
                string userEmail = users.Where(x => x.Id == idHolder.Id).First().Email;

                SendLetter(newsletter.Text, userEmail, parameters);
            }
        }

        private void SendLetter(string text, string userEmail, MailParams parameters)
        {
            string smtpHost = parameters.Host;
            int smtpPort = parameters.Port;
            string smtpUserName = parameters.UserName;
            string smtpUserPass = parameters.UserPassword;

            SmtpClient client = new SmtpClient(smtpHost, smtpPort);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(smtpUserName, smtpUserPass);
            client.EnableSsl = true;

            string msgFrom = smtpUserName;
            string msgTo = userEmail;
            string msgSubject = "FilmSearch Notification";

            string msgBody = text;

            MailMessage message = new MailMessage(msgFrom, msgTo, msgSubject, msgBody);

            message.IsBodyHtml = true;

            try
            {
                client.Send(message);
            }
            catch
            {

            }
        }
    }
}
