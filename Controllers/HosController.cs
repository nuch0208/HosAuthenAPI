using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using HosAuthenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HosAuthenAPI.Services;
using System.Globalization;

namespace HosAuthenAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    
    public class HosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GetSerial _service;
        public HosController(ApplicationDbContext context ,GetSerial service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet("{Cid}")]
        public ActionResult<PatientDto> GetHn(string Cid)
        {
            var patient = _context.Patients
                .Where(a => a.Cid == Cid)
                .Select(a => new PatientDto
                {
                    Hn = a.Hn,
                    Fname = a.Fname,
                    Lname = a.Lname,
                    Pname = a.Pname,
                    // Birthday = a.Birthday
                })
                .FirstOrDefault();

            if (patient == null)
            {
                return NotFound(); // Handle not found scenario
            }

            return Ok(patient);
        }
    
        // [HttpGet("{id}")] //ข้อมูลคนไข้
        
        // public IActionResult GetHn(string id)
        // {
        //     var query = 
        //         from a in _context.Patients
        //         where a.Cid == id
        //         select new
        //         {
        //             a.Hn, a.Fname, a.Lname, a.Birthday , ptname = $"{a.Pname} {a.Fname} {a.Lname}",
                    
        //         };
        //         return Json(query.Take(1));
        // }
        // [HttpGet("Dept")]   //ห้องตรวจ
        // public ActionResult GetDept(string Hn)
        // {
           
        //         DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
        //         var query = 
        //         from a in _context.Ovsts
        //         join b in _context.Spclties on  a.Spclty equals b.Spclty1
        //         join c in _context.Kskdepartments on  a.CurDep  equals  c.Depcode
        //         where a.Hn == Hn && a.Vstdate == dateOnly
        //         // where a.Vstdate == dateOnly && Convert.ToString( a.Oqueue ) == _para 
        //         select new
        //         {
        //             a.Vsttime, b.Name, c.Department
        //         };
        //         return Json(query.Take(1));
        // }

        [HttpGet("Dept")]
        public ActionResult<KskdepartmentDto> GetDept(string Hn)
        {
            DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
            var departmentInfo = _context.Ovsts
                .Join(_context.Spclties, a => a.Spclty, b => b.Spclty1, (a, b) => new { a, b })
                .Join(_context.Kskdepartments, ab => ab.a.CurDep, c => c.Depcode, (ab, c) => new { ab.a, ab.b, c })
                .Where(x => x.a.Hn == Hn && x.a.Vstdate == dateOnly)
                .Select(x => new KskdepartmentDto
                {
                    Vsttime = x.a.Vsttime,
                    SpcltyName = x.b.Name,
                    Department = x.c.Department
                })
                .FirstOrDefault();

            if (departmentInfo == null)
            {
                return NotFound(); // Handle not found scenario
            }

            return Ok(departmentInfo);
        }

        [HttpGet("Appoint")]
        public async Task<ActionResult> GetAppointmentsAsync()
        {
            DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
            var query =
                from o in _context.Oapps
                join p in _context.Patients on  o.Hn equals p.Hn
                join d in _context.Doctors on o.Doctor equals d.Code
                join v in _context.Ovsts on o.Hn equals v.Hn
                join k in _context.Kskdepartments on o.Depcode equals k.Depcode
                where o.Nextdate == dateOnly // Filter for current date
                group new { o, p, d, k, v } by new
                {
                    o.OappId,
                    o.Hn,
                    o.Doctor,
                    d.Name,
                    o.Vstdate,
                    o.Nextdate,
                    o.Nexttime,
                    o.Vn,
                    k.Department,
                    p.Pname,p.Fname,p.Lname,
                    p.Cid,
                    p.Hometel
                } into grouped
                select new
                {
                    grouped.Key.OappId,
                    grouped.Key.Hn,
                    grouped.Key.Doctor,
                    grouped.Key.Name, // Clinic Name
                    grouped.Key.Vstdate,
                    grouped.Key.Nextdate,
                    grouped.Key.Nexttime,
                    grouped.Key.Vn,
                    grouped.Key.Department,
                    grouped.Key.Pname,
                    grouped.Key.Fname,
                    grouped.Key.Lname,
                    grouped.Key.Cid,
                    grouped.Key.Hometel
                };

                    
            return Json(query);
        }

        [HttpGet("/app/{ById}")]
        public async Task<ActionResult<PatientDto>> GetAppointmentsByIdAsync(string ById)
        {
            DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
            
            var query = 
                from o in _context.Oapps
                join p in _context.Patients on o.Hn equals p.Hn
                join c in _context.Clinics on o.Clinic equals c.Clinic1
                join v in _context.Ovsts on o.Hn equals v.Hn
                join d in _context.Doctors on o.Doctor equals d.Code
                join k in _context.Kskdepartments on o.Depcode equals k.Depcode
                where p.Cid == ById && o.Nextdate == dateOnly
                group new { o, p, d, k } by new
                {
                    o.OappId,
                    o.Hn,
                    o.Doctor,
                    d.Name,
                    o.Vstdate,
                    o.Nextdate,
                    o.Nexttime,
                    o.Vn,
                    k.Department,
                    p.Pname,
                    p.Fname,
                    p.Lname,
                    p.Cid,
                    p.Hometel,
                    p.Birthday
                } into grouped
                select new PatientDto // Change this to your PatientDto structure
                {
                    OappId = grouped.Key.OappId,
                    Hn = grouped.Key.Hn,
                    Doctor = grouped.Key.Doctor,
                    DoctorName = grouped.Key.Name,
                    Vstdate = grouped.Key.Vstdate,
                    NextDate = grouped.Key.Nextdate,
                    NextTime = grouped.Key.Nexttime,
                    Vn = grouped.Key.Vn,
                    Department = grouped.Key.Department,
                    Pname = grouped.Key.Pname,
                    Fname = grouped.Key.Fname,
                    Lname = grouped.Key.Lname,
                    Cid = grouped.Key.Cid,
                    Hometel = grouped.Key.Hometel,
                    
                };

            return Ok(query.FirstOrDefault());
        }

        [HttpGet("/Pt/{Hn}")]
        public async Task<ActionResult<PttypenoDto>> GetPttypeAsync(string Hn)
        {
            var query = 
                from o in _context.Pttypenos
                join p in _context.Patients on o.Hn equals p.Hn
                join pt in _context.Pttypes on o.Pttype equals pt.Pttype1
                join h in _context.Hospcodes on o.Hospmain equals h.Hospcode1
                where o.Hn == Hn && o.Pttype == p.Pttype
            select new 
                {
                    o.Hn, o.Pttype, pt.Name, o.Begindate, o.Expiredate, o.Hospmain, Hname = h.Name
                };

            return Ok(query.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> CreateOpenVisit(OvstDto ovstDto)
        {
            // var otherClient = db.Ovsts.FirstOrDefault(c => c.HosGuid == clientDto.hosGuid);
            // if (otherClient != null)
            // {
            //     ModelState.AddModelError("HosGuid", "The HosGuid is already used");
            //     var validation = new ValidationProblemDetails(ModelState);
            //     return BadRequest(validation);
            // }

            var _Oqueue = await _service.GetSerialNumber();

            var ovst = new Ovst
            {
                HosGuid = "{"+Guid.NewGuid().ToString()+"}",
                Vn = DateTime.Now.ToString("yyMMddHHmmss", new CultureInfo("th-TH")),
                Hn = ovstDto.Hn,
                Vstdate = DateOnly.FromDateTime(DateTime.Now),
                Vsttime = TimeOnly.FromDateTime(DateTime.Now),
                Doctor = ovstDto.Doctor,
                Hospmain = ovstDto.Hospmain,
                Hospsub = ovstDto.Hospsub,
                Oqueue = Convert.ToInt32(_Oqueue),
                Ovstist = ovstDto.Ovstist,
                Ovstost = ovstDto.Ovstost,
                Pttype = ovstDto.Pttype,
                Pttypeno = ovstDto.Pttypeno,
                Rfrocs = ovstDto.Rfrocs,
                Spclty = ovstDto.Spclty,
                Hcode = "10734",
                CurDep = ovstDto.CurDep,
                CurDepBusy = "N",
                LastDep = ovstDto.LastDep,
                PtSubtype = 0,
                MainDep = ovstDto.MainDep,
                MainDepQueue = 0,
                VisitType = ovstDto.VisitType,
                NodeId = ovstDto.NodeId,
                Waiting = "Y",
                HasInsurance = ovstDto.HasInsurance,
                Staff = ovstDto.Staff,
                PtPriority = 0,
                OvstKey = "9564AB24894CF188CC14EB2D81857600",
            };

            _context.Ovsts.Add(ovst);
            _context.SaveChanges();

            return Ok(ovst);
        }

        
    }
}