using BAMS.Data.Interface;
using BAMS.Models;
using EightElements.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAMS.InputValidators
{
    public class DistrictValidator
    {
        public static async Task<List<string>> ValidateCreateDistrict(
            int userId, CreateDistrictDto dto, IUnitOfWork uow,ITextService textService, string lang = "en")
        {
            return await ValidateDistrictData(
                userId, dto.ProjectUid, 0, 
                dto.Name, dto.PIC, dto.Remarks, 
                uow,textService, lang);
        }

        public static async Task<List<string>> ValidateUpdateDistrict(
            int userId, int districtId, UpdateDistrictDto dto, IUnitOfWork uow,ITextService textService, string lang = "en")
        {
            return await ValidateDistrictData(
                userId, dto.ProjectUid, districtId, 
                dto.Name, dto.PIC, dto.Remarks, 
                uow, textService,lang);
        }


        private async static Task<List<string>> ValidateDistrictData(
            int userId, long projectUid, int districtId, 
            string districtName, string picName, string remarks, 
            IUnitOfWork uow,ITextService textService, string lang = "en")
        {
            var results = new List<string>();

            var project = await uow.ProjectRepository.GetByUidAsync(projectUid);
            if (project == null)
            {
                results.Add(textService.GetString("District_popup_val_project_not_found", lang));                
                return results;
            }

            var user = await uow.AccountRepository.GetByIdAsync(userId);
            if (user.ProjectId != 0 && user.ProjectId != project.Id)
            {
                results.Add(textService.GetString("District_popup_val_access_violation", lang));                
                return results;
            }

            var nameFound = await uow.DistrictRepository.CountAsync(d => 
                d.Name == districtName && d.Id != districtId);
            if (nameFound > 0)
            {
                results.Add(textService.GetString("District_popup_val_district_name_exist", lang));
            }

            if (string.IsNullOrEmpty(districtName))
            {
                results.Add(textService.GetString("District_popup_val_district_name_cannot_empty", lang));
            }

            if (districtName.Length > 100)
            {
                results.Add(textService.GetString("District_popup_val_district_name_exceed_characters", lang));
            }

            if (picName.Length > 100)
            {
                results.Add(textService.GetString("District_popup_val_pic_name_exceed_characters", lang));
            }

            if (remarks.Length > 1000)
            {
                results.Add(textService.GetString("District_popup_val_notes_exceed_characters", lang));
            }

            return results;
        }
    }
}
