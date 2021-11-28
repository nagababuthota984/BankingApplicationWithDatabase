using BankingApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BankingApplication.Services
{
    public class JsonFileHelper
    {

        /// <summary>
        /// Reads the data from json file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetData<T>(string filePath)
        {

            string data = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(data)) return new List<T>();
            return JsonConvert.DeserializeObject<List<T>>(data);
        }

        public static void WriteData<T>(List<T> dataToWrite)    //writes to json file
        {
            string serializedData = JsonConvert.SerializeObject(dataToWrite, Formatting.Indented);
            File.WriteAllText(Constant.filePath, serializedData);
            //data written into json.
        }

    }



}