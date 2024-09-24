using System.Security.Cryptography;
using System.Text;

namespace OpenAdm.Domain.Helpers;

public static class Criptografia
{
    private static string _key = string.Empty;
    private static string _iv = string.Empty;

    public static void Configure(string key, string iv)
    {
        ValidKey(key, iv);

        _key = key;
        _iv = iv;
    }

    private static void ValidKey(string key, string iv)
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(iv))
        {
            throw new Exception("As keys para cryptografia são inválidas!");
        }
    }

    public static string Encrypt(string plainText)
    {
        ValidKey(_key, _iv);

        using var aesAlg = Aes.Create();
        aesAlg.Key = GetBytes(_key);
        aesAlg.IV = GetBytes(_iv);

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var encrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(encrypt, encryptor, CryptoStreamMode.Write);
        using var swEncrypt = new StreamWriter(csEncrypt);

        swEncrypt.Write(plainText);
        swEncrypt.Close();
        csEncrypt.Close();

        return Convert.ToBase64String(encrypt.ToArray());
    }

    public static string Decrypt(string cipherText)
    {
        ValidKey(_key, _iv);

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = GetBytes(_key);
        aesAlg.IV = GetBytes(_iv);
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        using var msDecrypt = new MemoryStream(cipherBytes);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);

        return srDecrypt.ReadToEnd();
    }

    private static byte[] GetBytes(string key) => Encoding.UTF8.GetBytes(key);
}
