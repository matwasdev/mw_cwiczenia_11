namespace mw_cwiczenia_11.DTOs;

public class PatientPrescriptionsDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
    
    public List<PrescriptionDto> Prescriptions { get; set; }
}

public class PrescriptionDto{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentWithNameDto> Medicaments { get; set; }
    public DoctorDto Doctor { get; set; }
}

public class DoctorDto
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
}

public class MedicamentWithNameDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public int? Dose { get; set; }
    public string Description { get; set; }
}



