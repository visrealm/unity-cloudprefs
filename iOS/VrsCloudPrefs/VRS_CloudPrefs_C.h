

#import "VRS_CloudPrefs.h"


extern void _VRS_CloudPrefs_InitializeWithGameObjectName(NSString* gameObjectName);
extern bool _VRS_CloudPrefs_Synchronize(void);

extern NSString* _VRS_CloudPrefs_StringForKey(NSString* key);
extern void _VRS_CloudPrefs_SetStringForKey(NSString* key,
                                            NSString* value);
extern void _VRS_CloudPrefs_RemoveKey(NSString* key);
extern NSString* _VRS_CloudPrefs_AllKeys(void);


