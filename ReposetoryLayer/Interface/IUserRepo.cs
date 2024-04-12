using CommonLayer.Models;
using ReposetoryLayer.Entity;

namespace ReposetoryLayer.Interface
{
    public interface IUserRepo
    {
        public UserEntity UserRegistration(UserRegistrationModel registrationModel);
        public string UserLogin(UserLoginModel user);
        public object GetAllUsers();
        public bool Mail(string mail);
        public bool UpdateUserDetails(long userid, UserUpdateModel user);
        public bool UpdateUserDetialsName(string name, UserUpdateModel user);
        public bool Deleteuser(string fname);
        public ForgotPasswordModel ForgotPassword(string Email);
        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel);

        public object GetDetailsByName(string name);
        public int CountNumberOfUsers();

        //  public string UserLogins(UserLoginModel user, GetDetails getDetails);
        public TokenModel LoginMethod(UserLoginModel userLoginModel);
    }
}