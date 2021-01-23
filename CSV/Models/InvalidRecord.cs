using CSV.Models;
using System.Collections.Generic;

namespace CSP.Models
{
    //creating record list to be accessible globally, used to store invalid records and called in controller. 
    public static class InvalidRecord
    {
        public static List<Record> invalidRecords;

    }
}
