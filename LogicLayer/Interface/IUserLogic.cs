using CommonLayer.Models;
using ReposetoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Interface
{
    public interface IUserLogic
    {
         UserEntity UserRegistration(UserRegistrationModel registrationModel);
        string UserLogin(UserLoginModel user);
        public object GetAllUsers();
        public bool Deleteuser(string fname);
        public bool UpdateUserDetialsName(string name, UserUpdateModel user);
        public bool UpdateUserDetails(long userid, UserUpdateModel user);
        public bool Mail(string mail);
        public ForgotPasswordModel ForgotPassword(string Email);
        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel);
        public object GetDetailsByName(string name);
        public int CountNumberOfUsers();
        //  public string UserLogins(UserLoginModel user, GetDetails getDetails);
        public TokenModel LoginMethod(UserLoginModel userLoginModel);
       // public UserEntity UpdateDetails(UserUpdateModel userUpdateModel);
    }
}
