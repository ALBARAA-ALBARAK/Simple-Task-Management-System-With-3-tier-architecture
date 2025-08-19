using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace TaskManagementDataAccessLayer
{
    public class TaskDTO
    {
        public TaskDTO(int Id, string Name, string Description, DateTime DateAndTime, short TakesTimeInMinutes, string Details )
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.DateAndTime = DateAndTime;
            this.TakesTimeInMinutes = TakesTimeInMinutes;
            this.Details = Details;
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateAndTime { get; set; }
        public short TakesTimeInMinutes { get; set; }
        public string Details { get; set; }

    }

    public class TaskData
    {

        static public int AddNewTask(SqlParameter[] param)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param));

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("PRO_AddNewTask", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(param);

                connection.Open();
                object result = command.ExecuteScalar();

                if (result == DBNull.Value)
                    throw new InvalidOperationException("Failed to insert task or retrieve ID.");

                return Convert.ToInt32(result);
            }


        }

        static public bool UpdateTask(SqlParameter[] param)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param));

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("PRO_UpdateTask", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(param);

                connection.Open();
                command.ExecuteNonQuery();

                return true;
            }
        }

        static public short IsThereAnyTaskConflict(int TaskID, DateTime Start, short TakesTime)
        {
            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("PRO_IsThereAnyTaskConflict", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@TaskID", TaskID);
                command.Parameters.AddWithValue("@StartDateAndTime", Start);
                command.Parameters.AddWithValue("@TakesTimeInMin", TakesTime);

                connection.Open();
                object Result = command.ExecuteScalar();

                if (Result == DBNull.Value)
                    Result = 0;

                return Convert.ToInt16(Result);
            }

        }

        static public TaskDTO FindByID(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("PRO_FindTaskByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@TaskID", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TaskDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Task_ID")),
                            reader.GetString(reader.GetOrdinal("TaskName")),
                            reader.GetString(reader.GetOrdinal("TaskDescription")),
                            reader.GetDateTime(reader.GetOrdinal("TaskDateAndTime")),
                            reader.GetInt16(reader.GetOrdinal("TakesTime")),
                            reader.GetString(reader.GetOrdinal("Details"))
                        );
                    }
                    else
                        return null;
                }
            }

        }

        static public List<TaskDTO> GetPageOfTasks(int PageNumber , int PageSize)
        {
            var TaskList = new List<TaskDTO>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("PRO_GetPageOfTasks", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PageNumber", PageNumber);
                    command.Parameters.AddWithValue("@PageSize", PageSize);

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TaskList.Add(new TaskDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Task_ID")),
                                reader.GetString(reader.GetOrdinal("TaskName")),
                                reader.GetString(reader.GetOrdinal("TaskDescription")),
                                reader.GetDateTime(reader.GetOrdinal("TaskDateAndTime")),
                                reader.GetInt16(reader.GetOrdinal("TakesTime")),
                                reader.GetString(reader.GetOrdinal("Details"))
                            ));
                        }
                    }
                }

                return TaskList;
            }
        }

        static public List<TaskDTO> GetPageOfPastTasks(int PageNumber, int PageSize)
        {
            var TaskList = new List<TaskDTO>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("PRO_GetPageOfPastTasks", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PageNumber", PageNumber);
                    command.Parameters.AddWithValue("@PageSize", PageSize);
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TaskList.Add(new TaskDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Task_ID")),
                                reader.GetString(reader.GetOrdinal("TaskName")),
                                reader.GetString(reader.GetOrdinal("TaskDescription")),
                                reader.GetDateTime(reader.GetOrdinal("TaskDateAndTime")),
                                reader.GetInt16(reader.GetOrdinal("TakesTime")),
                                reader.GetString(reader.GetOrdinal("Details"))
                            ));
                        }
                    }
                }

                return TaskList;
            }
        }

        static public List<TaskDTO> GetPageOfActiveTasks(int PageNumber , int PageSize )
        {
            var TaskList = new List<TaskDTO>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("PRO_GetPageOfActiveTasks", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PageNumber", PageNumber);
                    command.Parameters.AddWithValue("@PageSize", PageSize);
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TaskList.Add(new TaskDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Task_ID")),
                                reader.GetString(reader.GetOrdinal("TaskName")),
                                reader.GetString(reader.GetOrdinal("TaskDescription")),
                                reader.GetDateTime(reader.GetOrdinal("TaskDateAndTime")),
                                reader.GetInt16(reader.GetOrdinal("TakesTime")),
                                reader.GetString(reader.GetOrdinal("Details"))
                            ));
                        }
                    }
                }

                return TaskList;
            }
        }

        static public bool DeleteTask(int taskId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("PRO_DeleteTask", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@TaskID", taskId);

                connection.Open();
                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);
            }

        }

    }
    
}
