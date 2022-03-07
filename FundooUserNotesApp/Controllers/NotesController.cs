namespace FundooUserNotesApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BusinessLayer.Interfaces;
    using CommonLayer.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using Newtonsoft.Json;
    using RepositoryLayer.Context;
    using RepositoryLayer.Entities;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        /// <summary>
        /// Global variables
        /// </summary>
        private readonly INoteBL notebl;
        private readonly FundooUserNotesContext fUNcontext;
        private readonly IMemoryCache memCache;
        private readonly IDistributedCache distCache;
        

        /// <summary>
        /// Construction function
        /// </summary>
        /// <param name="Nbl"></param>
        /// <param name="funContext"></param>
        public NotesController(INoteBL notebl, FundooUserNotesContext funContext, IMemoryCache memCache, IDistributedCache distCache)
        {
            this.notebl = notebl;
            this.fUNcontext = funContext;
            this.memCache = memCache;
            this.distCache = distCache;
        }

        /// <summary>
        /// Create note api
        /// </summary>
        /// <param name="noteModel"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public IActionResult CreateNote(NoteModel noteModel)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                if (this.notebl.CreateNote(noteModel, userid))
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = "Note created", data = noteModel });
                }
                else
                {
                    return this.BadRequest(new { status = 401, isSuccess = false, message = "Failed to create note" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// API to retrieve all user notes
        /// </summary>
        /// <returns></returns>
        [HttpGet("Show")]
        public IActionResult GetAllNotes()
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                IEnumerable<Note> notes = this.notebl.GetAllNotes(userid);
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

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "NotesList";
            string serializedNotesList;
            var notesList = new List<Note>();
            var redisNotesList = await this.distCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                notesList = JsonConvert.DeserializeObject<List<Note>>(serializedNotesList);
            }
            else
            {
                notesList = (List<Note>)this.notebl.GetEveryonesNotes();
                serializedNotesList = JsonConvert.SerializeObject(notesList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await this.distCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return this.Ok(notesList);
        }

        [HttpGet("ShowFromId")]
        public IActionResult GetIDNote(long noteid)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                IEnumerable<Note> noteFromID = this.notebl.GetIDNote(noteid);
                if (noteFromID != null)
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = "Successful", data = noteFromID });
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
        /// retrieving all notes for all users
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetEveryonesNotes()
        {
            try
            {
                IEnumerable<Note> notes = this.notebl.GetEveryonesNotes();
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
        [HttpPut("Update")]
        public IActionResult UpdateNote(Note note)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                if (note.UserId == userid)
                {
                    var result = this.notebl.UpdateNotes(note);
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
        [HttpPut("Archive")]
        public IActionResult ArchiveNote(long noteid)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var archNote = this.fUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (archNote.UserId == userid)
                {
                    var result = this.notebl.ArchiveNote(noteid);
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
        [HttpPut("Pin")]
        public IActionResult PinNote(long noteid)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var pinnedNote = this.fUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (pinnedNote.UserId == userid)
                {
                    var result = this.notebl.PinNote(noteid);
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
        [HttpPut("Delete")]
        public IActionResult DeleteNote(long noteid)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var deletedNote = this.fUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (deletedNote.UserId == userid)
                {
                    var result = this.notebl.DeleteNote(noteid);
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
                var fDeleNote = this.fUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (fDeleNote.UserId == userid)
                {
                    var result = this.notebl.ForeverDeleteNote(noteid);
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
        [HttpPut("AddColor")]
        public IActionResult AddNoteColor(string color, long noteid)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var colourNote = this.fUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (colourNote.UserId == userid)
                {
                    var result = this.notebl.AddNoteColor(color, noteid);
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
        /// API to remove from notes
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        [HttpPut("RemoveColor")]
        public IActionResult RemoveNoteColor(long noteid)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var colourNote = this.fUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (colourNote.UserId == userid)
                {
                    var result = this.notebl.RemoveNoteColor(noteid);
                    if (result == "Updated")
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Note color updated" });
                    }
                    return this.BadRequest(new { status = 400, isSuccess = false, Message = "Note color not updated" });
                }
                else
                {
                    return this.Unauthorized(new { status = 401, isSuccess = false, Message = "Not authorized to change this note" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.Message });
            }
        }

        /// <summary>
        /// API for adding a background image for a note
        /// </summary>
        /// <param name="imageURL"></param>
        /// <param name="noteid"></param>
        /// <returns></returns>
        [HttpPut("AddBgImage")]
        public IActionResult AddNoteBgImage(IFormFile imageURL, long noteid)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var noteBgImage = this.fUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (noteBgImage.UserId == userid)
                {
                    var result = this.notebl.AddNoteBgImage(imageURL, noteid);
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
        [HttpPut("RemoveBgImage")]
        public IActionResult DeleteNoteBgImage(long noteid)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var noteBgImage = this.fUNcontext.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (noteid == 0)
                {
                    return this.NotFound(new { status = 404, isSuccess = false, Message = "Noteid not entered, Please enter a note id!" });
                }
                if (noteBgImage.UserId == userid)
                {
                    var result = this.notebl.DeleteNoteBgImage(noteid);
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
