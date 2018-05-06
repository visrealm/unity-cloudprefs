
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>


extern void UnitySendMessage(const char* gameObjectName, const char* methodName, const char* message);


@protocol VRS_CloudPrefs_Delegate
-(void)cloudDidChange:(NSString*) userInfoJSON;
@end


@interface VRS_CloudPrefs : NSObject

+(VRS_CloudPrefs*)instance;

-(void)initializeWithGameObjectName:(const char*) gameObjectName;
-(BOOL)synchronize;

-(const char*)stringForKey:(const char*) key;
-(void)setString:(const char*) value forKey:(const char*) key;

-(void)removeKey:(const char*) key;
-(const char*)allKeys;

+(void)setDelegate:(id<VRS_CloudPrefs_Delegate>) delegate;


@end
