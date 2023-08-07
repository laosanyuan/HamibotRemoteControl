namespace HamibotRemoteControl.Models.DataBase
{
    internal class ApiCallCount
    {
        public string Url { get; set; }

        public DateTime Datetime { get; set; }

        public ApiOperation Operation { get; set; }

        public ApiCallCount()
        {
        }

        public ApiCallCount(string url, ApiOperation operation = ApiOperation.Get)
        {
            this.Url = url;
            this.Operation = operation;
            this.Datetime = DateTime.Now;
        }
    }

    public enum ApiOperation
    {
        Get,
        Put,
        Delete,
        Post,
    }
}
