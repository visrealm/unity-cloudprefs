using UnityEngine;
using System.Collections;

namespace VRS.CloudPrefs
{
  public interface ICrypt
  {
    bool Encrypt(string value, out string result);
    bool Decrypt(string value, out string result);
  }

}
