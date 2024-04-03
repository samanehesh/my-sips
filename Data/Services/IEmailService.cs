using SendGrid;
using Sips.SipsModels;

namespace Sips.Data.Services
{
    public interface IEmailService
    {
        Task<Response> SendSingleEmail(ComposeEmailModel payload);
    }

}
