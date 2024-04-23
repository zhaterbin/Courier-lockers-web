namespace Courier_lockers.Helper
{
    public interface IWPFHttpClient
    {
        public Task<(bool, string)> GetData(string action, dynamic param);

        public Task<(bool, string)> PostData(string action, dynamic param);
    }
}
