using Bams.Workflows.Enums;
using Bams.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows
{
    //Default implementation is available at Bams.Workflows.Default
    public interface IContractWorkflow
    {
        Task<WorkflowResult> CreateContract(int userId, ContractDto dto);
        Task<WorkflowResult> UpdateContract(int userId, ContractDto dto);
        Task<WorkflowResult> DeleteContract(int userId, long uid);
    }



}
