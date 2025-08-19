using System;
using System.Data;
using Microsoft.Data.SqlClient;

public static class DatabaseHelper
{
    private static readonly string _connectionString =
        "Server=localhost;Database=TaskManagementDataBase;User Id=sa;Password=123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";


    public static SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

}
