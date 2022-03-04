﻿using CommonLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ICollabRL
    {
        public bool AddCollab(CollabModel collaborator);
        public IEnumerable<Collaborator> Show(long noteid);
        public bool RemoveCollab(CollabModel collabModel);
    }
}