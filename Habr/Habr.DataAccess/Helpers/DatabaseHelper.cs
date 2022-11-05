using Microsoft.Data.SqlClient;

namespace Habr.DataAccess.Helpers;

public static class DatabaseHelper
{
    private const string MasterConnectionString =
        "Server=(LocalDB)\\MSSQLLocalDB;database=master;Integrated security=true";
    
    public static void CreateDatabase(string databaseName)
    {
        using (var connexion = new SqlConnection(MasterConnectionString))
        {
            connexion.Open();

            using (var command = new SqlCommand(string.Format(
                       @"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{0}') 
                                    create database [{0}];
                      ", databaseName), connexion))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}