using CommonLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReposetoryLayer.Context;
using ReposetoryLayer.Entity;
using ReposetoryLayer.Interface;
using ReposetoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposetoryLayer.Services
{
    public class NoteRepo:INotesRepo
    {
        private readonly FundooContext fundooContext;

        public NoteRepo(FundooContext fundooContext, IConfiguration config)
        {
            this.fundooContext = fundooContext;

        }
        // for creation of notes
        public string  CreateNotes(NotesModel request, long userID)
        {
            if (userID != 0)
            {


                UserEntity user = (from x in fundooContext.user
                                   where x.userID == userID
                                   select x).FirstOrDefault();
                if (user != null)
                {
                    NotesEntity note = new NotesEntity();
                    note.Title = request.Title;
                    note.Description = request.Description;
                    note.Color = request.Color;
                    note.Remainder = request.Reminder;
                    note.IsArchive = request.IsArchive;
                    note.IsPinned = request.IsPinned;
                    note.IsTrash = request.IsTrash;
                    note.CreatedAt = request.CreatedAt;
                    note.ModifiedAt = request.ModifiedAt;
                    note.UserID = userID;


                    fundooContext.Add(note);
                    fundooContext.SaveChanges();
                    return "Note Created Sucessfully";

                }
            }
            else
            {
                return null;
            }

            return null;
        }
        // to update the notes 
        public string UpdateNote(long userid, long noteid, NotesModel model)
        {
            var res = (from x in fundooContext.UserNotes
                       where x.UserID == userid && x.NoteId == noteid
                       select x).FirstOrDefault();

            if (res != null)
            {
                res.Title = model.Title;
                res.Description = model.Description;
                res.Color = model.Color;
                res.Remainder = model.Reminder;
                res.IsArchive = model.IsArchive;
                res.IsPinned = model.IsPinned;
                res.IsTrash = model.IsTrash;
                res.CreatedAt = model.CreatedAt;
                res.ModifiedAt = model.ModifiedAt;
                res.UserID = userid;
                fundooContext.SaveChanges();
                return "updated sucessfully ";
            }
            else
            {
                return "it's not updated ";
            }
        }
        // to delete the  notes 

        public string DeleteNote(long userid, long noteid)
        {
            var note = (from x in fundooContext.UserNotes
                        where x.UserID == userid && x.NoteId == noteid
                        select x).FirstOrDefault();
            if (note != null)
            {
                fundooContext.Remove(note);
                fundooContext.SaveChanges();
                return note.ToString();
            }
            else
            {
                return null;
            }

        }
        // to get all notes in an account
        public IEnumerable<NotesEntity> GetAllNotes()
        {
            var notes = fundooContext.UserNotes.ToList();
            if (notes.Count == 0 || notes == null)
            {
                return null;
            }
            else
            {
                return notes.ToList();
            }
        }
        // to add color to notes
        public NotesEntity AddColor(long userid, long noteid, string color)
        {
            var note = (from x in fundooContext.UserNotes
                        where x.NoteId == noteid && x.UserID == userid
                        select x).FirstOrDefault();

            if (note != null)
            {
                note.Color = color;
                fundooContext.Entry(note).State = EntityState.Modified;
                fundooContext.SaveChanges();
                return note;
            }
            else
            {
                return null;
            }
        }
        // to get notes by user id and notes id
        public NotesEntity GetNoteById(long userid, long noteid)
        {
            var note = (from x in fundooContext.UserNotes
                        where x.UserID == userid && x.NoteId == noteid
                        select x).FirstOrDefault();
            if (note != null)
            {
                return note;
            }
            else
            {
                return null;
            }
        }
        // to get notes by user id
        public IEnumerable<NotesEntity> GetNotesByUserId(long userid)
        {
            var notes = (from x in fundooContext.UserNotes
                         where x.UserID == userid
                         select x).ToList();
            if (notes == null)
            {
                return null;
            }
            else
            {
                return notes;
            }

        }
        public NotesEntity TogglePin(long userid, long noteid)
        {
            var pin = (from x in fundooContext.UserNotes
                         where x.NoteId == noteid && x.UserID == userid
                         select x).FirstOrDefault();
            if (pin != null)
            {
                if (pin.IsPinned == true)
                {
                    pin.IsPinned = false;
                    fundooContext.Entry(pin).State = EntityState.Modified;
                    fundooContext.SaveChanges();
                    return pin;
                }
                else
                {
                    pin.IsPinned = true;
                    fundooContext.Entry(pin).State = EntityState.Modified;
                    fundooContext.SaveChanges();
                    return pin;
                }
            }
            else
            {
                return null;
            }
        }

        public NotesEntity ToggleArchive(long userid, long noteid)
        {
            var archive = (from x in fundooContext.UserNotes
                         where x.NoteId == noteid && x.UserID == userid
                         select x).FirstOrDefault();
             if (archive == null)
            {
                return null;
            }
            else
            {
                if (archive.IsArchive == true)
                {
                    archive.IsArchive = false;
                    if (archive.IsPinned == true)
                    {
                        archive.IsPinned = false;
                    }
                    if (archive.IsTrash == true)
                    {
                        archive.IsTrash = false;
                    }
                    fundooContext.Entry(archive).State = EntityState.Modified;
                    fundooContext.SaveChanges();
                    return archive;

                }
                else
                {
                    archive.IsArchive = true;
                    if (archive.IsPinned == true)
                    {
                        archive.IsPinned = false;
                    }
                    if (archive.IsTrash == true)
                    {
                        archive.IsTrash = false;
                    }
                }
                fundooContext.Entry(archive).State = EntityState.Modified;
                fundooContext.SaveChanges();
                return archive;

            }

        }
        public NotesEntity ToggleTrash(long userid, long noteid)
        {
            var trash = (from x in fundooContext.UserNotes
                         where x.NoteId == noteid && x.UserID == userid
                         select x).FirstOrDefault();
            if (trash == null)
            {
                return null;
            }
            else
            {
                if (trash.IsTrash == true)
                {
                    trash.IsTrash = false;
                    if (trash.IsPinned == true)
                    {
                        trash.IsPinned = false;
                    }
                    if (trash.IsArchive == true)
                    {
                        trash.IsArchive = false;
                    }
                    fundooContext.Entry(trash).State = EntityState.Modified;
                    fundooContext.SaveChanges();
                }
                else
                {
                    trash.IsTrash = true;
                    if (trash.IsPinned == true)
                    {
                        trash.IsPinned = false;
                    }
                    if (trash.IsArchive == true)
                    {
                        trash.IsArchive = false;
                    }
                    fundooContext.Entry(trash).State = EntityState.Modified;
                    fundooContext.SaveChanges();
                }

                return trash;
            }

        }

    }

}
