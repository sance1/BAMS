using Bams.Workflows.Enums;
using BAMS.Data.Models;
using BAMS.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows
{
    //Default implementation is available at Bams.Workflows.Default
    public interface IProjectWorkflow
    {        
        Task<WorkflowResult> CreateProject(int userId, ProjectDto dto);
        Task<WorkflowResult> UpdateProject(int userId, ProjectDto dto);
        Task<WorkflowResult> DeleteProject(int userId, long uid);
    }

}
