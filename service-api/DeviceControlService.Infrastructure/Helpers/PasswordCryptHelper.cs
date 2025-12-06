using System.Security.Cryptography;
using System.Text;

namespace DeviceControlService.Infrastructure.Helpers;

public sealed class PasswordCryptHelper
{
    public static string GeneratePassword(string login, string password, string realm, string realmChallenge)
    {
        string md5hash = CreateMD5($"{login}:{realm}:{password}");
        using SHA256 sha256 = SHA256.Create();

        return ConvertToHesString(sha256.ComputeHash(Encoding.ASCII.GetBytes($"{realmChallenge}{md5hash}")));
    }

    private static string CreateMD5(string input)
    {
        using MD5 md5 = MD5.Create();

        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        return ConvertToHesString(hashBytes);
    }

    private static string ConvertToHesString(byte[] bytes)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString("X2").ToLower());
        }

        return sb.ToString();
    }
}

