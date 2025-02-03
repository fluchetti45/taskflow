using Microsoft.AspNetCore.Mvc;
using TodoAppAuth.Dtos;
using TodoAppAuth.Models;

namespace TodoAppAuth.Interfaces
{
    public interface ITask
    {
        Task<ReadTaskDTO> CreateTask(CreateTaskDTO task, string userId);

        Task<Tarea> UpdateTask(EditTaskDTO task);

        Task<Tarea> GetTask(int id);

        Task<IEnumerable<ReadTaskDTO>> GetTasks(int userId, string status);

        Task<IEnumerable<ReadTaskDTO>> GetTasksByStatus(int userId, string status = "");

        Task<bool> DeleteTask(int id);
    }
}
