(function () {
    "use strict";

    window.secureFetch = function (url, options) {
        const requestOptions = { ...(options || {}) };
        const method = (requestOptions.method || "GET").toUpperCase();
        const headers = new Headers(requestOptions.headers || {});

        if (["POST", "PUT", "PATCH", "DELETE"].includes(method)) {
            const token = document.querySelector('meta[name="csrf-token"]')?.getAttribute("content");
            if (!token) {
                return Promise.reject(new Error("No se encontró el token de seguridad CSRF."));
            }
            headers.set("X-CSRF-TOKEN", token);
        }

        requestOptions.headers = headers;
        return fetch(url, requestOptions);
    };

    window.getJsonOrThrow = async function (response) {
        let body = null;
        const contentType = response.headers.get("content-type") || "";
        if (contentType.includes("application/json")) {
            body = await response.json();
        }

        if (!response.ok) {
            const message = body?.mensaje || "No se pudo completar la operación.";
            throw new Error(message);
        }

        return body;
    };
})();
