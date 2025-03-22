mergeInto(LibraryManager.library, {
    SendLogToReactNative: function (messagePtr) {
        var message = UTF8ToString(messagePtr);
        if (window.ReactNativeWebView) {
            window.ReactNativeWebView.postMessage(message);
        } else {
            console.log("ReactNativeWebView not found. Message: " + message);
        }
    }
});
