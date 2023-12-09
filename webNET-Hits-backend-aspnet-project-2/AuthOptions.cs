using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace webNET_Hits_backend_aspnet_project_2
{
    public class AuthOptions
    {
        public const string ISSUER = "Project";
        public const string AUDIENCE = "Recipient";
        public static string KEY;

        public const int LIFETIME = 30;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }

        public AuthOptions(IConfiguration configuration)
        {
            KEY = configuration["AuthOptions:KEY"];
        }
    }
}
