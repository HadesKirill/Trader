using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace MyTraderBlazor.Logic
{
    public class ActualController
    {
        public ActualController() { }

        public bool TestConnect()
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;";

            using (DbConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Выполните здесь ваш код для работы с базой данных

                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool LoadData()
        {
            try
            {

                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        public bool GetData()
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
