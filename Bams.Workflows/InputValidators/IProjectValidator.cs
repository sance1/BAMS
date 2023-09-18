using BAMS.Data.Models;
using BAMS.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows.InputValidators
{
    //Default implementation is available at Bams.Workflows.Default
    public interface IProjectValidator
    {
        Task<List<string>> ValidateCreate(ProjectDto dto, string lang = "en");
        Task<List<string>> ValidateUpdate(ProjectDto dto, string lang = "en");
    }
}