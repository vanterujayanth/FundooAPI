using Dapper;
using Microsoft.EntityFrameworkCore;
using ReposetoryLayer.Context;
using ReposetoryLayer.Entity;
using ReposetoryLayer.GlobalException;
using ReposetoryLayer.Interface;
using ReposetoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReposetoryLayer.Services
{
    public  class ColabRepo:IColabRepo

    {
        private readonly FundooContext fundooContext;
        public ColabRepo(FundooContext fundooContext) 
        {
            this.fundooContext = fundooContext;
        }
        private bool Isvalid(string email)
        {
            string pattern = @"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]{3,}$";
            return Regex.IsMatch(email, pattern);
        }
        public CollaboratorEntity AddCollaborator(int noteid, string email, int userId)
        {
            var UserDetails = fundooContext.UserNotes
                              .Where(x => x.UserID == userId && x.NoteId == noteid)
                              .FirstOrDefault();

            if (UserDetails != null)
            {
                CollaboratorEntity colloborator = new CollaboratorEntity();
                colloborator.UserId = userId;
                colloborator.NoteId = noteid;
                colloborator.C__Email = email;
                fundooContext.Collaborator.Add(colloborator);
                fundooContext.SaveChanges();
                return colloborator;

            }
            else
            {
                return null;
            }
        }
        public string RemoveCollaborator(int NoteId, string email, int UserId)
        {
            var del_colloborator = (from x in fundooContext.Collaborator
                                    where x.C__Email == email
                                    select x).FirstOrDefault();

            if (del_colloborator != null)
            {
                fundooContext.Collaborator.Remove(del_colloborator);
                fundooContext.SaveChanges();
                return "Colloborator deleted successfully";
            }
            else
            {
                return "Invalid Details";
            }
        } 
        public CollaboratorEntity GetCollaborationbyid(int CollabId)
        {
            var user = (from x in fundooContext.Collaborator
                        where x.ID == CollabId
                        select x).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        //find count of collaborators of a particular user
        public int CountNumberCollaborators(long userId)
        {
            var count = fundooContext.UserNotes.Count(x => x.UserID == userId);
            if (count > 0)
            {
                return count;
            }
            else
            {
                return 0;
            }
        }

    }
}
