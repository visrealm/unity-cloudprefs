﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRS.CloudPrefs
{

  public class VrsCloudTester : MonoBehaviour
  {
    [Serializable]
    public struct KeyValuePair
    {
      public string key;
      public string value;
    }

    public KeyValuePair[] initial;

    // Use this for initialization
    void Start()
    {
      List<KeyValuePair> newValues = new List<KeyValuePair>();
      foreach (KeyValuePair kvp in initial)
      {
        KeyValuePair copy = kvp;
        if (VrsCloudPrefs.HasKey(kvp.key))
        {
          copy.value = VrsCloudPrefs.GetString(VrsCloudPrefs.ValueStore.LocalBeforeRemote, kvp.key);
          VrsCloudPrefs.SetString(VrsCloudPrefs.ValueStore.LocalBeforeRemote, kvp.key, copy.value);
        }
        else
        {
          VrsCloudPrefs.SetString(VrsCloudPrefs.ValueStore.LocalBeforeRemote, kvp.key, kvp.value);
        }
        newValues.Add(copy);
      }
      initial = newValues.ToArray();
    }


  }

}