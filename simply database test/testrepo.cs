using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace simply_database_test
{
    internal class testrepo
    {
        private string _connectionString;
        public testrepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<testallergi> GetAll()
        {
            var testallergis = new List<testallergi>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT A_ID, A_Name FROM Allergie", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var testallergi = new testallergi
                        {
                            ID = (int)reader["A_ID"],
                            Name = (string)reader["A_Name"],
                            
                        };
                        testallergis.Add(testallergi);
                    }
                }
            }
            return testallergis;
        }

    }
}
