using System.Text.Json;

namespace Courier_lockers.Helper
{
    public class JsonPolicy
    {
        //序列化策略
        public class UpperCaseNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name) =>
                name.ToUpper();
        }
    }
}
