using CommonLayer.Models;
using LogicLayer.Interface;
using ReposetoryLayer.Entity;
using ReposetoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services
{
    public class NotesLogic:INotesLogic
    {
        private readonly INotesRepo inotesRepo;
         public NotesLogic(INotesRepo inotesRepo)
        {
           this.inotesRepo = inotesRepo;
        }
        public string CreateNotes(NotesModel request, long userID)
        {
            return inotesRepo.CreateNotes(request, userID);
        }
        public string UpdateNote(long userid, long noteid, NotesModel model)
        {
            return inotesRepo.UpdateNote(userid, noteid, model);
        }
        public string DeleteNote(long userid, long noteid)
        {
            return inotesRepo.DeleteNote(userid, noteid);
        }
        public IEnumerable<NotesEntity> GetAllNotes()
        {
            return inotesRepo.GetAllNotes(); 
        }
        public IEnumerable<NotesEntity> GetNotesByUserId(long userid)
        {
            return inotesRepo.GetNotesByUserId(userid);
        }
        public NotesEntity AddColor(long userid, long noteid, string color)
        {
            return inotesRepo.AddColor(userid, noteid, color);
        }
        public NotesEntity GetNoteById(long userid, long noteid)
        {
            return inotesRepo.GetNoteById(userid, noteid);
        }
        public NotesEntity ToggleTrash(long userid, long noteid)
        {
            return inotesRepo.ToggleTrash(userid,noteid); 
                }
        public NotesEntity ToggleArchive(long userid, long noteid)
        {
            return inotesRepo.ToggleArchive(userid,noteid);
        }
        public NotesEntity TogglePin(long userid, long noteid)
        {
            return inotesRepo.TogglePin(userid,noteid);
        }
        public NotesEntity GetNotesByTitle(string title, string despriction)
        {
            return inotesRepo .GetNotesByTitle(title, despriction);
        }
        public  int CountNumberOfNotes(long userId)
        {
            return inotesRepo.CountNumberOfNotes(userId);
        }
        public IEnumerable<NotesEntity> GetNotesByDate(DateTime date)
        {
            return inotesRepo.GetNotesByDate(date);
        }
    }
}
