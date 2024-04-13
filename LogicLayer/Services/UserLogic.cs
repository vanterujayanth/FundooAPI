using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLayer.Models;
using LogicLayer.Interface;
using ReposetoryLayer.Entity;
using ReposetoryLayer.Interface;
using ReposetoryLayer.Migrations;

namespace LogicLayer.Services
{
    public  class UserLogic: IUserLogic
    {
        private readonly IUserRepo iuserRepo;
        public UserLogic(IUserRepo iuserRepo)
        {

            this.iuserRepo =  iuserRepo;
        }
        // for register
        public UserEntity UserRegistration(UserRegistrationModel registrationModel)
        {
            try
            {
                return iuserRepo.UserRegistration(registrationModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        // for login 
        public string UserLogin(UserLoginModel user)
        {
            return iuserRepo.UserLogin(user);
        }
        // to get all user details from the database
        public object GetAllUsers()
        {
            return iuserRepo.GetAllUsers();
        }
        // for update details of  user by user id
        public bool UpdateUserDetails(long userid, UserUpdateModel user)
        {
            return iuserRepo.UpdateUserDetails(userid, user);
        }

        // for update details of user by using the name
        public bool UpdateUserDetialsName(string name, UserUpdateModel user)
        {
            return iuserRepo.UpdateUserDetialsName(name, user);
        }
       
        // to check mail is exists or not
        public bool Mail(string mail)
        {
            return iuserRepo.Mail(mail);
        }
        // for  delete the user account from database
        public bool Deleteuser(string fname)
        {
            return iuserRepo.Deleteuser(fname);
        }
        // for forgot password 
        public ForgotPasswordModel ForgotPassword(string Email)
        {
            return iuserRepo.ForgotPassword(Email);
        }
        // for reset password
        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel)
        {
            return iuserRepo.ResetPassword(Email, resetPasswordModel);
        }

        public object GetDetailsByName(string name)
        {
            return iuserRepo.GetDetailsByName(name);

        }
        public int CountNumberOfUsers()
        {
            return iuserRepo.CountNumberOfUsers();
        }
        //public string UserLogins(UserLoginModel user, GetDetails getDetails)
        //{
        //    return iuserRepo.UserLogins(user, getDetails);
        //}
        public TokenModel LoginMethod(UserLoginModel userLoginModel)
        {
            return iuserRepo.LoginMethod(userLoginModel);
        }
        public UserEntity UpdateDetails(UserUpdateModel userUpdateModel)
        {
            return iuserRepo.UpdateDetails(userUpdateModel);
        }
    }
}
