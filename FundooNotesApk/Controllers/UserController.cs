using CommonLayer.Models;
using LogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReposetoryLayer.Entity;

using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using LogicLayer.Services;
using Microsoft.AspNetCore.Identity;

namespace FundooNotesApk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserLogic iuserLogic;
        private readonly MassTransit.IBus bus;
        // private readonly IConfiguration _config;
        public UserController(IUserLogic iuserLogic,MassTransit.IBus bus)
        {
            this.iuserLogic = iuserLogic;
            this.bus = bus;

        }
        [HttpPost]
        [Route("Register")]
        // httplocalhost//api/user/register
        public IActionResult RegisterUser(UserRegistrationModel registrationModel)
        {
            try
            {
                if (iuserLogic.Mail(registrationModel.Email))
                {
                    return BadRequest("email  alredy exists");

                }
                else
                {

                    var result = iuserLogic.UserRegistration(registrationModel);
                    if (result != null)
                    {
                        return Ok(new ResponseModel<UserEntity> {IsSuccess = true, Message = "Registration Successful", Data = result });
                    }
                    else
                    {
                        return BadRequest(new ResponseModel<UserEntity> { IsSuccess = false, Message = "Registration Failed" });
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        
        [HttpPost]
        [Route("Login")]
        public IActionResult LoginUser(UserLoginModel loginModel)
        {
            if (loginModel == null)
            {
                return BadRequest();
            }

            try
            {
               
                var resust = iuserLogic.UserLogin(loginModel);
                if (resust != null)
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "login Sucessful", Data = resust });
                }
                else
                {
                    return NotFound(new ResponseModel<UserEntity>{ IsSuccess = false, Message = "login fail" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        // to get all details
        [HttpGet]
        [Route("getAllDAta")]
        public IActionResult GetAllUser()
        {
            var getAllUsers = iuserLogic.GetAllUsers();
            
            if (getAllUsers != null)
            {
                return Ok(getAllUsers);
                

            }
            else
            {
                return BadRequest("Users Not Found...");
            }


        }
       
        // to check mail is exists or not
        [HttpGet]
        [Route("email")]
        public IActionResult Mail(string mail)
        {
            try
            {
                var user = iuserLogic.Mail(mail);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        // for delete the data of the user by using the user name
        [HttpDelete]
        [Route("Delteuser")]

        public IActionResult DeleteUser(string fanme)
        {
            bool delteUser = iuserLogic.Deleteuser(fanme);
            if (delteUser != false)
            {
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "user is deleted", Data = delteUser });
            }
            else
            {
                return NotFound("With the Name User not Found");
            }
        }

        // for update user data by using the user id
        [HttpPut]
        [Route("UpdateByUserId")]

        public IActionResult UpdateUser(long id, UserUpdateModel user)
        {
            var updateUser = iuserLogic.UpdateUserDetails(id, user);
            if (updateUser)
            {
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "user is updated", Data = updateUser });
            }
            else
            {
                return NotFound("User Id not found and Not Updated User Details");
            }
        }

        // for update the user data by using name 
        [HttpPut]
        [Route("UpdateByUserName")]

        public IActionResult UpdateUserByName(string name, UserUpdateModel user)
        {
            var updateUser = iuserLogic.UpdateUserDetialsName(name, user);
            if (updateUser)
            {
                return Ok(new ResponseModel<UserEntity>{ IsSuccess = true, Message = "user is updated" });
            }
            else
            {
                return NotFound("User Id not found and Not Updated User Details");
            }
        }
        // get details by name
        [HttpGet]
        [Route("GetDetailsByName")]

        public IActionResult GetDetailsByName(string name)
        {
            var details = iuserLogic.GetDetailsByName(name);
            if (details != null)
            {
                return Ok(new { IsSuccess = true, Message = "user details are present in database", Data = details });
            }
            else
            {
                return NotFound("User name not found ");
            }
        }
        //forgot password

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string Email)
        {

            var password = iuserLogic.ForgotPassword(Email);

            if (password != null)
            {
                Send send = new Send();
                ForgotPasswordModel forgotPasswordModel = iuserLogic.ForgotPassword(Email);
                send.SendMail(forgotPasswordModel.Email, forgotPasswordModel.Token);
                Uri uri = new Uri("rabbitmq:://localhost/FunDooNotesEmailQueue");
                var endPoint = await bus.GetSendEndpoint(uri);
                await endPoint.Send(forgotPasswordModel);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Mail sent Successfully", Data = password.Token });
            }
            else
            {
                // to Handle the case where password is null
                return NotFound(new ResponseModel<string> { IsSuccess = false, Message = "Email Does not Exist" });
            }
        }

        [Authorize]
        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel reset)
        {
            string Email = (from claim in User.Claims
                            where claim.Type == "Email"
                            select claim.Value).FirstOrDefault();

            var res = iuserLogic.ResetPassword(Email, reset);
            if (res)
            {
                return Ok(new ResponseModel<ResetPasswordModel>{ IsSuccess = true, Message = "Password Reset is done" });

            }
            else
            {
                return NotFound("Password is not Updated");
            }
        }
        //find count of users in user table
        [HttpGet]
        [Route("count of users")]
        public IActionResult GetUserCount()
        {
            var userCount = iuserLogic.CountNumberOfUsers();
            if (userCount != null)
            {
                return Ok(new { message = "Count the  Users :",userCount });

            }
            else
            {
                return NotFound("no one is using ");
            }
        }
        [HttpPost("LoginMethod")]
        public IActionResult LoginMethod(UserLoginModel model)
        {
            var login = iuserLogic.LoginMethod(model);
            if (login != null)
            {
                long UserId = login.userID;
                int userId = Convert.ToInt32(UserId);
                HttpContext.Session.SetInt32("userID", userId);
                return Ok(new ResponseModel<TokenModel>{ IsSuccess = true, Message = "User Login Successful", Data = login});
            }
            else
            {
                return NotFound("User Login Unsuccessful");
            }

        }
    }

}
