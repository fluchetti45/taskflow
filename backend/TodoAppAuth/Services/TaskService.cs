using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAppAuth.Context;
using TodoAppAuth.Dtos;
using TodoAppAuth.Interfaces;
using TodoAppAuth.Models;

namespace TodoAppAuth.Services
{
    public class TaskService : ITask
    {

        private readonly TodoContext _todoContext;

        public TaskService(TodoContext todoContext)
        {

            _todoContext = todoContext;
        }

        public async Task<Tarea> GetTask(int id)
        {
            Tarea tarea = await _todoContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            return tarea;
        }

        // AGREGAR DYNAMIC LINQ PARA PERMITIR FILTRAR POR STATUS O POR ORDEN DE CREACION/ACTUALIZACION ASC Y DESC.
        public async Task<IEnumerable<ReadTaskDTO>> GetTasks(int userId, string status)
        {
            // 
             
            List<Tarea> tareas = await _todoContext.Tasks.Where(t => t.UserId == userId).Include(t => t.Status).OrderByDescending(t=>t.UpdatedAt).ToListAsync();
            if(!string.IsNullOrEmpty(status))
            {
                tareas = tareas.Where(t => t.Status.StatusName == status).ToList();
            }
            IEnumerable<ReadTaskDTO> tareasDTO = tareas.Select(t => new ReadTaskDTO
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                StatusId = t.StatusId,
                StatusName = t.Status.StatusName,
                CreatedAt = t.CreatedAt,
            });

            return tareasDTO;
        }

        public async Task<IEnumerable<ReadTaskDTO>> GetTasksByStatus(int userId, string statusName)
        {
            //
            var status = await _todoContext.Statuses.FirstOrDefaultAsync(s => s.StatusName == statusName);
            if(status == null)
            {
                return null;
            }
            List<Tarea> tareas = await _todoContext.Tasks.Where(t => t.UserId == userId && t.StatusId == status.Id).OrderByDescending(t => t.CreatedAt).ToListAsync();
            IEnumerable<ReadTaskDTO> tareasDTO = tareas.Select(t => new ReadTaskDTO
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                StatusId = t.StatusId,
                StatusName = t.Status.StatusName,
                CreatedAt = t.CreatedAt,
            });
            return tareasDTO;
        }

        public async Task<ReadTaskDTO> CreateTask(CreateTaskDTO task, string userId)
        {
            // Crear la tarea
            Tarea tarea = new Tarea
            {
                Title = task.Title,
                Description = task.Description,
                UserId = int.Parse(userId),
                StatusId = task.statusId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            // Guardar la tarea en la base de datos
            _todoContext.Tasks.Add(tarea);
            await _todoContext.SaveChangesAsync();

            // Consultar la tarea con el estado relacionado (Eager Loading)
            tarea = await _todoContext.Tasks.Include(t => t.Status).FirstOrDefaultAsync(t => t.Id == tarea.Id);

            // Crear el DTO con la información de la tarea y su estado
            ReadTaskDTO tareaDTO = new()
            {
                Id = tarea.Id,
                Title = tarea.Title,
                Description = tarea.Description,
                StatusId = tarea.StatusId,
                CreatedAt = tarea.CreatedAt,
                StatusName = tarea.Status != null ? tarea.Status.StatusName : "Desconocido"
            };

            return tareaDTO;
        }
        public async Task<bool> DeleteTask(int id)
        {
            Tarea task = await _todoContext.Tasks.FindAsync(id);
            if (task != null)
            {
                _todoContext.Tasks.Remove(task);
                await _todoContext.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<Tarea> UpdateTask(EditTaskDTO task)
        {
            Tarea tarea = await this._todoContext.Tasks.FirstOrDefaultAsync(t => t.Id == task.Id);
            if (tarea == null)
            {
                return null;
            }
            tarea.Title = task.Title;
            tarea.Description = task.Description;
            tarea.StatusId = task.StatusId;
            tarea.UpdatedAt = DateTime.Now;
            await this._todoContext.SaveChangesAsync();
            return tarea;
        }
    }
}
