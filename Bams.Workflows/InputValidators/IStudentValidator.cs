using Bams.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows.InputValidators
{
    public interface IStudentValidator
    {
        Task<List<string>> ValidateCreate(int userId, StudentDto dto);
        Task<List<string>> ValidateUpdate(int userId, StudentDto dto);
    }
}
