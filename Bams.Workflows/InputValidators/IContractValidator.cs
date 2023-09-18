using Bams.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows.InputValidators
{
    //Default implementation is available at Bams.Workflows.Default
    public interface IContractValidator
    {
        Task<List<string>> ValidateCreate(ContractDto dto,string lang = "en");
        Task<List<string>> ValidateUpdate(ContractDto dto,string lang = "en");
    }
}
