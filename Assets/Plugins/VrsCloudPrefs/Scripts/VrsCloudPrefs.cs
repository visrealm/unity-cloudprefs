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
          Debug.LogError("VrsCloudPrefs: Local sotre initialisation failed");
          isOk = false;
        }
        else
        {
          Debug.Log("VrsCloudPrefs: Local store initialised");
        }
      }
      else
      {
        Debug.LogError("VrsCloudPrefs: No local store specified");
        isOk = false;
      }
      
      if (remoteStore)
      {
        if (!remoteStore.Initialise(this))
        {
          Debug.LogError("VrsCloudPrefs: Remote sotre initialisation failed");
          isOk = false;
        }
        else
        {
          Debug.Log("VrsCloudPrefs: Remote store initialised");
        }
      }
      else
      {
        Debug.LogError("VrsCloudPrefs: No remote store specified");
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
      string logMsg = "VrsCloudPrefs: CopyAllRemoteToLocal()";
      foreach (var k in remoteStore.Keys)
      {
        string remoteValue = remoteStore.GetString(k);
        logMsg += "\nSetting local " + k + " to " + remoteValue;
        localStore.SetString(k, remoteValue);
      }
      remoteStore.SetString(_CP_REMOTE_LAST_SYNC_DEVICE_ID, LocalDeviceId);
      HasSynced = true;
      Debug.Log(logMsg);
    }

    public void CopyAllLocalToRemote()
    {
      string logMsg = "VrsCloudPrefs: CopyAllLocalToRemote()";
      foreach (var k in localStore.Keys)
      {
        if (!IsLocalOnly(k))
        {
          string localValue = localStore.GetString(k);
          logMsg += "\nSetting remote " + k + " to " + localValue;
          remoteStore.SetString(k, localValue);
        }
      }
      remoteStore.SetString(_CP_REMOTE_LAST_SYNC_DEVICE_ID, LocalDeviceId);
      HasSynced = true;
      Debug.Log(logMsg);
    }

    public void Synchronise()
    {
      Debug.Log("VrsCloudPrefs: Synchronise()");

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
      Debug.Log("VrsCloudPrefs: OnCloudDataChanged()");
      remoteStore.OnRemoteDataChanged(message);
    }
    
    // Remote data store calls this once its processed the OnCloudDataChanged message
    public void RemoteKeysChanged(string[] keys, ChangeReason reason)
    {
      Debug.Log("VrsCloudPrefs: RemoteKeysChanged()");
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


    private void _SetInt(string key, int value)
    {
      localStore.SetInt(key, value);
      if (automaticSync)
      {
        remoteStore.SetInt(key, value);
      }
    }

    private int _GetInt(string key, int defaultValue)
    {
      if (automaticSync)
      {
        if (remoteStore.HasKey(key))
        {
          int remoteValue = remoteStore.GetInt(key, defaultValue);
          localStore.SetInt(key, remoteValue);
        }
      }

      return localStore.GetInt(key, defaultValue);
    }

    public static void SetInt(string key, int value)
    {
      _inst._SetInt(key, value);
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
      return _inst._GetInt(key, defaultValue);
    }



    private void _SetFloat(string key, float value)
    {
      localStore.SetFloat(key, value);
      if (automaticSync)
      {
        remoteStore.SetFloat(key, value);
      }
    }

    private float _GetFloat(string key, float defaultValue)
    {
      if (automaticSync)
      {
        if (remoteStore.HasKey(key))
        {
          float remoteValue = remoteStore.GetFloat(key, defaultValue);
          localStore.SetFloat(key, remoteValue);
        }
      }

      return localStore.GetFloat(key, defaultValue);
    }

    public static void SetFloat(string key, float value)
    {
      _inst._SetFloat(key, value);
    }

    public static float GetFloat(string key, float defaultValue = 0f)
    {
      return _inst._GetFloat(key, defaultValue);
    }


  }

}