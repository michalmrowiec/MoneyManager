using BlazorApp1.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp1.Server.Services
{
    public class TrackerFileService //: ITrackerService
    {
        public void Post(RecordItemDto recordItem)
        {
            SaveToJsonFile(recordItem);
        }

        public List<RecordItemDto> GetAllRecords()
        {
            return ReadFromFile();
        }

        private static void SaveToJsonFile(RecordItemDto recordItem)
        {
            string patch = "dataFileRecords.txt";
            List<RecordItemDto>? listOfRecords = new();

            if (!File.Exists(patch))
                File.Create(patch);

            using (StreamReader sr = File.OpenText(patch))
            {
                string json = sr.ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                    listOfRecords = JsonConvert.DeserializeObject<List<RecordItemDto>>(json);
            }

            if (listOfRecords == null)
                listOfRecords = new List<RecordItemDto>();

            listOfRecords.Add(recordItem);

            using (StreamWriter sw = new(patch))
            {
                string json = JsonConvert.SerializeObject(listOfRecords);
                sw.Write(json);
            }
        }

        private static List<RecordItemDto> ReadFromFile()
        {
            string patch = "dataFileRecords.txt";
            List<RecordItemDto> listOfRecords = new();

            if (!File.Exists(patch))
                return listOfRecords;

            using (StreamReader sr = File.OpenText(patch))
            {
                string file = sr.ReadToEnd();
                if (!string.IsNullOrEmpty(file))
                {
                    var json = JsonConvert.DeserializeObject<List<RecordItemDto>>(file);
                    listOfRecords = json ?? listOfRecords;
                }
            }
            return listOfRecords;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
