using FilePicker.Scanner;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.DataAccess
{
    public class SqLiteService
    {
        private  string DbPath { get; }
        private readonly string TableName = "FileInformations";
        private readonly string dateformat = "yyyy-MM-dd HH:mm:ss";

        public SqLiteService()
        {
            this.DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TomsFilePicker/fileInfos.db");
            var dir = Path.GetDirectoryName(this.DbPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }


            this.CreateTable();


        }

        private SQLiteConnection CreateOpenConnection()
        {
            var c = new SQLiteConnection($"Data Source={this.DbPath}");
            return c.OpenAndReturn();
        }

        private void CreateTable()
        {
            using (var conn = CreateOpenConnection())
            {
                string Createsql =
    @"CREATE TABLE if not exists `FileInformations` (
	`Directory` VARCHAR NOT NULL,
	`Name` VARCHAR NOT NULL,
	`Extension` INT NOT NULL,
	`FullPath` INT NOT NULL PRIMARY KEY,
	`CreatedAt` DATETIME NOT NULL,
	`ModifiedAt` DATETIME NOT NULL,
	`SizeB` INT NOT NULL
);";
                var sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = Createsql;
                sqlite_cmd.ExecuteNonQuery();
            }
        }

        public void ClearTable()
        {

            using (var conn = CreateOpenConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = $"Delete From {this.TableName}";
                cmd.ExecuteNonQuery();
            }
        }
        public void RebuildDataBaseByData(IEnumerable<FileRepresentation> dadda)
        {
            this.ClearTable();

            var lastElement = dadda.LastOrDefault();
            if (lastElement == null)
            {
                return;
            }

            using (var conn = CreateOpenConnection())
            {
                StringBuilder cmdTeext = new StringBuilder();
                cmdTeext.AppendLine($"INSERT INTO {this.TableName} VALUES");
                foreach (var f in dadda)
                {
                    cmdTeext.Append($"('{Escape(f.Directory)}', '{Escape(f.Name)}', '{Escape(f.Extension)}', '{Escape(f.FullPath)}', '{f.CreatedAt.ToString(this.dateformat)}', '{f.ModifiedAt.ToString(this.dateformat)}', {f.SizeB})");

                    if (f != lastElement)
                    {
                        cmdTeext.AppendLine(",");
                    }
                    else
                    {
                        cmdTeext.AppendLine(";");
                    }
                }
                var cmd = conn.CreateCommand();
                cmd.CommandText = cmdTeext.ToString();
                cmd.ExecuteNonQuery();
            }

            string Escape(string s)
            {
                return s.Replace("'", "''");
            }
        }

        public IEnumerable<FileRepresentation> ReadData()
        {
            List<FileRepresentation> result = new List<FileRepresentation>();

            using (var conn = CreateOpenConnection())
            {

                var cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT * FROM {this.TableName}";

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    FileRepresentation r = new FileRepresentation()
                    {
                        Directory = reader.GetString(0),
                        Name = reader.GetString(1),
                        Extension = reader.GetString(2),
                        FullPath = reader.GetString(3),
                        CreatedAt = reader.GetDateTime(4),
                        ModifiedAt = reader.GetDateTime(5),
                        SizeB = reader.GetInt64(6),
                    };
                    result.Add(r);
                }
            }
            return result;
        }
    }
}
