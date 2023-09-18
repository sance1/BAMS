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
    public class ContractValidator : IContractValidator
    {
        private IUnitOfWork _uow;
        private ITextService _text;


        public ContractValidator(IUnitOfWork uow, ITextService textService)
        {
            _uow = uow;
            _text = textService;
        }


        public async Task<List<string>> ValidateCreate(ContractDto dto,string lang = "en")
        {
            return await ValidateContract(dto,lang);
        }

        public async Task<List<string>> ValidateUpdate(ContractDto dto,string lang = "en")
        {
            return await ValidateContract(dto,lang);
        }


        private async Task<List<string>> ValidateContract(ContractDto dto, string lang = "en")
        {
            var results = new List<string>();

            var projectFound = await _uow.ProjectRepository.CountAsync(p =>
                p.Uid == dto.ProjectUid &&
                p.DeleteDate == null);

            if (projectFound == 0)
            {
                results.Add(_text.GetString("Contract_popup_val_project_not_found", lang));
            }

            var nameFound = await _uow.ContractRepository.CountAsync(
                c => c.Name == dto.Name && c.Uid != dto.Uid);
            if (nameFound > 0)
            {
                results.Add(_text.GetString("Contract_popup_val_name_already_used", lang));
            }

            if (string.IsNullOrEmpty(dto.Name))
            {
                results.Add(_text.GetString("Contract_popup_val_name_cannot_empty", lang));
            }

            if (dto.Name.Length > 100)
            {
                results.Add(_text.GetString("Contract_popup_val_name_exceed_characters", lang));
            }

            if (dto.StartDate > dto.EndDate)
            {
                results.Add(_text.GetString("Contract_popup_val_end_date_should_after_date", lang));
            }

            if (dto.Remarks.Length > 1000)
            {
                results.Add(_text.GetString("Contract_popup_val_notes_exceed_characters", lang));
            }

            return results;
        }
    }
}
