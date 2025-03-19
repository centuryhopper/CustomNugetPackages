

SESSION_FUNCTIONS = {
    checkExpiry: (basePath) =>
        {
            // avoid checking for jwt expiring when already on the session-expired page
            if (window.location.href.includes('session-expired'))
            {
                return;
            }
            let storedTimestamp = localStorage.getItem("expDate");
            // console.log('storedTimestamp: ' + storedTimestamp);
            if (storedTimestamp)
            {
                storedTimestamp = Number(storedTimestamp) / 1000
                let currentTime = Math.floor(Date.now() / 1000); // Get current time in seconds
                // console.log(basePath);
                // console.log(currentTime - storedTimestamp);
                if (currentTime >= storedTimestamp)
                {
                    // console.log('session expired');
                    // Redirect to session expired page
                    window.location.href = basePath;
                }
                else
                {
                    // Convert to milliseconds
                    let timeLeft = (storedTimestamp - currentTime) * 1000;
                    // console.log('timeLeft: ' + timeLeft);
                    setTimeout(() => this.checkExpiry(basePath), timeLeft); // Re-check at expiration time
                }
            }
            else {
                // If there's no session expiration data, recheck in 10 seconds
                setTimeout(() => this.checkExpiry(basePath), 10000);
            }
        },
 
        startIdleTimer: (dotNetHelper, timeout) => {
            let timer = null;
            let countDown;
            let idleModalWarningIsOpen = false;
 
            let idleTime = 0
            // give one minute warning
            /*
                timeout - warningtime = 60000
 
                formula:
                if timeout > 60000
                    warningTime = 60000
                else
                    warningTime = timeout - 10000
            */
 
            // make sure we have at least 30 seconds to deal with
            timeout = Math.max(30000, timeout)
            // 5 minutes
            const SESSION_DISPLAY_TIME_IN_MILLISECONDS = 300000
            let warningTime = timeout > SESSION_DISPLAY_TIME_IN_MILLISECONDS ? SESSION_DISPLAY_TIME_IN_MILLISECONDS : timeout - 10000
 
            function showWarning()
            {
                let timeLeftInSeconds = warningTime / 1000;
                // console.log('showing warning');
                idleModalWarningIsOpen = true;
                swal.fire({
                    title: "Session Timeout Warning",
                    html: `You have been idle. Redirecting in <b><span id="countdown">${timeLeftInSeconds}</span></b> seconds.`,
                    icon: "warning",
                    // in milliseconds
                    timer: warningTime,
                    allowOutsideClick: false,
                    showCancelButton: true,
                    confirmButtonText: "Stay Logged In",
                    cancelButtonText: "Logout",
                    didOpen: () => {
                        const countdownEl = document.getElementById("countdown");
                        countDown = setInterval(() => {
                            timeLeftInSeconds--;
                            //console.log(timeLeftInSeconds);
                            countdownEl.innerText = timeLeftInSeconds;
                            if (timeLeftInSeconds <= 0)
                            {
                                clearInterval(countDown);
                                // console.log('time ran out');
                                dotNetHelper.invokeMethodAsync("RedirectUser");
                            }
                        }, 1000);
                    }
                }).then((result) => {
                    idleModalWarningIsOpen = false;
                    if (result.isConfirmed) {
                        // console.log('resetting timer');
                        resetTimer();
                        // dotNetHelper.invokeMethodAsync("StayLoggedIn");
                    } else {
                        // console.log('redirecting user');
                        clearInterval(countDown)
                        clearTimeout(timer);
                        dotNetHelper.invokeMethodAsync("RedirectUser");
                    }
                });
            }
 
            function resetTimer() {
                clearInterval(countDown)
                clearTimeout(timer);
                timer = null;
                // console.log(window.location.href);
                // make sure to avoid firing off an idle timer when on the session-expired page (this should be the only page in the entire app that doesn't require idle timer being fired off)
                if (window.location.href.includes('session-expired'))
                {
                    return;
                }
                idleTime = 0;
                // show it in (timeout - warningTime milliseconds)
                // for example, if there's a total of 20 seconds of allowed idle time,
                // and the warning time is 15 seconds left then the pop up will appear 5 seconds after 20 second timer has started
                timer = setTimeout(showWarning, timeout - warningTime);
            }
 
            // Reset timer on user activity
            window.addEventListener("mousemove", () => {
                //console.log('moved mouse');
                // avoid resetting timer while pop up is open
                if (!idleModalWarningIsOpen)
                {
                    resetTimer()
                }
            });
            window.addEventListener("keydown", () => {
                //console.log('keydowned');
                if (!idleModalWarningIsOpen)
                {
                    resetTimer()
                }
            });
            window.addEventListener("scroll", () => {
                //console.log('scrolled');
                if (!idleModalWarningIsOpen)
                {
                    resetTimer()
                }
            });
            window.addEventListener("click", () => {
                //console.log('clicked');
                if (!idleModalWarningIsOpen)
                {
                    resetTimer()
                }
            });
 
            // Start the timer initially
            resetTimer();
        },
 
}


// closes the multiselect when user presses escape
window.dropdownInterop = {
    registerEscapeKeyHandler: function (dotNetObject) {
        document.addEventListener('keydown', function (event) {
            if (event.key === 'Escape') {
                console.log('escape pressed');
                dotNetObject.invokeMethodAsync('CloseDropdown');
            }
        });
    }
};

window.downloadFile = (base64Data, contentType, fileName) => {
    const blob = new Blob([Uint8Array.from(atob(base64Data), c => c.charCodeAt(0))], { type: contentType });
    const url = URL.createObjectURL(blob);

    // Create a temporary anchor element
    const a = document.createElement("a");
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);

    // Trigger download and clean up
    a.click();
    URL.revokeObjectURL(url);
    document.body.removeChild(a);
}

window.saveAsFile = function(filename, fileContent)
{
    var link = document.createElement('a')
    link.download = filename
    // mime-types: https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
    link.href = 'data:text/plain;charset=utf-8,'+encodeURIComponent(fileContent)
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
}