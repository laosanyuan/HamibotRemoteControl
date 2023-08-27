namespace HamibotRemoteControl.Tools
{
    internal class ResourcesHelper
    {
        public static T GetResource<T>(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (Application.Current.Resources.ContainsKey(key))
                {
                    return (T)Application.Current.Resources[key];
                }

                foreach (var tmp in Application.Current.Resources.MergedDictionaries)
                {
                    if (tmp.Keys.Contains(key))
                    {
                        return (T)tmp[key];
                    }
                }
            }

            return default;
        }
    }
}
