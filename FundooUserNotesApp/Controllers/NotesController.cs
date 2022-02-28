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
        private readonly FundooUserNotesContext FUNcontext;

        /// <summary>
        /// Construction function
        /// </summary>
        /// <param name="Nbl"></param>
        /// <param name="funContext"></param>
        public NotesController(INoteBL Nbl,FundooUserNotesContext funContext)
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
                if (this.Nbl.CreateNote(noteModel))
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = " note created successfully " });
                }
                else
                {
                    return this.BadRequest(new { status = 401, isSuccess = false, message = "unsuccessful Notes not Added" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("Shownotes")]
        public IEnumerable<Note> GetAllNotes()
        {
            try
            {
                IEnumerable<Note> notes = Nbl.GetAllNotes();
                if (notes != null)
                {
                    return this.Ok(new { status = 200, isSuccess = true, message = " Successful" });
                    
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

    }
}
