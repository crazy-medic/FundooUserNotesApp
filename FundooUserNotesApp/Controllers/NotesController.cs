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
        /// <summary>
        /// Global variables
        /// </summary>
        private readonly INoteBL Nbl;
        private readonly FundooUserNotesContext FUNcontext;

        /// <summary>
        /// Construction function
        /// </summary>
        /// <param name="Nbl"></param>
        /// <param name="funContext"></param>
        public NotesController(INoteBL Nbl, FundooUserNotesContext funContext)
        {
            this.Nbl = Nbl;
            this.FUNcontext = funContext;
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
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
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
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                IEnumerable<Note> notes = Nbl.GetAllNotes(userid);
                if (notes != null)
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = "Successful", data = notes });
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
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                if(note.UserId == userid)
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
                else
                {
                    return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Unauthorized" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Status = false, Message = ex.Message, InnerException = ex.InnerException });
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
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var ArchNote = this.FUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if(ArchNote.UserId == userid)
                {
                    var result = this.Nbl.ArchiveNote(noteid);
                    if (result == true)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Note Archived" });
                    }
                    if (result == false)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Note UnArchived" });
                    }
                    return this.BadRequest(new { status = 400, isSuccess = false, Message = "Internal error" });
                }
                else
                {
                    return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Not authorized to change this note" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = e.Message, InnerException = e.InnerException });
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
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var PinnedNote = this.FUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (PinnedNote.UserId == userid)
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
                else
                {
                    return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Not authorized to change this note" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = e.Message, InnerException = e.InnerException });
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
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var DeletedNote = this.FUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (DeletedNote.UserId == userid)
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
                else
                {
                    return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Not authorized to Delete this note" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = e.Message, InnerException = e.InnerException });
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
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var FDeleNote = this.FUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (FDeleNote.UserId == userid)
                {
                    var result = this.Nbl.ForeverDeleteNote(noteid);
                    if (result == true)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Deleted note forever" });
                    }
                    return this.BadRequest(new { status = 401, isSuccess = false, Message = "Incorrect note id" });
                }
                else
                {
                    return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Not authorized to change this note" });
                }

            }
            catch (Exception)
            {
                return this.BadRequest(new { status = 404, isSuccess = false, Message = "Note not found or already deleted" });
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
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var ColourNote = this.FUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (ColourNote.UserId == userid)
                {
                    var result = this.Nbl.AddNoteColor(color, noteid);
                    if (result == "Updated")
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Note color updated" });
                    }
                    return this.BadRequest(new { status = 401, isSuccess = false, Message = "Note color not updated" });
                }
                else
                {
                    return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Not authorized to change this note" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 401, isSuccess = false, Message = e.Message });
            }
        }

        /// <summary>
        /// API for adding a background image for a note
        /// </summary>
        /// <param name="imageURL"></param>
        /// <param name="noteid"></param>
        /// <returns></returns>
        [HttpPost("AddBgImage")]
        public IActionResult AddNoteBgImage(IFormFile imageURL, long noteid)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var NoteBgImage = this.FUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if(NoteBgImage.UserId == userid)
                {
                    var result = this.Nbl.AddNoteBgImage(imageURL,noteid);
                    if (result)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Note Bg Image updated" });
                    }
                    return this.BadRequest(new { status = 400, isSuccess = false, Message = "Note Bg image not updated" });
                }
                else
                {
                    return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Not logged in" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// API to remove Background image from a note
        /// </summary>
        [HttpDelete("RemoveBgImage")]
        public IActionResult DeleteNoteBgImage (long noteid)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var NoteBgImage = this.FUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if(noteid == 0)
                {
                    return this.NotFound(new { status = 404, isSuccess = false, Message = "Noteid not entered, Please enter a note id!" });
                }
                if(NoteBgImage.UserId == userid)
                {
                    var result = this.Nbl.DeleteNoteBgImage(noteid);
                    if (result)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Bg image deleted" });
                    }
                    return this.BadRequest(new { status = 400, isSuccess = false, Message = "Image note deleted" });
                }
                else
                {
                    return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Not logged in" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }
    }
}
