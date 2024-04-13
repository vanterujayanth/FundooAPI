using ReposetoryLayer.Entity;

namespace LogicLayer.Interface
{
    public interface ILabelLogic
    {
        public bool AddLabel(long userid, long noteid, string labelName);
        public LabelEntity UpdateLable(long userId, long labelId, string labelname);
        public IEnumerable<LabelEntity> GetAlllabels(long userid);
        public LabelEntity DeleteLabel(long userId, long labelId);
        public object GetDetailsByName(long noteid);
    }
}