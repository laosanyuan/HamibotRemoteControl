namespace HamibotRemoteControl.Interfaces
{
    /// <summary>
    /// ViewModel接受数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IViewModelReceiveData<in T>
    {
        void ReceiveData(T data);
    }
}
