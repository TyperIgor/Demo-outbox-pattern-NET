

using DemoProject.Domain.Models;

namespace DemoProject.Domain.Interfaces
{
    public interface IEmailRepository
    {
        Task InsertEmail(EmailNotification email);
    }
}
