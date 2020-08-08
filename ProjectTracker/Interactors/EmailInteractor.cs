using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectTracker.Interfaces;
using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProjectTracker.Interactors
{
    public class EmailInteractor : IEmailInteractor
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public EmailInteractor(IConfiguration configuration, ILogger<IEmailInteractor> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmail(EmailModel email)
        {
            if (_configuration == null) { throw new ArgumentNullException(nameof(_configuration)); }

            try
            {
                using (var client = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = _configuration["EmailConfig:EmailAddress"],
                        Password = _configuration["EmailConfig:EmailPassword"]
                    };

                    client.Credentials = credential;
                    client.Host = _configuration["EmailConfig:Host"];
                    client.Port = int.Parse(_configuration["EmailConfig:Port"]);
                    client.EnableSsl = bool.Parse(_configuration["EmailConfig:EnableSSL"]);

                    using (var emailMessage = new MailMessage())
                    {
                        //send to muliple recipients 
                        foreach (var item in email.ToEmails)
                        {
                            emailMessage.To.Add(new MailAddress(item));
                        }

                        //send to muliple CC recipients 
                        if (email.CCEmails?.Length > 0)
                        {
                            foreach (var item in email.CCEmails)
                            {
                                emailMessage.To.Add(new MailAddress(item));
                            }
                        }

                        //send to muliple BCC recipients 
                        if (email.BCCEmails?.Length > 0)
                        {
                            foreach (var item in email.BCCEmails)
                            {
                                emailMessage.To.Add(new MailAddress(item));
                            }
                        }

                        //Attach the documents
                        if (email.Attachments?.Length > 0)
                        {
                            foreach (var item in email.Attachments)
                            {
                                if (File.Exists(item))
                                {
                                    emailMessage.Attachments.Add(new Attachment(item));
                                }
                            }
                        }

                        emailMessage.From = new MailAddress(_configuration["EmailConfig:EmailAddress"]);
                        emailMessage.Subject = email.Subject;
                        emailMessage.Body = email.Message;
                        emailMessage.IsBodyHtml = email.SendAsHtml;

                        client.Send(emailMessage);
                    }
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                //if using gmail, check if 'less secure apps is ON'
                _logger.LogError(ex.Message);
            }
        }
    }
}
