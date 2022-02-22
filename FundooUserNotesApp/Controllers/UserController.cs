using BusinessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooUserNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserBL<User> bL;

        public UserController(IUserBL<User> bL)
        {
            this.bL = bL;
        }

        [HttpPost("SignUp")]
        public IActionResult Signup(SignUpModel signum)
        {
            try
            {
                if (signum == null)
                {
                    return NotFound(new { isSuccess = false, message = "All fields are mandatory" });
                }
                bL.SignUp(signum);
                return Ok(new { isSuccess = true, message = "Sign UP success" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("listallusers")]
        public ActionResult GetAllUserInformation()
        {
            try
            {
                var data = this.bL.GetAllData();
                if (data == null)
                {
                    return NotFound(new { status = 404, isSuccess = false, message = "There are no users for this site!" });
                }
                return Ok(new { status = 200, isSuccess = true, message = "Got all users", Data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[HttpGet("ResetPassword")]
        //public ActionResult ResetPassword()
        //{
        //    try
        //    {
        //        var Data1 = this.bL.ResetPassword();

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}
