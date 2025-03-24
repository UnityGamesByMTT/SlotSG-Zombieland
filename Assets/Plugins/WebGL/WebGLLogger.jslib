mergeInto(LibraryManager.library, {
    SendLogToReactNative: function (messagePtr) {
        var message = UTF8ToString(messagePtr);
        console.log('jslib fun : ' + message);
        if (window.ReactNativeWebView) {
            window.ReactNativeWebView.postMessage(message);
        } 
    }
});
