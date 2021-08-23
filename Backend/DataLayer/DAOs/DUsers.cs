using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataLayer.DTOs;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer.DAOs
{
    internal sealed class DUsers: DAO <DUser>
    {
        private const string FirstColumn = "Email"; /* PK */
        private const string SecondColumn = "Password";
        private static readonly Lazy<DUsers> lazy = new Lazy<DUsers>(() => new DUsers("Users"));
        internal static DUsers Instance { get => lazy.Value; }
        private DUsers(string _tableName) : base(_tableName) { }

        protected internal override DUser ReaderToDTO(SQLiteDataReader reader) => new DUser(reader.GetString(0), reader.GetString(1));
        protected internal override void Insert(DUser obj)
        {
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                bool failed = false;
                string output = String.Empty;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({FirstColumn}, {SecondColumn}) " +
                        $"VALUES (@1,@2);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"1", obj.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"2", obj.Password);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
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
                    if (failed) throw new SQLiteException($"Insert user: {obj.Email} has been failed since:{output}.");
                }
            }
        }

        protected internal override void Delete(DUser obj)
        {
            using (var connection = new SQLiteConnection(ConnectionPath))
            {
                bool failed = false;
                string output = String.Empty;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {TableName} WHERE {FirstColumn}= @email;";

                    SQLiteParameter emailParam = new SQLiteParameter(@"email", obj.Email);

                    command.Parameters.Add(emailParam);
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
                    if (failed) throw new SQLiteException($"Delete user: {obj.Email} has been failed since: {output}");
                }

            }
        }
        protected internal override void Update(DUser obj, string attributeName, object attributeVal) { }
    }
}


