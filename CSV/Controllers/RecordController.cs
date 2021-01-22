using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSP.Models;
using CSV.Models;
using CSV.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSV.Controllers
{
    public class RecordController : Controller
    {
        [HttpGet]
        public IActionResult Index(List<Record> records = null)
        {
            records = records == null ? new List<Record>() : records;

            return View(records);
        }

        [HttpPost]
        public IActionResult Index(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            if (file == null)
            {
                ViewBag.Message = "Please select a XML or CSV file";

                return View();
            }

            string fileName = $"{hostingEnvironment.WebRootPath}/files/{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            var records = this.GetRecordList(file.FileName);
            return Index(records);
        }

        public IActionResult GetInvalidRecordsToJson()
        {
            var invalidRecords = InvalidRecord.invalidRecords;
            return Ok(new { results = invalidRecords });
        }

        public IActionResult Export()
        {
            ExportInvalidRecords(InvalidRecord.invalidRecords);
            return RedirectToAction("Index");
        }

        private List<Record> GetRecordList(string fn)
        {
            List<Record> records = new List<Record>();
            var fileName = $"{Directory.GetCurrentDirectory()}{@"/wwwroot/files"}" + "/" + fn;

            var _recordService = new RecordService();            

            records = _recordService.ReadFile(fileName);

            CheckValidity(records);

            var invalidR = new List<Record>();

            foreach (Record r in GetInvalidRecords(records))
            {
                invalidR.Add(r);
            }

            InvalidRecord.invalidRecords = invalidR;

            return records;
        }

        private String GetRecordClass(Record record)
        {
            if (record.IsValid == false) return "invalid_row";
            return "";
        }

        private void CheckValidity(List<Record> records)
        {
            CheckBalanceValidity(records);
            CheckReferenceValidity(records);
        }

        private void CheckBalanceValidity(List<Record> records)
        {
            foreach (Record r in records)
            {
                double total = r.StartBalance + r.Mutation;
                
                String tb = total.ToString("#,##");
                String eb = r.EndBalance.ToString("#,##");

                String message = "[End Balance is not valid]";

                if (tb == eb) r.IsValid = null;
                else
                {
                    r.IsValid = false;
                    r.Note += message;
                }
            }            
        }        

        private void CheckReferenceValidity(List<Record> records)
        {
            String message = "[Reference is not valid]";

            var duplicate = records.GroupBy(x => x.Reference).Where(y => y.Count() > 1).Select(z => z.Key).ToList();

            foreach (Record r in records)
            {
                if (duplicate.Contains(r.Reference))
                {
                    r.IsValid = false;
                    r.Note += message;
                }
            }         
        }

        private void ExportInvalidRecords(List<Record> records)
        {
            var _recordService = new RecordService();
            var path = $"{Directory.GetCurrentDirectory()}{@"/wwwroot/files"}" + "/" + "invalid-records.csv";
        
            _recordService.WriteNewFile(path, records);
        }

        private List<Record> GetInvalidRecords(List<Record> records)
        {
            var invalidRecords = records.Where(x => x.IsValid == false).ToList();
            return invalidRecords;
        }
    }
}