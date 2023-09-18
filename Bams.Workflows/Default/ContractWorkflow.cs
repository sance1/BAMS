using Bams.Workflows.Enums;
using Bams.Workflows.Models;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using EightElements.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bams.Workflows.Default
{
    public class ContractWorkflow : IContractWorkflow
    {
        private IUnitOfWork _uow;
        private IChangelogService _changelog;
        private readonly ILogger<ContractWorkflow> _logger;


        public ContractWorkflow(
            IUnitOfWork unitOfWork,
            IChangelogService changelogService,
            ILogger<ContractWorkflow> logger)
        {
            _uow = unitOfWork;
            _changelog = changelogService;
            _logger = logger;
        }


        public async Task<WorkflowResult> CreateContract(
            int userId, ContractDto dto)
        {
            try
            {
                var contract = new Contract
                {
                    Uid = await _uow.ProjectRepository.GenerateUid(),
                    ProjectId = await _uow.ProjectRepository.GetIdByUid(dto.ProjectUid),
                    Name = dto.Name,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    ActivationCodes = dto.ActivationCodes,
                    Remarks = dto.Remarks,
                    CreateDate = DateTime.Now,
                    CreatedBy = userId
                };
                await _uow.ContractRepository.AddAsync(contract);

                return WorkflowResult.Success;

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }


        public async Task<WorkflowResult> UpdateContract(int userId, ContractDto dto)
        {
            try
            {
                var contract = await _uow.ContractRepository.GetByUidAsync(dto.Uid);
                if (contract == null)
                {
                    return WorkflowResult.DataNotFound;
                }

                var requestFound = await _uow.activationCodeRequestRepository.CountAsync(r =>
                    r.ContractId == contract.Id);
                if (requestFound > 0)
                {
                    return WorkflowResult.ActionProhibited;
                }

                string oldValue = JsonConvert.SerializeObject(contract);

                contract.ProjectId = await _uow.ProjectRepository.GetIdByUid(dto.ProjectUid);
                contract.Name = dto.Name;
                contract.StartDate = dto.StartDate;
                contract.EndDate = dto.EndDate;
                contract.ActivationCodes = dto.ActivationCodes;                
                contract.Remarks = dto.Remarks;
                await _uow.ContractRepository.UpdateAsync(contract);

                string newValue = JsonConvert.SerializeObject(contract);
                await _changelog.Log("Contract", contract.Id, userId, oldValue, newValue);

                return WorkflowResult.Success;
            }
            catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
        }

        public async Task<WorkflowResult> DeleteContract(int userId, long uid)
        {
            try
            {
                var contract = await _uow.ContractRepository.GetByUidAsync(uid);

                if (contract == null)
                {
                    return WorkflowResult.DataNotFound;
                }

                var requestFound = await _uow.activationCodeRequestRepository.CountAsync(r =>
                        r.ContractId == contract.Id);
                var codeFound = await _uow.activationCodeRepository.CountAsync(d => d.ContractId == contract.Id);
                if (requestFound > 0 && codeFound > 0)
                {
                    return WorkflowResult.ActionProhibited;
                }

                contract.DeleteDate = DateTime.Now;
                contract.DeletedBy = userId;
                await _uow.ContractRepository.UpdateAsync(contract);

                return WorkflowResult.Success;
            } catch (Exception e)
            {
                _logger.LogError($"error | {e.Message} | {e.StackTrace}");
                return WorkflowResult.UnknownError;
            }
            
        }
    }
}
