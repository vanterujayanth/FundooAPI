using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReposetoryLayer.Entity

{
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string? C__Email { get; set; }
        [ForeignKey("user")]
        public long UserId { get; set; }

        [JsonIgnore]
        public virtual UserEntity user { get; set; }

        [ForeignKey("UserNotes")]
        public long NoteId { get; set; }

        [JsonIgnore]
        public virtual NotesEntity UserNotes { get; set; }
    }
}
