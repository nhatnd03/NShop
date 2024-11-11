using System.Text;

namespace app1.Helper
{
    public class RandomKeyUtil
    {
        public static string GenerateRamdomKey(int length = 5)
        {
            var pattern = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZCXVBNM!@#$%^&*().,/';}{][\|_+-=";
            var sb = new StringBuilder();
            var rd = new Random(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0,pattern.Length)]);
            }
            return sb.ToString();
        }
    }
}
