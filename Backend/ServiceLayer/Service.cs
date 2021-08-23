using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using log4net;
using log4net.Config;
using System.IO;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Service
    {

        private BoardService BService;
        private UserService UService;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Service()
        {
            BService = new BoardService();
            UService = new UserService();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
        }

        ///<summary>
        ///This method loads the data from the persistance.
        ///You should call this function when the program starts. 
        ///</summary>
        public Response LoadData()
        {
            try
            {
                BService.LoadData();
                UService.LoadData();
                log.Debug($"Data has been loaded successfully.");
                return new Response();
            }
            catch(Exception e)
            {
                log.Debug($"Data loading has been failed since : {e.Message}");
                return new Response(e.Message);
            }
        }

        ///<summary>
        ///Removes all persistent data.
        ///</summary>
        public Response DeleteData()
        {
            try
            {
                BService.DeleteData();
                UService.DeleteData();
                log.Debug($"Data has been deleted successfully.");
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug($"Data deletion has been failed since : {e.Message}");
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="userEmail">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>

        public Response<User> Login(string userEmail, string password) { return UService.Login(userEmail, password); }

        /// <summary>        
        /// Log out an logged-in user. 
        /// </summary>
        /// <param name="userEmail">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
       
        public Response Logout(string userEmail) { return UService.Logout(userEmail); }

        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                BService.LimitColumn(creatorEmail, boardName, columnOrdinal, limit);
                log.Info($"Board: {boardName} belongs to: {creatorEmail} has been limited to {limit} capacity");
                return new Response();
            }catch(Exception e)
            {
                log.Error($"LimitColumn call has been blocked since: {e.Message}");
                return new Response(e.Message);            
            }
        }

        /// <summary>
        /// Get the limit of a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The limit of the column.</returns>
        public Response<int> GetColumnLimit(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                Column col = BService.GetFullColumn(creatorEmail, boardName, columnOrdinal);
                log.Info($"GetColumnLimit call has been succeed. "); /* Documante for debuging purposes.  */
                return Response<int>.FromValue(col.Limit);
            }
            catch(Exception e)
            {
                log.Error($"GetColumnLimit call has been blocked since: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }

        /// <summary>
        /// Get the name of a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The name of the column.</returns>
        public Response<string> GetColumnName(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                Column col = BService.GetFullColumn(creatorEmail, boardName, columnOrdinal);
                log.Info($"GetColumnName call has been succeeded. "); /* Documante for debuging purposes.  */
                return Response<string>.FromValue(col.Name);
            }
            catch(Exception e)
            {
                log.Error($"GetColumnName call has been blocked since: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
		/// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(string userEmail,string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                Task task = BService.AddTask(userEmail, creatorEmail, boardName, title, description, dueDate);
                log.Info($"Task: {title} created by: {userEmail} located at:{boardName} has been successfully added");
                return Response<Task>.FromValue(task);
            }
            catch (Exception e)
            {
                log.Error($"AddTask call has been blocked since: {e.Message}");
                return Response<Task>.FromError(e.Message);
            }
        }
        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                BService.UpdateTaskDueDate(userEmail, creatorEmail, boardName, columnOrdinal, taskId, dueDate);
                log.Info($"Due date for Task: {taskId} has been successfully changed to: {dueDate}"); /* Documante for debuging purposes.  */
                return new Response();
            }
            catch(Exception e)
            {
                log.Error($"Update Task: {taskId} due date property has been blocked since: {e.Message}");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                BService.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, taskId, title);
                log.Info($"Title for Task: {taskId} has been successfully changed to: {title}"); /* Documante for debuging purposes.  */
                return new Response();
            }
            catch (Exception e)
            {
                log.Error($"Update Task: {taskId} due date property has been blocked since: {e.Message}");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                BService.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, taskId, description);
                log.Info($"Description for Task: {taskId} has been successfully changed to: {description}"); /* Documante for debuging purposes.  */
                return new Response();
            }
            catch (Exception e)
            {
                log.Error($"Update Task: {taskId} description property has been blocked since: {e.Message}");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                BService.AdvanceTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId);
                log.Info($"Task:{taskId} created by: {userEmail} located at:{boardName} " +
                    $"has been successfully advanced to column ordinal:{columnOrdinal + 1}\n"); 
                return new Response();
            }
            catch(Exception e)
            {
                log.Error($"Task:{taskId} advancing has been blocked since: {e.Message}");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> GetColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                Column col = BService.GetFullColumn(creatorEmail, boardName, columnOrdinal);
                log.Info($"GetColumn call has been succeeded. "); /* Documante for debuging purposes.  */
                return Response<IList<Task>>.FromValue(col.Tasks);
            }
            catch (Exception e)
            {
                log.Error($"GetColumn call has been blocked since: {e.Message}");
                return Response<IList<Task>>.FromError(e.Message);
            }
        }


        /// <summary>
        /// Creates a new board for the logged-in user.
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddBoard(string userEmail, string boardName)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                BService.AddBoard(userEmail, boardName);
                log.Debug($"Board: {boardName} created by:{userEmail} has been successfully added");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error($"Add board: {boardName} via {userEmail} has been blocked since: {e.Message}");
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Adds a board created by another user to the logged-in user. 
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                BService.JoinBoard(userEmail, creatorEmail, boardName);
                log.Info($"User: {userEmail} joined board: {boardName} created by: {creatorEmail}");
                return new Response();
            }
            catch(Exception e)
            {
                log.Error($"User: {userEmail} could not joine board: {boardName} created by: {creatorEmail} as: {e.Message}");
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Removes a board.
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveBoard(string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                UService.ValidateUserLoggin(creatorEmail);
                BService.Remove(userEmail, creatorEmail, boardName);
                log.Debug($"Board:{boardName} created by: {creatorEmail} has been successfully removed");
                return new Response();
            }
            catch(Exception e)
            {
                log.Error($"Removing board:{boardName} created by: {creatorEmail} has been blocked since: {e.Message}");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Returns all the in-progress tasks of the logged-in user is assigned to.
        /// </summary>
        /// <param name="userEmail">Email of the logged in user</param>
        /// <returns>A response object with a value set to the list of tasks, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> InProgressTasks(string userEmail)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                Column inProgressTasks = BService.GetInProgressTask(userEmail);
                log.Info($"InProgressTasks via user: {userEmail} call has been succeeded. "); /* Documante for debuging purposes.  */
                return Response<IList<Task>>.FromValue(inProgressTasks.Tasks);
            }
            catch (Exception e)
            {
                log.Error($"InProgressTasks call via user: {userEmail} has been failed because: {e.Message}. "); 
                return Response<IList<Task>>.FromError(e.Message);
            }
        }

        /// <summary>
        /// Returns all the in-progress tasks as a Column of the logged-in user is assigned to.
        /// </summary>
        /// <param name="userEmail">Email of the logged in user</param>
        /// <returns>A response object with a value set to the column of tasks, The response should contain a error message in case of an error</returns>
        public Response<Column> InProgressTasksColumn(string userEmail)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                Column inProgressTasks = BService.GetInProgressTask(userEmail);
                log.Info($"InProgressTasks via user: {userEmail} call has been succeeded. "); /* Documante for debuging purposes.  */
                return Response<Column>.FromValue(inProgressTasks);
            }
            catch (Exception e)
            {
                log.Error($"InProgressTasks call via user: {userEmail} has been failed because: {e.Message}. ");
                return Response<Column>.FromError(e.Message);
            }
        }


        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userEmail">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string userEmail, string password)
        {
            return UService.Register(userEmail, password);
        }


        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param> /* CHECK emailAssignee is regisered  */
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                UService.ValidateUserRegistered(emailAssignee);
                BService.AssignTask(creatorEmail, boardName, columnOrdinal, taskId, emailAssignee);
                log.Info($"Task: {taskId} belong to board: {boardName} craeted by: {creatorEmail} has been assigned to: {emailAssignee}" +
                    $"instead of: {userEmail}");
                return new Response();
            }
            catch(Exception e)
            {
                log.Error($"Task: {taskId} reassign attemp to user: {emailAssignee} has been failed as : {e.Message}");
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Returns the list of board of a user. The user must be logged-in. The function returns all the board names the user created or joined.
        /// </summary>
        /// <param name="userEmail">The email of the user. Must be logged-in.</param>
        /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        public Response<IList<String>> GetBoardNames(string userEmail)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                IList<string> output = BService.GetBoardNames(userEmail);
                log.Info($"GetBoardNames via user: {userEmail} call has been succeeded. "); /* Documante for debuging purposes.  */
                return Response<IList<String>>.FromValue(output);
            }
            catch(Exception e)
            {
                log.Error($"GetBoardNames call via user: {userEmail} has been failed because: {e.Message}.");
                return Response<IList<String>>.FromError(e.Message);
            }
        }

        // ************ New code starts here ***************





        /// <summary>
        /// Adds a new column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The location of the new column. 
        /// Location for old columns with index>=columnOrdinal is increased by 1 (moved right). The first column is identified by 0, the location increases by 1 for each column.</param>
        /// <param name="columnName">The name for the new columns</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                BService.AddColumn(creatorEmail, boardName, columnOrdinal, columnName);
                log.Info($"Column: {columnName} has been added to board: {boardName} of user: {creatorEmail}.");
                return new Response();
            }
            catch(Exception e)
            {
                log.Error($"AddColumn call via user: {userEmail} has been failed because: {e.Message}.");
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// Removes a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                BService.RemoveColumn(creatorEmail, boardName, columnOrdinal);
                log.Info($"Column: {columnOrdinal} has been removed from board: {boardName} of user: {creatorEmail}.");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error($"RemoveColumn call via user: {userEmail} has been failed because: {e.Message}.");
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// Renames a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <param name="newColumnName">The new column name</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                BService.RenameColumn(creatorEmail, boardName, columnOrdinal, newColumnName);
                log.Info($"Column: {columnOrdinal} of board: {boardName} created by user: {creatorEmail} was renamed to: {newColumnName}.");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error($"RenameColumn call via user: {userEmail} has been failed because: {e.Message}.");
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Moves a column shiftSize times to the right. If shiftSize is negative, the column moves to the left
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. 
        /// The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <param name="shiftSize">The number of times to move the column, relativly to its current location. 
        /// Negative values are allowed</param>  
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                BService.MoveColumn(creatorEmail, boardName, columnOrdinal, shiftSize);
                log.Info($"Column: {columnOrdinal} of board: {boardName} created by user: {creatorEmail} was shifted {shiftSize} times.");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error($"MoveColumn call via user: {userEmail} has been failed because: {e.Message}.");
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Returns all the in-progress tasks of the logged-in user, mapped according to their board.
        /// </summary>
        /// <param name="userEmail">Email of the logged in user</param>
        /// <returns>A response object with a value set to the list of tasks, The response should contain a error message in case of an error</returns>
        public Response<Dictionary<Tuple<string, string>,Task>> GetMyInProgressTasks(string userEmail)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                var inProgressTasks = BService.InProgressTask(userEmail);
                log.Info($"InProgressTasks via user: {userEmail} call has been succeeded. "); /* Documante for debuging purposes.  */
                return Response<Dictionary<Tuple<string, string>, Task>>.FromValue(inProgressTasks);
            }
            catch (Exception e)
            {
                log.Error($"InProgressTasks call via user: {userEmail} has been failed because: {e.Message}. ");
                return Response<Dictionary<Tuple<string, string>, Task>>.FromError(e.Message);
            }
        }
        public Response<Column> GetFullColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                Column col = BService.GetFullColumn(creatorEmail, boardName, columnOrdinal);
                log.Info($"GetColumn call has been succeeded. "); /* Documante for debuging purposes.  */
                return Response<Column>.FromValue(col);
            }
            catch (Exception e)
            {
                log.Error($"GetColumn call has been blocked since: {e.Message}");
                return Response<Column>.FromError(e.Message);
            }
        }

        public Response<IList<Board>> GetMyBoards(string userEmail)
        {
            try
            {
                UService.ValidateUserLoggin(userEmail);
                IList<Board> output = BService.GetMyBoards(userEmail);
                log.Info($"GetMyBoards via user: {userEmail} call has been succeeded. "); /* Documante for debuging purposes.  */
                return Response<IList<Board>>.FromValue(output);
            }
            catch (Exception e)
            {
                log.Error($"GetMyBoards call via user: {userEmail} has been failed because: {e.Message}.");
                return Response<IList<Board>>.FromError(e.Message);
            }
        }

    }
}
