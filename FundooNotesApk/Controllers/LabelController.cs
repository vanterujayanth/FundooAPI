using CommonLayer.Models;
using LogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReposetoryLayer.Entity;

namespace FundooNotesApk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelLogic ilabelLogic;

        public LabelController(ILabelLogic ilabelLogic)
        {
            this.ilabelLogic = ilabelLogic;
        }
        [Authorize]
        [HttpPost("Addlabel")]
        public IActionResult Addlabel(long noteid, string labelName)
        {
            long userid = long.Parse(User.Claims.Where(x => x.Type == "UserID").
                          Select(x => x.Value).FirstOrDefault());

            var res = ilabelLogic.AddLabel(userid, noteid, labelName);
            if (res != null)
            {
                return Ok(new { success = true, message = "labelAdded", Data = res });

            }
            else
            {
                return BadRequest(new { success = false, message = "label not added" });
            }
        }
        [Authorize]
        [HttpPut("Updatelabel")]
        public IActionResult Updatelabel(long labelid, string labelName)
        {
            long userid = long.Parse(User.Claims.FirstOrDefault
                          (x => x.Type == "UserID")?.Value);

            var res = ilabelLogic.UpdateLable(userid, labelid, labelName);
            if (res != null)
            {
                return Ok(new { success = true, message = "label is updated", Data = res });

            }
            else
            {
                return BadRequest(new { success = false, message = "label not updated" });
            }

        }
        [Authorize]
        [HttpGet("GetAllLabels")]
        public IActionResult GetAlllabels()
        {
            long userid = long.Parse(User.Claims.FirstOrDefault
                          (x => x.Type == "UserID")?.Value ?? "0");

            var labels = ilabelLogic.GetAlllabels(userid);
            if (labels != null)
            {
                return Ok(new ResponseModel<LabelEntity>{ IsSuccess = true, Message = "all labels are", Data =(LabelEntity)labels });

            }
            else
            {
                return NotFound(new ResponseModel<LabelEntity> { IsSuccess = false, Message = "something went wrong" });
            }

        }
        [Authorize]
        [HttpDelete("Deletelabel")]
        public IActionResult DeleteLabel(long labelid)
        {
            long userid = long.Parse(User.Claims.FirstOrDefault
                          (x => x.Type == "UserID")?.Value);

            var res = ilabelLogic.DeleteLabel(userid, labelid);
            if (res != null)
            {
                return Ok(new ResponseModel<LabelEntity> { IsSuccess = true, Message = "label is deleted", Data = res });
            }
            else
            {
                return BadRequest(new ResponseModel<LabelEntity> { IsSuccess = false, Message = "label not deleted", Data = res });
            }
        }
    }

}
