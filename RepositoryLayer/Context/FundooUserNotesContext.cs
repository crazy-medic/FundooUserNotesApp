using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class FundooUserNotesContext : DbContext
    {
        public FundooUserNotesContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> UserTable { get; set; }

        public DbSet<Note> NotesTable { get; set; }

        public DbSet<Collaborator> CollabTable { get; set; }
        public DbSet<Label> LabelsTable { get; set; }

    }
}
