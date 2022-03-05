using CommonLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBL
    {
        public bool CreateLabel(LabelModel labelModel);
        public IEnumerable<Label> GetAllNoteLabels(long userid);
        public bool RemoveNoteLabel(LabelModel labelModel);
    }
}
