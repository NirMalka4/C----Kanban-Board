using System;
using IntroSE.Kanban.Backend.Business_Layer.Board;

namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
    public struct Task
    {
        public readonly int Id;
        public readonly DateTime CreationTime;
        public readonly string Title;
        public readonly string Description;
        public readonly DateTime DueDate;
        public readonly string emailAssignee;


        internal Task(int id, DateTime creationTime, string title, string description, DateTime DueDate, string emailAssignee)
        {
            Id = id;
            CreationTime = creationTime;
            Title = title;
            Description = description;
            this.DueDate = DueDate;
            this.emailAssignee = emailAssignee;

        }
        internal Task(BTask src)
        {
            Id = src.ID;
            CreationTime = src.CreationTime;
            Title = src.Title;
            Description = src.Description is DBNull ? "" : (string)src.Description;
            DueDate = src.DueDate;
            emailAssignee = src.Assignee;

        }

    }
}
