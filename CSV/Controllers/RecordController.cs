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
            var _recordService = new RecordService();
            var records = _recordService.GetRecordList(file.FileName);
            return Index(records);
        }

        public IActionResult Export()
        {
            ExportInvalidRecords(InvalidRecord.invalidRecords);
            return RedirectToAction("Index");
        }       

        private void ExportInvalidRecords(List<Record> records)
        {
            var _recordService = new RecordService();
            var path = $"{Directory.GetCurrentDirectory()}{@"/wwwroot/files"}" + "/" + "invalid-records.csv";
        
            _recordService.WriteNewFile(path, records);
        }

        private String GetRecordClass(Record record)
        {
            if (record.IsValid == false) return "invalid_row";
            return "";
        }

    }
}