using CSV.Interface;
using CSV.Models;
using CsvHelper;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
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

        private String GetChildNodes(XmlNode node, String attribute)
        {
            var value = node.ChildNodes.Cast<XmlNode>().Where(x => x.Name.ToString() == attribute).Select(n => n.InnerText).SingleOrDefault();
            return value;
        }
    }
}

