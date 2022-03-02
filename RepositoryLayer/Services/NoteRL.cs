using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class NoteRL : INoteRL
    {
        /// <summary>
        /// Variables
        /// </summary>
        FundooUserNotesContext context;
        IConfiguration config;

        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="context"></param>
        /// <param name="_config"></param>
        public NoteRL(FundooUserNotesContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        /// <summary>
        /// Create note code
        /// </summary>
        /// <param name="noteModel"></param>
        /// <returns></returns>
        public bool CreateNote(NoteModel noteModel, long userid)
        {
            try
            {
                Note newNotes = new Note();
                newNotes.NoteId = noteModel.NoteId;
                newNotes.Title = noteModel.Title;
                newNotes.Body = noteModel.Body;
                newNotes.Reminder = noteModel.Reminder;
                newNotes.Color = noteModel.Color;
                newNotes.BgImage = noteModel.BgImage;
                newNotes.IsArchived = noteModel.IsArchived;
                newNotes.IsPinned = noteModel.IsPinned;
                newNotes.IsDeleted = noteModel.IsDeleted;
                newNotes.CreatedAt = DateTime.Now;
                newNotes.UserId = userid;
                //Adding the data to database
                this.context.NotesTable.Add(newNotes);
                //Save the changes in database
                int result = this.context.SaveChanges();
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

        /// <summary>
        /// Show user all his notes using this function
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Note> GetAllNotes(long userid)
        {
            try
            {
                return this.context.NotesTable.ToList().Where(x=>x.UserId == userid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update note and then change ModifiedAt time to modified date and time
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public string UpdateNotes(Note note)
        {
            try
            {
                if (note.NoteId != 0)
                {
                    this.context.Entry(note).State = EntityState.Modified;
                    this.context.SaveChanges();
                    return "Done";
                }
                return "Failed";
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Archive note function using ID of the note
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public bool ArchiveNote(long noteid)
        {
            try
            {
                var note = this.context.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (note.IsArchived == false)
                {
                    note.IsArchived = true;
                    note.IsPinned = false;
                    note.ModifiedAt = DateTime.Now;
                    this.context.Entry(note).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    note.IsArchived = false;
                    note.ModifiedAt = DateTime.Now;
                    this.context.Entry(note).State = EntityState.Modified;
                    context.SaveChanges();
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Function to Pin the note using note id
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public bool PinNote(long noteid)
        {
            try
            {
                var note = this.context.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (note.IsPinned == false)
                {
                    note.IsPinned = true;
                    note.IsArchived = false;
                    note.IsDeleted = false;
                    note.ModifiedAt = DateTime.Now;
                    this.context.Entry(note).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
                else 
                {
                    note.IsPinned = false;
                    note.ModifiedAt = DateTime.Now;
                    this.context.Entry(note).State = EntityState.Modified;
                    context.SaveChanges();
                    return false;
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete Note function using the ID of the note
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public bool DeleteNote(long noteid)
        {
            try
            {
                var note = this.context.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                if (note.IsDeleted == false)
                {
                    note.IsDeleted = true;
                    note.IsArchived = false;
                    note.IsPinned = false;
                    context.Entry(note).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    note.IsDeleted = false;
                    note.ModifiedAt = DateTime.Now;
                    this.context.Entry(note).State = EntityState.Modified;
                    context.SaveChanges();
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Forever delete a note
        /// </summary>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public bool ForeverDeleteNote(long noteid)
        {
            try
            {
                if (noteid > 0)
                {
                    var notes = this.context.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                    if (notes != null)
                    {
                        this.context.NotesTable.Remove(notes);
                        this.context.SaveChangesAsync();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Function for adding note color
        /// </summary>
        /// <param name="color"></param>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public string AddNoteColor(string color, long noteid)
        {
            try
            {
                if (noteid > 0)
                {
                    var note = this.context.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                    if (note != null)
                    {
                        note.Color = color;
                        note.ModifiedAt = DateTime.Now;
                        this.context.SaveChangesAsync();
                        return "Updated";
                    }
                    else
                    {
                        return "Failed";
                    }
                }
                return "Failed";
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Function for removing note color
        /// </summary>
        /// <param name="color"></param>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public string RemoveNoteColor(long noteid)
        {
            try
            {
                if (noteid > 0)
                {
                    var note = this.context.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                    if (note != null)
                    {
                        note.Color = "";
                        note.ModifiedAt = DateTime.Now;
                        this.context.SaveChangesAsync();
                        return "Updated";
                    }
                    else
                    {
                        return "Failed";
                    }
                }
                return "Failed";
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// function for adding a background image for a note
        /// </summary>
        /// <param name="imageURL"></param>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public bool AddNoteBgImage(IFormFile imageURL, long noteid)
        {
            try
            {
                if (noteid > 0)
                {
                    var note = this.context.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                    if (note != null)
                    {
                        Account acc = new Account(
                            config["Cloudinary:cloud_name"],
                            config["Cloudinary:api_key"],
                            config["Cloudinary:api_secret"]
                            );
                        Cloudinary Cld = new Cloudinary(acc);
                        var path = imageURL.OpenReadStream();
                        ImageUploadParams upLoadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(imageURL.FileName,path)
                        };
                        var UploadResult = Cld.Upload(upLoadParams);
                        note.BgImage = UploadResult.Url.ToString();
                        note.ModifiedAt = DateTime.Now;
                        this.context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteNoteBgImage(long noteid)
        {
            try
            {
                if (noteid > 0)
                {
                    var note = this.context.NotesTable.Where(x => x.NoteId == noteid).SingleOrDefault();
                    if (note != null)
                    {
                        note.BgImage = "";
                        note.ModifiedAt = DateTime.Now;
                        this.context.SaveChangesAsync();
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
