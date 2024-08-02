using System.Security.Cryptography;
using System.Text;

namespace ShopBackgroundSystem.Helpers
{
    public class MD5Helper
    {
        public static string GetMD5(string input)
        {
            StringBuilder sb = new StringBuilder();
            MD5 md5 = MD5.Create();
            //将输入字符串转换成字节数组
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            //MD5加密
            byte[] md5uffer = md5.ComputeHash(buffer);
            //把每一个单独的加密字节转换成字符串
            foreach (byte item in md5uffer)
            {
                //最终以16进制的形式表示数据
                //是每个字符占两个字节来表示
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
