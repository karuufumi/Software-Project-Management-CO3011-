using AutoMapper;
using backend.Models.Domain;
using backend.Models.DTOs.Request;
using backend.Models.DTOS.Response;

namespace backend.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserRequestDTO, User>();
            CreateMap<UpdateUserRequestDTO, User>().ForAllMembers(opts => opts.Condition((src, dest, srcMem) => srcMem != null));
            CreateMap<User, UserResponseDTO>();
        }
    }
}