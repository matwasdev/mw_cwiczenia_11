using mw_cwiczenia_11.DTOs;

namespace mw_cwiczenia_11.Services;

public interface IDbService
{
    Task AddPrescriptionAsync(CreatePrescriptionRequestDto requestDto);
    Task<PatientPrescriptionsDto> GetPatientPrescriptionsAsync(int id);
}