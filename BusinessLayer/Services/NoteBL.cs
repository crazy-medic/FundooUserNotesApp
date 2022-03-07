﻿using BusinessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NoteBL : INoteBL
    {
        /// <summary>
        /// Variables
        /// </summary>
        INoteRL Nrl;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Nrl"></param>
        public NoteBL(INoteRL Nrl)
        {
            this.Nrl = Nrl;
        }

        /// <summary>
        /// Adding a new note function
        /// </summary>
        /// <param name="noteModel"></param>
        /// <returns></returns>
        public bool CreateNote(NoteModel noteModel,long userid)
        {
            try
            {
                return this.Nrl.CreateNote(noteModel,userid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieving all notes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Note> GetAllNotes(long userid)
        {
            try
            {
                return this.Nrl.GetAllNotes(userid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve a specific note
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public IEnumerable<Note> GetIDNote(long noteid)
        {
            try
            {
                var result = this.Nrl.GetIDNote(noteid);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updating note contents and update time
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public string UpdateNotes(Note note)
        {
            try
            {
                string result = this.Nrl.UpdateNotes(note);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Un/Archive a note
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public bool ArchiveNote(long noteid)
        {
            try
            {
                var result = this.Nrl.ArchiveNote(noteid);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Un/Pin the note
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public bool PinNote(long noteid)
        {
            try
            {
                var result = this.Nrl.PinNote(noteid);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete/recover a note
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public bool DeleteNote(long noteid)
        {
            try
            {
                var result = this.Nrl.DeleteNote(noteid);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete a note forever
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public bool ForeverDeleteNote(long noteid)
        {
            try
            {
                var result = this.Nrl.ForeverDeleteNote(noteid);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// add a color to a note
        /// </summary>
        /// <param name="color"></param>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public string AddNoteColor(string color, long noteid)
        {
            try
            {
                var result = this.Nrl.AddNoteColor(color,noteid);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Remove a color assigned to a note
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public string RemoveNoteColor(long noteid)
        {
            try
            {
                var result = this.Nrl.RemoveNoteColor(noteid);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// add any background image to the note
        /// </summary>
        /// <param name="imageURL"></param>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public bool AddNoteBgImage(IFormFile imageURL, long noteid)
        {
            try
            {
                var result = this.Nrl.AddNoteBgImage(imageURL, noteid);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete the background image from a note
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public bool DeleteNoteBgImage(long noteid)
        {
            try
            {
                var result = this.Nrl.DeleteNoteBgImage(noteid);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Note> GetEveryonesNotes()
        {
            try
            {
                return this.Nrl.GetEveryonesNotes();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
