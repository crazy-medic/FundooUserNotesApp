using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class CollabRL : ICollabRL
    {
        /// <summary>
        /// Global variables
        /// </summary>
        private readonly FundooUserNotesContext FUNContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FUNContext"></param>
        public CollabRL(FundooUserNotesContext FUNContext)
        {
            this.FUNContext = FUNContext;
        }
        
        /// <summary>
        /// Function to add a collaborator to a note
        /// </summary>
        /// <param name="collaborator"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool AddCollab(CollabModel collab)
        {
            try
            {
                var Collabnotedata = this.FUNContext.NotesTable.Where(x => x.NoteId == collab.NotesId).SingleOrDefault();
                var Collabuserdata = this.FUNContext.UserTable.Where(x => x.EmailID == collab.EmailId).SingleOrDefault();
                if (Collabnotedata != null && Collabuserdata != null)
                {
                    Collaborator newCollaborator = new Collaborator();
                    newCollaborator.UserID = Collabuserdata.UserID;
                    newCollaborator.NoteId = collab.NotesId;
                    newCollaborator.CollabEmail = collab.EmailId;
                    //Adding the data to database
                    this.FUNContext.CollabTable.Add(newCollaborator);
                }

                //Save the changes in database
                int result = this.FUNContext.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RemoveCollab(CollabModel collabModel)
        {
            try
            {
                var collab = this.FUNContext.CollabTable.Where(x => x.CollabEmail == collabModel.EmailId).SingleOrDefault();
                if(collab != null)
                {
                    this.FUNContext.CollabTable.Remove(collab);
                    this.FUNContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// displays all collabs of a note
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public IEnumerable<Collaborator> Show(long noteid)
        {
            try
            {
                return this.FUNContext.CollabTable.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
