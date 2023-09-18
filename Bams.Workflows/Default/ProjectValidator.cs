using Bams.Workflows.InputValidators;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using BAMS.Workflows.Models;
using EightElements.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows.Default
{
    public class ProjectValidator : IProjectValidator
    {
        private IUnitOfWork _uow;
        private ITextService _text;


        public ProjectValidator(IUnitOfWork uow, ITextService textService)
        {
            _uow = uow;
            _text = textService;
        }


        public async Task<List<string>> ValidateCreate(ProjectDto dto, string lang = "en")
        {
            return await ValidateProject(dto, lang);
        }

        public async Task<List<string>> ValidateUpdate(ProjectDto dto, string lang = "en")
        {
            return await ValidateProject(dto, lang);
        }


        private async Task<List<string>> ValidateProject(ProjectDto dto, string lang = "en")
        {
            var results = new List<string>();

            var nameFound = await _uow.ProjectRepository.CountAsync(p =>
                p.Name == dto.Name &&
                p.Uid != dto.Uid);

            /*if (nameFound > 0)
            {
                results.Add(_text.GetString("Project_popup_val_project_already_exists", lang));
            }*/

            if (string.IsNullOrEmpty(dto.Name))
            {
                results.Add(_text.GetString("Project_popup_val_project_name_cannot_empty", lang));
            }

            if (dto.Name.Length > 100)
            {
                results.Add(_text.GetString("Project_popup_val_project_name_exceed_char",lang));
            }

            if (string.IsNullOrEmpty(dto.PartnerName))
            {
                results.Add(_text.GetString("Project_popup_val_partner_name_cannot_empty",lang));
            }

            if (dto.PartnerName.Length > 100)
            {
                results.Add(_text.GetString("Project_popup_val_partner_name_exceed_char",lang));
            }

            if (dto.ContactPerson.Length > 100)
            {
                results.Add(_text.GetString("Project_popup_val_contract_person_exceed_char",lang));
            }

            if (dto.Remarks.Length > 1000)
            {
                results.Add(_text.GetString("Project_popup_val_notes_cannot_exceed_char",lang));
            }

            return results;
        }
    }
}