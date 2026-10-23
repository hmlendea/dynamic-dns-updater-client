using System.Threading.Tasks;

namespace DynamicDnsUpdater.Client
{
    public interface IDnsRecordService
    {
        Task Update(string domainName, string providerName);
    }
}
