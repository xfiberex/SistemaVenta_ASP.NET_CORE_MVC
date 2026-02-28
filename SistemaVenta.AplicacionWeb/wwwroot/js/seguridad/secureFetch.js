(function () {
    function getCsrfToken() {
        const tokenNode = document.querySelector('meta[name="csrf-token"]');
        return tokenNode ? tokenNode.getAttribute("content") : "";
    }

    function secureFetch(url, options) {
        const requestOptions = options || {};
        const method = (requestOptions.method || "GET").toUpperCase();

        if (!["GET", "HEAD", "OPTIONS", "TRACE"].includes(method)) {
            const headers = new Headers(requestOptions.headers || {});
            const csrfToken = getCsrfToken();

            if (csrfToken) {
                headers.set("X-CSRF-TOKEN", csrfToken);
            }

            requestOptions.headers = headers;
        }

        return fetch(url, requestOptions);
    }

    window.secureFetch = secureFetch;
})();
