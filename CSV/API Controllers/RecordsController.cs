using System;
using System.Collections.Generic;
using System.IO;
using CSP.Models;
using CSV.Models;
using CSV.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CSP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;
        public static String _fileName;

        public RecordsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public String Upload([FromForm] FileUpload objectFile)
        {
            try
            {
                if(objectFile.files.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "/files/";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (FileStream fileStream = System.IO.File.Create(path + objectFile.files.FileName))
                    {
                        _fileName = objectFile.files.FileName;
                        objectFile.files.CopyTo(fileStream);
                        fileStream.Flush();
                        return "Succeeded";
                    }
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception e)
            {
                return "Internal server error: " + e;
            }           
        }

        // /api/records
        [HttpGet]
        public IEnumerable<Record> Get()
        {
            var _recordService = new RecordService();
            var records = _recordService.GetRecordList(_fileName);
            
            return InvalidRecord.invalidRecords;
        }
    }
}