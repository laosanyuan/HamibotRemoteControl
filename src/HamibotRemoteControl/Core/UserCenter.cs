namespace HamibotRemoteControl.Core
{
    class UserCenter
    {
        private static UserCenter _instance = new UserCenter();

        public static UserCenter Instance => _instance;



        public string Token { get; private set; }


        private UserCenter()
        {
        }
    }
}
