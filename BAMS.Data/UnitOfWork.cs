using BAMS.Data.Interface;
using BAMS.Data.Models;
using BAMS.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BAMS.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _context;
        public ProjectRepository ProjectRepository { get; }
        public PageTextRepository pageTextRepository { get; }
        public DistrictRepository DistrictRepository { get; }
        public AdministrativeUnitRepository AdministrativeUnitRepository { get; }
        public AdministrativeLevelRepository administrativeLevelRepository { get; }
        public SchoolRepository SchoolRepository { get; }
        public ContractRepository ContractRepository { get; }
        public AccountRepository AccountRepository { get; }                        
        //public TextRepository TextRepository { get; }
        public ApplicationRepository ApplicationRepository { get; }

        public RoleRepository RoleRepository { get; }
        public AccessRepository accessRepository { get; }
        public RolePermissionRepository rolePermissionRepository { get; }
        public ActivationCodeRequestRepository activationCodeRequestRepository { get; }
        public ActivationCodeUploadRepository activationCodeUploadRepository { get; }
        public ActivationCodeRepository activationCodeRepository { get; }
        public LogTrackingRepository logTrackingRepository { get; }
        public UserAccountRepository UserAccountRepository { get; }
        public ChangelogRepository ChangelogRepository { get; }
        public MessageRepository MessageRepository { get; }
        public MessageAttachmentRepository MessageAttachmentRepository { get; }
        public MessageRecipientRepository MessageRecipientRepository { get; }
        public CountryRepository countryRepository { get; }
        public ProvinceRepository provinceRepository { get; }
        public UnitOfWork(DataContext dataContext)
        {
            _context = dataContext;

            ProjectRepository = new ProjectRepository(_context);
            pageTextRepository = new PageTextRepository(_context);
            //deprecated:
            DistrictRepository = new DistrictRepository(_context);
            AdministrativeUnitRepository = new AdministrativeUnitRepository(_context);
            SchoolRepository = new SchoolRepository(_context);
            AccountRepository = new AccountRepository(_context);
            ContractRepository = new ContractRepository(_context);
            RoleRepository = new RoleRepository(_context);
            accessRepository = new AccessRepository(_context);
            rolePermissionRepository = new RolePermissionRepository(_context);
            activationCodeRequestRepository = new ActivationCodeRequestRepository(_context);
            activationCodeUploadRepository = new ActivationCodeUploadRepository(_context);
            activationCodeRepository = new ActivationCodeRepository(_context);
            //TextRepository = new TextRepository(_context);
            ApplicationRepository = new ApplicationRepository(_context);
            UserAccountRepository = new UserAccountRepository(_context);
            logTrackingRepository = new LogTrackingRepository(_context);
            ChangelogRepository = new ChangelogRepository(_context);
            MessageRepository = new MessageRepository(_context);
            MessageAttachmentRepository = new MessageAttachmentRepository(_context);
            MessageRecipientRepository = new MessageRecipientRepository(_context);
            countryRepository = new CountryRepository(_context);
            provinceRepository = new ProvinceRepository(_context);
            administrativeLevelRepository = new AdministrativeLevelRepository(_context);

        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public DataContext GetContext()
        {
            return _context;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<ActivationCodeRequest> GetContractWithActivationCodeRequest(long contractUid)
        {
            var data = ContractRepository.dbSet
                .Join(activationCodeRequestRepository.dbSet, a => a.Id, b => b.ContractId, (a, b) => new { contract = a, acR = b })
                .Where(a => a.contract.Uid == contractUid)
                .SingleOrDefault();
            
            if (data == null) return null;

            var result = data.acR;
            result.Contract = data.contract;

            return result;
        }

        public async Task<List<Contract>> GetContractsIncludeAsync(Func<IQueryable<Contract>, IOrderedQueryable<Contract>> orderBy = null,
            Expression<Func<Contract, bool>> predicate = null,
            int pageSize = 10, int skip = 0, bool ignoreFilter = false)
        {
            var query = predicate != null ? ContractRepository.dbSet.Where(predicate) : ContractRepository.dbSet.Where(a => true);

            if (orderBy != null)
            {
                query = orderBy(query).Select(a => a);
            }

            var query2 = query.Include(a => a.ActivationCodeRequest).ThenInclude(a => a.ActivationCodeUpload).Skip(skip).Take(pageSize);
            if (ignoreFilter)
            {
                query2 = query2.IgnoreQueryFilters();
            }

            return await query2.Include(a => a.Project).ToListAsync();
        }

        public async Task<List<MessageRecipient>> GetInboxAsync(Func<IQueryable<MessageRecipient>, IOrderedQueryable<MessageRecipient>> orderBy = null,
            Expression<Func<MessageRecipient, bool>> predicate = null,
            int pageSize = 10, int skip = 0, bool ignoreFilter = false)
        {
            var query = predicate != null ? MessageRecipientRepository.dbSet.Where(predicate) : MessageRecipientRepository.dbSet.Where(a => true);

            if (orderBy != null)
            {
                query = orderBy(query).Select(a => a);
            }

            var query2 = query.Include(a => a.Message).ThenInclude(a => a.Account).ThenInclude(a => a.Role).Skip(skip).Take(pageSize);
            if (ignoreFilter)
            {
                query2 = query2.IgnoreQueryFilters();
            }

            return await query2.ToListAsync();
        }
    }
}
