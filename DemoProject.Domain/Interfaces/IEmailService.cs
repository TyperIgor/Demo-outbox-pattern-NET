using System;


namespace DemoProject.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject);
    }
}
