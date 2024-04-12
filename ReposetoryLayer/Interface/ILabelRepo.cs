using ReposetoryLayer.Entity;

namespace ReposetoryLayer.Interface
{
    public interface ILabelRepo
    {
        public bool AddLabel(long userid, long noteid, string labelName);
        public LabelEntity UpdateLable(long userId, long labelId, string labelname);
        public IEnumerable<LabelEntity> GetAlllabels(long userid);
        public LabelEntity DeleteLabel(long userId, long labelId);
    }
}