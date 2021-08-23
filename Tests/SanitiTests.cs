using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Business_Layer;
using IntroSE.Kanban.Backend.Business_Layer.Board;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;
using NUnit.Framework;


namespace Tests
{
    class SanitiTests
    {
        Service s = new Service();
        IntroSE.Kanban.Backend.Business_Layer.Board.Board b;
        UserController uc = new UserController();

        [SetUp]
        public void SetUp()
        {
            b = new IntroSE.Kanban.Backend.Business_Layer.Board.Board("eliben@gmail.com", "check");
            b.AddMember("eli");
            s.Register("aaa@aaa.aaa", "Aaa1");
            s.Register("baa@aaa.aaa", "Baa1");
            s.Register("caa@aaa.aaa", "Caa1");
            s.Register("daa@aaa.aaa", "Daa1");

            s.AddBoard("aaa@aaa.aaa", "A1");

            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "taskDesc", new DateTime(2025, 02, 02));


        }

        [TearDown]
        public void TearDown()
        {
            s.DeleteData();
            uc.DeleteData();
        }


        [Test]
        public void isRun()
        {

        }

        //users:

        [Test]
        [TestCase("aaa@aaa.aaa")]
        [TestCase("aaaaaa.aaa")]
        [TestCase("aaa@aaa")]
        public void user_negative_badEmail(string email)
        {
            Response r = s.Register(email, "Ggg1");
            if (!r.ErrorOccured)
                Assert.Fail($"registered with bad email: {email} \n {r.ErrorMessage}");
        }

        [Test]
        [TestCase("12")]
        [TestCase("AAAA12")]
        [TestCase("AaaaaaaaaAaaaaaaaaa13")]
        public void user_negative_badPassword(string password)
        {
            Response r = s.Register("newMail@mail.com", password);
            if (!r.ErrorOccured)
            {
                Assert.Fail($"registered with bad password: {password} \n {r.ErrorMessage}");
                uc.DeleteData();
            }
        }

        [Test]
        public void user_negative_login_notRegistered()
        {
            Response r = s.Login("sui@notReg.com", "pass");
            if (!r.ErrorOccured)
                Assert.Fail($"logged in with not registered user: {r.ErrorMessage}");
        }

        //Boards

        [Test]
        [TestCase("baa@aaa.aaa", "Baa1", "B1")]
        [TestCase("baa@aaa.aaa", "Baa1", "A1")]
        [TestCase("baa@aaa.aaa", "Baa1", "11")]
        public void board_positive_createBoard(string email, string pass, string bname)
        {
            s.Login(email, pass);

            Response r = s.AddBoard(email, bname);
            if (r.ErrorOccured)
                Assert.Fail($"error while making a valid board, boardname: {bname}, email: {email}");
        }

        [Test]
        public void board_negative_noSuchUser()
        {
            Response r = s.AddBoard("asoin@awfas.awf", "OKOK");
            if (!r.ErrorOccured)
                Assert.Fail($"made board with not regiostered user asoin@awfas.awf");
        }

        [Test]
        public void board_negative_notLoggedInUser()
        {
            Response r = s.AddBoard("aaa@aaa.aaa", "OKOK");
            if (!r.ErrorOccured)
                Assert.Fail($"made board with not regiostered user asoin@awfas.awf");
        }

