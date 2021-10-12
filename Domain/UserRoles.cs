using System.Runtime.Serialization;

namespace Domain
{
    public enum UserRoles
    {
        [EnumMember(Value = "student")]
        Student = 0,
        
        [EnumMember(Value = "coach")]
        Coach = 1,
        
        [EnumMember(Value = "admin")]
        Admin = 2
    }
}