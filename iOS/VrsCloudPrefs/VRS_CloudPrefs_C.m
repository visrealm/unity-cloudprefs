
#import "VRS_CloudPrefs_C.h"
#import "UnityString.m"


extern void _VRS_CloudPrefs_InitializeWithGameObjectName(NSString* gameObjectName)
{
  [[VRS_CloudPrefs instance] initializeWithGameObjectName:UnityStringFromNSString(gameObjectName)];
  
}

extern bool _VRS_CloudPrefs_Synchronize(void)
{
  return [[VRS_CloudPrefs instance] synchronize];
  
}

extern NSString* _VRS_CloudPrefs_StringForKey(NSString* key)
{
  return NSStringFromUnityString([[VRS_CloudPrefs instance] stringForKey:UnityStringFromNSString(key)]);
}

extern void _VRS_CloudPrefs_SetStringForKey(NSString* key, NSString* value)
{
  [[VRS_CloudPrefs instance] setString:UnityStringFromNSString(value) forKey:UnityStringFromNSString(key)];
  
}

extern void _VRS_CloudPrefs_RemoveKey(NSString* key)
{
  [[VRS_CloudPrefs instance] removeKey:UnityStringFromNSString(key)];
}

extern NSString* _VRS_CloudPrefs_AllKeys(void)
{
  return NSStringFromUnityString([[VRS_CloudPrefs instance] allKeys]);
}
