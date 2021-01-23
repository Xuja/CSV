using CSP.Models;
using CSV.Interface;
using CSV.Models;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace CSV.Services
{
    public class RecordService : IParserService
    {
        public List<Record> ReadFile(string path)
        {
            try
            {
                if (path.EndsWith(".csv"))
                {
                    using (var reader = new StreamReader(path, Encoding.GetEncoding("ISO-8859-1")))
                    using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en")))
                    {
                        var records = csv.GetRecords<Record>().ToList();
                        return records;
                    }
                }
                else if(path.EndsWith(".xml"))
                {
                    List<Record> records = new List<Record>();

                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);

                    XmlNodeList elemList = doc.GetElementsByTagName("record");
                    Record _obj = null;

                    foreach(XmlNode chldNode in elemList)
                    {
                        _obj = new Record();
                        _obj.Reference = Int32.Parse(chldNode.Attributes["reference"].Value);
                        _obj.AccountNumber = GetChildNodes(chldNode, "accountNumber");
                        _obj.Description = GetChildNodes(chldNode, "description");
                        _obj.StartBalance = Double.Parse(GetChildNodes(chldNode, "startBalance"));
                        _obj.Mutation = Double.Parse(GetChildNodes(chldNode, "mutation"));
                        _obj.EndBalance = Double.Parse(GetChildNodes(chldNode, "endBalance"));

                        records.Add(_obj);
                    }
                    return records;                  
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }              
        }

        public void WriteNewFile(string path, List<Record> records)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("ISO-8859-1")))
            using (CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.CreateSpecificCulture("en")))
            {
                cw.WriteHeader<Record>();
                cw.NextRecord();
                foreach (Record r in records)
                {
                    cw.WriteRecord<Record>(r);
                    cw.NextRecord();
                }
            }
        }

        public String GetChildNodes(XmlNode node, String attribute)
        {
            var value = node.ChildNodes.Cast<XmlNode>().Where(x => x.Name.ToString() == attribute).Select(n => n.InnerText).SingleOrDefault();
            return value;
        }

        public List<Record> GetRecordList(string fn)
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

        public void CheckValidity(List<Record> records)
        {
            CheckBalanceValidity(records);
            CheckReferenceValidity(records);
        }

        public void CheckBalanceValidity(List<Record> records)
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

        public void CheckReferenceValidity(List<Record> records)
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

        public List<Record> GetInvalidRecords(List<Record> records)
        {
            var invalidRecords = records.Where(x => x.IsValid == false).ToList();
            return invalidRecords;
        }
    }
}

