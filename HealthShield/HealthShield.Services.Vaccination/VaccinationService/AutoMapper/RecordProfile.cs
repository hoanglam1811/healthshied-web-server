using AutoMapper;
using Vaccinations;
using VaccinationService.Entities;

public class RecordProfile : Profile
{
    public RecordProfile()
    {
        CreateMap<VaccinationRecord, VaccinationRecordResponse>()
          .ReverseMap();
        CreateMap<CreateVaccinationRecordRequest, VaccinationRecord>()
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
          .ReverseMap();
        CreateMap<UpdateVaccinationRecordRequest, VaccinationRecord>()
          .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
          .ReverseMap();

    }
}
