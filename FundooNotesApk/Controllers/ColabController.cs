using CommonLayer.Models;
using LogicLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReposetoryLayer.Entity;
using ReposetoryLayer.Migrations;

namespace FundooNotesApk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColabController : ControllerBase
    {
        private readonly IColabLogic icolabLogic;
        public ColabController(IColabLogic icolabLogic)
        {
            this.icolabLogic = icolabLogic;   
        }
        [Authorize]
        [HttpPost("Add_or_Create")]
        public IActionResult CreateColloborator(int noteId, string collob_Email)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserID").FirstOrDefault().Value);  
            if (userId != 0)
            {
                var collob = icolabLogic.AddCollaborator(noteId, collob_Email, userId);
                if (collob != null)
                {
                    return Ok(new ResponseModel<CollaboratorEntity>{ IsSuccess = true, Message = "Register Successfull", Data = collob });
                }
                else
                {
                    return NotFound(new ResponseModel<CollaboratorEntity> { IsSuccess = false, Message = "Register failed" });
                }
            }
            else
            {
                return BadRequest("invalid user");
            }

        }


        [HttpGet("GetAll")]
        [Authorize]
        public IActionResult GetColloboratorById(int colloborator_id)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserID").FirstOrDefault().Value);
            if (userId != 0)
            {
                var user = icolabLogic.GetCollaborationbyid(colloborator_id);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound("Colloborator Not Found");
                }

            }
            else
            {
                return BadRequest("please enter valid colloborator");
            }

        }

        [HttpDelete("Remove")]
        [Authorize]

        public IActionResult DeleteColloborator(int NoteId, string email, int UserId)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserID").FirstOrDefault().Value);
            if (userId != 0)
            {
                var del_collob = icolabLogic.RemoveCollaborator(NoteId, email, UserId);
                if (del_collob != null)
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Data = del_collob });
                }
                else
                {
                    return NotFound("Not Deleted");
                }

            }
            else
            {
                return BadRequest("please enter the valid email");
            }


        }
    }
}
