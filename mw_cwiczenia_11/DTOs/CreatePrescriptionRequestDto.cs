using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using mw_cwiczenia_11.Models;

namespace mw_cwiczenia_11.DTOs;

public class CreatePrescriptionRequestDto
{
    
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    
    
    public int IdDoctor { get; set; }
    public PatientDto Patient { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
    
    
}

public class PatientDto
{
    [Required]
    public int IdPatient { get; set; }
    
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }
    
    
    [Required]
    [Column(TypeName = "date")]
    public DateTime Birthdate { get; set; }
}


public class MedicamentDto
{
    [Required]
    public int IdMedicament { get; set; }
    
    public int? Dose { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Details { get; set; }
}