using Task = ProjectTracker.Entities.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.Interfaces
{
    public interface ITaskInteractor
    {
        List<Task> GetProjectTasks(int? projectId);

        Task GetTaskDetails(int? id);

        (bool, string) CreateTask(Task task);

        (bool, string) UpdateTask(int id, Task task);

        bool TaskExists(string taskName);

        Task<bool> DeleteAttachment(int id);
    }
}
