using System.Threading.Tasks;

using NuciAPI.Client;

using DynamicDnsUpdater.Client.Requests;
using NuciWeb.HTTP;
using NuciAPI.Responses;
using System.Net.Http;
using Microsoft.Extensions.Options;
using DynamicDnsUpdater.Client.Configuration;
using System;

namespace DynamicDnsUpdater.Client
{
    public sealed class DnsRecordService(
        INuciApiClient apiClient,
        IOptions<ApiSettings> apiSettings)
        : IDnsRecordService
    {
        public async Task Update(string domainName, string providerName)
        {
            NuciApiRequestAuthorisationInfo authorisationInfo = new()
            {
                BearerToken = apiSettings.Value.ApiKey,
                ClientId = $"DynamicDnsUpdater.Client.{Environment.MachineName}"
            };
            PutDnsRecordRequest request = new()
            {
                IpAddress = NetworkUtils.GetPublicIpAddress(),
                DnsProvider = providerName
            };

            NuciApiResponse response = apiClient.SendRequestAsync<PutDnsRecordRequest, NuciApiSuccessResponse>(
                HttpMethod.Put,
                request,
                authorisationInfo,
                $"DnsRecords/{domainName}").Result;
        }
    }
}
