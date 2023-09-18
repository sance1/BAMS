using System;
using BAMS.Data.Interface;
using BAMS.Data.Models;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace EightElements.Services.Default
{
    public class ChangelogService : IChangelogService
    {
        private IUnitOfWork _uow;
        public ChangelogService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task Log(string table, int sourceId, int editor, string oldValue, string newValue)
        {
            var oldJson = JObject.Parse(oldValue);
            CleanupJson(ref oldJson);
            var newJson = JObject.Parse(newValue);
            CleanupJson(ref newJson);

            //TODO: only save the changed values, instead of all values
            var changelog = new Changelog
            {
                Date = DateTime.Now,
                TableName = table,
                SourceId = sourceId,
                Editor = editor,
                OldValue = oldJson.ToString(),
                NewValue = newJson.ToString()
            };
            await _uow.ChangelogRepository.AddAsync(changelog);
        }

        private void CleanupJson(ref JObject json)
        {
            json.Remove("Uid");
            json.Remove("CreateDate");
            json.Remove("CreatedBy");
            json.Remove("DeleteDate");
            json.Remove("DeletedBy");
        }
    }
}
