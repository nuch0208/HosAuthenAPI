using System;
using System.Collections.Generic;

namespace HosAuthenAPI.Models;

public partial class PatientDto
{
    public int? OappId { get; set; }
    public string? Hn { get; set; }
    public string? Doctor { get; set; }
    public string? DoctorName { get; set; }
    public DateOnly? Vstdate { get; set; }
    public DateOnly? NextDate { get; set; }
    public TimeOnly? NextTime { get; set; }
    public string? Vn { get; set; }
    public string? Department { get; set; }
    public string? Pname { get; set; }
    public string? Fname { get; set; }
    public string? Lname { get; set; }
    public string? Cid { get; set; }
    public string? Hometel { get; set; }
}
