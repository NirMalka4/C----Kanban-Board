using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataLayer.DTOs;

namespace IntroSE.Kanban.Backend.DataLayer.DAOs
{
    internal sealed class DMembers : DAO <DMember>
    {
        private const string FirstColumn = "CreatorEmail"; /* PK, FK -> Users */
        private const string SecondColumn = "BoardName"; /* PK, FK -> Boards */
        private const string ThirdColumn = "UserEmail"; /* PK, FK -> Users */

        private static readonly Lazy<DMembers> lazy = new Lazy<DMembers>(() => new DMembers("Members"));
        internal static DMembers Instance { get => lazy.Value; }
        private DMembers(string _tableName) : base(_tableName) { }

        protected internal override DMember ReaderToDTO(SQLiteDataReader reader) => 
            new DMember(reader.GetString(0), reader.GetString(1), reader.GetString(2));
        protected internal override void Insert(DMember obj)
        {
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                bool failed = false;
                string output = String.Empty;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({FirstColumn} ,{SecondColumn}, {ThirdColumn}) " +
                        $"VALUES (@1,@2,@3);";

                    SQLiteParameter creatorParam = new SQLiteParameter(@"1", obj.CreatorEmail);
                    SQLiteParameter bnameParam = new SQLiteParameter(@"2", obj.BoardName);
                    SQLiteParameter userParam = new SQLiteParameter(@"3", obj.UserEmail);

                    command.Parameters.Add(creatorParam);
                    command.Parameters.Add(bnameParam);
                    command.Parameters.Add(userParam);
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
                    if (failed) throw new SQLiteException($"Add Member: {obj.UserEmail} to Board: {obj.BoardName} " +
                        $"created by: {obj.CreatorEmail} has been failed since: {output}.");
                }
            }
        }

        protected internal override void Delete(DMember obj)
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
                        $"and {SecondColumn}=@BoardName and {ThirdColumn}=@Member;";

                    SQLiteParameter emailParam = new SQLiteParameter(@"CreatorEmail", obj.CreatorEmail);
                    SQLiteParameter bNameParam = new SQLiteParameter(@"BoardName", obj.BoardName);
                    SQLiteParameter memberParam = new SQLiteParameter(@"Member", obj.UserEmail);


                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(bNameParam);
                    command.Parameters.Add(memberParam);

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
                    if (failed) throw new SQLiteException($"Delete Member: {obj.UserEmail} " +
                        $"of Board: {obj.BoardName} created by User: {obj.CreatorEmail} " +
                        $"has been failed since: {output}");
                }

            }
        }

        ///<summary> Gets all specific board members. </summary>
        internal IList<DTO> Select(string creator, string bname)
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName} where {FirstColumn}= {"'"+creator+"'"} " +
                    $"and {SecondColumn} = {"'"+bname+"'"};";
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
        protected internal override void Update(DMember obj, string attributeName, object attributeVal) { }
    }
}
