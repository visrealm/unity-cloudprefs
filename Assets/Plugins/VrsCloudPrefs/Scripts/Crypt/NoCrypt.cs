using UnityEngine;
using System.Collections;

namespace VRS.CloudPrefs
{

  [CreateAssetMenu(fileName = "NoCrypt", menuName = "VRS/CloudPrefs/Crypt/NoCrypt", order = 0)]
  public class NoCrypt : Crypt
  {
    public override bool Decrypt(string value, out string result)
    {
      result = value;
      return true;
    }

    public override bool Encrypt(string value, out string result)
    {
      result = value;
      return true;
    }
  }

}