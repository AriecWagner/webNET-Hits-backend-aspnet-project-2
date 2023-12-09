using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using webNET_Hits_backend_aspnet_project_2.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace webNET_Hits_backend_aspnet_project_2.Services
{
    public class TokenService
    {
        private readonly UsersService _userService;
        readonly AppDbContext _dbContext;
        public TokenService(AppDbContext context, UsersService userService)
        {
            _dbContext = context;
            _userService = userService;
        }
        public string GenerateToken(Guid userId, AuthOptions authentification)
        {
            if (userId == null)
            {
                return null;
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,userId.ToString())
                };

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            CreateOrUpdateTokenInfo(userId, encodedJwt);

            return encodedJwt;
        }

        public bool CheckCurrentToken(Guid UserId)
        {
            TokenModel tokenInfo = _dbContext.Tokens.FirstOrDefault(u => u.UserId == UserId);

            if (tokenInfo == null)
            {
                return false;
            }

            return tokenInfo.IsValid;
        }

        public void RevokeToken(Guid userId)
        {
            TokenModel currentToken = _dbContext.Tokens.FirstOrDefault(t => t.UserId == userId);
            //_dbContext.Tokens.Remove(currentToken);
            currentToken.IsValid = false;
            _dbContext.Tokens.Update(currentToken);
            _dbContext.SaveChanges();
        }

        public void CreateOrUpdateTokenInfo(Guid UserId, string token)
        {
            TokenModel tokenInfo = _dbContext.Tokens.FirstOrDefault(x => x.UserId == UserId);

            if (tokenInfo == null)
            {
                tokenInfo = new TokenModel
                {
                    UserId = UserId,
                    Token = token,
                    CreateTime = DateTime.UtcNow,
                    IsValid = true,
                };

                _dbContext.Tokens.Add(tokenInfo);
                _dbContext.SaveChanges();
            }
            else
            {
                tokenInfo.Token = token;
                tokenInfo.CreateTime = DateTime.UtcNow;
                tokenInfo.IsValid = true;

                _dbContext.Tokens.Update(tokenInfo);
                _dbContext.SaveChanges();
            }
        }
    }
}
