using AutoMapper;
using backend.Models.Domain;
using backend.Models.DTOs.Request;
using backend.Models.DTOS.Response;

namespace backend.Mappings
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<CreateBookRequestDTO, Book>();
            CreateMap<UpdateBookRequestDTO, Book>().ForAllMembers(opts => opts.Condition((src, dest, srcMem) => srcMem != null));
            CreateMap<Book, BookResponseDTO>();
        }
    }
}