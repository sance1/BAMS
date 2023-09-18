using Bams.Workflows.Enums;
using Bams.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows
{
    public interface IAdministrativeUnitWorkflow
    {
        Task<WorkflowResult> Create(int userId, AdministrativeUnitDto dto);
        Task<WorkflowResult> Update(int userId, AdministrativeUnitDto dto);
        Task<WorkflowResult> Delete(int userId, long uid);
    }
}
