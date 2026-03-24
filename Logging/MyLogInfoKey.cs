using NuciLog.Core;

namespace DynamicDnsUpdater.Client.Logging
{
    public sealed class MyLogInfoKey : LogInfoKey
    {
        MyLogInfoKey(string name) : base(name) { }

        public static LogInfoKey DomainName => new MyLogInfoKey(nameof(DomainName));

        public static LogInfoKey IpAddress => new MyLogInfoKey(nameof(IpAddress));

        public static LogInfoKey Provider => new MyLogInfoKey(nameof(Provider));

        public static LogInfoKey ApiBaseUrl => new MyLogInfoKey(nameof(ApiBaseUrl));

        public static LogInfoKey ResponseCode => new MyLogInfoKey(nameof(ResponseCode));

        public static LogInfoKey ResponseMessage => new MyLogInfoKey(nameof(ResponseMessage));
    }
}
