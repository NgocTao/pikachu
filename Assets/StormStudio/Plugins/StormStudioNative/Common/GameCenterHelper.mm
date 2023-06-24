//
//  GameCenterHelper.m
//  Unity-iPhone
//
//  Created by Nguyen Hoai Phuong on 1/14/20.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "UnityAppController.h"
#import "StringHelper.h"

@interface GameCenterManager : NSObject<GKGameCenterControllerDelegate>
@property (nonatomic, strong) UIViewController *presentationController;
@property (nonatomic, readonly) BOOL isAuthenticating;
@property (nonatomic, readonly) NSMutableDictionary<NSString*, NSNumber*>* playerScores;

+ (instancetype)sharedManager;
- (void)authenticatePlayer: (NSString*)objectCallbackName;
- (void)showLeaderboard: (NSString*)leaderboardId;
- (void)reportScore:(NSInteger)score forLeaderboard:(NSString*)leaderboardId;
- (void)loadPlayerScore:(NSString*)leaderboardId;
@end

@implementation GameCenterManager

#pragma mark Singelton
      
+ (instancetype)sharedManager {
    static GameCenterManager *sharedManager;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedManager = [[GameCenterManager alloc] init];
    });
    return sharedManager;
}

#pragma mark Initialization
      
- (id)init {
    self = [super init];
    if (self) {
        self.presentationController = UnityGetGLViewController();
        _playerScores = [[NSMutableDictionary alloc] init];
    }
    return self;
}

#pragma mark Player Authentication
      
- (void)authenticatePlayer:(NSString*)objectCallbackName {
    if (!GKLocalPlayer.localPlayer.authenticated)
    {
        _isAuthenticating = YES;
        GKLocalPlayer *localPlayer = [GKLocalPlayer localPlayer];
        [localPlayer setAuthenticateHandler:
         ^(UIViewController *viewController, NSError *error) {
            if (viewController != nil) {
                [self.presentationController presentViewController:viewController
                animated:YES completion:nil];
            } else if (GKLocalPlayer.localPlayer.authenticated) {
                _isAuthenticating = NO;
                UnitySendMessage([objectCallbackName UTF8String], "GameCenterAuthenDidFinish", "");
                NSLog(@"Player successfully authenticated");
            } else if (error) {
                _isAuthenticating = NO;
                NSLog(@"Game Center authentication error: %@", error);
            }
        }];
    }
}

- (void)showLeaderboard: (NSString*)leaderboardId{
   GKGameCenterViewController *gcViewController = [[GKGameCenterViewController alloc] init];
   gcViewController.gameCenterDelegate = self;
   gcViewController.viewState = GKGameCenterViewControllerStateLeaderboards;
   gcViewController.leaderboardIdentifier = leaderboardId;
   gcViewController.leaderboardTimeScope = GKLeaderboardTimeScopeAllTime;
    
   [self.presentationController presentViewController:gcViewController animated:YES completion:nil];
}

- (void)reportScore:(NSInteger)score forLeaderboard:(NSString *)leaderboardId{
    if (GKLocalPlayer.localPlayer.authenticated)
    {
        GKScore *gScore = [[GKScore alloc]
        initWithLeaderboardIdentifier:leaderboardId];
        gScore.value = score;
        gScore.context = 0;
        
        [GKScore reportScores:@[gScore]
        withCompletionHandler:^(NSError *error) {
            if (!error) {
                NSLog(@"Score reported successfully!");
            }
            else {
                NSLog(@"Unable to report score");
            }
        }];
    }
}

- (void)loadPlayerScore:(NSString*)leaderboardId {
    if (GKLocalPlayer.localPlayer.authenticated) {
        GKLeaderboard* leaderboard = [[GKLeaderboard alloc] initWithPlayers:[NSArray arrayWithObject:GKLocalPlayer.localPlayer]];
        leaderboard.identifier = leaderboardId;
        leaderboard.timeScope = GKLeaderboardTimeScopeAllTime;
        [leaderboard loadScoresWithCompletionHandler:^(NSArray<GKScore *> * _Nullable scores, NSError * _Nullable error) {
            
            if(error == nil && scores.count > 0) {
                [_playerScores setObject:[NSNumber numberWithLong:[scores objectAtIndex:0].value] forKey:leaderboardId];
            }
            else {
                [_playerScores setObject:[NSNumber numberWithLong:0] forKey:leaderboardId];
            }
        }];
    }
}

#pragma mark GameKit Delegate Methods
      
- (void)gameCenterViewControllerDidFinish:
  (GKGameCenterViewController *)gameCenterViewController {
    [self.presentationController dismissViewControllerAnimated:YES completion:nil];
}

@end

extern "C" {
    bool _isAuthenticated() {
        return GKLocalPlayer.localPlayer.authenticated;
    }
    
    void _authenticatePlayer(const char* objectCallbackName) {
        [[GameCenterManager sharedManager] authenticatePlayer: [NSString stringWithUTF8String:objectCallbackName]];
    }

    char* _getPlayerId() {
        return cStringCopy([GKLocalPlayer.localPlayer.playerID UTF8String]);
    }
    
    bool _isAuthenticating() {
        return [GameCenterManager sharedManager].isAuthenticating;
    }

    long _getPlayerScore(const char* leaderboardId) {
        NSNumber* score = [[GameCenterManager sharedManager].playerScores objectForKey:[NSString stringWithUTF8String:leaderboardId]];
        if (score != nil)
            return score.longValue;
        
        return -1;
    }

    void _loadPlayerScore(const char* leaderboardId) {
        [[GameCenterManager sharedManager] loadPlayerScore: [NSString stringWithUTF8String:leaderboardId]];
    }
    
    void _reportScore(int score, const char* leaderboardId) {
        [[GameCenterManager sharedManager] reportScore: score forLeaderboard:[NSString stringWithUTF8String:leaderboardId]];
    }

    void _showLeaderboard(const char* leaderboardId) {
        [[GameCenterManager sharedManager] showLeaderboard: [NSString stringWithUTF8String:leaderboardId]];
    }
}
