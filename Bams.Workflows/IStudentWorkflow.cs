using Bams.Workflows.Enums;
using Bams.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows
{
    public interface IStudentWorkflow
    {
        Task<WorkflowResult> CreateStudent(int userId, StudentDto dto);
        Task<WorkflowResult> UpdateStudent(int userId, StudentDto dto);
        Task<WorkflowResult> DeleteStudent(int userId, long uid);
        Task<WorkflowResult> ActivateAccount(int userId, long uid);
        Task<WorkflowResult> DismissClass(int userId, string className);
    }
}
