using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataLayer.DTOs;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer.DAOs
{
    internal sealed class DColumns : DAO <DColumn>
    {
        private const string FirstColumn = "CreatorEmail"; /* PK, FK -> Users */
        private const string SecondColumn = "BoardName";/* PK, FK -> Boards */
        private const string ThirdColumn = "Ordinal"; 
        private const string ForthColumn = "Name";
        private const string FifthColumn = "\"Limit\"";
        private const string SixthColumn = "CurrentCapacity";
        private const string SeventhColumn = "Position"; /* PK */

        private static readonly Lazy<DColumns> lazy = new Lazy<DColumns>(() => new DColumns("Columns"));
        internal static DColumns Instance { get => lazy.Value; }
        private DColumns(string _tableName) : base(_tableName) { }

        protected internal override DColumn ReaderToDTO(SQLiteDataReader reader) =>
            new DColumn(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), 
                reader.GetInt32(4), (uint)reader.GetInt32(5), reader.GetInt32(6));
        protected internal override void Insert(DColumn obj)
        {
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                bool failed = false;
                string output = String.Empty;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({FirstColumn}, {SecondColumn}, {ThirdColumn}, " +
                        $"{ForthColumn}, {FifthColumn} ,{SixthColumn}, {SeventhColumn}) " +
                        $"VALUES (@1, @2, @3, @4, @5, @6, @7);";

                    SQLiteParameter creatorParam = new SQLiteParameter(@"1", obj.CreatorEmail);
                    SQLiteParameter boardParam = new SQLiteParameter(@"2", obj.BoardName);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"3", obj.Ordinal);
                    SQLiteParameter nameParam = new SQLiteParameter(@"4", obj.Name);
                    SQLiteParameter limitParam = new SQLiteParameter(@"5", obj.Limit);
                    SQLiteParameter ccParam = new SQLiteParameter(@"6", obj.CurrentCapacity);
                    SQLiteParameter posParam = new SQLiteParameter(@"7", obj.Position);

                    command.Parameters.Add(creatorParam);
                    command.Parameters.Add(boardParam);
                    command.Parameters.Add(ordinalParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(limitParam);
                    command.Parameters.Add(ccParam);
                    command.Parameters.Add(posParam);

                    command.Prepare();

                    failed = command.ExecuteNonQuery() <= 0;
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
                    if (failed) throw new SQLiteException($"Insert Column: {obj.Name} belongs to Board: {obj.BoardName}" +
                        $"has been failed since: {output}.");
                }
            }
        }

        protected internal override void Delete(DColumn obj)
        {
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                bool failed = false;
                string output = String.Empty;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"delete from {TableName} where {FirstColumn}=@CreatorEmail " +
                        $"and {SecondColumn}=@BoardName and {SeventhColumn}=@Position;";


                    SQLiteParameter emailParam = new SQLiteParameter(@"CreatorEmail", obj.CreatorEmail);
                    SQLiteParameter bNameParam = new SQLiteParameter(@"BoardName", obj.BoardName);
                    SQLiteParameter posParam = new SQLiteParameter(@"Position", obj.Position);


                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(bNameParam);
                    command.Parameters.Add(posParam);

                    command.Prepare();

                    failed = command.ExecuteNonQuery() <= 0;
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
                    if (failed) throw new SQLiteException($"Delete Column: {obj.Name} " +
                        $"of Board: {obj.BoardName} created by User: {obj.CreatorEmail} " +
                        $"has been failed since: {output}");
                }
            }
        }

        ///<summary> 
        ///Gets all specific board columns. 
        ///</summary>
        internal IList<DTO> Select(string creator,string bname)
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName} where {FirstColumn}= {"'"+creator+ "'"} " +
                    $"and {SecondColumn} = {"'" + bname+ "'"};";
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

        protected internal override void Update(DColumn obj, string attributeName, object attributeVal) 
        {
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                bool failed = false;
                string output = String.Empty;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"update {TableName} set {@attributeName} = {@attributeVal} " +
                        $"where {FirstColumn} = @1 and {SecondColumn} = @2 and {SeventhColumn} = @3;";

                    SQLiteParameter attributeNameParam = new SQLiteParameter("@attributeName", attributeName);
                    SQLiteParameter attributeValParam = new SQLiteParameter("@attributeVal", attributeVal);
                    SQLiteParameter emailParam = new SQLiteParameter("@1", obj.CreatorEmail);
                    SQLiteParameter bNameParam = new SQLiteParameter("@2", obj.BoardName);
                    SQLiteParameter posParam = new SQLiteParameter("@3", obj.Position);

                    command.Parameters.Add(attributeNameParam);
                    command.Parameters.Add(attributeValParam);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(bNameParam);
                    command.Parameters.Add(posParam);

                    command.Prepare();

                    failed = command.ExecuteNonQuery() <= 0;
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
                    if (failed) throw new SQLiteException($"Update Column: {obj.Name} " +
                        $"of Board: {obj.BoardName} created by User: {obj.CreatorEmail} " +
                        $"has been failed since: {output}");
                }
            }

        }
    }
}
