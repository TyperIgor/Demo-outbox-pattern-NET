using MimeKit;
using DemoProject.Domain.Interfaces;
using MailKit.Net.Smtp;
using MassTransit;
using DemoProject.Domain.Models;

namespace DemoProject.Domain.Services
{
    public class EmailService(IEmailRepository emailRepository) : IEmailService
    {
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IEmailRepository _emailRepository = emailRepository ?? throw new ArgumentNullException();

        public async Task SendEmailAsync(string email, string subject)
        {
            var message = new MimeMessage();

            BuildEmail(message);

            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("localhost", 1025);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                var emailNotification = BuildEmailNotification(email, subject);

                await _emailRepository.InsertEmail(emailNotification); 

            }
            catch (Exception)
            {

                throw;
            }

        }

        private static MimeMessage BuildEmail(MimeMessage message)
        {
            var from =  new MailboxAddress("myself", "noreply@typer.com");
            message.From.Add(from);
            var to = new MailboxAddress("Typer", "igor_purosso@hotmail.com");
            message.To.Add(to);
            message.Subject = "Hey this is a notification from igor using .net";

            return message;
        }

        private static EmailNotification BuildEmailNotification(string email, string subject)
        {
            return new EmailNotification()
            {
                Email = email,
                Subject = subject
            };
        }

    }
}
