

#import "VRS_CloudPrefs.h"
#import "UnityString.m"


VRS_CloudPrefs* _instance = nil;
const char* VRS_CloudPrefs_CloudDidChange = "OnCloudDataChanged"; // Pass `userInfo` dictionary as JSON string.


@interface VRS_CloudPrefs ()
@property (nonatomic, strong) NSString *gameObjectName;
@property (nonatomic, weak) id<VRS_CloudPrefs_Delegate> delegate;
@property (nonatomic, weak) NSUbiquitousKeyValueStore *keyValueStore;
@end


@implementation VRS_CloudPrefs


#pragma mark - Singleton

+(VRS_CloudPrefs*)instance
{
    if (_instance == nil) { _instance = [self new]; }
    return _instance;
}


#pragma mark - Features

+(void)setDelegate:(id<VRS_CloudPrefs_Delegate>) delegate;
{ [self instance].delegate = delegate; }

-(void)initializeWithGameObjectName:(const char*) gameObjectName;
{
    self.gameObjectName = NSStringFromUnityString(gameObjectName);
    // NSLog(@"[VRS_CloudPrefs initializeWithGameObjectName:`%@`]", self.gameObjectName);
    
    // Cloud.
    self.keyValueStore = [NSUbiquitousKeyValueStore defaultStore];
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(cloudDidChange:)
                                                 name:NSUbiquitousKeyValueStoreDidChangeExternallyNotification
                                               object:self.keyValueStore];
}

-(BOOL)synchronize
{
    // NSLog(@"[VRS_CloudPrefs synchronize]");
    BOOL synchronized = [self.keyValueStore synchronize];
    // NSLog(@"synchronized:%@", (synchronized) ? @"YES" : @"NO");
    return synchronized;
}

-(void)cloudDidChange:(NSNotification*) notification
{
    // NSLog(@"[VRS_CloudPrefs cloudDidChange:]");
    
    NSError *error;
    NSData *userInfoJSONData = [NSJSONSerialization dataWithJSONObject:notification.userInfo
                                                               options:0
                                                                 error:&error];
    NSString *userInfoJSON = [[NSString alloc] initWithData:userInfoJSONData encoding:NSUTF8StringEncoding];
    
    // To Unity.
    UnitySendMessage(UnityStringFromNSString(self.gameObjectName), VRS_CloudPrefs_CloudDidChange, UnityStringFromNSString(userInfoJSON));
    
    /*
    NSLog(@"[VRS_CloudPrefs UnitySendMessage(%@, %@, %@)]",
          self.gameObjectName,
          NSStringFromUnityString(VRS_CloudPrefs_CloudDidChange),
          userInfoJSON);
    */
    
    // To Sandbox app if any.
    if (self.delegate != nil) [self.delegate cloudDidChange:userInfoJSON];
}


#pragma mark - Key-value store

-(const char*)stringForKey:(const char*) key
{
    // NSLog(@"[VRS_CloudPrefs stringForKey:`%@`]", NSStringFromUnityString(key));
    return UnityStringFromNSString([self.keyValueStore stringForKey:NSStringFromUnityString(key)]);
}

-(void)setString:(const char*) value forKey:(const char*) key
{
    // NSLog(@"[VRS_CloudPrefs setString:%@ forKey:`%@`]", NSStringFromUnityString(value), NSStringFromUnityString(key));
    [self.keyValueStore setString:NSStringFromUnityString(value) forKey:NSStringFromUnityString(key)];
}

-(void)removeKey:(const char*) key
{
  // NSLog(@"[VRS_CloudPrefs removeKey:`%@`]", NSStringFromUnityString(key));
  [self.keyValueStore removeObjectForKey:NSStringFromUnityString(key)];
}

-(const char *)allKeys
{
  NSArray* keys = [[self.keyValueStore dictionaryRepresentation] allKeys];
  
  // NSLog(@"[VRS_CloudPrefs allKeys]");
  return UnityStringFromNSString([keys componentsJoinedByString:@";"]);
}

@end