        [Test]
        public void board_positive_addTask()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            Response r = s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            if (r.ErrorOccured)
                Assert.Fail($"faild to make valid task");
        }

        [Test]
        public void board_positive_updateTask_title()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            Response r = s.UpdateTaskTitle("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, "newTitle");
            if (r.ErrorOccured)
                Assert.Fail($"faild to updateValid task title");
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void board_positive_updateTask_desc(string desc)
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            Response r = s.UpdateTaskDescription("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, desc);
            if (r.ErrorOccured)
                Assert.Fail($"faild to updateValid task desc: {desc}");
        }

        [Test]
        public void board_positive_updateTask_dueDate_forward()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            Response r = s.UpdateTaskDueDate("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, new DateTime(2030, 02, 03));
            if (r.ErrorOccured)
                Assert.Fail($"faild to updateValid task dueDate: 2030,02,03");
        }

        [Test]
        public void board_positive_updateTask_dueDate_backwards()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            Response r = s.UpdateTaskDueDate("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, new DateTime(2023, 11, 03));
            if (r.ErrorOccured)
                Assert.Fail($"faild to updateValid task dueDate: 2023, 11, 03");
        }

        [Test]
        public void board_negative_createTask_notLoggedInUser()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            Response r = s.AddTask("bbbaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            if (!r.ErrorOccured)
                Assert.Fail($"added task bnut user not logged in: bbbaa@aaa.aaa");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void board_negative_createTask_invalidTitle(string title)
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            Response r = s.AddTask("bbbaa@aaa.aaa", "aaa@aaa.aaa", "A1", title, "desc", new DateTime(2025, 02, 02));

            if (!r.ErrorOccured)
                Assert.Fail($"added task with invalid title: {title}");
        }

        [Test]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void board_negative_createTask_invalidDesc(string desc)
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            Response r = s.AddTask("bbbaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", desc, new DateTime(2025, 02, 02));

            if (!r.ErrorOccured)
                Assert.Fail($"added task with invalid desc: {desc}");
        }

        [Test]
        public void board_negative_createTask_invaliddueDate()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            Response r = s.AddTask("bbbaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2020, 02, 02));

            if (!r.ErrorOccured)
                Assert.Fail($"added task with past dueDate: 2020, 02, 02");
        }


        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void board_negative_updateTask_title(string title)
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            Response r = s.UpdateTaskTitle("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, title);
            if (!r.ErrorOccured)
                Assert.Fail($"updated task with innvalid title: {title}");
        }

        [Test]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void board_negative_updateTask_desc(string desc)
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            Response r = s.UpdateTaskDescription("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, desc);
            if (!r.ErrorOccured)
                Assert.Fail($"updated task with innvalid description: {desc}");
        }

        [Test]
        public void board_negative_updateTask_dueDate()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            Response r = s.UpdateTaskDueDate("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, new DateTime(2012, 02, 02));
            if (!r.ErrorOccured)
                Assert.Fail($"updated task with innvalid dueDate: 2012, 02, 02");
        }

        [Test]
        public void Board_negative_updateTask_DONECol_title()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            s.AdvanceTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1);
            s.AdvanceTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 1, 1);


            Response r = s.UpdateTaskTitle("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, "newTitle");
            if (!r.ErrorOccured)
                Assert.Fail($"updated title on finished column");

            r = s.UpdateTaskTitle("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 1, 1, "newTitle");
            if (!r.ErrorOccured)
                Assert.Fail($"updated title on finished column");
        }

        [Test]
        public void Board_negative_updateTask_DONECol_desc()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            s.AdvanceTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1);
            s.AdvanceTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 1, 1);


            Response r = s.UpdateTaskDescription("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, "newdesc");
            if (!r.ErrorOccured)
                Assert.Fail($"updated desc on finished column");

            r = s.UpdateTaskTitle("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 1, 1, "newTitle");
            if (!r.ErrorOccured)
                Assert.Fail($"updated desc on finished column");
        }

        [Test]
        public void Board_negative_updateTask_DONECol_dueDate()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            s.AdvanceTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1);
            s.AdvanceTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 1, 1);


            Response r = s.UpdateTaskDueDate("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, new DateTime(2027, 02, 02));
            if (!r.ErrorOccured)
                Assert.Fail($"updated DueDate on finished column");

            r = s.UpdateTaskTitle("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 1, 1, "newTitle");
            if (!r.ErrorOccured)
                Assert.Fail($"updated DueDate on finished column");
        }

        //members and assignees functionality            "baa@aaa.aaa", "Baa1"

        [Test]
        public void board_positive_addTaskAsAMember()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.Login("baa@aaa.aaa", "Baa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            s.JoinBoard("baa@aaa.aaa", "aaa@aaa.aaa", "A1");

            Response r = s.AddTask("baa@aaa.aaa", "aaa@aaa.aaa", "A1", "task2", "desc2", new DateTime(2030, 05, 06));
            if (r.ErrorOccured)
                Assert.Fail($"faild to add task to a board by a valid board member");
        }

        [Test]
        public void board_positive_updateTaskAsAssignee()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.Login("baa@aaa.aaa", "Baa1");
            s.Login("caa@aaa.aaa", "Caa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            s.JoinBoard("baa@aaa.aaa", "aaa@aaa.aaa", "A1");
            s.JoinBoard("caa@aaa.aaa", "aaa@aaa.aaa", "A1");

            s.AddTask("baa@aaa.aaa", "aaa@aaa.aaa", "A1", "task2", "desc2", new DateTime(2030, 05, 06));
            s.AssignTask("baa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 2, "caa@aaa.aaa");

            Response r = s.UpdateTaskTitle("caa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 2, "newTitle");
            if (r.ErrorOccured)
                Assert.Fail($"faild to assign task to a board member by a valid board member");
        }

        [Test]
        public void board_positive_updateTaskAsAssignee_simple()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.Login("baa@aaa.aaa", "Baa1");
            s.Login("caa@aaa.aaa", "Caa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            s.JoinBoard("caa@aaa.aaa", "aaa@aaa.aaa", "A1");
            s.AssignTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, "caa@aaa.aaa");

            Response r = s.UpdateTaskTitle("caa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, "newTitle");
            if (r.ErrorOccured)
                Assert.Fail($"faild to assign task to a board member by a valid board member");
        }

        [Test]
        public void board_positive_updateTaskAsAssignee_advance()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.Login("baa@aaa.aaa", "Baa1");
            s.Login("caa@aaa.aaa", "Caa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            s.JoinBoard("caa@aaa.aaa", "aaa@aaa.aaa", "A1");
            s.AssignTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, "caa@aaa.aaa");

            Response r = s.AdvanceTask("caa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1);
            if (r.ErrorOccured)
                Assert.Fail($"faild to advance task to valid col by valid member and assignee");

            r = s.AdvanceTask("caa@aaa.aaa", "aaa@aaa.aaa", "A1", 1, 1);
            if (r.ErrorOccured)
                Assert.Fail($"faild to advance task to valid col by valid member and assignee");


        }

        [Test]
        public void board_negative_updateTaskAsAssignee_advance()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.Login("baa@aaa.aaa", "Baa1");
            s.Login("caa@aaa.aaa", "Caa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            s.JoinBoard("caa@aaa.aaa", "aaa@aaa.aaa", "A1");
            s.AssignTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, "caa@aaa.aaa");

            s.AdvanceTask("caa@aaa.aaa", "aaa@aaa.aaa", "A1", 1, 1);
            s.AdvanceTask("caa@aaa.aaa", "aaa@aaa.aaa", "A1", 2, 1);
            Response r = s.AdvanceTask("caa@aaa.aaa", "aaa@aaa.aaa", "A1", 2, 1);
            if (!r.ErrorOccured)
                Assert.Fail($"advanced task 3 times illegal");

        }

        [Test]
        public void board_negative_updateTaskAsAssignee_advancebyNotAssignee()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.Login("baa@aaa.aaa", "Baa1");
            s.Login("caa@aaa.aaa", "Caa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            s.JoinBoard("caa@aaa.aaa", "aaa@aaa.aaa", "A1");
            s.AssignTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, "caa@aaa.aaa");

            Response r = s.AdvanceTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1);
            if (!r.ErrorOccured)
                Assert.Fail($"advanced task by creator but not assignee");

        }

        [Test]
        public void board_negative_updateTaskNotAssignee()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.Login("baa@aaa.aaa", "Baa1");
            s.Login("caa@aaa.aaa", "Caa1");
            s.AddBoard("aaa@aaa.aaa", "A1");

            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));

            s.JoinBoard("baa@aaa.aaa", "aaa@aaa.aaa", "A1");
            s.JoinBoard("caa@aaa.aaa", "aaa@aaa.aaa", "A1");

            s.AddTask("baa@aaa.aaa", "aaa@aaa.aaa", "A1", "task2", "desc2", new DateTime(2030, 05, 06));
            s.AssignTask("baa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 2, "caa@aaa.aaa");

            Response r = s.UpdateTaskTitle("baa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 2, "newTitle");
            if (!r.ErrorOccured)
                Assert.Fail($"succeeded in updating task as not assignee");
        }


        [Test]
        public void board_positive_check_getTasks_acrossdiffBoards()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.Login("baa@aaa.aaa", "Baa1");
            s.Login("caa@aaa.aaa", "Caa1");
            s.Login("daa@aaa.aaa", "Daa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));//1
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task2", "desc2", new DateTime(2025, 03, 02));//2

            s.JoinBoard("baa@aaa.aaa", "aaa@aaa.aaa", "A1");
            s.AddTask("baa@aaa.aaa", "aaa@aaa.aaa", "A1", "task4", "desc4", new DateTime(2030, 05, 06));//3

            s.AddBoard("daa@aaa.aaa", "D1");
            s.AddTask("daa@aaa.aaa", "daa@aaa.aaa", "D1", "task3", "desc3", new DateTime(2025, 02, 02));//1


            s.JoinBoard("caa@aaa.aaa", "aaa@aaa.aaa", "A1");
            s.JoinBoard("caa@aaa.aaa", "daa@aaa.aaa", "D1");

            s.AssignTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1, "caa@aaa.aaa");
            s.AssignTask("baa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 3, "caa@aaa.aaa");

            s.AssignTask("daa@aaa.aaa", "daa@aaa.aaa", "D1", 0, 1, "caa@aaa.aaa");

            s.AdvanceTask("caa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1);
            s.AdvanceTask("caa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 3);
            s.AdvanceTask("caa@aaa.aaa", "daa@aaa.aaa", "D1", 0, 1);


            Response<IList<IntroSE.Kanban.Backend.ServiceLayer.Objects.Task>> l = s.InProgressTasks("caa@aaa.aaa");


            if (l.Value.Count != 3)
                Assert.Fail("not good");
        }

        [Test]
        public void getColName_Positive()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.Login("baa@aaa.aaa", "Baa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));//1
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task2", "desc2", new DateTime(2025, 03, 02));//2

            Response r = s.GetColumnName("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0);
            Response r1 = s.GetColumnName("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 1);
            Response r2 = s.GetColumnName("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 2);

        }

        [Test]
        public void Column_Limit()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.Login("baa@aaa.aaa", "Baa1");
            s.AddBoard("aaa@aaa.aaa", "A1");
            s.LimitColumn("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1);

            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));//1
            Response r = s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task2", "desc2", new DateTime(2025, 03, 02));//2

            if (!r.ErrorOccured)
                Assert.Fail();

        }

        [Test]
        public void Column_Limit2()
        {
            s.Login("aaa@aaa.aaa", "Aaa1");
            s.Login("baa@aaa.aaa", "Baa1");
            s.AddBoard("aaa@aaa.aaa", "A1");

            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task1", "desc", new DateTime(2025, 02, 02));//1

            s.LimitColumn("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 5);
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task2", "desc2", new DateTime(2025, 02, 02));//1
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task3", "desc3", new DateTime(2025, 02, 02));//1
            s.AddTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", "task4", "desc4", new DateTime(2025, 02, 02));//1




            s.AdvanceTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 1);
            s.AdvanceTask("aaa@aaa.aaa", "aaa@aaa.aaa", "A1", 0, 2);
        }

        [Test]
        public void minNumberOfCollums()
        {
            try
            {
                b.RemoveColumn(0);
                b.RemoveColumn(1);
                Assert.Fail("cannot have less then 2 collums");
            }
            catch (Exception)
            {

            }

        }



        [Test]
        public void placeOfAddedTask()
        {
            try
            {
                b.AddTask("eli", "BackEnd", "adding the tests", new DateTime(2025, 02, 02));
                IList<int> i = b.Columns[0]._Mapper["eli"];
                foreach (int p in i)
                {
                    if (b.Columns[0]._Tasks[p].Title == "BackEnd")
                        Assert.Pass();

                }
                Assert.Fail("the task wasn't transfered correctly");
            }
            catch (Exception)
            {

            }

        }
        [Test]
        public void unMovableCollumns()
        {
            try
            {
                b.Columns[0].AddTask("eli", "BackEnd", "adding the tests", new DateTime(2025, 02, 02), 1, "eli", "thth");
                b.MoveColumn(0, 1);
                Assert.Fail("cannot move a collumn with a task");
            }
            catch (Exception e)
            {

            }

        }

        [Test]
        public void MovableCollumns()
        {
            try
            {
                b.MoveColumn(0, 1);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);

            }

        }

        [Test]
        public void tasksAfterRemoveCollumn()
        {
            b.Columns[1].AddTask("eli", "BackEnd", "adding the tests", new DateTime(2025, 02, 02), 1, "eli", "thth");
            b.RemoveColumn(1);
            bool ans = false;
            IList<int> i = b.Columns[0]._Mapper["eli"];
            foreach (int p in i)
            {
                if (b.Columns[0]._Tasks[p].Title == "BackEnd")
                    ans = true;

            }
            if (!ans)
            {
                Assert.Fail("the task wasn't transfered correctly");
            }


        }

        [Test]
        public void tasksAfterRemoveCollumn_tooManyTasks()
        {
            try
            {
                b.LimitColumn(1, 6);
                b.LimitColumn(0, 2);
                b.Columns[1].AddTask("eli", "BackEnd", "adding the tests", new DateTime(2025, 02, 02), 1, "eli", "thth");
                b.Columns[1].AddTask("eli", "BackEnd", "adding the tests", new DateTime(2025, 02, 02), 2, "eli", "thth");
                b.Columns[1].AddTask("eli", "BackEnd", "adding the tests", new DateTime(2025, 02, 02), 3, "eli", "thth");
                b.Columns[1].AddTask("eli", "BackEnd", "adding the tests", new DateTime(2025, 02, 02), 4, "eli", "thth");
                b.Columns[1].AddTask("eli", "BackEnd", "adding the tests", new DateTime(2025, 02, 02), 5, "eli", "thth");
                b.RemoveColumn(1);
                Assert.Fail("the task wasn't transfered correctly");
            }
            catch (Exception)
            {

            }

        }


    }


}