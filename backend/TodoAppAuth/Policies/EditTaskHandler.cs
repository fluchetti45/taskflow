using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TodoAppAuth.Models;
using TodoAppAuth.Services;

public class EditTaskHandler : AuthorizationHandler<EditTaskRequirement>
{
    private readonly TaskService _taskService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EditTaskHandler(TaskService taskService, IHttpContextAccessor httpContextAccessor)
    {
        _taskService = taskService;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EditTaskRequirement requirement)
    {
        int userId = int.Parse(context.User.FindFirstValue("id"));

        // Obtener el ID de la tarea desde la ruta
        var httpContext = _httpContextAccessor.HttpContext;
        var routeData = httpContext?.GetRouteData();
        if (routeData != null && routeData.Values.TryGetValue("id", out var taskIdObj) && int.TryParse(taskIdObj.ToString(), out var taskId))
        {

            Tarea tarea = await _taskService.GetTask(taskId);
            if (tarea != null && (tarea.UserId == userId || context.User.IsInRole("Admin")))
            {
                context.Succeed(requirement);
            }
        }
    }
}
