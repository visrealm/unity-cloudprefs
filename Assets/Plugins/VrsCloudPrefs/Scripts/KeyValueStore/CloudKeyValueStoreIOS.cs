using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VRS.CloudPrefs
{

  public class CloudKeyValueStoreIOS : KeyValueStore
  {
    [DllImport("__Internal")]
    private static extern void VRS_CloudPrefs_InitializeWithGameObjectName(string gameObjectName);

    [DllImport("__Internal")]
    private static extern bool VRS_CloudPrefs_Synchronize();

    [DllImport("__Internal")]
    private static extern string VRS_CloudPrefs_StringForKey(string key, string defaultValue);

    [DllImport("__Internal")]
    private static extern void VRS_CloudPrefs_SetStringForKey(string key, string value);

    [DllImport("__Internal")]
    private static extern void VRS_CloudPrefs_RemoveKey(string key);

    [DllImport("__Internal")]
    private static extern string VRS_CloudPrefs_AllKeys();

    public class UserInfo
    {
      public enum NSUbiquitousKeyValueStoreChangeReason
      {
        NSUbiquitousKeyValueStoreServerChange,
        NSUbiquitousKeyValueStoreInitialSyncChange,
        NSUbiquitousKeyValueStoreQuotaViolationChange,
        NSUbiquitousKeyValueStoreAccountChange
      }

      public NSUbiquitousKeyValueStoreChangeReason NSUbiquitousKeyValueStoreChangeReasonKey;
      public string[] NSUbiquitousKeyValueStoreChangedKeysKey;
    }


    public override bool Initialise(VrsCloudPrefs manager)
    {
      Manager = manager;

      Debug.Log("VrsCloudPrefs: CloudKeyValueStoreIOS.Initialise()");

      VRS_CloudPrefs_InitializeWithGameObjectName(manager.gameObject.name);
      VRS_CloudPrefs_Synchronize();
      return true;
    }

    public override IEnumerable<string> Keys
    {
      get
      {
        string keysValue = VRS_CloudPrefs_AllKeys();
        foreach (string k in keysValue.Split(';'))
        {
          yield return k;
        }
      }
    }

    public override void OnRemoteDataChanged(string message)
    {
      // Parse JSON.
      UserInfo userInfo = new UserInfo();
      JsonUtility.FromJsonOverwrite(message, userInfo);

      // Get notification payload.
      var changeReason = (VrsCloudPrefs.ChangeReason)userInfo.NSUbiquitousKeyValueStoreChangeReasonKey;
      string[] changedKeys = userInfo.NSUbiquitousKeyValueStoreChangedKeysKey;

      // Callback
      Manager.RemoteKeysChanged(changedKeys, changeReason);

    }

    public override bool HasKey(string key)
    {
      const string NULL_VALUE = "<NULL>";
      return VRS_CloudPrefs_StringForKey(key, NULL_VALUE) != NULL_VALUE;
    }

    public override void DeleteKey(string key)
    {
      VRS_CloudPrefs_RemoveKey(key);
    }

    public override string GetString(string key, string defaultValue = "")
    {
      return VRS_CloudPrefs_StringForKey(key, defaultValue);
    }

    public override void SetString(string key, string value)
    {
      VRS_CloudPrefs_SetStringForKey(key, value);
    }
  }

}