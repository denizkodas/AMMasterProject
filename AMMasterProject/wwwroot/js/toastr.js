/*"toast-success"*/

function toaster(value, css) {

   
    const toastContainer = document.createElement('div');
    toastContainer.className = 'toast-container';
    toastContainer.id = 'toastPlacement';
    toastContainer.setAttribute('data-original-class', 'toast-container');

    const toast = document.createElement('div');
    toast.className = 'toast ' + css + ' show align-items-center';
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');

    const toastContent = document.createElement('div');
    toastContent.className = 'd-flex';

    const toastBody = document.createElement('div');
    toastBody.className = 'toast-body';
    toastBody.innerText = value;

    const closeButton = document.createElement('button');
    closeButton.className = 'btn-close ';
    closeButton.setAttribute('type', 'button');
    closeButton.setAttribute('data-bs-dismiss', 'toast');
    closeButton.setAttribute('aria-label', 'Close');

    toastContent.appendChild(toastBody);
    toastContent.appendChild(closeButton);

    toast.appendChild(toastContent);

    toastContainer.appendChild(toast);

    // Add the toast container to your desired element in the DOM
    // For example:
    document.body.appendChild(toastContainer);

    // Auto-dismiss after 5 seconds
    setTimeout(function () {
        toastContainer.remove();
    }, 5000);
}