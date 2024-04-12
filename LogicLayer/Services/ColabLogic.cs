using ReposetoryLayer.Entity;
using ReposetoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services
{
    public class ColabLogic : IColabLogic
    {
        private readonly IColabRepo _colabRepo;

        public ColabLogic(IColabRepo colabRepo)
        {
            _colabRepo = colabRepo;
        }
        public CollaboratorEntity AddCollaborator(int noteid, string email, int userId)
        {
            return _colabRepo.AddCollaborator(noteid, email, userId);
        }
        public string RemoveCollaborator(int NoteId, string email, int UserId)
        {
            return _colabRepo.RemoveCollaborator(NoteId, email, UserId);
        }
        public CollaboratorEntity GetCollaborationbyid(int CollabId)
        {
            return _colabRepo.GetCollaborationbyid(CollabId);
        }
    }
}
