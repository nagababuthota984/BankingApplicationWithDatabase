using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication.Services
{
    public interface IDataProvider
    {
        List<T> GetData<T>();
        void WriteData<T>(List<T> dataToWrite);
    }
}
