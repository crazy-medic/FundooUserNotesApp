using BusinessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
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
                    return NotFound(new { status = 404, isSuccess = false, message = "All fields are mandatory" });
                }
                bL.SignUp(signum);
                return Ok(new { status = 200, isSuccess = true, message = "Sign UP success" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[HttpGet("listallusers")]
        //public ActionResult GetAllUserInformation()
        //{
        //    try
        //    {
        //        var data = this.bL.GetAllData();
        //        if (data == null)
        //        {
        //            return NotFound(new { status = 404, isSuccess = false, message = "There are no users for this site!" });
        //        }
        //        return Ok(new { status = 200, isSuccess = true, message = "Got all users", Data = data });
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        [HttpPost("Login")]
        public IActionResult UserLogin(UserLogin LogUser)
        {
            try
            {
                if (LogUser == null)
                {
                    return NotFound(new { status = 404, isSuccess = false, message = "All fields are mandatory" });
                }
                bL.UserLogin(LogUser);
                return Ok(new { status = 200, isSuccess = true, message = "Sign UP success" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("ResetPassword")]
        [Authorize]
        public ActionResult ResetPassword(ResetPassword rpass)
        {
            try
            {
                var email = User.Claims.FirstOrDefault(e => e.Type == "Email").Value;
                var Data1 = this.bL.ResetPassword(rpass);
                if(Data1 == true)
                {
                    return this.Ok(new { status = 200, isSuccess = true, Message = "Password reset success" });
                }
                else
                {
                    return this.BadRequest(new { Status = 400, isSuccess = false, Message = "Failed to reset password" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(string email)
        {
            if(email == null)
            {
                return this.BadRequest(new { Status = 400, isSuccess = false, Message = "Enter an email" });
            }
            try
            {
                if (this.bL.SendResetLink(email) != null)
                {
                    return this.Ok(new { Status = 200, isSuccess = true, Message = "Reset password link sent" });
                }
                else
                {
                    return this.BadRequest(new { Status = 400, isSuccess = false, Message = "Email not found in database" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }
    }
}
