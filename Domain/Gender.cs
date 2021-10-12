using System.Runtime.Serialization;

namespace Domain { 
    public enum Gender
    {
        [EnumMember(Value = "male")]
        Male = 0,
        
        [EnumMember(Value = "female")]
        Female = 1
    }
}