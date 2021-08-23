using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntroSE.Kanban.Backend.Business_Layer;
using log4net;
using System.Reflection;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class UserService
    {
        private readonly UserController UC; /* User Controller  */
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        internal UserService() { UC = new UserController(); }

        ///<summary> Loads the data from the persistance. </summary>
        internal void LoadData() { UC.LoadData(); }

        ///<summary> Removes all persistent data.</summary>
        internal void DeleteData() { UC.DeleteData(); }

        /// <summary> Registers a new user. </summary>
        internal Response Register(string email, string password) 
        {
            try
            {
                UC.Register(email, password);
                log.Debug($"User: {email} has been successfully registered");
                return new Response();
            }
            catch (Exception e) 
            {
                log.Error(e.Message);
                return new Response(e.Message); 
            }

        }

        /// <summary> Login registered user. </summary>
        internal Response<User> Login(string email, string password) 
        {
            try
            {
                UC.Login(email, password);
                log.Info($"User: {email} has been successfully logged in\n");
                return Response<User>.FromValue(new User(email));
            }
            catch (Exception e) 
            {
                log.Error($"User: {email} login has been blocked since: {e.Message}");
                return Response<User>.FromError(e.Message); 
            }
        }

        /// <summary> Logout logged user. </summary>
        internal Response Logout(string email) 
        {
            try
            {
                UC.Logout(email);
                log.Info($"User: {email} logged out successfully"); /* Documante for debuging purposes.  */
                return new Response();
            }
            catch (ArgumentException e) 
            {
                log.Error($"User: {email} logout has been blocked since: {e.Message}");
                return new Response(e.Message); 
            }
            
        }

        /// <summary> Determine whether user is registred and logged in. </summary>
        internal void ValidateUserLoggin(string email) 
        {
            UC.ValidateUserLoggin(email); 
        }

        /// <summary> Determine whether user is registred. </summary>
        internal void ValidateUserRegistered(string userEmail) {  UC.ValidateUserRegistered(userEmail); }

    }
}
