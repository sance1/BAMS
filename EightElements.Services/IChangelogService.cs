using System.Threading.Tasks;

namespace EightElements.Services
{
    public interface IChangelogService
    {
        Task Log(
            string table, int sourceId, int editor, 
            string oldValue, string newValue);
    }
}
