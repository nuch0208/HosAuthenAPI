using System.Globalization;
using HosAuthenAPI.Models;
using MySqlConnector;


namespace HosAuthenAPI.Services
{
    public class GetSerial
    {
        private readonly string _connectionString;

        public  GetSerial(string connectionString)
        {
            _connectionString = connectionString;
        } 

        public async Task<string> GetSerialNumber()
        {
            string serialNumber = null;

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT get_serialnumber(@name) AS oqueue", connection))
                {
                    // Create and add the MySqlParameter
                    DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
                    var name = "ovst-q-" + dateOnly.ToString("yyMMdd"); 
                    command.Parameters.Add(new MySqlParameter("@name", MySqlDbType.String));
                    command.Parameters["@name"].Value = name; // Assign a value to the parameter
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            serialNumber = reader["oqueue"].ToString();
                        }
                    }
                }
            }

            return serialNumber;
        }  

  

        public async Task<string> GetSerialNumberByParam(string param)
        {
            string serialNumber = null;

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT get_serialnumber(@param) AS oqueue", connection))
                {
                    command.Parameters.Add(new MySqlParameter("@param", param));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            serialNumber = reader["oqueue"].ToString();
                        }
                    }
                }
            }

            return serialNumber;
        } 


     }
}