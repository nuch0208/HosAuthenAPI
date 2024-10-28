using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using HosAuthenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HosAuthenAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    
    public class HosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public ActionResult<PatientDto> GetHn(string id)
        {
            var patient = _context.Patients
                .Where(a => a.Cid == id)
                .Select(a => new PatientDto
                {
                    Hn = a.Hn,
                    Fname = a.Fname,
                    Lname = a.Lname,
                    Pname = a.Pname,
                    Birthday = a.Birthday
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
        public ActionResult GetAppointmentsAsync()
        {
            DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
            var query =
                from o in _context.Oapps
                join p in _context.Patients on  o.Hn equals p.Hn
                join d in _context.Doctors on o.Doctor equals d.Code
                join v in _context.Ovsts on o.Hn equals v.Hn
                join k in _context.Kskdepartments on o.Depcode equals k.Depcode
                where o.Nextdate == dateOnly // Filter for current date
            //    select new
            //      {
            //         o.OappId, o.Hn, o.Doctor, d.Name, o.Vstdate, o.Nextdate, o.Vn ,k.Department , ptname = $"{p.Pname} {p.Fname} {p.Lname}", p.Cid
                    
            //      };
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
                    PatientName = $"{p.Pname} {p.Fname} {p.Lname}",
                    p.Cid
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
                    grouped.Key.PatientName,
                    grouped.Key.Cid
                };

                    
            return Json(query.Take(100));
        }

        [HttpGet("App/{ById}")]
        public ActionResult GetAppointmentsฺByIdAsync(string ById)
        {
            DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
             var query = 
                from o in _context.Oapps
                join p in _context.Patients on  o.Hn equals p.Hn
                join c in _context.Clinics on o.Clinic equals c.Clinic1
                join d in _context.Doctors on o.Doctor equals d.Code
                join v in _context.Ovsts on o.Hn equals v.Hn
                // where o.Nextdate == dateOnly // Filter for current date
                where p.Hn == ById && o.Nextdate == dateOnly
               select new
                 {
                    o.OappId, o.Doctor, d.Name, v. Vstdate, o.Nextdate,o.Spclty , ptname = $"{p.Pname} {p.Fname} {p.Lname}",
                    
                 };
             return Json(query.Take(5));
        }

    }
}