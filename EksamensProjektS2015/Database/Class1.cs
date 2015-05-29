using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Database
{
    public static class Functions
    {
        /*public static void UpdateTable<T>(SQLiteConnection dbConn, SQLiteCommand dbComm,string tableName, string[] attNames, ref T[] attvalues)
        {
            string sql = "Update table " + tableName + " set " + " where " + attNames;
            dbComm = new SQLiteCommand(sql, dbConn);
        }
        public static void Insertvalues<T>(string attName, ref T attVal)
        {
            string sql = "";
        }
        public static SQLiteDataReader TableSelectRow(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName, string tIdName, string tId)
        {
            string sql = "select * from " + tName + " where " + tIdName + "=" + tId;
            dbComm = new SQLiteCommand(sql, dbConn);
            return dbComm.ExecuteReader();
        }*/
        /// <summary>
        /// If you manually wants to write your own piece of code.
        /// It will execute non query.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="content"></param>
        public static void ManualFunction(SQLiteConnection dbConn, SQLiteCommand dbComm, string sql)
        {
            dbComm = new SQLiteCommand(sql, dbConn);
            dbComm.ExecuteNonQuery();
        }
        /// <summary>
        /// For creating a new Database file, if a file with same name doesn't already exist.
        /// Don't add *.db at the end of parameter.
        /// </summary>
        /// <param name="dbName"></param>
        public static void CreateDatabase(string dbName)
        {
            string dbPath = @"" + dbName + ".db";
            if (File.Exists(dbPath))
            {
                return;
            }
            else
            {
                SQLiteConnection.CreateFile(dbName + ".db");
            }
        }
        /// <summary>
        /// For creating a new table if a table with the same name doesn't already exist.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="tName"></param>
        /// <param name="tId"></param>
        /// <param name="tValues"></param>
        public static void CreateTable(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName, string[] tValues)
        {
            string sql = "Create table if not exists " + tName + "(";
            for (int i = 0; i < tValues.Length; i++)
            {
                sql += i == 0 ? tValues[i] : ", " + tValues[i];
            }
            sql += ")";
            dbComm = new SQLiteCommand(sql, dbConn);
            dbComm.ExecuteNonQuery();
        }
        /// <summary>
        /// Delete a table with the given name entirely.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="tName"></param>
        public static void DropTable(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName)
        {
            string sql = "drop table" + tName;
            dbComm = new SQLiteCommand(sql, dbConn);
            dbComm.ExecuteNonQuery();
        }
        /// <summary>
        /// For inserting values into an existing table.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="tName"></param>
        /// <param name="tId"></param>
        /// <param name="tValues"></param>
        public static void InsertValues(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName, string[] tValues)
        {
            string sql = "Insert into " + tName + " values (";
            for (int i = 0; i < tValues.Length; i++)
            {
                sql += i == 0 ? tValues[i] : ", " + tValues[i];
            }
            sql += ")";
            dbComm = new SQLiteCommand(sql, dbConn);
            dbComm.ExecuteNonQuery();
        }
        /// <summary>
        /// For updating values in an existing table.
        /// Updates only value in a row according to an int attribute.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="tName"></param>
        /// <param name="tIdName"></param>>
        /// <param name="tId"></param>
        /// <param name="tValues"></param>
        public static void UpdateTable(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName, string tIdName, int tId, string[] tValues)
        {
            string sql = "Update " + tName + " set ";
            for (int i = 0; i < tValues.Length; i++)
            {
                sql += i == 0 ? tValues[i] : ", " + tValues[i];
            }
            sql += " where " + tIdName + "=" + tId;
            dbComm = new SQLiteCommand(sql, dbConn);
            dbComm.ExecuteNonQuery();
        }
        /// <summary>
        /// For updating values in an existing table.
        /// Updates only value in a row according to a string attribute.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="tName"></param>
        /// <param name="tIdName"></param>>
        /// <param name="tId"></param>
        /// <param name="tValues"></param>
        public static void UpdateTable(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName, string tIdName, string tId, string[] tValues)
        {
            string sql = "Update " + tName + " set ";
            for (int i = 0; i < tValues.Length; i++)
            {
                sql += i == 0 ? tValues[i] : ", " + tValues[i];
            }
            sql += " where " + tIdName + "=" + tId;
            dbComm = new SQLiteCommand(sql, dbConn);
            dbComm.ExecuteNonQuery();
        }
        /// <summary>
        /// For emptying an existing table.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="tName"></param>
        public static void EmptyTable(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName)
        {
            string sql = "Delete from " + tName;
            dbComm = new SQLiteCommand(sql, dbConn);
            dbComm.ExecuteNonQuery();
        }
        /// <summary>
        /// Empty a row with a string value in an certain attribute.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="tName"></param>
        /// <param name="tIdName"></param>
        /// <param name="tId"></param>
        public static void EmptyRow(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName, string tIdName, string tId)
        {
            string sql = "Delete from " + tName + " where " + tIdName + "=" + tId;
            dbComm = new SQLiteCommand(sql, dbConn);
            dbComm.ExecuteNonQuery();
        }
        /// <summary>
        /// Empty a row with an int value in a certain attribute.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="tName"></param>
        /// <param name="tIdName"></param>
        /// <param name="tId"></param>
        public static void EmptyRow(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName, string tIdName, int tId)
        {
            string sql = "Delete from " + tName + " where " + tIdName + "=" + tId;
            dbComm = new SQLiteCommand(sql, dbConn);
            dbComm.ExecuteNonQuery();
        }
        /// <summary>
        /// Alter the name of a table to another.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="currentName"></param>
        /// <param name="newName"></param>
        public static void AlterTableName(SQLiteConnection dbConn, SQLiteCommand dbComm, string currentName, string newName)
        {
            string sql = "Alter table" + currentName + " rename to " + newName;
            dbComm = new SQLiteCommand(sql, dbConn);
            dbComm.ExecuteNonQuery();
        }
        /// <summary>
        /// Adding an attribute to an existing table.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="tName"></param>
        /// <param name="tAttribute"></param>
        public static void AddTableAttribute(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName, string tAttribute)
        {
            string sql = "Alter table " + tName + " add " + tAttribute;
            dbComm = new SQLiteCommand(sql, dbConn);
            dbComm.ExecuteNonQuery();
        }
        /// <summary>
        /// Select all rows from a table.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="tName"></param>
        /// <returns></returns>
        public static SQLiteDataReader TableSelectAll(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName)
        {
            string sql = "select * from " + tName;
            dbComm = new SQLiteCommand(sql, dbConn);
            return dbComm.ExecuteReader();
        }
        /// <summary>
        /// Select specific row from a table, with a string attribute.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="tName"></param>
        /// <param name="tIdName"></param>
        /// <param name="tId"></param>
        /// <returns></returns>
        public static SQLiteDataReader TableSelectRow(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName, string tIdName, string tId)
        {
            string sql = "select * from " + tName + " where " + tIdName + "=" + tId;
            dbComm = new SQLiteCommand(sql, dbConn);
            return dbComm.ExecuteReader();
        }
        /// <summary>
        /// Select specific rows from a table, with an int attribute.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="dbComm"></param>
        /// <param name="tName"></param>
        /// <param name="tIdName"></param>
        /// <param name="tId"></param>
        /// <returns></returns>
        public static SQLiteDataReader TableSelectRow(SQLiteConnection dbConn, SQLiteCommand dbComm, string tName, string tIdName, int tId)
        {
            string sql = "select * from " + tName + " where " + tIdName + "=" + tId;
            dbComm = new SQLiteCommand(sql, dbConn);
            return dbComm.ExecuteReader();
        }
    }
}
