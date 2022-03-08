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

        public IEnumerable<Label> GetAllNoteLabels(long userid)
        {
            try
            {
                return this.context.LabelsTable.Where(x => x.UserId == userid).ToList();
                
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
                var label = this.context.LabelsTable.Where(x => x.LabelName == labelData.LabelName).FirstOrDefault();
                if (label != null)
                {
                    this.context.LabelsTable.Remove(label);
                    this.context.SaveChanges();
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

        public bool RemoveNoteLabel(LabelModel labelModel)
        {
            try
            {
                var label = this.context.LabelsTable.Where(x => x.LabelName == labelModel.LabelName).FirstOrDefault();
                if (label != null)
                {
                    this.context.LabelsTable.Remove(label);
                    this.context.SaveChanges();
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

        public bool UpdateLabel(string oldLabelName, string newLabelName)
        {
            try
            {
                var updateLabel = this.context.LabelsTable.Where(x => x.LabelName == oldLabelName).FirstOrDefault();
                if (updateLabel != null)
                {
                    updateLabel.LabelName = newLabelName;
                    this.context.LabelsTable.Update(updateLabel);
                    this.context.SaveChanges();
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
    }
}

