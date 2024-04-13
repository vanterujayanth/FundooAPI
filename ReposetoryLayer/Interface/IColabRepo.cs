using ReposetoryLayer.Entity;

namespace ReposetoryLayer.Interface
{
    public interface IColabRepo
    {
        public CollaboratorEntity AddCollaborator(int noteid, string email, int userId);
        public string  RemoveCollaborator(int NoteId, string email, int UserId);
        public CollaboratorEntity GetCollaborationbyid(int CollabId);
        public int CountNumberCollaborators(long userId);
    }
}