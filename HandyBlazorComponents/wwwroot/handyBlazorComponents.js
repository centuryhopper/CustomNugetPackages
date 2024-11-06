
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