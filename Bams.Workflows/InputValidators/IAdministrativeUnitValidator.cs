using Bams.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows.InputValidators
{
    public interface IAdministrativeUnitValidator
    {
        Task<List<string>> ValidateCreate(AdministrativeUnitDto dto);
        Task<List<string>> ValidateUpdate(AdministrativeUnitDto dto);
    }
}
