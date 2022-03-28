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
    public class LabelBL : ILabelBL
    {
        ILabelRL labelRl;

        public LabelBL(ILabelRL labelRl)
        {
            this.labelRl = labelRl;
        }
        public bool AssignLabel(LabelModel labelModel)
        {
            try
            {
                return this.labelRl.AssignLabel(labelModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CreateLabel(string labelname, long userid)
        {
            try
            {
                return this.labelRl.CreateLabel(labelname, userid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Label> GetAllNoteLabels(long userid)
        {
            try
            {
                return this.labelRl.GetAllNoteLabels(userid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RemoveLabel(Label labelData)
        {
            try
            {
                return this.labelRl.RemoveLabel(labelData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RemoveNoteLabel(LabelModel labelModel)
        {
            try
            {
                return this.labelRl.RemoveNoteLabel(labelModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateLabel(string oldLabelName, string newLabelName)
        {
            try
            {
                return this.labelRl.UpdateLabel(oldLabelName, newLabelName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
