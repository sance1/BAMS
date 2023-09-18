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
    public class SchoolValidator : ISchoolValidator
    {
        private IUnitOfWork _uow;
        private ITextService _text;

        public SchoolValidator(IUnitOfWork uow, ITextService textService)
        {
            _uow = uow;
            _text = textService;
        }


        public async Task<List<string>> ValidateCreate(int userId, SchoolDto dto, string lang = "en")
        {
            return await ValidateSchool(userId, dto, lang);
        }


        public async Task<List<string>> ValidateUpdate(int userId, SchoolDto dto, string lang = "en")
        {
            return await ValidateSchool(userId, dto, lang);
        }


        private async Task<List<string>> ValidateSchool(int userId, SchoolDto dto, string lang = "en")
        {
            var results = new List<string>();

            //var district = await _uow.DistrictRepository.GetByUidAsync(dto.DistrictUid);

            //if (district == null)
            //{
            //    results.Add(_text.GetString("District not found", "en"));
            //    return results;
            //}

            var admUnit = await _uow.AdministrativeUnitRepository.GetByUidAsync(dto.AdmUnitUid);

            if (admUnit == null)
            {
                results.Add(_text.GetString("School_popup_val_district_not_found", lang));
                return results;
            }

            var user = await _uow.AccountRepository.GetByIdAsync(userId);

            if (user.ProjectId != 0 && user.ProjectId != admUnit.ProjectId)
            {
                results.Add(_text.GetString("School_popup_val_access_violation", lang));
                return results;
            }

            if (user.ProjectId == null && user.AdministrativeUnitId != admUnit.Id)
            {
                results.Add(_text.GetString("School_popup_val_access_violation", lang));
                return results;
            }

            var nameFound = await _uow.SchoolRepository.CountAsync(s =>
                s.Name == dto.Name && s.Uid != dto.Uid);
            if (nameFound > 0)
            {
                results.Add(_text.GetString("School_popup_val_school_name_already_exist", lang));
            }

            if (string.IsNullOrEmpty(dto.Name))
            {
                results.Add(_text.GetString("School_popup_val_school_name_cant_empty", lang));
            }

            if (dto.Name.Length > 100)
            {
                results.Add(_text.GetString("School_popup_val_school_name_exceed_char", lang));
            }            

            if (dto.Address.Length > 100)
            {
                results.Add(_text.GetString("School_popup_val_school_address_exceed_char", lang));
            }

            if (dto.PIC.Length > 100)
            {
                results.Add(_text.GetString("School_popup_val_pic_name_exceed_char", lang));
            }

            if (dto.Remarks != null && dto.Remarks.Length > 1000)
            {
                results.Add(_text.GetString("School_popup_val_notes_exceed_char", lang));
            }

            return results;

        }
    }
}
