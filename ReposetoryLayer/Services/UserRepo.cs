using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReposetoryLayer.Context;
using ReposetoryLayer.Entity;
using ReposetoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using ReposetoryLayer.GlobalException;
using System.Diagnostics.Eventing.Reader;


namespace ReposetoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FundooContext fundooContext;
        private readonly IConfiguration config;
        public UserRepo(FundooContext fundooContext, IConfiguration config)
        {
            this.fundooContext = fundooContext;
            this.config = config;
        }
        // to encode 
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                var encodedData = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(encodedData);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        // to decode
        public static string DecodePassword(string password)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(password));
            }
            catch (FormatException ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine("Error decoding password: " + ex.Message);
                // Return a default value or throw a custom exception
                return null;                            // or return a default password
            }
        }
        public bool IsValidEmail(string email)
        {

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public UserEntity UserRegistration(UserRegistrationModel registrationModel)
        {

            if (!IsValidEmail(registrationModel.Email))
            {
                throw new InvalidEmailFormatException("Invalid email format");
            }
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = registrationModel.FirstName;
                userEntity.LastName = registrationModel.LastName;
                userEntity.Email = registrationModel.Email;
                //  userEntity.Password = registrationModel.Password;
                userEntity.Password = EncodePasswordToBase64(registrationModel.Password);
                //userEntity CreatedDate =DateTime.Now;
                //userEntity UpdatedDate =DateTime.Now;

                fundooContext.Add(userEntity);
                fundooContext.SaveChanges();

                return userEntity;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public UserEntity UpdateDetails(UserUpdateModel userUpdateModel)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = userUpdateModel.FirstName;
                userEntity.LastName = userUpdateModel.FirstName;
                fundooContext.SaveChanges();
                return userEntity;
            }
            catch(Exception ex) 
            {
                return null;
            }
           
        
        }
        // for update user details
        public bool UpdateUserDetails(long userid, UserUpdateModel user)
        {
            var updateuser = (from x in fundooContext.user
                              where x.userID == userid
                              select x).FirstOrDefault();

            if (updateuser != null)
            {
                updateuser.FirstName = user.FirstName;
                updateuser.LastName = user.LastName;
                updateuser.Email = user.Email;
                fundooContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        //Update by UserDetails with firstName

        public bool UpdateUserDetialsName(string name, UserUpdateModel user)
        {
            var updateuser = (from x in fundooContext.user
                              where x.FirstName == name
                              select x).FirstOrDefault();

            if (updateuser != null)
            {
                updateuser.FirstName = user.FirstName;
                updateuser.LastName = user.LastName;
                updateuser.Email = user.Email;
                fundooContext.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }
        }
        //search a user by name and show their details.
        public object GetDetailsByName(string name)
        {
            var details = (from x in fundooContext.user
                           where x.FirstName == name
                           select x).FirstOrDefault();

            if (details != null)
            {
                return details;
            }
            else
            { 
                return null;
            }
        }
    
                
        
        // to delete the user data from database
        public bool Deleteuser(string fname)
        {
            var delete = (from x in fundooContext.user
                          where x.FirstName == fname
                          select x).FirstOrDefault();

            if (delete != null)
            {
                fundooContext.user.Remove(delete);
                fundooContext.SaveChanges();
                return true;
            }
            return false;
        }
        //log in class
        public string UserLogin(UserLoginModel user)
        {
            try
            {
                string token = "";
                var userlogin = (from x in fundooContext.user
                                 where x.Email == user.Email
                                 select x).FirstOrDefault();

                // for checking email and password is contain in database
                if (userlogin != null)
                {
                    string userpass = DecodePassword(userlogin.Password);

                    if (userlogin.Email.Equals(user.Email) && userpass.Equals(user.Password))
                    {
                        return GenerateToken(userlogin.userID, userlogin.Email);

                    }
                }   
       
            }
            catch (Exception ex)
            {
                Console.WriteLine("not founded !");

            }
            return null;
        }
        // get all data from the database 
        public  object GetAllUsers()
        {
            var data = fundooContext.user.ToList();
              if (data != null)
                {
                    return data;
                }
                else
                {
                    return null;
                }

        }
       
        // foe token creation
        private string GenerateToken(long userID, string Email)
        {
            // Create a symmetric security key using the JWT key specified in the configuration
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            // Create signing credentials using the security key and HMAC-SHA256 algorithm
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // Define claims to be included in the JWT
            var claims = new[]
            {
                new Claim("Email",Email),
                new Claim("UserID",userID.ToString())
            };


            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Audience"],
               claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

            // method to check  the mail is their or not

            public bool Mail(string mail)
            {
            var userlogin = (from x in fundooContext.user
                             where x.Email == mail
                             select x).FirstOrDefault();

            if (userlogin!=null)   
               return true;
            
            else
            return false;

            }
        // method for forget password
           
        public ForgotPasswordModel ForgotPassword(string Email)
        {
            var User = (from user in fundooContext.user
                        where user.Email == Email
                        select user).FirstOrDefault();
            ForgotPasswordModel forgotPassword = new ForgotPasswordModel();
            forgotPassword.Email = User.Email;
            forgotPassword.UserId = User.userID;
            forgotPassword.Token = GenerateToken(User.userID, User.Email);
            return forgotPassword;
        }

        // metod for reset password
        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel)
        {
            UserEntity User = (from x in fundooContext.user
                               where x.Email == Email
                               select x).FirstOrDefault();

            if (User != null)
            {
                User.Password = EncodePasswordToBase64(resetPasswordModel.ConfirmPassword);
                //User.ChangedAt = DateTime.Now;
                fundooContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        //find count of users in user table

        public int CountNumberOfUsers()
        {
            var count = fundooContext.user.Count();
            if (count > 0)
            {
                return count;
            }
            else
            {
                return 0;
            }
        }
        public TokenModel LoginMethod(UserLoginModel userLoginModel)
        {
            var user = fundooContext.user.FirstOrDefault(s => s.Email == userLoginModel.Email);
            if (user != null)
            {
                //model.Password= EncodePassword(user.Password);
                TokenModel tk = new TokenModel();
                if (userLoginModel.Password == DecodePassword(user.Password) && userLoginModel.Email == user.Email)
                {
                    tk.Email = user.Email;
                    tk.FirstName = user.FirstName;
                    tk.LastName = user.LastName;
                  //  tk.Token = GenerateToken(user.Email, user.userID);
                    tk.userID = user.userID;
                    return tk;

                }
                return null;

            }
            return null;
        }
        //public string UserLogins(UserLoginModel user,GetDetails getDetails)
        //{
        //    try
        //    {
        //        string token = "";
        //        var userlogin = (from x in fundooContext.user
        //                         where x.Email == user.Email
        //                         select x).FirstOrDefault();

        //        // for checking email and password is contain in database
        //        if (userlogin != null)
        //        {
        //            string userpass = DecodePassword(userlogin.Password);

        //            if (userlogin.Email.Equals(user.Email) && userpass.Equals(user.Password))
        //            {
        //                return userlogin.Password;
        //               // return GenerateToken(userlogin.userID, userlogin.Email,userlogin.FirstName,userlogin.LastName,userlogin.Password);

        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("not founded !");

        //    }
        //    return null;
        //}
    }
}
