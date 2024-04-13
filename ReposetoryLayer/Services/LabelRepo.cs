using Microsoft.EntityFrameworkCore;
using ReposetoryLayer.Context;
using ReposetoryLayer.Entity;
using ReposetoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposetoryLayer.Services
{
    public class LabelRepo: ILabelRepo
    {
        private readonly FundooContext fundooContext;
        public LabelRepo(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public bool AddLabel(long userid, long noteid, string labelName)
        {
            var note = (from x in fundooContext.UserNotes
                        where x.UserID == userid && x.NoteId == noteid
                        select x).FirstOrDefault();

            if (note == null)
            {
                return false;
            }
            else
            {
                LabelEntity lb = new LabelEntity();
                lb.UserID = userid;
                lb.NoteId = noteid;
                lb.LabelName = labelName;
                fundooContext.Add(lb);
                fundooContext.SaveChanges();
                return true;
            }
        }
        public LabelEntity UpdateLable(long userId, long labelId, string labelname)
        {
            var label = (from x in fundooContext.Label
                         where x.UserID == userId && x.LabelID == labelId
                         select x).FirstOrDefault();

            if (label != null)
            {
                label.LabelName = labelname;
                fundooContext.Entry(label).State = EntityState.Modified;
                fundooContext.SaveChanges();
                return label;
            }
            return null;

        }

        public IEnumerable<LabelEntity> GetAlllabels(long userid)
        {
            var label = fundooContext.Label
                        .Where(x => x.UserID == userid)
                         .ToList();

            if (label != null)
            {
                return label;
            }
            else
            {
                return null;
            }
        }

        public LabelEntity DeleteLabel(long userId, long labelId)
        {
            var label = (from x in fundooContext.Label
                         where x.UserID == userId && x.LabelID == labelId
                         select x).FirstOrDefault();

            if (label != null)
            {
                fundooContext.Remove(label);
                fundooContext.SaveChanges();
                return label;
            }
            else
            {
                return null;
            }

        }
        // find notes that belongs to a particular label name
        public object GetDetailsByName(long noteid)
        {
            var details = (from x in fundooContext.UserNotes
                           where x.NoteId == noteid
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

    }
}
