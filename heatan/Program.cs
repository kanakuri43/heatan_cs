using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heatan
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dc = new DatabaseController();
            using (SqlCommand command = new SqlCommand("usp_insert_room_status", dc.Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@arg_sense_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@arg_temperature", 12.34);
                command.Parameters.AddWithValue("@arg_heating_status", 1);

                command.Parameters.Add("ReturnValue", SqlDbType.Int);
                command.Parameters["ReturnValue"].Direction = ParameterDirection.ReturnValue;

                command.ExecuteNonQuery();

            }

        }
    }
}
