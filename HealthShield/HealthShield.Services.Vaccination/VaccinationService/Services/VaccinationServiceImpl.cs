using Grpc.Core;
using VaccinationService.Entities;
using Vaccinations;
using AutoMapper;

namespace VaccinationService.Services
{
	public partial class VaccinationServiceImpl : Vaccinations.VaccinationService.VaccinationServiceBase
	{
		private readonly IGenericRepository<Vaccine> _vaccineRepository;
		private readonly IGenericRepository<VaccinationPackage> _packageRepository;
		private readonly IGenericRepository<PackageVaccine> _packageVaccineRepository;
		private readonly IGenericRepository<VaccinationRecord> _vaccinationRecordRepository;
		private readonly IMapper _mapper;

		public VaccinationServiceImpl(IGenericRepository<Vaccine> vaccineRepository, IMapper mapper, IGenericRepository<VaccinationPackage> packageRepository,
      IGenericRepository<PackageVaccine> packageVaccineRepository,
      IGenericRepository<VaccinationRecord> vaccinationRecordRepository)
		{
      _vaccineRepository = vaccineRepository;
      _mapper = mapper;
      _packageRepository = packageRepository;
      _packageVaccineRepository = packageVaccineRepository;
      _vaccinationRecordRepository = vaccinationRecordRepository;
		}

		public override async Task<VaccineResponse> GetVaccineById(VaccineRequest request, ServerCallContext context)
		{
			var vaccine = await _vaccineRepository.GetByIdAsync(request.VaccineId);
			if (vaccine == null)
				throw new RpcException(new Status(StatusCode.NotFound, "Vaccine not found"));

			return _mapper.Map<VaccineResponse>(vaccine);
		}

    public override async Task<VaccinesResponse> GetAllVaccines(Empty empty, ServerCallContext context)
		{
			var vaccines = await _vaccineRepository.GetAllAsync();

			var res = new VaccinesResponse();
      res.Vaccines.AddRange(_mapper.Map<IEnumerable<VaccineResponse>>(vaccines));
      return res;
		}

    public override async Task<VaccineResponse> CreateVaccine(CreateVaccineRequest request, ServerCallContext context)
		{
			var vaccine = await _vaccineRepository.AddAsync(_mapper.Map<Vaccine>(request));

			return _mapper.Map<VaccineResponse>(vaccine);
		}

    public override async Task<VaccineResponse> UpdateVaccine(UpdateVaccineRequest request, ServerCallContext context)
		{
      var vaccine = await _vaccineRepository.GetByIdAsync(request.Id);
      if (vaccine == null)
          throw new RpcException(new Status(StatusCode.NotFound, "Vaccine not found"));

      var result = await _vaccineRepository.UpdateAsync(_mapper.Map<Vaccine>(request));

      return _mapper.Map<VaccineResponse>(result);
		}

    public override async Task<VaccineResponse> DeleteVaccine(VaccineRequest request, ServerCallContext context)
    {
        var vaccine = await _vaccineRepository.GetByIdAsync(request.VaccineId);
        if (vaccine == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Vaccine not found"));

        await _vaccineRepository.DeleteAsync(vaccine);

        return _mapper.Map<VaccineResponse>(vaccine);
    }
	}
}
