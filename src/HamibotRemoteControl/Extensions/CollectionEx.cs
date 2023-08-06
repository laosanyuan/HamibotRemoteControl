using System.Collections;

namespace HamibotRemoteControl.Extensions
{
    internal static class CollectionEx
    {
        /// <summary>
        /// 集合是否为空
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsEmpty(this ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }
    }
}
