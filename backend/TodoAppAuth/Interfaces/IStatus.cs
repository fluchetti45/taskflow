using Microsoft.AspNetCore.Mvc;
using TodoAppAuth.Models;

namespace TodoAppAuth.Interfaces
{
    public interface IStatus
    {
        Task<List<AppStatus>> GetAllStatus();
        Task<AppStatus> GetStatus(int id);
        Task<AppStatus> GetStatus(string statusName);

        Task<AppStatus> CreateStatus(string statusName);
        
        Task<ActionResult> EditStatus(int id);
        Task<ActionResult> DeleteStatus(int id);
    }
}
