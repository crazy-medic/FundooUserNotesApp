using BusinessLayer.Interfaces;
using CommonLayer.Models;
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
        public bool CreateLabel(LabelModel labelModel)
        {
            try
            {
                return this.labelRl.CreateLabel(labelModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
