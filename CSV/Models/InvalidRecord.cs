using CSV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CSP.Models
{
    //creating record list to be accessible globally, used to store invalid records and called in controller. 
    public static class InvalidRecord
    {
        public static List<Record> invalidRecords;

    }
}
