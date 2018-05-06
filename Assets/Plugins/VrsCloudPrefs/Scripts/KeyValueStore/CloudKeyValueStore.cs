using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRS.CloudPrefs
{

  [CreateAssetMenu(fileName = "CloudKeyValueStore", menuName = "VRS/CloudPrefs/CloudKeyValueStore", order = 1)]
  public class CloudKeyValueStore : KeyValueStore
  {
    private KeyValueStore concreteStore;

    public override bool Initialise(VrsCloudPrefs manager)
    {
      Manager = manager;

      Debug.Log("VrsCloudPrefs: CloudKeyValueStore.Initialise()");

#if UNITY_EDITOR
      Debug.Log("VrsCloudPrefs: CloudKeyValueStore Initialising Editor Store");

      var nativeStore = ScriptableObject.CreateInstance<LocalKeyValueStore>();
      nativeStore.KeyPrefix = "REMOTE.";
      concreteStore = nativeStore;
#elif UNITY_IOS
      Debug.Log("VrsCloudPrefs: CloudKeyValueStore Initialising IOS Store");

      concreteStore = ScriptableObject.CreateInstance<CloudKeyValueStoreIOS>();
#elif UNITY_ANDROID
      Debug.Log("VrsCloudPrefs: CloudKeyValueStore Initialising Android Store");

      var nativeStore = ScriptableObject.CreateInstance<LocalKeyValueStore>();
      nativeStore.KeyPrefix = "REMOTE.";
      concreteStore = nativeStore;
#endif

      return concreteStore.Initialise(manager);
    }

    public override void OnRemoteDataChanged(string message)
    {
      concreteStore.OnRemoteDataChanged(message);
    }

    public override IEnumerable<string> Keys
    {
      get { return concreteStore.Keys; }
    }

    public override void DeleteAll()
    {
      concreteStore.DeleteAll();
    }

    public override void DeleteKey(string key)
    {
      concreteStore.DeleteKey(key);
    }

    public override bool GetBool(string key, bool defaultValue = false)
    {
      return concreteStore.GetBool(key, defaultValue);
    }

    public override float GetFloat(string key, float defaultValue = 0)
    {
      return concreteStore.GetFloat(key, defaultValue);
    }

    public override int GetInt(string key, int defaultValue = 0)
    {
      return concreteStore.GetInt(key, defaultValue);
    }

    public override string GetString(string key, string defaultValue = "")
    {
      return concreteStore.GetString(key, defaultValue);
    }

    public override bool HasKey(string key)
    {
      return concreteStore.HasKey(key);
    }

    public override void SetBool(string key, bool value)
    {
      concreteStore.SetBool(key, value);
    }

    public override void SetFloat(string key, float value)
    {
      concreteStore.SetFloat(key, value);
    }

    public override void SetInt(string key, int value)
    {
      concreteStore.SetInt(key, value);
    }

    public override void SetString(string key, string value)
    {
      concreteStore.SetString(key, value);
    }
  }

}