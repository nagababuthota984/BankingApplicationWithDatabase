using BankingApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace BankingApplication.Services
{
    public class SqlHelper
    {
        public static string connectionString = "Server = localhost\\SQLEXPRESS; Database = BankStore; Trusted_Connection = True";
        public void EstablishConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
        }
        
        public List<Account> FetchAccountsByBankId(string bankid)
        {
            List<Account> accounts = new List<Account>();
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("select * from account where bankid=@bankid;");
                SqlParameter sqlParameter = new SqlParameter("@bankid", bankid);
                cmd.Parameters.Add(sqlParameter);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Account acc = new Account
                    {
                        AccountId = reader["accountId"].ToString(),
                        AccountNumber = reader["accountNumber"].ToString(),
                        UserName = reader["username"].ToString(),
                        Balance = Convert.ToDecimal(reader["balance"]),
                        Transactions = new List<Transaction>(),


                    };
                    acc.SetPassword(reader["password"].ToString());
                    accounts.Add(acc);
                }
                return accounts;
            }
            
        }
     



    }
}
