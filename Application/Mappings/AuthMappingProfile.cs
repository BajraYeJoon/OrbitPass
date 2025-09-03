using AutoMapper;
using OrbitPass.Core.DTOs;
using OrbitPass.Core.Entities;

namespace OrbitPass.Application.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // PasswordHash will be set separately
            .ForMember(dest => dest.OrbitCoins, opt => opt.MapFrom(src => 200)); // Default starting coins

        CreateMap<User, AuthResponse>()
                    .ForMember(dest => dest.Token, opt => opt.Ignore()) // Handle separately

            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
    }
}