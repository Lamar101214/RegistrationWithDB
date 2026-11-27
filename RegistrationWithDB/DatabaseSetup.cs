using System;
using System.Data.SqlClient;
using System.IO;
namespace RegistrationWithDB
{
    internal static class DatabaseSetup
    {
        // Stable writable location: rebuilding the project never overwrites saved members.
        internal static readonly string Folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegistrationWithDB");
        internal static string ConnectionString
        {
            get
            {
                var builder = new SqlConnectionStringBuilder();
                builder.DataSource = @".\SQLEXPRESS";
                builder.IntegratedSecurity = true;
                builder.InitialCatalog = "ClubDB";
                builder.ConnectTimeout = 30;

                return builder.ConnectionString;
            }
        }
        internal static void Initialize()
        {
            Directory.CreateDirectory(Folder);
            var builder = new SqlConnectionStringBuilder(ConnectionString);
            builder.InitialCatalog = "master";
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT DB_ID(N'ClubDB')", connection))
                {
                    if (command.ExecuteScalar() == DBNull.Value)
                    {
                        string mdf = Path.Combine(Folder, "ClubDB.mdf").Replace("'", "''");
                        string ldf = Path.Combine(Folder, "ClubDB_log.ldf").Replace("'", "''");
                        command.CommandText = File.Exists(Path.Combine(Folder, "ClubDB.mdf"))
                            ? "CREATE DATABASE [ClubDB] ON (FILENAME = N'" + mdf + "') FOR ATTACH"
                            : "CREATE DATABASE [ClubDB] ON PRIMARY (NAME = N'ClubDB', FILENAME = N'" + mdf + "') LOG ON (NAME = N'ClubDB_log', FILENAME = N'" + ldf + "')";
                        command.ExecuteNonQuery();
                    }
                }
            }
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(@"IF OBJECT_ID(N'dbo.ClubMembers', N'U') IS NULL
                CREATE TABLE dbo.ClubMembers (
                    ID INT NOT NULL PRIMARY KEY,
                    StudentId BIGINT NOT NULL UNIQUE,
                    FirstName VARCHAR(50) NOT NULL,
                    MiddleName VARCHAR(50) NOT NULL,
                    LastName VARCHAR(50) NOT NULL,
                    Age INT NOT NULL CHECK (Age BETWEEN 1 AND 120),
                    Gender VARCHAR(50) NOT NULL,
                    Program VARCHAR(50) NOT NULL);", connection))
            { connection.Open(); command.ExecuteNonQuery(); }
        }
    }
}
