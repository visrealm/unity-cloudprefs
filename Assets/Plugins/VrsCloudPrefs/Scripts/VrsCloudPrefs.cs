using UnityEngine;
using System.Collections;

namespace VRS.CloudPrefs
{
  public class VrsCloudPrefs : MonoBehaviour
  {
    public enum ValueStore
    {
      LocalOnly,
      LocalBeforeRemote,
      RemoteOnly,
      RemoteBeforeLocal
    }

    public enum ChangeReason
    {
      ServerChange,
      InitialSyncChange,
      QuotaViolationChange,
      AccountChange
    }

    public KeyValueStore localStore;
    public KeyValueStore remoteStore;
    public bool automaticSync;

    public string LocalDeviceId { get { return SystemInfo.deviceUniqueIdentifier; } }
    public string UserAccountId
    {
      get
      {
        string remoteValue = remoteStore.GetString(_CP_USER_ACCOUNT_ID, "");
        if (remoteValue == "")
        {
          remoteValue = LocalDeviceId;
          remoteStore.SetString(_CP_USER_ACCOUNT_ID, remoteValue);
        }
        localStore.SetString(_CP_USER_ACCOUNT_ID, remoteValue);
        return remoteValue;
      }
    }

    public bool HasSynced
    {
      get { return localStore.GetBool(_CP_LOCAL_HAS_SYNCED, false); }
      private set { localStore.SetBool(_CP_LOCAL_HAS_SYNCED, value); }
    }

    public bool HasRemoteData
    {
      get { return remoteStore.GetString(_CP_USER_ACCOUNT_ID, "") != "";  }
    }

    private static VrsCloudPrefs _inst;

    // Local-only keys
    private readonly string _CP_LOCAL_HAS_SYNCED = "_VRS_CP_LHS";

    // Remote keys
    private readonly string _CP_REMOTE_LAST_SYNC_DEVICE_ID = "_VRS_CP_LSDID";
    private readonly string _CP_USER_ACCOUNT_ID = "_VRS_CP_UAID";
    
    // Use this for initialization
    void Awake()
    {
      if (_inst != null && _inst != this)
      {
        GameObject.DestroyImmediate(gameObject);
        return;
      }

      bool isOk = true;

      if (localStore)
      {
        if (!localStore.Initialise(this))
        {
          Debug.LogError("Local sotre initialisation failed");
          isOk = false;
        }
      }
      else
      {
        Debug.LogError("No local store specified");
        isOk = false;
      }
      
      if (remoteStore)
      {
        if (!remoteStore.Initialise(this))
        {
          Debug.LogError("Remote sotre initialisation failed");
          isOk = false;
        }
      }
      else
      {
        Debug.LogError("No remote store specified");
        isOk = false;
      }

      if (isOk)
      {
        DontDestroyOnLoad(gameObject);
        _inst = this;
      }
    }

    public bool IsLocalOnly(string key)
    {
      return key == _CP_LOCAL_HAS_SYNCED;
    }

    public void CopyAllRemoteToLocal()
    {
      foreach (var k in remoteStore.Keys)
      {
        localStore.SetString(k, remoteStore.GetString(k));
      }
      remoteStore.SetString(_CP_REMOTE_LAST_SYNC_DEVICE_ID, LocalDeviceId);
      HasSynced = true;
    }

    public void CopyAllLocalToRemote()
    {
      foreach (var k in localStore.Keys)
      {
        if (!IsLocalOnly(k))
        {
          remoteStore.SetString(k, localStore.GetString(k));
        }
      }
      remoteStore.SetString(_CP_REMOTE_LAST_SYNC_DEVICE_ID, LocalDeviceId);
      HasSynced = true;
    }

    public void Synchronise()
    {
      if (remoteStore.GetString(_CP_REMOTE_LAST_SYNC_DEVICE_ID, LocalDeviceId) == LocalDeviceId)
      {
        // last sync device was this one
        if (HasSynced)
        {
          // this device has synced before. just set all remote keys
          CopyAllLocalToRemote();
        }
        else
        {
          CopyAllRemoteToLocal();
        }
      }
      else
      {
        CopyAllRemoteToLocal();
      }
    }

    // native OS plugin calls this when remote data changes
    // pass it on the the remote store for processing
    public void OnCloudDataChanged(string message)
    {
      remoteStore.OnRemoteDataChanged(message);
    }
    
    // Remote data store calls this once its processed the OnCloudDataChanged message
    public void RemoteKeysChanged(string[] keys, ChangeReason reason)
    {
      if (automaticSync)
      {
        foreach (string key in keys)
        {
          localStore.SetString(key, remoteStore.GetString(key, localStore.GetString(key)));
        }
      }
    }

    public static bool HasKey(string key)
    {
      return _inst.localStore.HasKey(key);
    }

    private void _SetString(string key, string value)
    {
      localStore.SetString(key, value);
      if (automaticSync)
      {
        remoteStore.SetString(key, value);
      }
    }

    private string _GetString(string key, string defaultValue = "")
    {
      if (automaticSync)
      {
        if (remoteStore.HasKey(key))
        {
          string remoteValue = remoteStore.GetString(key, defaultValue);
          localStore.SetString(key, remoteValue);
        }
      }

      return localStore.GetString(key, defaultValue);
    }

    public static void SetString(string key, string value)
    {
      _inst._SetString(key, value);
    }

    public static string GetString(string key, string defaultValue = "")
    {
      return _inst._GetString(key, defaultValue);
    }

  }

}