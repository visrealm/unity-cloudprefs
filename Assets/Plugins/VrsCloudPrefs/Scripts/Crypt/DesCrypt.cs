using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;

namespace VRS.CloudPrefs
{

  [CreateAssetMenu(fileName = "DesCrypt", menuName = "VRS/CloudPrefs/Crypt/DesCrypt", order = 0)]
  public class DesCrypt : Crypt
  {
    public string password = "IamZETOwow!123";
    public int keyIterations = 555;

    public override bool Decrypt(string value, out string result)
    {
      result = "";
      try
      {
        byte[] cipherBytes = Convert.FromBase64String(value);

        using (var memoryStream = new MemoryStream(cipherBytes))
        {
          DESCryptoServiceProvider des = new DESCryptoServiceProvider();

          using (var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(_Key(), _GetIV()), CryptoStreamMode.Read))
          using (var streamReader = new StreamReader(cryptoStream))
          {
            result = streamReader.ReadToEnd();
          }
        }
      }
      catch (Exception e)
      {
        Debug.LogWarning("Decrypt Exception: " + e);
        return false;
      }
      return true;
    }

    public override bool Encrypt(string value, out string result)
    {
      result = "";
      try
      {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        using (var memoryStream = new MemoryStream())
        using (var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(_Key(), _GetIV()), CryptoStreamMode.Write))
        {
          byte[] plainTextBytes = Encoding.UTF8.GetBytes(value);

          cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
          cryptoStream.FlushFinalBlock();

          result = Convert.ToBase64String(memoryStream.ToArray());
        }
      }
      catch (Exception e)
      {
        Debug.LogWarning("Encrypt Exception: " + e);
        return false;
      }
      return true;
    }

    byte[] _key = null;
    byte[] _Key()
    {
      if (_key == null || _key.Length == 0)
      {
        Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, _GetIV(), keyIterations);
        _key = rfc2898DeriveBytes.GetBytes(8);
      }
      return _key;
    }

    byte[] _GetIV()
    {
      return Encoding.UTF8.GetBytes(Application.identifier);
    }

  }

}