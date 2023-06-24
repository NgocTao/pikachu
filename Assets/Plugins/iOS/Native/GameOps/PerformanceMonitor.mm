#import <Foundation/Foundation.h>
#import <FirebasePerformance/FirebasePerformance.h>

@interface PerformaceMonitor : NSObject
@property (nonatomic, strong) NSMutableDictionary* activeTraces;
+ (instancetype)sharedManager;
- (void) startTraceWithName: (NSString*) name;
- (void) stopTraceWithName: (NSString*) name;
- (void) setTrace:(NSString*)name withValue:(NSString*)value forAttribute:(NSString*)attribute;
@end

@implementation PerformaceMonitor
{
    dispatch_queue_t _lockQueue;
}
#pragma mark Singelton
      
+ (instancetype)sharedManager {
    static PerformaceMonitor *sharedManager;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedManager = [[PerformaceMonitor alloc] init];
    });
    return sharedManager;
}

#pragma mark Initialization
      
- (id)init
{
    self = [super init];
    if (self) {
        _activeTraces = [[NSMutableDictionary alloc] init];
        _lockQueue = dispatch_queue_create("PerformaceMonitor lock queue", DISPATCH_QUEUE_SERIAL);
    }
    return self;
}

- (void)setTrace:(id)object forKey:(NSString *)key {
  dispatch_async(_lockQueue, ^{
    self->_activeTraces[key] = object;
  });
}

- (id)traceForKey:(NSString *)key {
  __block id object;
  dispatch_sync(_lockQueue, ^{
    object = self->_activeTraces[key];
  });
  return object;
}

- (void)removeTraceForKey:(NSString *)key {
  dispatch_async(_lockQueue, ^{
   [self->_activeTraces removeObjectForKey:key];
  });
}

- (void)startTraceWithName:(NSString *)name
{
    FIRTrace *trace = [self traceForKey:name];
    if (trace != nil){
        NSLog(@"Trace %@ already existed, let stop", name);
        [trace stop];
        [self removeTraceForKey: name];
    }

    trace = [FIRPerformance startTraceWithName:name];
    // trace might be nil!!
    if (trace != nil)
    {
        NSLog(@"Trace %@ is started!", name);
        [self setTrace:trace forKey:name];
    }
}

- (void)stopTraceWithName:(NSString *)name
{
    FIRTrace *trace = [self traceForKey:name];
    if (trace == nil){
        NSLog(@"Trace %@ not exist!", name);
        return;
    }
    
    [trace stop];
    [self removeTraceForKey:name];
    NSLog(@"Trace %@ is stopped!", name);
}

- (void)setTrace:(NSString *)name withValue:(NSString *)value forAttribute:(NSString *)attribute
{
    FIRTrace *trace = [self traceForKey:name];
    if (trace == nil){
        NSLog(@"Trace %@ not exist!", name);
        return;
    }
    
    [trace setValue:value forAttribute:attribute];
}

- (void)trace:(NSString *)name incrementMetric:(NSString *)metric byValue:(int)value
{
    FIRTrace *trace = [self traceForKey:name];
    if (trace == nil){
        NSLog(@"Trace %@ not exist!", name);
        return;
    }

    [trace incrementMetric: metric byInt: value];
}
@end

extern "C" {
    void _startPMTrace(const char* name) 
    {
        [[PerformaceMonitor sharedManager] startTraceWithName:[NSString stringWithUTF8String:name]];
    }

    void _setPMTraceAttribute(const char* name, const char* attribute, const char* value)
    {
        [[PerformaceMonitor sharedManager] setTrace:[NSString stringWithUTF8String:name] withValue:[NSString stringWithUTF8String:value] forAttribute:[NSString stringWithUTF8String:attribute]];
    }

    void _incrementPMTraceMetric(const char* name, const char* metric, int value)
    {
        [[PerformaceMonitor sharedManager] trace:[NSString stringWithUTF8String:name] incrementMetric:[NSString stringWithUTF8String:metric] byValue:value];
    }
    
    void _stopPMTrace(const char* name) 
    {
        [[PerformaceMonitor sharedManager] stopTraceWithName:[NSString stringWithUTF8String:name]];
    }
}
