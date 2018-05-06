using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRS.CloudPrefs
{
  [CreateAssetMenu(fileName = "CryptKeyValueStore", menuName = "VRS/CloudPrefs/CryptKeyValueStore", order = 1)]
  public class CryptKeyValueStoreDecorator : KeyValueStore
  {
    public KeyValueStore valueStore;
    public Crypt cryptMethod;
    public bool saltValueWithKey = true;

		public override bool Initialise(VrsCloudPrefs manager)
		{
      return true;
		}

		public override IEnumerable<string> Keys
    {
      get { return valueStore.Keys; }
    }

    public override void DeleteAll()
    {
      valueStore.DeleteAll();
    }

    public override void DeleteKey(string key)
    {
      valueStore.DeleteKey(key);
    }

    public override bool HasKey(string key)
    {
      return valueStore.HasKey(key);
    }

    public override string GetString(string key, string defaultValue = "")
    {
      string encrypted = valueStore.GetString(key, defaultValue);
      if (encrypted.Length == 0)
        return defaultValue;

      string decrypted;
      if (!cryptMethod.Decrypt(encrypted, out decrypted))
      {
        decrypted = defaultValue;
      }
      else if (saltValueWithKey)
      {
        if (decrypted.StartsWith(key))
        {
          decrypted = decrypted.Substring(key.Length);
        }
        else
        {
          Debug.LogWarning("Retreived value failed validity check. Returning default.");
          decrypted = defaultValue;
        }
      }
      return decrypted;
    }


    public override void SetString(string key, string value)
    {
      string encrypted;
      if (cryptMethod.Encrypt(saltValueWithKey ? key + value : value, out encrypted))
      {
        valueStore.SetString(key, encrypted);
      }
      else
      { 
        Debug.LogError("Failed to encrypt value");
      }
    }
  }
}