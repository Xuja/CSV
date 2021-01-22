using CSV.Models;
using System.Collections.Generic;

namespace CSV.Interface
{
    public interface IParserService
    {
        List<Record> ReadFile(string path);

        void WriteNewFile(string path, List<Record> records);
    }
}
