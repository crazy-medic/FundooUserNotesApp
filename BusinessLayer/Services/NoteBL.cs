using BusinessLayer.Interfaces;
using CommonLayer.Models;
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
        public IEnumerable<Note> GetAllNotes()
        {
            try
            {
                return this.Nrl.GetAllNotes();
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
    }
}
