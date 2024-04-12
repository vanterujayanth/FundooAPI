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
    public class LabelLogic:ILabelLogic
    {
        private readonly ILabelRepo ilabelRepo; 
        public LabelLogic(ILabelRepo ilabelREpo) 
        {
            this.ilabelRepo = ilabelREpo;
        }

        public bool AddLabel(long userid, long noteid, string labelName)
        {
            return ilabelRepo.AddLabel(userid, noteid, labelName);
        }
        public LabelEntity UpdateLable(long userId, long labelId, string labelname)
        {
            return ilabelRepo.UpdateLable(userId,labelId,labelname);
        }
        public IEnumerable<LabelEntity> GetAlllabels(long userid)
        {
            return ilabelRepo.GetAlllabels(userid);
        }
        public LabelEntity DeleteLabel(long userId, long labelId)
        {
            return ilabelRepo.DeleteLabel(userId, labelId);
        }
    }
}
