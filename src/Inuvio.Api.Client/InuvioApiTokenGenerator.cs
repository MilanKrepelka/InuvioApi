using ASOL.Inuvio.Api.Client.Wrappers;
using ASOL.Inuvio.Api.Client.Contracts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ASOL.Inuvio.Api.Client
{
    /// <summary>
    /// Generates and decodes JWT tokens for iNuvio API authentication.
    /// </summary>
    internal class InuvioApiTokenGenerator : IInuvioApiTokenGenerator
    {
        private readonly string _issuer = "iNuvioAgent";
        private readonly string _passPhrase = "87142cc3248b4888065194e55eb91d5a93ea44cdb37b40ea4b39aedb211177f26737db73d9a53173f74bfc1a9387e9621ed096e9179d9e975f8b32917f912456";
        private readonly JwtSecurityTokenHandler _handler = new();
        private readonly IDateTimeNowWrapper _dateTimeNowWrapper;
        private TimeSpan _tokenLifetime = TimeSpan.FromHours(3);

        /// <summary>
        /// Initializes a new instance of the <see cref="InuvioApiTokenGenerator"/> class.
        /// </summary>
        /// <param name="dateTimeNowWrapper">The date time wrapper for testability.</param>
        /// <param name="tokenLifetimeInSeconds">The token lifetime in seconds. Default is 180 seconds (3 minutes).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dateTimeNowWrapper"/> is null.</exception>
        public InuvioApiTokenGenerator(IDateTimeNowWrapper dateTimeNowWrapper, int tokenLifetimeInSeconds = 60 * 3)
        {
            _tokenLifetime = TimeSpan.FromSeconds(tokenLifetimeInSeconds);
            _dateTimeNowWrapper = dateTimeNowWrapper ?? throw new ArgumentNullException(nameof(dateTimeNowWrapper));
        }
        
        /// <inheritdoc/>
        public string GenerateToken()
        {
            var buffer = Encoding.UTF8.GetBytes(_passPhrase);
            var token = _handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _issuer,
                Audience = "iNuivo - eServer",
                Expires = _dateTimeNowWrapper.UtcNow.Add(_tokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(buffer), SecurityAlgorithms.HmacSha256Signature)
            });

            return _handler.WriteToken(token);
        }

        /// <inheritdoc/>
        public string DecodeToken(string token)
        {
            var securityToken = _handler.ReadJwtToken(token);
            return securityToken.ToString();
        }
    }
}
