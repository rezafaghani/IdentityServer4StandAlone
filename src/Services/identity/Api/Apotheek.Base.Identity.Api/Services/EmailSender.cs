using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Serilog;

namespace Apotheek.Base.Identity.Api.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(ILogger logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.Information($"Email: {email}, subject: {subject}, message: {htmlMessage}");

            return Task.FromResult(0);
        }
    }
}
