using AutoMapper;
using backend.Models.Domain;
using backend.Models.DTOs.Request;
using backend.Models.DTOS.Response;

namespace backend.Mappings
{
    public class UserQueueProfile : Profile
    {
        public UserQueueProfile()
        {
            CreateMap<UserQueueRequestDTO, UserQueue>();
            CreateMap<UserQueue, UserQueueResponseDTO>();
        }
    }
}