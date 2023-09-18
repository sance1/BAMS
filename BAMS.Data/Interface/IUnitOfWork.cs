using BAMS.Data.Models;
using BAMS.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BAMS.Data.Interface
{
    public interface IUnitOfWork
    {
        void Save();
        Task SaveAsync();
        DataContext GetContext();
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();

        ProjectRepository ProjectRepository { get; }
        CountryRepository countryRepository { get; }
        PageTextRepository pageTextRepository { get; }
        DistrictRepository DistrictRepository { get; }
        AdministrativeUnitRepository AdministrativeUnitRepository { get; }
        AdministrativeLevelRepository administrativeLevelRepository { get; }
        SchoolRepository SchoolRepository { get; }
        AccountRepository AccountRepository { get; }
        ContractRepository ContractRepository { get; }
        //TextRepository TextRepository { get; }
        ApplicationRepository ApplicationRepository { get; }

        RoleRepository RoleRepository { get; }
        AccessRepository accessRepository { get; }
        RolePermissionRepository rolePermissionRepository { get; }
        ActivationCodeRequestRepository activationCodeRequestRepository { get; }
        ActivationCodeUploadRepository activationCodeUploadRepository { get; }
        ActivationCodeRepository activationCodeRepository { get; }
        UserAccountRepository UserAccountRepository { get; }

        ChangelogRepository ChangelogRepository { get; }
        LogTrackingRepository logTrackingRepository { get; }

        MessageRepository MessageRepository { get; }
        MessageAttachmentRepository MessageAttachmentRepository { get; }
        MessageRecipientRepository MessageRecipientRepository { get; }
        ProvinceRepository provinceRepository { get; }
        Task<ActivationCodeRequest> GetContractWithActivationCodeRequest(long contractUid);
        Task<List<Contract>> GetContractsIncludeAsync(Func<IQueryable<Contract>, IOrderedQueryable<Contract>> orderBy = null,
            Expression<Func<Contract, bool>> predicate = null,
            int pageSize = 10, int skip = 0, bool ignoreFilter = false);
        Task<List<MessageRecipient>> GetInboxAsync(Func<IQueryable<MessageRecipient>, IOrderedQueryable<MessageRecipient>> orderBy = null,
            Expression<Func<MessageRecipient, bool>> predicate = null,
            int pageSize = 10, int skip = 0, bool ignoreFilter = false);
    }
}