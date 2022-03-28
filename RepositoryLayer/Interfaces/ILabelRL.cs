using CommonLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRL
    {
        public bool AssignLabel(LabelModel labelModel);
        public IEnumerable<Label> GetAllNoteLabels(long userid);
        public bool RemoveNoteLabel(LabelModel labelModel);
        public bool RemoveLabel(Label labelData);
        public bool UpdateLabel(string oldLabelName, string newLabelName);
        public bool CreateLabel(string labelname, long userid);
    }
}
