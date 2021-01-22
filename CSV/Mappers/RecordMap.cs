using CSV.Models;
using CsvHelper.Configuration;

namespace CSV.Mappers
{
    public sealed class RecordMap : ClassMap<Record>
    {
        public RecordMap() : base()
        {
            Map(x => x.Reference).Name(Constants.CsvHeaders.Reference);
            Map(x => x.AccountNumber).Name(Constants.CsvHeaders.AccountNumber);
            Map(x => x.Description).Name(Constants.CsvHeaders.Description);
            Map(x => x.StartBalance).Name(Constants.CsvHeaders.StartBalance);
            Map(x => x.Mutation).Name(Constants.CsvHeaders.Mutation);
            Map(x => x.EndBalance).Name(Constants.CsvHeaders.EndBalance);
        }
    }
}
