using System.Threading.Tasks;

namespace GestionFerias_CTPINVU.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string bodyHtml);
    }
}
