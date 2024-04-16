using CommonLayer.Models;
using GreenPipes.Caching;
using LogicLayer.Interface;
using LogicLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ReposetoryLayer.Context;
using ReposetoryLayer.Entity;
using ReposetoryLayer.Interface;
using ReposetoryLayer.Migrations;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;

namespace FundooNotesApk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesLogic inotesLogic;
        private readonly IDistributedCache _distributedCache;
        private readonly FundooContext _fundooContext;

        public NotesController(INotesLogic intesLogic, IDistributedCache distributedCache, FundooContext fundooContext)
        {
            inotesLogic = intesLogic;
            _distributedCache = distributedCache;
            _fundooContext = fundooContext;
        }
        [Authorize]
        [HttpPost("CreateNotes")]
        public IActionResult Create([FromForm] NotesModel model)
        {
            try
            {
                long userid = Convert.ToInt64(HttpContext.Session.GetInt32("userID"));
                // var userid = long.Parse(User.Claims.Where(x => x.Type == "UserID").FirstOrDefault().Value);
                var result = inotesLogic.CreateNotes(model, userid);
                if (result == null)
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "created Notes Successfully.", Data = result });

                }
                else
                {
                    return NotFound(new ResponseModel<NotesEntity> { IsSuccess = false, Message = "failed to create Notes" });
                }
            }

            catch (Exception e)
            {

                return NotFound(e.Message);
            }

}
        [HttpPut("UpdateNotes")]
        public IActionResult UpdateNotes(long Userid, long Noteid, [FromForm] NotesModel model)
        {
            var notes = inotesLogic.UpdateNote(Userid, Noteid, model);
            if (notes != null)
            {
                return Ok(new ResponseModel<bool>{ IsSuccess = true, Message = "Note Updated SuccessFully", Data = true });
            }
            else
            {
                return NotFound(new ResponseModel<NotesEntity>{ IsSuccess = false, Message = "Note not updated" });
            }
        }

        [HttpDelete("DeleteNote")]
        public IActionResult DeleteNote(long Userid, long Noteid)
        {
            var res = inotesLogic.DeleteNote(Userid, Noteid);
            if (res != null)
            {
                return Ok(new ResponseModel<NotesEntity>{ Message = "Note Deleted Successfully" });

            }
            else
            {
                return NotFound("Note not deleted or User//Note found");
            }
        }
        [HttpGet("GetAllNotes")]
        public IActionResult GetNotes()
        {
            var notes = inotesLogic.GetAllNotes();
            if (notes != null)
            {
                return Ok(new ResponseModel<NotesEntity>{ Message = "All Notes Fetched", Data = (NotesEntity)notes });
            }
            else
            {
                return NotFound(new { Message = "Error Occured" });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("AddColour")]
        public IActionResult AddColor(long noteid, string color)
        {
            long userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var note = inotesLogic.AddColor(userid, noteid, color);
            if (note != null)
            {
                return Ok(new ResponseModel<NotesEntity>{ Message = "Color Added Successfully", Data = note });
            }
            else
            {
                return NotFound(new ResponseModel<NotesEntity> { Message = "Color not Added", Data = note });
            }
        }

        [Authorize]
        [HttpGet("NoteById")]
        public IActionResult GetNoteById(long noteid)
        {
            long userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var note = inotesLogic.GetNoteById(userid, noteid);
            if (note != null)
            {
                return Ok(new ResponseModel<NotesEntity> { Message = "Note Fetched Successfully", Data = note });
            }
            else
            {
                return NotFound(new ResponseModel<NotesEntity> { Message = "Note not fetched", Data = note });
            }
        }
        [HttpGet("Get_By_Notes_Title")]
        public IActionResult GetNotesByTitle(String  title,string description)
        {
            var note = inotesLogic.GetNotesByTitle(title, description);
            if(note != null)
            {
                return Ok(new ResponseModel<NotesEntity> { Data = note });
            }
            else
            {
                return NotFound("data not found");

            }
        }

        [HttpGet("GetNotesByUserId")]
        public IActionResult GetNotesByUserId(long userid)
        {
            var notes = inotesLogic.GetNotesByUserId(userid);
            if (notes != null)
            {
                return Ok(new ResponseModel<NotesEntity> { Message = "User notes are", Data = (NotesEntity)notes });
            }
            else
            {
                return NotFound(new ResponseModel<NotesEntity> { Message = "User notes are not found", Data = (NotesEntity)notes });
            }
        }
        [Authorize]
        [HttpPost("GogglePin")]
        public IActionResult TogglePin(long noteid)
        {
            long userid = long.Parse(User.Claims.Where(x => x.Type == "UserID").FirstOrDefault().Value);
            var pin = inotesLogic.TogglePin(userid, noteid);
            if (pin != null)
            {
                return Ok(new ResponseModel<NotesEntity> { IsSuccess = true, Message = "TogglePinned Successsfully", Data = pin });
            }
            else
            {
                return NotFound(new ResponseModel<NotesEntity> { IsSuccess = false, Message = "TogglePin not Successful/Something went wrong", Data = pin });
            }
        }

        [Authorize]
        [HttpPost("GoggleArchive")]
        public IActionResult ToggleArchive(long noteid)
        {
            long userid = long.Parse(User.Claims.Where(x => x.Type == "UserID").FirstOrDefault().Value);
            var archive = inotesLogic.ToggleArchive(userid, noteid);
            if (archive != null)
            {
                return Ok(new ResponseModel<NotesEntity> { IsSuccess = true, Message = "ToggleArchive Successsfully", Data = archive });
            }
            else
            {
                return Ok(new ResponseModel<NotesEntity> { IsSuccess = false, Message = "ToggleArchive not Successful/Something went wrong", Data = archive });
            }

        }

        [Authorize]
        [HttpPost("GoogleTrash")]

        public IActionResult ToggleTrash(long noteid)
        {
            long userid = long.Parse(User.Claims.Where(x => x.Type == "UserID").FirstOrDefault().Value);
            var trash = inotesLogic.ToggleTrash(userid, noteid);
            if (trash != null)
            {
                return Ok(new ResponseModel<NotesEntity> { IsSuccess = true, Message = "ToggleTrashed Successsfully", Data = trash });
            }
            else
            {
                return NotFound(new ResponseModel<NotesEntity> { IsSuccess = false, Message = "Toggled not successful", Data = trash });
            }
        }

               
        [HttpGet]
        [Route("Get/{Title}/{IsAchive}")]
        public async Task<List<NotesEntity>> GetAll(string Title, bool IsAchive)
        {
            if (!IsAchive)
            {
                return _fundooContext.UserNotes.Where(x => x.Title == Title).OrderByDescending(x => x.CreatedAt).ToList();
            }
            string CachKey = Title;
            byte[] CachedData = await _distributedCache.GetAsync(CachKey);
            List<NotesEntity> notesEntities = new();
            if (CachedData != null)
            {
                var CatchedDataString = Encoding.UTF8.GetString(CachedData);
                notesEntities = System.Text.Json.JsonSerializer.Deserialize<List<NotesEntity>>(CatchedDataString);

            }
            else
            {
                notesEntities  =_fundooContext.UserNotes.Where(x => x.Title == Title).OrderByDescending(x => x.CreatedAt).ToList();

                string cachedDataString = JsonConvert.SerializeObject(notesEntities);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _distributedCache.SetAsync(CachKey, dataToCache, options);
            }
            return notesEntities;
        }
        [HttpGet]
        [Route("count_Of_Notes")]
        public IActionResult GetNotesCount(long userid)
        {
            var userCount = inotesLogic.CountNumberOfNotes(userid);
            if (userCount != null)
            {
                return Ok(new { message = "Count the  Notes :", userCount });

            }
            else
            {
                return NotFound("no notes are created  ");
            }
        }
        [HttpGet("GetNotesByDate")]
        public IActionResult GetNotesBydate(DateTime date)
        {
            var notes = inotesLogic.GetNotesByDate(date);
            if (notes != null)
            {
                return Ok(new { Message = "this are  notes are found ", Data = notes });
            }
            else
            {
                return NotFound(new  { Message = "User notes are not found", Data = notes });
            }
        }
    }
}
