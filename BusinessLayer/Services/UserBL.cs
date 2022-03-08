using BusinessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserBL : IUserBL<User>
    {
        IUserRL<User> userRL;
        public UserBL(IUserRL<User> userRL)
        {
            this.userRL = userRL;
        }

        /// <summary>
        /// Registers the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public bool SignUp(SignUpModel user)
        {
            try
            {
                return this.userRL.SignUp(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the alldata.
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<User> GetAllData()
        //{
        //    return this.userRL.GetAllData();
        //}

        /// <summary>
        /// Users the login.
        /// </summary>
        /// <param name="user1">The user1.</param>
        /// <returns></returns>
        public LoginResponse UserLogin(UserLogin LogUser)
        {
            try
            {
                return this.userRL.UserLogin(LogUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sends the reset link.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public string SendResetLink(string email)
        {
            try
            {
                return this.userRL.SendResetLink(email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public bool ResetPassword(string email, string Password, string ConfirmPassword)
        {
            try
            {
                bool result = this.userRL.ResetPassword(email, Password, ConfirmPassword);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<User> GetEveryUser()
        {
            try
            {
                return this.userRL.GetEveryUser();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
