using ASIST_Web_API.DTO;
using AutoMapper;
using Domain;

namespace ASIST_Web_API.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateStudentDto, Student>();
            CreateMap<Student, StudentDto>();
            CreateMap<ModifyStudentDto, Student>();
            CreateMap<CreateCoachDto, Coach>();
            CreateMap<Coach, CoachDto>();
            CreateMap<ModifyCoachDto, Coach>();
            CreateMap<Admin, AdminDto>();
            CreateMap<CreateAdminDto, Admin>();
            CreateMap<CreateSkillStudentDto, SkillStudent>();
            CreateMap<SkillStudent, SkillStudentDto>();
            CreateMap<CreateOrganisationDto, Organisation>();
            CreateMap<Organisation, OrganisationDto>();
            CreateMap<CreateSportAdviceDto, SportStudent>();
            CreateMap<SportStudent, SportAdviceDto>();
            CreateMap<TestAttempt, TestAttemptDto>();
            CreateMap<Sport, SportDto>();
            CreateMap<Skill, SkillDto>();
        }
    }
}