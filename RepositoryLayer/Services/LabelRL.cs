using CommonLayer.Models;
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
    public class LabelRL : ILabelRL
    {
        FundooUserNotesContext context;

        public LabelRL(FundooUserNotesContext context)
        {
            this.context = context;
        }
        public bool CreateLabel(LabelModel labelModel)
        {
            try
            {
                var note = context.NotesTable.Where(x => x.NoteId == labelModel.NotesId).FirstOrDefault();
                if (note != null)
                {
                    Label label = new Label()
                    {
                        LabelName = labelModel.LabelName,
                        NoteId = note.NoteId,
                        UserId = note.UserId,
                    };
                    this.context.LabelsTable.Add(label);
                    int result = this.context.SaveChanges();
                    if (result > 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
