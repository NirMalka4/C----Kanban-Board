using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using IntroSE.Kanban.Backend.DataLayer.DTOs;

namespace IntroSE.Kanban.Backend.DataLayer.DAOs
{
    /* 
     * These objects contain method for retrieving, storing and updating DTO's.
     * Each object is responsible for a single DTO type (single DB table).
     * Each object is implemented as a singleton to enforce single communication path with specific table.
     * T is of DTO type.
    */
    internal abstract class DAO <T> where T:DTO
    {
        protected readonly string ConnectionPath;
        protected readonly string TableName;

        internal DAO (string _tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            ConnectionPath = $"Data Source={path}; Version=3;";
            TableName = _tableName;
        }

        protected internal abstract T ReaderToDTO(SQLiteDataReader reader);

        ///<summary> 
        ///Add new row to the DB with the attributes of obj.
        ///</summary>
        protected internal abstract void Insert(T obj);

        ///<summary> 
        ///Delete the record from the DB representing obj.
        ///</summary>
        protected internal abstract void Delete(T obj);

        ///<summary> 
        ///Update attribute of obj compatible record in the DB. 
        ///</summary>
        protected internal abstract void Update(T obj, string attributeName, Object attributeVal);

        ///<summary> 
        /// Return all rows from this instance compatibale table.
        /// for example: DTasks.Select() will return all DTask records at the DB.
        ///</summary>
        internal IList<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                        results.Add(ReaderToDTO(dataReader));
                }
                finally
                {
                    if (dataReader != null)
                        dataReader.Close();
                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }

        ///<summary> 
        /// Remove all component content(includes all class which are part of it)
        ///</summary>
        internal virtual void Clear()
        {
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                bool failed = false;
                string output = String.Empty;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {TableName};";

                    command.Prepare();

                    failed = command.ExecuteNonQuery() < 0;
                }
                catch (Exception e)
                {
                    output = new string(e.Message);
                    failed = true;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    if (failed) throw new SQLiteException($"Delete Table: {TableName} has been failed since: {output}.");
                }
            }
        }
    }
}
