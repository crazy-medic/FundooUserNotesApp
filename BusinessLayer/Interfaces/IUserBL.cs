﻿using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUserBL<TEntity>
    {
        public bool SignUp(SignUpModel user);
        public LoginResponse UserLogin(UserLogin LogUser);
        public IEnumerable<TEntity> GetAllData();
        public bool SendResetLink(string email);
        public bool ResetPassword(ResetPassword resetPassword);
    }
}
