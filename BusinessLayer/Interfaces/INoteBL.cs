using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface INoteBL
    {
        public bool CreateNote(NoteModel noteModel,long userid);
        public IEnumerable<Note> GetAllNotes(long userid);
        public string UpdateNotes(Note note);
        public bool ArchiveNote(long noteid);
        public bool PinNote(long noteid);
        public bool DeleteNote(long noteid);
        public bool ForeverDeleteNote(long noteid);
        public string AddNoteColor(string color, long noteid);
        public bool AddNoteBgImage(IFormFile imageURL, long noteid);
        public bool DeleteNoteBgImage(long noteid);
    }
}
