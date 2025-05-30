TEST_FUNCTIONS = {
    checkExpiry: (dotNetObject, basePath, idleTimeOut, id, JWT_TOKEN_NAME, JWT_TOKEN_EXP_DATE) =>
        {
            let jwtTimeout = null;
            let idleTimer = null;
            let countDownInterval = null;
            function resetAllTimers()
            {
                clearInterval(countDownInterval);
                clearTimeout(jwtTimeout);
                clearTimeout(idleTimer);
            }
    
            async function showSessionExpiredPopup(msg)
            {
                localStorage.removeItem(JWT_TOKEN_NAME)
                localStorage.removeItem(JWT_TOKEN_EXP_DATE)
                await dotNetObject.invokeMethodAsync("RedirectUser")
                swal.fire({
                    title: "Notice",
                    html: `${msg}`,
                    icon: "info",
                    allowOutsideClick: false,
                    showCancelButton: false,
                    confirmButtonText: "Click here to start again",
                }).then(result => {
                    if (result.isConfirmed)
                    {
                        window.location.href = basePath;
                    }
                });
            }
    
            function monitorJwtToken()
            {
                clearTimeout(jwtTimeout)
                // avoid checking for jwt expiring when modal is up
                if (swal.isVisible())
                {
                    //console.log("sweet alert pop up is open");
                    return
                }
                let storedTimestamp = localStorage.getItem(JWT_TOKEN_EXP_DATE);
                //console.log('storedTimestamp: ' + storedTimestamp);
                if (!storedTimestamp)
                {
                    // If there's no session expiration data, recheck in 10 seconds
                    jwtTimeout = setTimeout(() => monitorJwtToken(basePath), 10000); 
                    return
                }
                storedTimestamp = Number(storedTimestamp) / 1000
                // Get current time in seconds
                let currentTime = Math.floor(Date.now() / 1000);
                // console.log(basePath);
                // console.log(currentTime - storedTimestamp);
                if (currentTime >= storedTimestamp)
                {
                    // console.log('session expired');
                    // Redirect to session expired page
                    resetAllTimers()
                    //console.log('token expired');
                    showSessionExpiredPopup('Your token has Expired.')
                }
                else
                {
                    // Convert to milliseconds
                    let timeLeft = (storedTimestamp - currentTime) * 1000;
                    // console.log('timeLeft: ' + timeLeft);
                    jwtTimeout = setTimeout(() => monitorJwtToken(basePath), timeLeft); // Re-check at expiration time
                }
            }
    
            function startIdleTimer (timeout)
            {         
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
     
                function restartIdleTimer()
                {
    
                    showWarning = async () =>
                    {
                        let timeLeftInSeconds = warningTime / 1000;
                        // console.log('showing warning');
                        if (swal.isVisible())
                        {
                            resetAllTimers()
                            return
                        }
                        let result = await swal.fire({
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
                                countDownInterval = setInterval(() => {
                                    timeLeftInSeconds--;
                                    //console.log(timeLeftInSeconds);
                                    countdownEl.innerText = timeLeftInSeconds;
                                    if (timeLeftInSeconds <= 0)
                                    {
                                        swal.close();
                                        resetAllTimers()
                                        showSessionExpiredPopup('Your session has Expired.')
                                    }
                                }, 1000);
                            }
                        });
                        if (result.isConfirmed) {
                            restartIdleTimer();
                        } else {
                            resetAllTimers()
                            showSessionExpiredPopup('Your session has Expired.')
                        }
                    }
    
                    clearInterval(countDownInterval)
                    clearTimeout(idleTimer);
                    idleTimer = null;
                    idleTime = 0;
                    // show it in (timeout - warningTime milliseconds)
                    // for example, if there's a total of 20 seconds of allowed idle time,
                    // and the warning time is 15 seconds left then the pop up will appear 5 seconds after 20 second timer has started
                    idleTimer = setTimeout(showWarning, timeout - warningTime);
                }
     
                // Reset timer on user activity
                window.addEventListener("mousemove", () => {
                    //console.log('moved mouse');
                    // avoid resetting timer while any of the swal pop ups are open
                    if (!swal.isVisible())
                    {
                        restartIdleTimer()
                    }
                });
                window.addEventListener("keydown", () => {
                    //console.log('keydowned');
                    if (!swal.isVisible())
                    {
                        restartIdleTimer()
                    }
                });
                window.addEventListener("scroll", () => {
                    //console.log('scrolled');
                    if (!swal.isVisible())
                    {
                        restartIdleTimer()
                    }
                });
                window.addEventListener("click", () => {
                    //console.log('clicked');
                    if (!swal.isVisible())
                    {
                        restartIdleTimer()
                    }
                });
     
                // Start the timer initially
                restartIdleTimer();
            }
    
            if (!id || ![1,2].includes(Number(id)))
            {
                return
            }
            switch (Number(id))
            {
                case 1:
                    monitorJwtToken()
                    break
                case 2:
                    startIdleTimer(idleTimeOut)
                    break
            }
        },
}