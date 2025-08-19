using Microsoft.AspNetCore.Mvc;
using TaskManagementDataAccessLayer;

namespace TaskManagementWepAPI.Controllers
{
    [Route("api/TaskManagement")]
    [ApiController]
    public class TaskManagement : ControllerBase
    {

        [HttpPost("AddTask",Name = "AddTask")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<TaskDTO> AddTask(TaskDTO newTaskDTO)
        {
            if (newTaskDTO == null)
            {
                return BadRequest("Invalid Task data.");
            }

            TaskManagementBusinessLayer.Task newtask = new TaskManagementBusinessLayer.Task(newTaskDTO);

            if (newtask.Save())
            {
                newTaskDTO.Id = newtask.Id;
                return CreatedAtRoute("GetTaskById", new { id = newTaskDTO.Id }, newTaskDTO);
            }
            else
            {
                return Conflict(newtask.ConflictType.ToString());
            }
        }



        [HttpPut("{id}", Name = "UpdateTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TaskDTO> UpdateTask(int id, TaskDTO newTaskDTO)
        {
            if (id < 0 || newTaskDTO == null || string.IsNullOrEmpty(newTaskDTO.Name) || newTaskDTO.DateAndTime < DateTime.Now || newTaskDTO.TakesTimeInMinutes < 0)
            {
                return BadRequest("Invalid task data.");
            }

            TaskManagementBusinessLayer.Task task = TaskManagementBusinessLayer.Task.Find(id);

            if (task == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            task.Name = newTaskDTO.Name;
            task.Description = newTaskDTO.Description;
            task.DateAndTime = newTaskDTO.DateAndTime;
            task.TakesTimeInMinutes = newTaskDTO.TakesTimeInMinutes;
            task.Details = newTaskDTO.Details;

            if (task.Save())
            {
                newTaskDTO.Id = task.Id;
                return Ok(task.GetTaskDTO);
            }
            else
            {
                return Conflict(task.ConflictType.ToString());
            }
        }



        [HttpDelete("{id}", Name = "DeleteTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteTask(int id)
        {
            if (id < 0)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (TaskManagementBusinessLayer.Task.DeleteTask(id))
                return Ok($"Task with ID {id} has been deleted.");
            else
                return NotFound($"Task with ID {id} not found. no rows deleted!");
        }



        [HttpGet("{id}", Name = "GetTaskById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TaskDTO> GetTaskById(int id)
        {
            if (id < 0)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            TaskManagementBusinessLayer.Task task = TaskManagementBusinessLayer.Task.Find(id);

            if (task == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            TaskDTO TDTO = task.GetTaskDTO;

            return Ok(TDTO);
        }



        [HttpGet("GetTasksAsPages", Name = "GetPageOfTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<TaskDTO>> GetPageOfTasks(int PageNumber = 1, int PageSize = 5)
        {
            List<TaskDTO> tasks = TaskManagementBusinessLayer.Task.GetPageOfTasks(PageNumber, PageSize);
            if (tasks.Count == 0)
            {
                return NotFound("No Tasks Found!");
            }
            return Ok(tasks);
        }



        [HttpGet("GetPastTaskAsPages", Name = "GetPageOfPastTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<TaskDTO>> GetPageOfPastTasks(int PageNumber = 1, int PageSize = 5)
        {
            List<TaskDTO> tasks = TaskManagementBusinessLayer.Task.GetPageOfPastTasks(PageNumber, PageSize);
            if (tasks.Count == 0)
            {
                return NotFound("No Tasks Found!");
            }
            return Ok(tasks);
        }



        [HttpGet("GetActiveTasksAsPages", Name = "GetPageOfActiveTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<TaskDTO>> GetPageOfActiveTasks(int PageNumber = 1, int PageSize = 5)
        {
            List<TaskDTO> tasks = TaskManagementBusinessLayer.Task.GetPageOfActiveTasks(PageNumber, PageSize);
            if (tasks.Count == 0)
            {
                return NotFound("No Tasks Found!");
            }
            return Ok(tasks);
        }

    }
}