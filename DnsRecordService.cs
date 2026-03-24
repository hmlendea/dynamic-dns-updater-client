using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using NuciAPI.Client;
using NuciAPI.Responses;
using NuciLog.Core;
using NuciWeb.HTTP;

using DynamicDnsUpdater.Client.Configuration;
using DynamicDnsUpdater.Client.Requests;
using DynamicDnsUpdater.Client.Logging;
using System.Linq;

namespace DynamicDnsUpdater.Client
{
    public sealed class DnsRecordService(
        INuciApiClient apiClient,
        IOptions<ApiSettings> apiSettings,
        ILogger logger)
        : IDnsRecordService
    {
        public async Task Update(string domainName, string providerName)
        {
            IEnumerable<LogInfo> logInfos =
            [
                new(MyLogInfoKey.DomainName, domainName),
                new(MyLogInfoKey.Provider, providerName),
                new(MyLogInfoKey.ApiBaseUrl, apiSettings.Value.BaseUrl)
            ];

            logger.Info(
                MyOperation.UpdateDnsRecord,
                OperationStatus.Started,
                logInfos);

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

            logInfos = logInfos.Append(new(MyLogInfoKey.IpAddress, request.IpAddress));

            try
            {
                NuciApiResponse response =
                    await apiClient.SendRequestAsync<PutDnsRecordRequest, NuciApiSuccessResponse>(
                        HttpMethod.Put,
                        request,
                        authorisationInfo,
                        $"DnsRecords/{domainName}");

                if (!response.IsSuccessful)
                {
                    logInfos = logInfos
                        .Append(new(MyLogInfoKey.ResponseCode, response.Code))
                        .Append(new(MyLogInfoKey.ResponseMessage, response.Message));

                    throw new HttpRequestException(
                        $"The Dynamic DNS Updater API request has failed.");
                }

                logger.Info(
                    MyOperation.UpdateDnsRecord,
                    OperationStatus.Success,
                    logInfos);
            }
            catch (Exception exception)
            {
                logger.Error(
                    MyOperation.UpdateDnsRecord,
                    OperationStatus.Failure,
                    exception,
                    logInfos);
            }
        }
    }
}
