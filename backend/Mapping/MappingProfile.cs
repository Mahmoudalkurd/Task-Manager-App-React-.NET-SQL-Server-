using AutoMapper;
using backend.Domain.Entities;
using backend.DTOs;

namespace backend.Infrastructure.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tag, TagDto>().ReverseMap();
    }
}
