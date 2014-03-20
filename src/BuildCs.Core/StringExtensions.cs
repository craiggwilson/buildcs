
namespace BuildCs
{
    public static class StringExtensions
    {
        public static string F(this string target, params object[] args)
        {
            return string.Format(target, args);
        }
    }
}