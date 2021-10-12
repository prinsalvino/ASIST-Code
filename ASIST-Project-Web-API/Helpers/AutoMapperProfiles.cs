using ASIST.DTO;
using AutoMapper;
using Domain;

namespace ASIST.Helpers
{
    public class AutoMapperProfiles : Profile
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
            CreateMap<CreateSkillDto, SkillStudent>();
            CreateMap<SkillStudent, SkillDto>();
            CreateMap<CreateOrganisationDto, Organisation>();
            CreateMap<Organisation, OrganisationDto>();
        }
    }
}