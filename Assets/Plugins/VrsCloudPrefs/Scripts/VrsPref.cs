using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace VRS.CloudPrefs {

  public class VrsPref : ScriptableObject
  {
    public enum SyncMode
    {
      LocalOnly,
      SyncRemote
    }

    [SerializeField]
    public string Key { get; private set; }

    [SerializeField]
    public SyncMode Mode { get; private set; }

    [SerializeField]
    public string DefaultValue { get; private set; }

    [SerializeField]
    public bool StorePerDeviceValue { get; private set; }

    public string Value
    {
      get { return VrsCloudPrefs.GetString(VrsCloudPrefs.ValueStore.LocalBeforeRemote, DefaultValue);  }
      set { VrsCloudPrefs.SetString(VrsCloudPrefs.ValueStore.LocalBeforeRemote, Key, value); }
    }

    public UnityEvent OnRemoteValueChanged;

    private void Init(string key, string defaultValue, SyncMode mode)
    {
      Key = key;
      DefaultValue = defaultValue;
      Mode = mode;
    }

    public static VrsPref Create(string key, string defaultValue, SyncMode mode)
    {
      VrsPref inst = ScriptableObject.CreateInstance<VrsPref>();
      inst.Init(key, defaultValue, mode);
      return inst;
    }
  }

}