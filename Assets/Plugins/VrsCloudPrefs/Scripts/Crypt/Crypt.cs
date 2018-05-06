using UnityEngine;
using System.Collections;

namespace VRS.CloudPrefs
{
  public abstract class Crypt : ScriptableObject, ICrypt
  {
    public abstract bool Decrypt(string value, out string result);
    public abstract bool Encrypt(string value, out string result);
  }
}