namespace HamibotRemoteControl.Common
{
    /// <summary>
    /// 自然排序比较器
    /// </summary>
    public class NaturalSortComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            int i = 0, j = 0;

            while (i < x.Length && j < y.Length)
            {
                if (char.IsDigit(x[i]) && char.IsDigit(y[j]))
                {
                    int numX = 0, numY = 0;

                    while (i < x.Length && char.IsDigit(x[i]))
                    {
                        numX = numX * 10 + (x[i] - '0');
                        i++;
                    }

                    while (j < y.Length && char.IsDigit(y[j]))
                    {
                        numY = numY * 10 + (y[j] - '0');
                        j++;
                    }

                    if (numX != numY)
                        return numX.CompareTo(numY);
                }
                else
                {
                    if (x[i] != y[j])
                        return x[i].CompareTo(y[j]);

                    i++;
                    j++;
                }
            }

            return x.Length - y.Length;
        }
    }
}
