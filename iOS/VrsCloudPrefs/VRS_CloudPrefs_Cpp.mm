
#import "VRS_CloudPrefs_Cpp.h"

extern "C"
{
  
void VRS_CloudPrefs_InitializeWithGameObjectName(const char* gameObjectName)
{
  [[VRS_CloudPrefs instance] initializeWithGameObjectName:gameObjectName];
  
}

bool VRS_CloudPrefs_Synchronize(void)
{
  return [[VRS_CloudPrefs instance] synchronize];
  
}

const char* VRS_CloudPrefs_StringForKey(const char* key)
{
  return [[VRS_CloudPrefs instance] stringForKey:key];
}

void VRS_CloudPrefs_SetStringForKey(const char* key, const char* value)
{
  [[VRS_CloudPrefs instance] setString:value forKey:key];
  
}

void VRS_CloudPrefs_RemoveKey(const char* key)
{
  [[VRS_CloudPrefs instance] removeKey:key];
}

const char* VRS_CloudPrefs_AllKeys(void)
{
  return [[VRS_CloudPrefs instance] allKeys];
}
}
