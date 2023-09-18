using Bams.Workflows.InputValidators;
using Bams.Workflows.Models;
using BAMS.Data.Interface;
using EightElements.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows.Default
{
    public class AdministrativeUnitValidator : IAdministrativeUnitValidator
    {
        private IUnitOfWork _uow;
        private ITextService _text;


        public AdministrativeUnitValidator(IUnitOfWork uow, ITextService textService)
        {
            _uow = uow;
            _text = textService;
        }

        public async Task<List<string>> ValidateCreate(AdministrativeUnitDto dto)
        {
            return await ValidateAdministrativeUnit(dto);
        }

        public async Task<List<string>> ValidateUpdate(AdministrativeUnitDto dto)
        {
            return await ValidateAdministrativeUnit(dto);
        }


        private async Task<List<string>> ValidateAdministrativeUnit(AdministrativeUnitDto dto)
        {
            var results = new List<string>();

            var projectFound = await _uow.ProjectRepository.CountAsync(p =>
               p.Uid == dto.ProjectUid &&
               p.DeleteDate == null);

            if (projectFound == 0)
            {
                results.Add(_text.GetString("Administrative_popup_val_project_not_found", "en"));
            }

            if (dto.ParentUid > 0)
            {
                var parentFound = await _uow.AdministrativeUnitRepository.CountAsync(u =>
                    u.Uid == dto.ParentUid &&
                    u.DeleteDate == null);

                if (parentFound == 0)
                {
                    results.Add(_text.GetString("Administrative_popup_val_administrative_not_found","en"));
                }
            }

            if (string.IsNullOrEmpty(dto.Name))
            {
                results.Add(_text.GetString("Administrative_popup_val_administrative_name_empty","en"));
            }
            if (dto.Name.Length > 100)
            {
                results.Add(_text.GetString("Administrative_popup_val_name_cannot_exceed_char","en"));
            }                        

            if (dto.PIC.Length > 100)
            {
                results.Add(_text.GetString("Administrative_popup_val_pic_cannot_exceed_char","en"));
            }

            if (dto.Remarks.Length > 1000)
            {
                results.Add(_text.GetString("Administrative_popup_val_note_cannot_exceed_char", "en"));
            }

            return results;
        }
    }
}
