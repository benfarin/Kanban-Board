using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Board
    {
        private string userEmail;
        private Column[] columns;
        private int taskId;
        private bool is_UserLoggedin;

        public Board(string userEmail)
        {
            this.userEmail = userEmail;
            columns = new Column[3];
            Column backLog = new Column("BackLog", 0);
            Column inProgress = new Column("InProgress", 1);
            Column done = new Column("Done", 2);
            columns[0] = backLog;
            columns[1] = inProgress;
            columns[2] = done;
            taskId = 0;
            is_UserLoggedin = false;


        }

        public string GetUserEmail()
        {
            return userEmail;
        }

        public void SetIsULoggedIn(bool a)
        {
            this.is_UserLoggedin = a;
        }
        public void AddNewTask(string title, string description, DateTime dueDate)
        {
            if (is_UserLoggedin)
            {
                taskId++;
                columns[0].AddTask(taskId, title, description, dueDate);
            }
            else
                throw new Exception("User  is not logged in");
        }

        public void LimitTasks(int columnId, int limitNum)
        {
            if (is_UserLoggedin)
                columns[columnId].SetLimitNum(limitNum);
            else
                throw new Exception("User  is not logged in");
        }

        public void AdvanceTask(int currentColId, int taskId)
        {
            if (is_UserLoggedin)
            {
                if (currentColId == columns.Length - 1) // if you're in the last column
                {
                    throw new Exception("You can't advance tasks from the last column");
                }
                columns[currentColId + 1].AddTasksToDict(taskId, columns[currentColId].GetTaskById(taskId)); // add task to the next column
                columns[currentColId].DeleteTask(taskId); // delete task from current column
            }
            else
                throw new Exception("User is not logged in");
        }

        public void UpdateTaskDueDate(int colId, int taskId, DateTime dueDate)
        {
            if (is_UserLoggedin)
                columns[colId].GetTaskById(taskId).SetDueDate(dueDate);
            else
                throw new Exception("User is not logged in");
        }
        public void UpdateTaskTitle(int colId, int taskId, string title)
        {
            if (is_UserLoggedin)
                columns[colId].GetTaskById(taskId).SetTitle(title);
            else
                throw new Exception("User is not logged in");
        }

        public void UpdateTaskDescription(int colId, int taskId, string description)
        {
            if (is_UserLoggedin)
                columns[colId].GetTaskById(taskId).SetDescription(description);
            else
                throw new Exception("User is not logged in");
        }

        public Column GetColumnById(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal > columns.Length)
            {
                throw new Exception("Invalid column ordinal");
            }
            return columns[columnOrdinal];
        }

        public void GetColumnByName(string colName)
        {
            int colId = -1;
            for (int i = 0; i < columns.Length - 1; i++)
            {
                colId = columns[i].GetColumnIdByName(colName);
                if (colId != -1)
                    this.GetColumnById(colId);
            }
            if (colId == -1)
            {
                throw new Exception("there's no column named " + colName);
            }
        }
    }
}
