using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSP.Models;
using CSV.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {

        // /api/records
        public IEnumerable<Record> Get()
        {
            return InvalidRecord.invalidRecords;
        }
    }
}