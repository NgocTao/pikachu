#import "SharkfoodMuteSwitchDetector.h"

extern "C" {
    void _checkStatus() {
        [[SharkfoodMuteSwitchDetector shared] checkStatus];
    }
    
    bool _isMuted() {
        return [SharkfoodMuteSwitchDetector shared].isMute;
    }
    
    bool _isChecking() {
        return [SharkfoodMuteSwitchDetector shared].isChecking;
    }
}
