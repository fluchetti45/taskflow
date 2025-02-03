using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TodoAppAuth.Models;
using TodoAppAuth.Services;

namespace TodoAppAuth.Policies
{
    public class EditUserHandler : AuthorizationHandler<EditUserRequirement>
    {
        private readonly UserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;

        public EditUserHandler(UserService userService, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _userService = userService;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EditUserRequirement requirement)
        {
            //
            int uId = int.Parse(context.User.FindFirstValue("id"));
            var httpContextAccessor = _contextAccessor.HttpContext;
            var routeData = httpContextAccessor?.GetRouteData();
            if (routeData != null && routeData.Values.TryGetValue("id", out var userIdObj) && int.TryParse(userIdObj.ToString(), out var userId))
            {

                AppUser user = await _userService.GetUser(userId);
                if (user != null && (user.Id == userId || context.User.IsInRole("Admin")))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
