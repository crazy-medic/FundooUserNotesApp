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
    public class CollabBL : ICollabBL
    {
        ICollabRL collabRL;

        /// <summary>
        /// constructor function
        /// </summary>
        /// <param name="collabRL"></param>
        public CollabBL(ICollabRL collabRL)
        {
            this.collabRL = collabRL;
        }

        /// <summary>
        /// Function call to add a collaborator to any note
        /// </summary>
        /// <param name="collaborators"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool AddCollab(CollabModel collaborator)
        {
            try
            {
                return this.collabRL.AddCollab(collaborator);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Note> GetEveryCollab()
        {
            try
            {
                return this.collabRL.GetEveryCollab();
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
                return this.collabRL.RemoveCollab(collabModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Collaborator> Show(long noteid)
        {
            try
            {
                return this.collabRL.Show(noteid);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
