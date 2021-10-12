using System.Runtime.Serialization;

namespace Domain
{
    public enum SkillType
    {
        [EnumMember(Value = "crawling")]
        Crawling = 0,
        
        [EnumMember(Value = "jumping")]
        Jumping = 1,
        
        [EnumMember(Value = "agility")]
        Agility = 2,
        
        [EnumMember(Value = "ballhandling")]
         BallHandling= 3,
         
         [EnumMember(Value = "rolling")] 
         Rolling = 4
    }
}