﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class TaskDalController : DalController
    {

        private const string MessageTableName = "Task";
        public TaskDalController() : base(MessageTableName)
        {

        }

        public List<DTOs.TaskDTO> SelectAllTasks()
        {
            List<DTOs.TaskDTO> result = Select().Cast<DTOs.TaskDTO>().ToList();

            return result;
        }



        public bool Insert(DTOs.TaskDTO TASK)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {MessageTableName} ({DTOs.DTO.IDColumnName} ,{DTOs.TaskDTO.MessageTitleColumnName},{DTOs.TaskDTO.MessagedescriptionColumnName},{DTOs.TaskDTO.MessageDueDateColumnName},{DTOs.TaskDTO.MessageDueDateColumnName}{DTOs.TaskDTO.MessageCreationTimeColumnName}{DTOs.TaskDTO.MessagecolumnColumnName}) " +
    $"VALUES (@idVal,@Titelval,@descriptionVal,@DueDateVal,@CreationVal,@ColumnIdVal);";
                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", TASK.Id);
                    SQLiteParameter titleParam = new SQLiteParameter(@"Titelval", TASK.Description);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", TASK.DueDate);
                    SQLiteParameter duedateParam = new SQLiteParameter(@"DueDateVal", TASK.DueDate);
                    SQLiteParameter creationtimeParam = new SQLiteParameter(@"CreationVal", TASK.CreationTime);
                    SQLiteParameter columnideParam = new SQLiteParameter(@"ColumnIdVal", TASK.ColumnId);





                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(duedateParam);
                    command.Parameters.Add(creationtimeParam);
                    command.Parameters.Add(columnideParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    //log error
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        protected override DTOs.TaskDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            DTOs.TaskDTO result = new DTOs.TaskDTO((long)reader.GetValue(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), (long)reader.GetValue(5));
            return result;

        }
    }
}