﻿using CommonLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface INoteRL
    {
        public bool CreateNote(NoteModel noteModel);
        public IEnumerable<Note> GetAllNotes();
        public string UpdateNotes(Note note);
        public bool ArchiveNote(long noteid);
        public bool PinNote(long noteid);
        public bool DeleteNote(long noteid);
        public bool ForeverDeleteNote(long noteid);
    }
}
