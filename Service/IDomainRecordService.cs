using System.Threading.Tasks;

namespace DynamicDnsUpdater.Client.Service
{
    public interface IDomainRecordService
    {
        Task Update(string domainName, string providerName);
    }
}
