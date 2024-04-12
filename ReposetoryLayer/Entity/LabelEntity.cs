using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReposetoryLayer.Entity
{
    public class LabelEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LabelID { get; set; }
        public string LabelName { get; set; }

        [ForeignKey("user")]
        public long UserID { get; set; }

        [JsonIgnore]
        public virtual UserEntity user { get; set; }

        [ForeignKey("UserNotes")]
        public long NoteId { get; set; }

        [JsonIgnore]
        public virtual NotesEntity UserNotes { get; set; }

    }
}
