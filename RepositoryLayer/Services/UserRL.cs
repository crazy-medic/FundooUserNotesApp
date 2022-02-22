using CommonLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL<User>
    {
        /// <summary>
        /// Variables for this class.
        /// </summary>
        FundooUserNotesContext context;
        IConfiguration _config;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public UserRL(FundooUserNotesContext context, IConfiguration config)
        {
            this.context = context;
            _config = config;
        }

        /// <summary>
        /// Registering user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool SignUp(SignUpModel user)
        {
            try
            {
                User newUser = new User();
                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.EmailID = user.EmailID;
                newUser.Password = Encryptpass(user.Password);
                newUser.CreatedAt = DateTime.Now;

                this.context.UserTable.Add(newUser);

                int result = this.context.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieving All users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllData()
        {
            return context.UserTable.ToList();
        }

        /// <summary>
        /// Attempting to log user into the site.
        /// </summary>
        /// <param name="user1"></param>
        /// <returns></returns>
        public LoginResponse UserLogin(UserLogin user1)
        {
            try
            {
                User existingLogin = this.context.UserTable.Where(X => X.EmailID == user1.EmailId).FirstOrDefault();
                if (Decryptpass(existingLogin.Password) == user1.Password)
                {
                    LoginResponse login = new LoginResponse();
                    string token;
                    token = GenerateJWTToken(existingLogin.EmailID, existingLogin.Id);
                    login.Id = existingLogin.Id;
                    login.FirstName = existingLogin.FirstName;
                    login.LastName = existingLogin.LastName;
                    login.EmailId = existingLogin.EmailID;
                    login.Createat = existingLogin.CreatedAt;
                    login.ModifiedAt = existingLogin.ModifiedAt;
                    login.Token = token;
                    return login;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Generating Token
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        private string GenerateJWTToken(string EmailId, long Id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(ClaimTypes.Email,EmailId),
                new Claim("Id",Id.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Encrypting Password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Encryptpass(string password)
        {
            string msg = "";
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }

        /// <summary>
        /// Decrypting 
        /// </summary>
        /// <param name="encryptpwd"></param>
        /// <returns></returns>
        private string Decryptpass(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

        /// <summary>
        /// Sending email Link
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool SendResetLink(string email)
        {
            try
            {
                User existingLogin = this.context.UserTable.Where(X => X.EmailID == email).FirstOrDefault();
                if (existingLogin.EmailID != null)
                {
                    var token = GenerateJWTToken(existingLogin.EmailID, existingLogin.Id);
                    new MsmqOperation().Sender(token);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Reseting Password
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool ResetPassword(ResetPassword resetPassword)
        {
            try
            {
                var Entries = this.context.UserTable.FirstOrDefault(x => x.EmailID == resetPassword.EmailId);
                if (Entries != null)
                {
                    if (resetPassword.Password == resetPassword.ConfirmPassword)
                    {
                        Entries.Password = Encryptpass(resetPassword.Password);
                        this.context.Entry(Entries).State = EntityState.Modified;
                        this.context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
