using System.Collections.Generic;

namespace VRS.CloudPrefs
{
  public interface IKeyValueStore
  {
    bool Initialise(VrsCloudPrefs manager);
    void OnRemoteDataChanged(string message);

    void SetString(string key, string value);
    void SetInt(string key, int value);
    void SetFloat(string key, float value);
    void SetBool(string key, bool value);

    string GetString(string key, string defaultValue = "");
    int GetInt(string key, int defaultValue = 0);
    float GetFloat(string key, float defaultValue = 0f);
    bool GetBool(string key, bool defaultValue = false);

    IEnumerable<string> Keys { get; }
    bool HasKey(string key);

    void DeleteKey(string key);
    void DeleteAll();
  }
}