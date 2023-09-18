using System.Threading.Tasks;
using EightElements.Services.Models;

namespace EightElements.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}