namespace FundooUserNotesApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLayer.Interfaces;
    using CommonLayer.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using RepositoryLayer.Context;
    using RepositoryLayer.Entities;

    /// <summary>
    /// Start of APIs
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CollabController : ControllerBase
    {
        /// <summary>
        /// Collection of Objects
        /// </summary>
        private readonly ICollabBL collabBL;
        private readonly FundooUserNotesContext fUNContext;

        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="collabBL"></param>
        /// <param name="fUNContext"></param>
        public CollabController(ICollabBL collabBL, FundooUserNotesContext fUNContext)
        {
            this.collabBL = collabBL;
            this.fUNContext = fUNContext;
        }

        /// <summary>
        /// API for adding a collaboration to a note
        /// </summary>
        /// <param name="collabModel"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public IActionResult AddCollab(CollabModel collabModel)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var collabnote = this.fUNContext.NotesTable.Where(x => x.NoteId == collabModel.NotesId).SingleOrDefault();
                if (collabnote.UserId == userid)
                {
                    var result = this.collabBL.AddCollab(collabModel);
                    if (result)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Member added to collab" });
                    }
                    return this.BadRequest(new { status = 400, isSuccess = false, Message = "Failed to add member" });
                }
                return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Not authorized to add collab to this note" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// Api to show a note with noteid
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        [HttpGet("Show")]
        public IActionResult ShowCollab(long noteid)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                IEnumerable<Collaborator> collabnote = this.fUNContext.CollabTable.Where(x => x.NoteId == noteid).ToList();
                if (collabnote != null)
                {
                    var result = this.collabBL.Show(noteid);
                    if (result != null)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Displaying all collabs", data = collabnote });
                    }
                    else
                    {
                        return this.BadRequest(new { status = 400, isSuccess = false, Message = "Failed" });
                    }
                }
                return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Not authorized to view all collabs of this note" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// API to remove a collaborator from a note
        /// </summary>
        /// <param name="collabModel"></param>
        /// <returns></returns>
        [HttpDelete("Remove")]
        public IActionResult RemoveCollab(CollabModel collabModel)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var collabmember = this.fUNContext.CollabTable.Where(x => x.CollabEmail == collabModel.EmailId).SingleOrDefault();
                var noteOwner = this.fUNContext.NotesTable.Where(x => x.NoteId == collabModel.NotesId).SingleOrDefault();
                if (noteOwner.UserId == userid)
                {
                    var result = this.collabBL.RemoveCollab(collabModel);
                    if (result)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Deleted collab for user", data = collabModel.EmailId });
                    }
                    return this.BadRequest(new { status = 400, isSuccess = false, Message = "Check the collab mail or noteid" });
                }
                return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Not authorized to add collab to this note" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }
    }
}
