using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Settings
{
    public class JwtSettings
    {
        public const string Defaultsection = "Jwt";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string JwtKey { get; set; }
        public int Lifitime { get; set; }
        public int RefreshTokenValidityInDays { get; set; }
        public string Authority { get; set; }
    }
}
