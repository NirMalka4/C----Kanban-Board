using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataLayer.DTOs;
using System.Data.SQLite;
using System.Data;

namespace IntroSE.Kanban.Backend.DataLayer.DAOs
{
    internal sealed class DTasks : DAO<DTask>
    {
        private const string FirstColumn = "ID"; /* PK  */
        private const string SecondColumn = "CreationTime";
        private const string ThirdColumn = "DueDate";
        private const string ForthColumn = "Title";
        private const string FifthColumn = "Description";
        private const string SixthColumn = "Assignee";
        private const string SeventhColumn = "CreatorEmail"; /* PK, FK -> Users */
        private const string EighthColumn = "BoardName"; /* PK, FK -> Boards */
        private const string NinethColumn = "Ordinal";

        private static readonly Lazy<DTasks> lazy = new Lazy<DTasks>(() => new DTasks("Tasks"));
        internal static DTasks Instance { get => lazy.Value; }
        private DTasks(string _tableName) : base(_tableName) { }

        protected internal override DTask ReaderToDTO(SQLiteDataReader reader) =>
            new DTask(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                reader.GetValue(4), reader.GetString(5), reader.GetString(6), reader.GetString(7), reader.GetInt32(8));
        protected internal override void Insert(DTask obj)
        {
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                bool failed = false;
                string output = String.Empty;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({FirstColumn}, {SecondColumn}, {ThirdColumn}" +
                        $", {ForthColumn}, {FifthColumn} ,{SixthColumn}, {SeventhColumn}, {EighthColumn}, {NinethColumn} ) " +
                        $"VALUES (@1, @2, @3, @4, @5, @6, @7, @8, @9);";

                    SQLiteParameter idParam = new SQLiteParameter(@"1", obj.ID);
                    SQLiteParameter creationParam = new SQLiteParameter(@"2", obj.CreationTime);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"3", obj.DueDate);
                    SQLiteParameter titleParam = new SQLiteParameter(@"4", obj.Title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"5", obj.Description);
                    SQLiteParameter assigneeParam = new SQLiteParameter(@"6", obj.Assignee);
                    SQLiteParameter creatorParam = new SQLiteParameter(@"7", obj.CreatorEmail);
                    SQLiteParameter boardParam = new SQLiteParameter(@"8", obj.BoardName);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"9", obj.Ordinal);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(creationParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(assigneeParam);
                    command.Parameters.Add(creatorParam);
                    command.Parameters.Add(boardParam);
                    command.Parameters.Add(ordinalParam);
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
                    if (failed) throw new SQLiteException($"Insert Task: {obj.ID} to Column: {obj.Ordinal} " +
                        $"of Board: {obj.BoardName} created by User: {obj.CreatorEmail} has been failed since: {output}.");
                }
            }
        }

        protected internal override void Delete(DTask obj)
        {
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                bool failed = false;
                string output = String.Empty;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"delete from {TableName} where {FirstColumn}=@ID " +
                        $"and {SeventhColumn}=@CreatorEmail and {EighthColumn}=@BoardName and {NinethColumn}=@Ordinal;";

                    SQLiteParameter IdParam = new SQLiteParameter(@"ID", obj.ID);
                    SQLiteParameter emailParam = new SQLiteParameter(@"CreatorEmail", obj.CreatorEmail);
                    SQLiteParameter bNameParam = new SQLiteParameter(@"BoardName", obj.BoardName);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"Ordinal", obj.Ordinal);


                    command.Parameters.Add(IdParam);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(bNameParam);
                    command.Parameters.Add(ordinalParam);

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
                    if (failed) throw new SQLiteException($"Delete Task: {obj.ID} " +
                        $"at Column: {obj.Ordinal} of Board: {obj.BoardName} created by User: {obj.CreatorEmail} " +
                        $"has been failed since: {output}");
                }
            }
        }

        protected internal override void Update(DTask obj, string attributeName, object attributeVal)
        {
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                bool failed = false;
                string output = String.Empty;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"update {TableName} set {@attributeName} = {"'" + @attributeVal + "'"} " +
                        $"where {FirstColumn} = @7 and {SeventhColumn} = @8 and {EighthColumn} = @9;";

                    SQLiteParameter attributeNameParam = new SQLiteParameter("@attributeName", attributeName);
                    SQLiteParameter attributeValParam = new SQLiteParameter("@attributeVal", attributeVal);
                    SQLiteParameter idParam = new SQLiteParameter("@7", obj.ID);
                    SQLiteParameter creatorParam = new SQLiteParameter("@8", obj.CreatorEmail);
                    SQLiteParameter boardParam = new SQLiteParameter("@9", obj.BoardName);

                    command.Parameters.Add(attributeNameParam);
                    command.Parameters.Add(attributeValParam);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(creatorParam);
                    command.Parameters.Add(boardParam);


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
                    if (failed) throw new SQLiteException($"Update Task: {obj.ID} " +
                        $"at Column: {obj.Ordinal} of Board: {obj.BoardName} created by User: {obj.CreatorEmail} " +
                        $"has been failed since: {output}");
                }
            }
        }

        ///<summary> 
        ///Gets all specific column tasks. 
        ///</summary>
        internal IList<DTO> Select(string creator, string bname, int ordinal)
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName} where {SeventhColumn}= {"'" + creator + "'"} " +
                    $"and {EighthColumn} = {"'" + bname + "'"} and {NinethColumn}={ordinal};";
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
        /// Update all tasks of specific column ordinal property
        ///</summary>
        internal void Update(string creator, string bname, int ordinal, int newOrdinal)
        {
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                bool failed = false;
                string output = String.Empty;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"update {TableName} set {NinethColumn} = {@newOrdinal} where {SeventhColumn}= {"'" + creator + "'"}" +
                        $"and {EighthColumn} = {"'" + bname + "'"} and {NinethColumn}={ordinal};";

                    SQLiteParameter attributeValParam = new SQLiteParameter("@newOrdinal", newOrdinal);

                    command.Parameters.Add(attributeValParam);


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
                    if (failed) throw new SQLiteException(output);
                }
            }

        }
    }
}