using Bams.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows.InputValidators
{
    //Default implementation is available at Bams.Workflows.Default
    public interface ISchoolValidator
    {
        Task<List<string>> ValidateCreate(int userId, SchoolDto dto, string lang = "en");
        Task<List<string>> ValidateUpdate(int userId, SchoolDto dto, string lang = "en");
    }
}
