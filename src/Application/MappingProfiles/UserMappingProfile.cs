using Application.DTOs.Users;
using AutoMapper;
using Domain.Entities.ApplicationIdentity;

namespace Application.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterDTO, ApplicationUser>();

        }
    }
}
