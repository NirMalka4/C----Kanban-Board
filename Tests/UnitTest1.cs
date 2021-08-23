using IntroSE.Kanban.Backend.Business_Layer;
using IntroSE.Kanban.Backend.ServiceLayer;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTest1
{
    //tests here are without moq because the functionality is tested in BUser
    public class Tests
    {

        UserController us = new UserController();
        [SetUp]
        public void Setup()
        {
            us = new UserController();
        }


        [TearDown]
        public void TearDown()
        {
            us.DeleteData();
        }

        [Test]
        public void Register_goodEmailAndGoodPassword_success()
        {
            //Arrange
            string email = "elibenshimol7@gmail.com";
            string password = "Ebs123456";
            try
            {
                //Act
                us.Register(email, password);
            }
            catch (Exception e)
            {

                Assert.Fail(e.Message);

            }
        }

        [Test]
        public void Register_goodEmailAndBadPassword_fail()
        {

            //Arrange
            string email = "elibenshimol6@gmail.com";
            string password = "123456";
            try
            {
                //Act
                us.Register(email, password);
                //Assert
                Assert.Fail("Illegal password");
            }

            catch (Exception)
            {

            }
        }


        [Test]
        public void Register_badEmailAndGoodPassword_fail()
        {

            //Arrange
            string email = "elibenshimol6";
            string password = "Ebs123456";
            try
            {
                //Act
                us.Register(email, password);
                //Assert
                Assert.Fail("Illegal email");
            }

            catch (Exception)
            {

            }
        }



        [Test]
        public void Register_badEmailAndBadPassword_fail()
        {

            //Arrange
            string email = "elibenshimol6";
            string password = "123456";
            try
            {
                //Act
                us.Register(email, password);
                //Assert
                Assert.Fail("Illegal password and illegal email");
            }

            catch (Exception)
            {

            }
        }



        [Test]
        public void Register_registeredEmail_fail()
        {

            //Arrange
            string email = "elibenshimol6@gmail.com";
            string password = "Ebs123456";
            try
            {
                //Act
                us.Register(email, password);
                us.Register(email, password);

                //Assert
                Assert.Fail("Email already registered");
            }

            catch (Exception)
            {

            }
        }


        [Test]
        public void Register_nullEmail_fail()
        {

            //Arrange
            string email = null;
            string password = "Ebs123456";
            try
            {
                //Act
                us.Register(email, password);

                //Assert
                Assert.Fail("Email already registered");
            }

            catch (Exception)
            {

            }
        }






        [Test]
        public void Login_registeredUser_success()
        {

            //Arrange
            string email = "elibenshimol6@gmail.com";
            string password = "Ebs123456";
            us.Users.Add(email, new BUser(email, password));
            //Act
            us.Login(email, password);
            //Assert
            Assert.AreEqual(true, ((BUser)us.Users[email]).Status, "Doesn't login a user who is already registered");


        }


        [Test]
        public void Login_unRegisteredUser_fail()
        {

            //Arrange
            string email = "elibenshimol6@gmail.com";
            string password = "Ebs123456";
            try
            {
                //Act
                us.Login(email, password);
                //Assert
                Assert.Fail("Doesn't need to allow a user which is not logged in to connect");

            }
            catch (Exception)
            {

            }


        }



        [Test]
        public void ValidateUserRegistered_UnRegisteredUser_Fail()
        {

            //Arrange
            string email = "elibenshimol6@gmail.com";
            //Act
            try
            {

                us.ValidateUserRegistered(email);

            }
            catch (Exception e)
            {

            }


        }

        [Test]
        public void ValidateUserRegistered_RegisteredUser_Successs()
        {

            //Arrange
            string email = "elibenshimol6@gmail.com";
            string password = "Ebs123456";
            try
            {
                //Act
                us.Users.Add(email, new BUser(email, password));
                us.ValidateUserRegistered(email);

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }



        }

        [Test]
        public void ValidateUserRegistered_nullEmail_Fail()
        {

            //Arrange
            string email = null;
            try
            {
                //Act
                us.ValidateUserRegistered(email);
                Assert.Fail("the email is null");

            }
            catch (Exception)
            {

            }



        }
    }
}