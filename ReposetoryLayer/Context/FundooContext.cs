using Microsoft.EntityFrameworkCore;
using ReposetoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposetoryLayer.Context
{
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UserEntity> user { get; set; }
        public DbSet<NotesEntity> UserNotes { get; set; }
        public DbSet<LabelEntity> Label { get; set; }
        public DbSet<CollaboratorEntity> Collaborator { get; set; }

    }
}
