using CommonLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICollabBL
    {
        public bool AddCollab(CollabModel collaborator);
        public IEnumerable<Collaborator> Show(long noteid);
        public bool RemoveCollab(CollabModel collabModel);
        public IEnumerable<Collaborator> GetEveryCollab();
    }
}
