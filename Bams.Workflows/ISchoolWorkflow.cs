using Bams.Workflows.Enums;
using Bams.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows
{
    //Default implementation is available at Bams.Workflows.Default
    public interface ISchoolWorkflow
    {
        Task<WorkflowResult> CreateSchool(int userId, SchoolDto dto);
        Task<WorkflowResult> UpdateSchool(int userId, SchoolDto dto);
        Task<WorkflowResult> DeleteSchool(int userId, long uid);
    }


}
