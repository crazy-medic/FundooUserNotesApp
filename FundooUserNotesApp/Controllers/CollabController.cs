using BusinessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooUserNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBL collabBL;
        private readonly FundooUserNotesContext FUNContext;

        public CollabController(ICollabBL collabBL, FundooUserNotesContext FUNContext)
        {
            this.collabBL = collabBL;
            this.FUNContext = FUNContext;
        }

        [HttpPost("Add")]
        public IActionResult AddCollab(CollabModel collabModel)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var collabnote = FUNContext.NotesTable.Where(x => x.NoteId == collabModel.NotesId).SingleOrDefault();
                if(collabnote.UserId == userid)
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

        [HttpGet("Show")]
        public IActionResult ShowCollab(long noteid)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                IEnumerable<Collaborator> collabnote = FUNContext.CollabTable.Where(x => x.NoteId == noteid).ToList();
                if (collabnote != null)
                {
                    var result = this.collabBL.Show(noteid);
                    if (result!=null)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Displaying all collabs", data = collabnote });
                    }
                    return this.BadRequest(new { status = 400, isSuccess = false, Message = "Failed" });
                }
                return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Not authorized to view all collabs of this note" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        [HttpDelete("Remove")]
        public IActionResult RemoveCollab(CollabModel collabModel)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var collabnote = FUNContext.CollabTable.Where(x => x.NoteId == collabModel.NotesId).SingleOrDefault();
                if(collabnote.UserID == userid)
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
