using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataLayer.DTOs;

namespace IntroSE.Kanban.Backend.DataLayer.DAOs
{
    internal sealed class DBoards : DAO <DBoard>
    {
        private const string FirstColumn = "CreatorEmail"; /* PK, FK -> Users   */
        private const string SecondColumn = "BoardName"; /* PK */
        private const string ThirdColumn = "Capacity"; 
        private const string ForthColumn = "NextKey";
        private const string FifthColumn = "ColumnIndexer";
        private const string SixthColumn = "Right";


        private static readonly Lazy<DBoards> lazy = new Lazy<DBoards>(() => new DBoards("Boards"));
        internal static DBoards Instance { get => lazy.Value; }
        private DBoards(string _tableName) :base(_tableName) { }

        protected internal override DBoard ReaderToDTO(SQLiteDataReader reader) => 
            new DBoard(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), 
                reader.GetInt32(3), reader.GetInt32(4),reader.GetInt32(5));

        protected internal override void Insert(DBoard obj)
        {
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                bool failed = false;
                string output = String.Empty;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({FirstColumn} ,{SecondColumn}, {ThirdColumn}, " +
                        $"{ForthColumn}, {FifthColumn}, {SixthColumn}) VALUES (@1, @2, @3, @4, @5, @6);";

                    SQLiteParameter creatorParam = new SQLiteParameter(@"1", obj.CreatorEmail);
                    SQLiteParameter boardParam = new SQLiteParameter(@"2", obj.BoardName);
                    SQLiteParameter capacityParam = new SQLiteParameter(@"3", obj.Capacity);
                    SQLiteParameter nextKeyParam = new SQLiteParameter(@"4", obj.NextKey);
                    SQLiteParameter columnIndexerParam = new SQLiteParameter(@"5", obj.ColumnIndexer);
                    SQLiteParameter rightParam = new SQLiteParameter(@"6", obj.Right);

                    command.Parameters.Add(creatorParam);
                    command.Parameters.Add(boardParam);
                    command.Parameters.Add(capacityParam);
                    command.Parameters.Add(nextKeyParam);
                    command.Parameters.Add(columnIndexerParam);
                    command.Parameters.Add(rightParam);

                    command.Prepare();

                    failed = command.ExecuteNonQuery() <= 0;
                }
                catch(Exception e)
                {
                    output = new string(e.Message);
                    failed = true;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    if(failed) throw new SQLiteException($"Insert Board:{obj.BoardName} by User: {obj.CreatorEmail}" +
                        $"has been blocked since: {output}."); 
                }
            }
        }

        protected internal override void Delete(DBoard obj)
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
                        $"and {SecondColumn}=@BoardName;";


                    SQLiteParameter emailParam = new SQLiteParameter(@"CreatorEmail", obj.CreatorEmail);
                    SQLiteParameter bNameParam = new SQLiteParameter(@"BoardName", obj.BoardName);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(bNameParam);

                    command.Prepare();

                    failed = command.ExecuteNonQuery() <= 0;
                }
                catch(Exception e)
                {
                    output = new string(e.Message);
                    failed = true;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    if (failed) throw new SQLiteException($"Delete Board: {obj.BoardName} " +
                        $"created by User: {obj.CreatorEmail} has been failed since: {output}");
                }
            }
        }

        protected internal override void Update(DBoard obj, string attributeName, object attributeVal) 
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
                        $"where {FirstColumn} = @1 and {SecondColumn} = @2;";

                    SQLiteParameter attributeNameParam = new SQLiteParameter("@attributeName", attributeName);
                    SQLiteParameter attributeValParam = new SQLiteParameter("@attributeVal", attributeVal);
                    SQLiteParameter emailParam = new SQLiteParameter("@1", obj.CreatorEmail);
                    SQLiteParameter bNameParam = new SQLiteParameter("@2", obj.BoardName);

                    command.Parameters.Add(attributeNameParam);
                    command.Parameters.Add(attributeValParam);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(bNameParam);

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
                    if (failed) throw new SQLiteException($"Update Board : {obj.BoardName} capacity of User: {obj.CreatorEmail} failed " +
                        $"since : {output}.");
                }
            }
        }

        ///<summary>
        /// Delete all data from DB associated with Board component (Tables: Columns, Members, Tasks)
        /// Used by BoardController to clear the DB if it was not loaded before.
        ///.</summary>
        internal override void Clear()
        {
            base.Clear();
            DColumns.Instance.Clear();
            DMembers.Instance.Clear();
            DTasks.Instance.Clear();
        }
    }
}


