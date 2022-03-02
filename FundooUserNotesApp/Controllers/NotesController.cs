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
    public class NotesController : ControllerBase
    {
        private readonly INoteBL Nbl;

        /// <summary>
        /// Construction function
        /// </summary>
        /// <param name="Nbl"></param>
        /// <param name="funContext"></param>
        public NotesController(INoteBL Nbl)
        {
            this.Nbl = Nbl;
        }

        /// <summary>
        /// Create note api
        /// </summary>
        /// <param name="noteModel"></param>
        /// <returns></returns>
        [HttpPost("CreateNote")]
        public IActionResult CreateNote(NoteModel noteModel)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                if (this.Nbl.CreateNote(noteModel,userid))
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = "Note created" });
                }
                else
                {
                    return this.BadRequest(new { status = 401, isSuccess = false, message = "Failed to create note" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400,isSuccess = false, Message=e.InnerException.Message});
            }
        }

        /// <summary>
        /// API to retrieve all user notes
        /// </summary>
        /// <returns></returns>
        [HttpGet("Shownotes")]
        public IActionResult GetAllNotes()
        {
            try
            {
                IEnumerable<Note> notes = Nbl.GetAllNotes();
                if (notes != null)
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = "Successful" });
                }
                else
                {
                    return this.NotFound(new { status = 404, isSuccess = false, message = "No Notes Found" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// API to update contents of a note and update its ModifiedAt time
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPut("UpdateNote")]
        public IActionResult UpdateNote(Note note)
        {
            try
            {
                var result = this.Nbl.UpdateNotes(note);
                if (result.Equals("Done"))
                {
                    return this.Ok(new { Success = true, message = "Note Updated successfully " });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Error while updating notes" });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message, InnerException = ex.InnerException });
            }
        }

        /// <summary>
        /// API to Un/Archive a note
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        [HttpPost("ArchiveNote")]
        public IActionResult ArchiveNote(long noteid)
        {
            try
            {
                var result = this.Nbl.ArchiveNote(noteid);
                if(result == true)
                {
                    return this.Ok(new { status = 200, isSuccess = true, Message = "Note Archived" });
                }
                if(result == false)
                {
                    return this.Ok(new { status = 200, isSuccess = true, Message = "Note UnArchived" });
                }
                return this.BadRequest(new { status = 400, isSuccess = false, Message = "Internal error" });
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message, InnerException = e.InnerException });
            }
        }

        /// <summary>
        /// API for Un/Pin a note
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        [HttpPost("PinNote")]
        public IActionResult PinNote(long noteid)
        {
            try
            {
                var result = this.Nbl.PinNote(noteid);
                if (result == true)
                {
                    return this.Ok(new { status = 200, isSuccess = true, Message = "Note Pinned" });
                }
                if (result == false)
                {
                    return this.Ok(new { status = 200, isSuccess = true, Message = "Note UnPinned" });
                }
                return this.BadRequest(new { status = 400, isSuccess = false, Message = "Internal error" });
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message, InnerException = e.InnerException });
            }
        }

        /// <summary>
        /// Delete the note using note id
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        [HttpDelete("DeleteNote")]
        public IActionResult DeleteNote(long noteid)
        {
            try
            {
                var result = this.Nbl.DeleteNote(noteid);
                if (result == true)
                {
                    return this.Ok(new { status = 200, isSuccess = true, Message = "Note Deleted" });
                }
                if (result == false)
                {
                    return this.Ok(new { status = 200, isSuccess = true, Message = "Note Restored" });
                }
                return this.BadRequest(new { status = 400, isSuccess = false, Message = "Internal error" });
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message, InnerException = e.InnerException });
            }
        }

        /// <summary>
        /// Deletes the note forever
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        [HttpDelete("ForeverDelete")]
        public IActionResult ForeverDeleteNote(long noteid)
        {
            try
            {
                var result = this.Nbl.ForeverDeleteNote(noteid);
                if(result == true)
                {
                    return this.Ok(new { status = 200, isSuccess = true, Message = "Deleted note forever" });
                }
                return this.BadRequest(new { status = 401, isSuccess = false, Message = "Incorrect note id" });
            }
            catch (Exception)
            {
                return this.NotFound(new { status = 404, isSuccess = false, Message = "Note not found or already deleted" });
            }
        }

        /// <summary>
        /// API for color addition to notes
        /// </summary>
        /// <param name="color"></param>
        /// <param name="noteid"></param>
        /// <returns></returns>
        [HttpPost("AddColor")]
        public IActionResult AddNoteColor(string color, long noteid)
        {
            try
            {
                var result = this.Nbl.AddNoteColor(color,noteid);
                if (result == "Updated")
                {
                    return this.Ok(new { status = 200, isSuccess = true, Message = "Note color updated" });
                }
                return this.BadRequest(new { status = 401, isSuccess = false, Message = "Note color not updated" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 401, isSuccess = false, Message = e.Message });
            }
        }
    }
}
