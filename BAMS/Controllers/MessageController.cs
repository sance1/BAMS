using AutoMapper;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using BAMS.Helpers;
using BAMS.Models;
using EightElements.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BAMS.Controllers
{
    public class MessageController : BaseController
    {
        private IUnitOfWork _uow;
        private IMapper _mapper { get; set; }
        private readonly ILogger<MessageController> _logger;
        private ITextService _textService;
        private readonly IConfiguration _config;

        public MessageController(
            IUnitOfWork unitOfWork,
            ITextService textService,
            ILogger<MessageController> logger,
            IConfiguration config
        ) : base(unitOfWork, textService)
        {
            _uow = unitOfWork;
            _logger = logger;
            _textService = textService;
            _config = config;

            if (_mapper == null)
            {
                var mapperConfig = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CreateMessageDTO, Message>(MemberList.Source)
                        .ForMember(dest => dest.AccountId,
                            act => act.MapFrom(src => src.SenderId));
                    cfg.CreateMap<Message, ReadMessageDTO>()
                        .ForMember(dest => dest.Sender, act => act.MapFrom(src => src.Account))
                        .ForMember(dest => dest.Attachments, act => act.MapFrom(src => src.MessageAttachments));
                    cfg.CreateMap<Account, MessageAccountDTO>()
                        .ForMember(dest => dest.Name, act => act.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.Role, act => act.MapFrom(src => src.Role.Name));
                    cfg.CreateMap<MessageRecipient, InboxDTO>(MemberList.Destination)
                        .ForMember(dest => dest.Uid, act => act.MapFrom(src => src.Message.Uid))
                        .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Message.Title))
                        .ForMember(dest => dest.Body,
                            act => act.MapFrom(src =>
                                StripTags(src.Message.Body)
                                    .Substring(0, Math.Min(25, StripTags(src.Message.Body).Length))))
                        .ForMember(dest => dest.Sender, act => act.MapFrom(src => src.Message.Account));
                    cfg.CreateMap<MessageAttachment, MessageAttachmentsDTO>();
                });

                _mapper = mapperConfig.CreateMapper();
            }
        }

        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Message,
            Permission = Permissions.Read)]
        public IActionResult Index(string search = "")
        {
            ViewData["Search"] = search;
            return View();
        }

        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Message,
            Permission = Permissions.Read)]
        public async Task<IActionResult> GetMessage(long uid)
        {
            try
            {
                var message = (await _uow.MessageRepository.GetAsync(predicate: a => a.Uid == uid,
                    includeProperties: "Account,MessageAttachments")).FirstOrDefault();
                if (message == null)
                {
                    return NotFound(new {message = _textService.GetString("Message_popup_txt_email_not_found", "en") });
                }

                message.Account.Role = await _uow.RoleRepository.GetByIdAsync(message.Account.RoleId);

                var receivedMessage =
                    (await _uow.MessageRecipientRepository.GetAsync(predicate: a =>
                        a.MessageId == message.Id && a.AccountId == UserId)).SingleOrDefault();
                if (receivedMessage == null)
                {
                    return NotFound(new {message = _textService.GetString("Message_popup_txt_email_not_found", "en") });
                }

                if (!receivedMessage.IsRead)
                {
                    receivedMessage.IsRead = true;
                    await _uow.MessageRecipientRepository.UpdateAsync(receivedMessage);
                }

                var dto = _mapper.Map<ReadMessageDTO>(message);

                return Content(JsonConvert.SerializeObject(dto));
            }
            catch (Exception e)
            {
                _logger.LogError($"error while get data | {e.Message}");
                return new BadRequestObjectResult(e.Message);
            }
        }

        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Message,
            Permission = Permissions.Read)]
        public async Task<IActionResult> GetInbox(string search = "", int page = 0, int pageSize = 10)
        {
            try
            {
                var pred = PredicateBuilder.True<MessageRecipient>();
                pred = pred.And(a => a.AccountId == UserId);
                if (!string.IsNullOrEmpty(search))
                {
                    pred = pred.And(a =>
                        a.Message.Title.Contains(search) || a.Message.Body.Contains(search) ||
                        a.Message.Account.UserName.Contains(search));
                }

                var data = await _uow.GetInboxAsync(predicate: pred, skip: page, pageSize: pageSize,
                    orderBy: a => a.OrderByDescending(o => o.Id));
                var count = await _uow.MessageRecipientRepository.CountAsync(pred);

                var dto = _mapper.Map<List<InboxDTO>>(data);

                var result = new DataInboxDTO()
                {
                    Data = dto,
                    Prev = page - pageSize >= 0 ? page - pageSize : -1,
                    Next = page + pageSize < count ? page + pageSize : -1
                };

                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error while get data | {e.Message}");
                return new BadRequestObjectResult(e.Message);
            }
        }

        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Message,
            Permission = Permissions.Create)]
        public async Task<IActionResult> CreateMessage(long uid = 0, string act = "")
        {
            var setting = _config[$"MenuConfiguration:Message:{RoleId}"];
            ViewData["Message"] = null;
            ViewData["Action"] = "";
            ViewData["ToTeacher"] = setting.Contains("teacher");
            ViewData["ToAdmin"] = setting.Contains("admin");
            ViewData["ToDistrict"] = setting.Contains("district");
            ViewData["ToProvince"] = setting.Contains("province");      

            if (uid > 0)
            {
                var message = (await _uow.MessageRepository.GetAsync(predicate: a => a.Uid == uid,
                    includeProperties: "MessageRecipients,Account")).FirstOrDefault();
                message.Account.Role = await _uow.RoleRepository.GetByIdAsync(message.Account.RoleId);
                if (message != null)
                {
                    var isRecipient = message.MessageRecipients.Any(a => a.AccountId == UserId);
                    if (isRecipient)
                    {
                        ViewData["Message"] = message;
                        ViewData["Action"] = act;
                    }
                }
            }

            return View("CreateMessage");
        }

        [HttpPost]
        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Message,
            Permission = Permissions.Create)]
        public async Task<IActionResult> Create([FromForm] CreateMessageDTO dto)
        {
            try
            {
                if (dto.Recipients == null && !dto.AllTeacher && !dto.ToAdmin && !dto.AllDistrict && !dto.AllProvince)
                {
                    return Failed(GetText("Message_popup_txt_invalid_recipients"));
                }

                var accountRecipients = new List<Account>();

                if (dto.Recipients != null)
                {
                    accountRecipients = await _uow.AccountRepository.GetAsync(predicate: a =>
                        dto.Recipients.Contains(a.UserName) && a.DeleteDate == null);
                    if (accountRecipients.Count != dto.Recipients.Count)
                    {
                        var invalidUser = dto.Recipients.Except(accountRecipients.Select(a => a.UserName).ToList())
                            .ToList();
                        return Failed(GetText("Message_popup_txt_invalid_recipients") + " (" +
                                      String.Join(", ", invalidUser) + ")");
                    }
                }

                if (dto.AllTeacher)
                {
                    var predicate = PredicateBuilder.True<Account>();
                    predicate = predicate.And(a => a.RoleId == 14 && a.DeleteDate == null);

                    if (DistrictId > 0)
                    {
                        predicate = predicate.And(a => a.DistrictId == DistrictId);
                    }

                    if (ProjectId > 0)
                    {
                        predicate = predicate.And(a => a.ProjectId == ProjectId);
                    }

                    var allTeacher =
                        await _uow.AccountRepository.GetAsync(predicate: predicate);
                    accountRecipients.AddRange(allTeacher);
                }

                if (dto.AllDistrict)
                {
                    var predicate = PredicateBuilder.True<Account>();
                    predicate = predicate.And(a => a.RoleId == 13 && a.DeleteDate == null);

                    if (ProjectId > 0)
                    {
                        predicate = predicate.And(a => a.ProjectId == ProjectId);
                    }

                    var allDistrict =
                        await _uow.AccountRepository.GetAsync(predicate: predicate);
                    accountRecipients.AddRange(allDistrict);
                }

                if (dto.AllProvince)
                {
                    var predicate = PredicateBuilder.True<Account>();
                    predicate = predicate.And(a => a.RoleId == 18 && a.DeleteDate == null);

                    if (ProjectId > 0)
                    {
                        predicate = predicate.And(a => a.ProjectId == ProjectId);
                    }

                    var allProvince =
                        await _uow.AccountRepository.GetAsync(predicate: predicate);
                    accountRecipients.AddRange(allProvince);
                }


                if (dto.ToAdmin)
                {
                    var toAdmin =
                        await _uow.AccountRepository.GetAsync(predicate: a => a.RoleId == 1 && a.DeleteDate == null);
                    accountRecipients.AddRange(toAdmin);
                }

                dto.SenderId = UserId;
                var message = _mapper.Map<Message>(dto);
                message.Uid = await _uow.MessageRepository.GenerateUid();
                message.CreateDate = DateTime.Now;
                message.CreatedBy = message.AccountId;
                message = await _uow.MessageRepository.AddAsync(message);

                var recipients = new List<MessageRecipient>();
                foreach (var acc in accountRecipients)
                {
                    var rec = new MessageRecipient()
                    {
                        MessageId = message.Id,
                        Uid = await _uow.MessageRecipientRepository.GenerateUid(),
                        AccountId = acc.Id,
                        CreateDate = DateTime.Now,
                        CreatedBy = message.AccountId
                    };
                    recipients.Add(rec);
                }

                await _uow.MessageRecipientRepository.AddRangeAsync(recipients);

                message.Account = await _uow.AccountRepository.GetByIdAsync(message.AccountId);

                if (dto.Attachments != null && dto.Attachments.Count > 0)
                {
                    var directory = _config["Resources:AttachmentPath"];
                    var acceptedFileExt = new string[]
                        {".xlsx", ".xls", ".jpg", ".png", ".bmp", ".doc", ".docx", ".pdf"};
                    var attachments = new List<MessageAttachment>();
                    var errors = new List<string>();

                    foreach (var file in dto.Attachments)
                    {
                        if (file == null || file.Length == 0)
                        {
                            errors.Add(GetText("Message_popup_txt_file_not_found"));
                            continue;
                        }

                        var ext = Path.GetExtension(file.FileName).ToLower();
                        if (!acceptedFileExt.Contains(ext))
                        {
                            errors.Add(GetText("Message_popup_txt_file_type_not_supported"));
                            continue;
                        }

                        var filename = $"{message.Id}_{file.FileName}";

                        string filePath = Path.Combine(directory, filename);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        var attach = new MessageAttachment()
                        {
                            Uid = await _uow.MessageAttachmentRepository.GenerateUid(),
                            MessageId = message.Id,
                            FileName = filename,
                            CreateDate = DateTime.Now,
                            CreatedBy = message.AccountId
                        };
                        attachments.Add(attach);
                    }

                    if (attachments.Count > 0)
                    {
                        await _uow.MessageAttachmentRepository.AddRangeAsync(attachments);
                    }
                }

                var result = _mapper.Map<ReadMessageDTO>(message);

                return Content(JsonConvert.SerializeObject(new {status = 0, message = "success"}), "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error while create message | {e.Message}", e);
                return new BadRequestObjectResult(e.Message);
            }
        }

        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Message,
            Permission = Permissions.Read)]
        public async Task<IActionResult> SearchName(string search = "")
        {
            try
            {
                var result = new List<object>();
                if (string.IsNullOrEmpty(search))
                {
                    return Content(JsonConvert.SerializeObject(result), "application/json");
                }


                var pred = PredicateBuilder.True<Account>();
                if (ProjectId > 0)
                {
                    pred = pred.And(a => a.ProjectId == ProjectId);
                }

                if (ContractId > 0)
                {
                    pred = pred.And(a => a.ContractId == ContractId);
                }

                if (DistrictId > 0)
                {
                    pred = pred.And(a => a.DistrictId == DistrictId);
                }

                pred = pred.And(a => a.Id != UserId);

                pred = pred.And(a => a.UserName.Contains(search));
                var data = await _uow.AccountRepository.GetAsync(predicate: pred, skip: 0, pageSize: 10,
                    orderBy: a => a.OrderByDescending(o => o.Id));
                foreach (var val in data)
                {
                    var a = new
                    {
                        id = val.UserName,
                        text = val.UserName
                    };
                    result.Add(a);
                }

                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            catch (Exception e)
            {
                _logger.LogError($"error while get data | {e.Message}");
                return new BadRequestObjectResult(e.Message);
            }
        }

        [PermitAccess(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
            Group = AccessGroups.Message,
            Permission = Permissions.Read)]
        public async Task<IActionResult> DownloadAttachment(long uid)
        {
            try
            {
                var attachment =
                    (await _uow.MessageAttachmentRepository.GetAsync(predicate: a => a.Uid == uid,
                        includeProperties: "Message")).FirstOrDefault();
                if (attachment == null)
                {
                    return new BadRequestObjectResult(new
                        {message = GetText("Message_popup_txt_request_not_found") });
                }

                var recipient = await _uow.MessageRecipientRepository.GetAsync(predicate: a =>
                    a.MessageId == attachment.MessageId && a.AccountId == UserId);


                if (recipient == null && attachment.Message.AccountId != UserId)
                {
                    return new BadRequestObjectResult(new
                        {message = GetText("Message_popup_txt_request_not_found") });
                }

                var directory = _config["Resources:AttachmentPath"];
                string filePath = Path.Combine(directory, attachment.FileName);

                Byte[] fileByteArray;

                using (Stream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    fileByteArray = ReadFully(fileStream);
                }

                var contentType = MimeMapping.MimeUtility.GetMimeMapping(attachment.FileName);

                return File(fileByteArray, contentType, attachment.FileName);
            }
            catch (Exception e)
            {
                _logger.LogError($"error while download attachment | {e.Message}", e);
                return new BadRequestObjectResult(e.Message);
            }
        }

        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }

        private string StripTags(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        private string DecodeHtmlEntities(string text)
        {
            return HttpUtility.HtmlDecode(text);
        }
    }
}