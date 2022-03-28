/// <summary>
/// Controller file for all API regarding Labels
/// </summary>
namespace FundooUserNotesApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLayer.Interfaces;
    using CommonLayer.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using RepositoryLayer.Context;
    using RepositoryLayer.Entities;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelController : ControllerBase
    {
        /// <summary>
        /// variables
        /// </summary>
        private readonly ILabelBL labelBL;
        private readonly FundooUserNotesContext fUNcontext;

        /// <summary>
        /// Initializes a new instance of the LabelController class
        /// </summary>
        /// <param name="labelBL"></param>
        /// <param name="FUNcontext"></param>
        public LabelController(ILabelBL labelBL, FundooUserNotesContext fUNcontext)
        {
            this.labelBL = labelBL;
            this.fUNcontext = fUNcontext;
        }

        [HttpPost("Create")]
        public IActionResult CreateLabel(string labelname)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                if (labelname == string.Empty)
                {
                    return this.NotFound(new { status = 204, isSuccess = false, Message = "Label name cannot be empty" });
                }
                else
                {
                    var result = this.labelBL.CreateLabel(labelname,userid);
                    if (result)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Label created" });
                    }
                    return this.BadRequest(new { status = 400, isSuccess = false, Message = "failed" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// API to create a label for note of noteid
        /// </summary>
        /// <param name="labelModel"></param>
        /// <returns></returns>
        [HttpPost("Assign")]
        public IActionResult AssignLabel(LabelModel labelModel)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var LabelNote = this.fUNcontext.NotesTable.Where(x => x.NoteId == labelModel.NotesId).SingleOrDefault();
                if (LabelNote.UserId == userid)
                {
                    var result = this.labelBL.AssignLabel(labelModel);
                    if (result)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Label created", data = labelModel.LabelName });
                    }
                    return this.BadRequest(new { status = 400, isSuccess = false, Message = "failed" });
                }
                return this.Unauthorized(new { status = 401, isSuccess = false, Message = "User not logged in" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// API to retrieve all user labels of the user logged in
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GettAllNoteLabels()
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                IEnumerable<Label> noteLabels = this.labelBL.GetAllNoteLabels(userid);
                if (noteLabels != null)
                {
                    return this.Ok(new { status = 200, isSuccess = true, Message = "Showing all labels of user", data = noteLabels });
                }
                else
                {
                    return this.BadRequest(new { status = 400, isSuccess = false, Message = "Failed to get labels" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// Update a label
        /// </summary>
        /// <param name="oldLabelName"></param>
        /// <param name="newLabelName"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public IActionResult UpdateLabel(string oldLabelName, string newLabelName)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var updateLabel = this.fUNcontext.LabelsTable.Where(x => x.LabelName == oldLabelName).FirstOrDefault();
                if (updateLabel.UserId == userid)
                {
                    var result = this.labelBL.UpdateLabel(oldLabelName, newLabelName);
                    if (result)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Label Updated", data = newLabelName });
                    }
                    else
                    {
                        return this.BadRequest(new { status = 400, isSuccess = false, Message = "failed" });
                    }
                }
                else
                {
                    return this.Unauthorized(new { status = 401, isSuccess = false, Message = "User not logged in" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// Unassign a note of its label
        /// </summary>
        /// <param name="labelModel"></param>
        /// <returns></returns>
        [HttpPut("RemoveFromNote")]
        public IActionResult RemoveNoteLabel(LabelModel labelModel)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var notedata = this.fUNcontext.NotesTable.Where(x => x.NoteId == labelModel.NotesId).FirstOrDefault();
                if (notedata.UserId == userid)
                {
                    var result = this.labelBL.RemoveNoteLabel(labelModel);
                    if (result)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Label removed from note", data = labelModel.LabelName });
                    }
                    else
                    {
                        return this.BadRequest(new { status = 400, isSuccess = false, Message = "failed" });
                    }
                }
                else
                {
                    return this.Unauthorized(new { status = 401, isSuccess = false, Message = "User not logged in" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// Permanently remove a label
        /// </summary>
        /// <param name="labelName"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public IActionResult RemoveLabel(string labelName)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var labelData = this.fUNcontext.LabelsTable.Where(x => x.LabelName == labelName).FirstOrDefault();
                if (labelData.UserId == userid)
                {
                    var result = this.labelBL.RemoveLabel(labelData);
                    if (result)
                    {
                        return this.Ok(new { status = 200, isSuccess = true, Message = "Label removed", data = labelName });
                    }
                    else
                    {
                        return this.BadRequest(new { status = 400, isSuccess = false, Message = "failed" });
                    }
                }
                else
                {
                    return this.Unauthorized(new { status = 401, isSuccess = false, Message = "User not logged in" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = 400, isSuccess = false, Message = e.InnerException.Message });
            }
        }
    }
}
