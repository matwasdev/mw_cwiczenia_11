using Microsoft.EntityFrameworkCore;
using mw_cwiczenia_11.Data;
using mw_cwiczenia_11.DTOs;
using mw_cwiczenia_11.Exceptions;
using mw_cwiczenia_11.Models;

namespace mw_cwiczenia_11.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddPrescriptionAsync(CreatePrescriptionRequestDto requestDto)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            
            if(requestDto.DueDate < requestDto.Date)
                throw new BadRequestException("Due date must be greater than or equal to date");
            
            try
            {
                
                var patient = await _context.Patients.FirstOrDefaultAsync(e => e.IdPatient == requestDto.Patient.IdPatient);
                int patientId = requestDto.Patient.IdPatient;
                if (patient == null)
                {
                    patient = new Patient
                    {
                        FirstName = requestDto.Patient.FirstName,
                        LastName = requestDto.Patient.LastName,
                        Birthdate = requestDto.Patient.Birthdate,
                        
                    };
                    
                    await _context.Patients.AddAsync(patient);
                    await _context.SaveChangesAsync();
                    patientId = patient.IdPatient;
                }
                
                var doctor = await _context.Doctors.FirstOrDefaultAsync(e => e.IdDoctor == requestDto.IdDoctor);
                if (doctor == null)
                    throw new NotFoundException($"Doctor with id {requestDto.IdDoctor} not found");
                
                var medicamentsIds = requestDto.Medicaments.Select(m => m.IdMedicament).ToList();
                if(medicamentsIds.Count > 10)
                    throw new BadRequestException("Prescriptions cannot contain more than 10 medicaments");

                foreach (var medicamentId in medicamentsIds)
                {
                    var exists = _context.Medicaments.Any(e => e.IdMedicament == medicamentId);
                    if (!exists)
                        throw new NotFoundException($"Medicament with id {medicamentId} not found");
                }

                
                var prescription = new Prescription
                {
                    Date = requestDto.Date,
                    DueDate = requestDto.DueDate,
                    IdPatient = patientId,
                    IdDoctor = doctor.IdDoctor
                };
                await _context.Prescriptions.AddAsync(prescription);
                await _context.SaveChangesAsync();
                int prescriptionId = prescription.IdPrescription;

                foreach (var medicament in requestDto.Medicaments)
                {
                    var prescriptionMedicament = new PrescriptionMedicament
                    {
                        IdMedicament = medicament.IdMedicament,
                        IdPrescription = prescriptionId,
                        Dose = medicament.Dose,
                        Details = medicament.Details,
                    };
                    await _context.PrescriptionMedicaments.AddAsync(prescriptionMedicament);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<PatientPrescriptionsDto> GetPatientPrescriptionsAsync(int id)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(e => e.IdPatient == id);
        if(patient == null)
            throw new NotFoundException($"Patient with id {id} not found");

        
        var rawPrescriptions = await _context.Prescriptions
            .Where(e => e.IdPatient==id)
            .Include(e => e.Doctor)
            .Include(e => e.PrescriptionMedicaments)
                .ThenInclude(pm => pm.Medicament)
            .ToListAsync();            
        
        var prescriptions = rawPrescriptions.Select(e => new PrescriptionDto
        {
            IdPrescription = e.IdPrescription,
            Date = e.Date,
            DueDate = e.DueDate,
            Doctor = new DoctorDto
            {
                IdDoctor = e.IdDoctor,
                FirstName = e.Doctor.FirstName,
            },
            Medicaments = e.PrescriptionMedicaments.Select(m => new MedicamentWithNameDto
            {
                IdMedicament = m.IdMedicament,
                Name = m.Medicament.Name,
                Dose = m.Dose,
                Description = m.Medicament.Description,
            }).ToList()
        })
            .OrderBy(e => e.DueDate)
            .ToList();

        var patientPrescriptions = new PatientPrescriptionsDto()
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            
            Prescriptions = prescriptions,
        };
        return patientPrescriptions;
    }

    
}