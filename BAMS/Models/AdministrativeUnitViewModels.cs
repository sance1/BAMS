using System;

namespace BAMS.Models
{
    public class AdministrativeUnitModel
    {
        public long Uid { get; set; }
        public string ProjectName { get; set; }
        public string Name { get; set; }
        public string PIC { get; set; }
        public string Remarks { get; set; }
        public int Schools { get; set; }
        public int Students { get; set; }
        public int ActivationCodes { get; set; }
        public int ActivationCodesActive { get; set; }
    }

    public class AdmUnitDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UploadAdmUnitDTO
    {
        public string AdministrativeUnitName { get; set; }
        public string PIC { get; set; }
    }
    
    public class UploadAdministrativeUnitDto
    {
        public string AdministrativeUnitName { get; set; }
        public string PIC { get; set; }
        public string ParentAdministrativeUnitName { get; set; }
        public string Remakrs { get; set; }
    }
}
