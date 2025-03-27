using AutoMapper;
using Google.Protobuf.Collections;
using Vaccinations;
using VaccinationService.Entities;

public class PackageProfile : Profile
{
    public PackageProfile()
    {
        CreateMap<VaccinationPackage, PackageResponse>()
          .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Math.Round((double)src.Price, 2)))
          .ForMember(dest => dest.Vaccines, opt => opt.MapFrom((src, dest, destMember, context) => 
            src.PackageVaccines != null 
                ? src.PackageVaccines
                    .Where(pv => pv.Vaccine != null) // Ensure no null values
                    .Select(pv => context.Mapper.Map<VaccineResponse>(pv.Vaccine)) // Use context.Mapper
                    .ToList()
                : new List<VaccineResponse>()))
          .ReverseMap();
        CreateMap<CreatePackageRequest, VaccinationPackage>()
          .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Math.Round((double)src.Price, 2)))
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
          .ReverseMap();
        CreateMap<UpdatePackageRequest, VaccinationPackage>()
          .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Math.Round((double)src.Price, 2)))
          .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
          .ReverseMap();
    }

    private static RepeatedField<VaccineResponse> ConvertToRepeatedField(IEnumerable<VaccineResponse> vaccines)
    {
      var repeated = new RepeatedField<VaccineResponse>();
      repeated.AddRange(vaccines);
      return repeated;
    }
}
