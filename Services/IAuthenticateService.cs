using Hubtel.Wallets.Api.Models;

namespace Hubtel.Wallets.Api.Services
{
    public interface IAuthenticateService

    {
       User AuthenticateUser(string Name, string Password);

    }
}
