using CommonLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRL<TEntity>
    {
        public bool SignUp(SignUpModel user);
        public LoginResponse UserLogin(UserLogin LogUser);
        //public IEnumerable<User> GetAllData();
        public string SendResetLink(string email);
        public bool ResetPassword(ResetPassword resetPassword);
    }
}