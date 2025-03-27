using VaccinationService.Entities;
using Microsoft.EntityFrameworkCore;
using Vaccinations;
using Grpc.Core;

namespace VaccinationService.Services
{
	public partial class VaccinationServiceImpl : Vaccinations.VaccinationService.VaccinationServiceBase
  {
    public override async Task<VaccinationRecordResponse> GetVaccinationRecordById(VaccinationRecordRequest request, ServerCallContext context)
		{
			var record = await _vaccinationRecordRepository.GetByIdAsync(request.RecordId);

			if (record == null)
				throw new RpcException(new Status(StatusCode.NotFound, "Record not found"));

			var response = _mapper.Map<VaccinationRecordResponse>(record);

			return response;
		}

    public override async Task<VaccinationRecordsResponse> GetAllVaccinationRecords(Empty request, ServerCallContext context)
		{
			var records = await _vaccinationRecordRepository.GetAllAsync();

			var result = new VaccinationRecordsResponse();
      result.VacccinationRecords.AddRange(_mapper.Map<IEnumerable<VaccinationRecordResponse>>(records));
      return result;
    }
    public override async Task<VaccinationRecordResponse> CreateVaccinationRecord(CreateVaccinationRecordRequest request, ServerCallContext context)
    {
      var record = await _vaccinationRecordRepository.AddAsync(_mapper.Map<VaccinationRecord>(request));
      
      return _mapper.Map<VaccinationRecordResponse>(record);
    }

    public override async Task<VaccinationRecordResponse> UpdateVaccinationRecord(UpdateVaccinationRecordRequest request, ServerCallContext context)
    {
      var record = await _vaccinationRecordRepository.GetByIdAsync(request.Id);

      if (record == null)
        throw new RpcException(new Status(StatusCode.NotFound, "Record not found"));

      var result = await _vaccinationRecordRepository.UpdateAsync(_mapper.Map<VaccinationRecord>(request));

      return _mapper.Map<VaccinationRecordResponse>(result);
    }

    public override async Task<VaccinationRecordResponse> DeleteVaccinationRecord(VaccinationRecordRequest request, ServerCallContext context)
    {
      var result = await _vaccinationRecordRepository.GetByIdAsync(request.RecordId);
      if (result == null)
        throw new RpcException(new Status(StatusCode.NotFound, "Record not found"));

      await _vaccinationRecordRepository.DeleteAsync(result);

      return _mapper.Map<VaccinationRecordResponse>(result);
    }
  }
}
