using CommonLayer.Models;
using ReposetoryLayer.Entity;

namespace ReposetoryLayer.Interface
{
    public interface INotesRepo
    {
        public string CreateNotes(NotesModel request, long userID);
        public string UpdateNote(long userid, long noteid, NotesModel model);
        public string DeleteNote(long userid, long noteid);
        public IEnumerable<NotesEntity> GetAllNotes();
        public NotesEntity AddColor(long userid, long noteid, string color);
        public NotesEntity GetNoteById(long userid, long noteid);
        public IEnumerable<NotesEntity> GetNotesByUserId(long userid);
        public NotesEntity ToggleTrash(long userid, long noteid);
        public NotesEntity ToggleArchive(long userid, long noteid);
        public NotesEntity TogglePin(long userid, long noteid);
        public NotesEntity GetNotesByTitle(string title, string despriction);
        public int CountNumberOfNotes(long userId);
        public IEnumerable<NotesEntity> GetNotesByDate(DateTime date);
    }
}