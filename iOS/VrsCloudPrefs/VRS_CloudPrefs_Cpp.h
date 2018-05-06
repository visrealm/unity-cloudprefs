

#import "VRS_CloudPrefs.h"

extern "C"
{

void VRS_CloudPrefs_InitializeWithGameObjectName(const char* gameObjectName);
bool VRS_CloudPrefs_Synchronize(void);

const char* VRS_CloudPrefs_StringForKey(const char* key);
void VRS_CloudPrefs_SetStringForKey(const char* key,
                                    const char* value);
void VRS_CloudPrefs_RemoveKey(const char* key);
const char* VRS_CloudPrefs_AllKeys(void);


}
