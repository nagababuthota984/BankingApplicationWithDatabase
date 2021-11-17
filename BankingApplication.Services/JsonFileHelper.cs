using BankingApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BankingApplication.Services
{
    public class JsonFileHelper : IDataProvider
    {


        public List<T> GetData<T>()//reads data from json file
        {

            string data = File.ReadAllText(Constant.filePath);
            if (string.IsNullOrEmpty(data)) return new List<T>();
            return JsonConvert.DeserializeObject<List<T>>(data);
        }

        public void WriteData<T>(List<T> dataToWrite)    //writes to json file
        {
            string serializedData = JsonConvert.SerializeObject(dataToWrite, Formatting.Indented);
            File.WriteAllText(Constant.filePath, serializedData);
            //data written into json.
        }

    }


    
}
