using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VRS.CloudPrefs
{

  [CreateAssetMenu(fileName = "LocalKeyValueStore", menuName = "VRS/CloudPrefs/LocalKeyValueStore", order = 1)]
  public class LocalKeyValueStore : KeyValueStore
  {
    private readonly string KEYS_KEY = ".keys";
    private readonly char KEYS_SEP = ';';

    private HashSet<string> _keys =  new HashSet<string>();

    public string KeyPrefix = "";

    public override IEnumerable<string> Keys
    {
      get
      {
        _ReadKeys();

        foreach (string k in _keys)
        {
          yield return k;
        }
      }
    }

    public override void DeleteKey(string key)
    {
      PlayerPrefs.DeleteKey(key);
    }

    public override string GetString(string key, string defaultValue = "")
    {
      return PlayerPrefs.GetString(KeyPrefix + key, defaultValue);
    }

    public override bool HasKey(string key)
    {
      return PlayerPrefs.HasKey(key);
    }

    public override void SetString(string key, string value)
    {
      _RegisterKey(key);

      PlayerPrefs.SetString(KeyPrefix + key, value);
    }

    private void _RegisterKey(string key)
    {
      _ReadKeys();
      _keys.Add(key);
      _WriteKeys();
    }

    private void _ReadKeys()
    {
      if (_keys.Count == 0)
      {
        _keys = new HashSet<string>(GetString(KEYS_KEY).Split(KEYS_SEP));
      }
    }

    private void _WriteKeys()
    {
      PlayerPrefs.SetString(KeyPrefix + KEYS_KEY, string.Join(KEYS_SEP.ToString(), _keys.ToArray()));
    }
  }

}