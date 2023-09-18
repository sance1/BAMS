using System;
using System.Collections.Generic;
using System.Text;

namespace Bams.Workflows.Enums
{
    public enum WorkflowResult
    {
        Success,
        DataNotFound,
        ActionProhibited,
        AccessViolation,

        ActivationCodeNotFound,
        UnknownError
    }
}
