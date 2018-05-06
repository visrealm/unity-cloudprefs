using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRS.CloudPrefs
{

  public abstract class KeyValueStore : ScriptableObject, IKeyValueStore
  {
    public VrsCloudPrefs Manager { get; protected set; }

    public abstract bool Initialise(VrsCloudPrefs manager);
    public virtual void OnRemoteDataChanged(string message) { }


    public virtual void SetInt(string key, int value)
    {
      SetString(key, value.ToString());
    }

    public virtual void SetFloat(string key, float value)
    {
      SetString(key, value.ToString());
    }

    public virtual void SetBool(string key, bool value)
    {
      SetString(key, value.ToString());
    }

    public abstract void SetString(string key, string value);

    public virtual int GetInt(string key, int defaultValue = 0)
    {
      string value = GetString(key);
      int result;
      if (!Int32.TryParse(value, out result))
      {
        result = defaultValue;
      }
      return result;
    }

    public virtual float GetFloat(string key, float defaultValue = 0f)
    {
      string value = GetString(key);
      float result;
      if (!Single.TryParse(value, out result))
      {
        result = defaultValue;
      }
      return result;
    }

    public virtual bool GetBool(string key, bool defaultValue = false)
    {
      string value = GetString(key);
      bool result;
      if (!Boolean.TryParse(value, out result))
      {
        result = defaultValue;
      }
      return result;
    }
    public abstract string GetString(string key, string defaultValue = "");

    public abstract IEnumerable<string> Keys { get; }
    public abstract bool HasKey(string key);

    public abstract void DeleteKey(string key);

    public virtual void DeleteAll()
    {
      foreach (string key in Keys)
      {
        DeleteKey(key);
      }
    }

  }

}