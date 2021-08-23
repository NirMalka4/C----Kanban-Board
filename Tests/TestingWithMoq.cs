using IntroSE.Kanban.Backend.Business_Layer;
using IntroSE.Kanban.Backend.Business_Layer.User;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class TestingWithMoq
    {
        UserController uc;
        Mock<IUser> user1;
        String email;
        String password;



        [SetUp]
        public void SetUp()
        {
            uc = new UserController();
            user1 = new Mock<IUser>();
            email = "elibenshimol6@gmail.com";
            password = "Ebs123456";
            user1.SetupGet(m => m.email).Returns(email);
            user1.SetupGet(m => m.password).Returns(password);
        }



        [TearDown]
        public void TearDown()
        {
            uc.DeleteData();
        }



        [Test]
        public void Register_checkingAddToList_Success()
        {
            String email = "elibenshimol6@gmail.com";
            uc.RegisterForTesting(email, user1.Object);
            Assert.AreEqual(user1.Object, uc.Users[email], "user should be recorded");
        }



        [Test]
        public void Register_registeredEmail_fail()
        {

            //Arrange
            try
            {
                //Act
                uc.RegisterForTesting(email, user1.Object);
                uc.RegisterForTesting(email, user1.Object);

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
            try
            {
                //Act
                uc.RegisterForTesting(email, user1.Object);
                //Assert
                Assert.Fail("Email already registered");
            }

            catch (Exception)
            {

            }
        }



        [Test]
        public void Login_checkingStatusChanged_Success()
        {
            String email = "elibenshimol6@gmail.com";
            String password = "Ebs123456";
            BUser u = new BUser(email, password);
            user1.SetupGet(m => m.email).Returns(email);
            user1.SetupGet(m => m.password).Returns(password);
            uc.Users[email] = u;
            uc.LoginForTesting(user1.Object);
            Assert.AreEqual(true, u.Status, "user should be recorded");
        }
    }

}
