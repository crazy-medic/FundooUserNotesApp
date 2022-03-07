namespace FundooUserNotesApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BusinessLayer.Interfaces;
    using CommonLayer.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using RepositoryLayer.Entities;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the UserController class
        /// </summary>
        private readonly IUserBL<User> bL;

        /// <summary>
        /// assignment of objects
        /// </summary>
        /// <param name="bL"></param>
        public UserController(IUserBL<User> bL)
        {
            this.bL = bL;
        }

        /// <summary>
        /// Sign up api
        /// </summary>
        /// <param name="signum"></param>
        /// <returns></returns>
        [HttpPost("SignUp")]
        public IActionResult Signup(SignUpModel signum)
        {
            try
            {
                if (signum == null)
                {
                    return this.NotFound(new { status = 404, isSuccess = false, message = "All fields are mandatory" });
                }
                this.bL.SignUp(signum);
                return this.Ok(new { status = 200, isSuccess = true, message = "Sign UP success" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        //deprecated api
        /// <summary>
        /// API to check list of users of the Fundoo app
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Login API
        /// </summary>
        /// <param name="LogUser"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult UserLogin(UserLogin logUser)
        {
            try
            {
                if (logUser.EmailId == null)
                {
                    return this.NotFound(new { status = 404, isSuccess = false, message = "All fields are mandatory" });
                }
                LoginResponse result = this.bL.UserLogin(logUser);
                if(result.EmailId != null)
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = "Sign UP success", data = result.Token });
                }
                return this.BadRequest(new { status = 401, isSuccess = false, Message = "Internal error" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Reset/set new password API
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="ConfirmPassword"></param>
        /// <returns></returns>
        [HttpPut("ResetPassword")]
        [Authorize]
        public ActionResult ResetPassword(string password, string confirmPassword)
        {
            try
            {
                // var email = User.Claims.FirstOrDefault(e => e.Type == "Email").Value;
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var reset = this.bL.ResetPassword(email, password, confirmPassword);
                return this.Ok(new { status = 200, isSuccess = true, Message = "Password change success" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// API to request for creating new password when user forgets password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(string email)
        {
            if (email == null)
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
