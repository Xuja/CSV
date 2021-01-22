using CsvHelper.Configuration.Attributes;

namespace CSV.Models
{
    public class Record
    {
        [Name(Constants.CsvHeaders.Reference)]
        public int Reference { get; set; }

        [Name(Constants.CsvHeaders.AccountNumber)]
        public string AccountNumber { get; set; }

        [Name(Constants.CsvHeaders.Description)]
        public string Description { get; set; }

        [Name(Constants.CsvHeaders.StartBalance)]
        public double StartBalance { get; set; }

        [Name(Constants.CsvHeaders.Mutation)]
        public double Mutation { get; set; }

        [Name(Constants.CsvHeaders.EndBalance)]
        public double EndBalance { get; set; }

        [Optional]
        public bool? IsValid { get; set; }

        [Optional]
        public string Note { get; set; }
    }
}
