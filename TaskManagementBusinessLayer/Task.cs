using System.Data;
using Microsoft.Data.SqlClient;
using TaskManagementDataAccessLayer;

namespace TaskManagementBusinessLayer
{
    public class Task
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enConflictHandelir
        {
            None = 0,
            StartOverlaps = 1,        
            EndOverlaps = 2,
            ContainsExisting = 3,    
            StartAndEndOverlap = 4,  
            OnThePast = 5
        }
        public enConflictHandelir ConflictType = enConflictHandelir.None;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateAndTime { get; set; }
        public short TakesTimeInMinutes { get; set; }
        public string? Details { get; set; }

        public TaskDTO GetTaskDTO 
        {
            get { return new TaskDTO(this.Id, this.Name, this.Description, this.DateAndTime, this.TakesTimeInMinutes, this.Details ); }
        }

        public Task(TaskDTO taskDTO, int TaskID = -1, enMode enMode = enMode.AddNew)
        {
            this.Id = TaskID;
            this.Name = taskDTO.Name;
            this.Description = taskDTO.Description;
            this.DateAndTime = taskDTO.DateAndTime;
            this.TakesTimeInMinutes = taskDTO.TakesTimeInMinutes;
            this.Details = taskDTO.Details;

            Mode = enMode;
        }

        private static SqlParameter NewParam(string sp, SqlDbType type, object value) => new SqlParameter(sp, type) { Value = value };

        private SqlParameter[] TaskDBParameters
        {
            get
            {
                return new SqlParameter[]
                {
                    NewParam("@Name", SqlDbType.NVarChar, this.Name),
                    NewParam("@Description", SqlDbType.NVarChar, this.Description),
                    NewParam("@DateAndTime", SqlDbType.DateTime, this.DateAndTime),
                    NewParam("@TakesTime", SqlDbType.SmallInt, this.TakesTimeInMinutes),
                    NewParam("@Details", SqlDbType.NVarChar, this.Details),
                };
            }
        }

        private bool _IsTheTaskOnThePaste()
        {
            DateTime RealDAteAndTime = DateTime.Now;

            return (this.DateAndTime < RealDAteAndTime );
        }

        private bool _TaskConflict()
        {
            if (_IsTheTaskOnThePaste())
            {
                ConflictType = enConflictHandelir.OnThePast;
                return false;
            }

            ConflictType = (enConflictHandelir)TaskData.IsThereAnyTaskConflict(this.Id, this.DateAndTime, this.TakesTimeInMinutes);

            if (ConflictType != enConflictHandelir.None) 
                return false;
            else
                return true;
        }

        private bool _UpdateTask()
        {
            if (!_TaskConflict())
            {
                return false;
            }

            List<SqlParameter> paramList = new List<SqlParameter>(TaskDBParameters);

            paramList.Add(NewParam("@Task_ID", SqlDbType.Int, this.Id));

            SqlParameter[] parameters = paramList.ToArray();

            return TaskData.UpdateTask(parameters);
        }

        private bool _AddNewTask()
        {
            if (!_TaskConflict())
            {
                return false;
            }

            this.Id = TaskData.AddNewTask(TaskDBParameters);

            return (this.Id != -1);
        }

        static public Task Find(int id)
        {
            TaskDTO taskDTO = TaskData.FindByID(id);

            if (taskDTO != null)
            {
                return new Task(taskDTO, id, enMode.Update);
            }
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTask())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateTask();

            }

            return false;
        }

        static public List<TaskDTO> GetPageOfTasks(int PageNumber ,int PageSize )
        {
            return TaskData.GetPageOfTasks(PageNumber, PageSize);
        }

        static public List<TaskDTO> GetPageOfPastTasks(int PageNumber, int PageSize)
        {
            return TaskData.GetPageOfPastTasks(PageNumber, PageSize);
        }

        static public List<TaskDTO> GetPageOfActiveTasks(int PageNumber , int PageSize )
        {
            return TaskData.GetPageOfActiveTasks(PageNumber, PageSize);
        }

        static public bool DeleteTask(int ID)
        {
            return TaskData.DeleteTask(ID);
        }

    }
    
}
