using AutoMapper;
using Vaccinations;
using VaccinationService.Entities;

public class VaccineProfile : Profile
{
    public VaccineProfile()
    {
        CreateMap<Vaccine, VaccineResponse>()
          .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Math.Round((double)src.Price, 2)))

          .ReverseMap();
        CreateMap<CreateVaccineRequest, Vaccine>()
          .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Math.Round((double)src.Price, 2)))
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
          .ReverseMap();
        CreateMap<UpdateVaccineRequest, Vaccine>()
          .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Math.Round((double)src.Price, 2)))
          .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
          .ReverseMap();

    }
}
