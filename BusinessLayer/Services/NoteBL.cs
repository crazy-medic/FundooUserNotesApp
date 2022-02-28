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
        public bool CreateNote(NoteModel noteModel)
        {
            try
            {
                return this.Nrl.CreateNote(noteModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

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
    }
}
