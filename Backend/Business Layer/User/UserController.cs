using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IntroSE.Kanban.Backend.Business_Layer.User;
using IntroSE.Kanban.Backend.DataLayer.DTOs;

[assembly: InternalsVisibleTo("Tests")]

namespace IntroSE.Kanban.Backend.Business_Layer
{
    internal class UserController
    {
        internal readonly Dictionary<string, Object> Users = new Dictionary<string, Object>();

        /* Flag which indicates whether data from DB was loaded */
        private bool Loaded = false;

        private readonly DTO MyDalObject = new DUser();

        internal UserController() { }

        ///<summary> 
        ///Determine whether user assocaited with param email is already exists. 
        ///</summary>
        public void ValidateUserRegistered(string email)
        {
            if (email == null) throw new ArgumentException("Null is illegal argument for User email property.");
            if (!Users.ContainsKey(email)) throw new ArgumentException($"User :{email} is not registered");
        }

        ///<summary> 
        ///Validate user associated with param email is alredy registered and logged in. 
        ////summary>
        public void ValidateUserLoggin(string email)
        {
            ValidateUserRegistered(email);
            if (!((BUser)Users[email]).Status) throw new ArgumentException($"User: {email} is not logged in");
        }

        ///<summary> 
        ///Loads the data from the persistance layer.  
        ///</summary>
        public void LoadData()
        {
            foreach (DUser user in MyDalObject.Select())
                Users[user.Email] = new BUser(user);
            Loaded = true;
        }

        ///<summary>
        /// if(Loaded) - Delete the data from either persistance layer and runtime program memory allocation.  
        /// else, delete all data associated with us coermponent from the DB.
        /// </summary>
        public void DeleteData()
        {
            if (Loaded)
            {
                foreach (KeyValuePair<string, Object> p in Users)
                {
                    ((BUser)(p).Value).Delete();
                    Users.Remove(p.Key);
                }
            }
            else
                MyDalObject.Clear();
            Users.Clear();


        }

        ///<summary> 
        ///Add new User instance with the supplied params. 
        ///</summary>
        public void Register(string email, string password)
        {
            if (email == null) throw new ArgumentException("Null is illegal argument for User email property.");
            if (Users.ContainsKey(email)) throw new ArgumentException($"Email: {email} is already registered");
            Users.Add(email, new BUser(email, password));
        }

        //the function is only used for testing
        public void RegisterForTesting(string email, IUser u1)
        {
            if (email == null) throw new ArgumentException("Null is illegal argument for User email property.");
            if (Users.ContainsKey(email)) throw new ArgumentException($"Email: {email} is already registered");
            Users.Add(email, u1);

        }


        ///<summary>
        ///Login user associated with param email via param password.
        ///</summary>
        public void Login(string email, string password)
        {
            ValidateUserRegistered(email);
            ((BUser)Users[email]).Login(password);
        }


        //used only for testing, the validateUserRegistered method is removed manually because we don't have a way to remove it using moq
        public void LoginForTesting(IUser u1)
        {
            ((BUser)Users[u1.email]).Login(u1.password);
        }

        ///<summary> 
        ///Logout user associated with param email. 
        ///</summary>
        public void Logout(string email)
        {
            ValidateUserLoggin(email);
            ((BUser)Users[email]).Status = false; ;
        }

    }
}