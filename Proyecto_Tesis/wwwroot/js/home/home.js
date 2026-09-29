"use strict";

document.addEventListener("DOMContentLoaded", () => {
    const consentimiento = document.getElementById("consentimiento");
    const consentCard = document.getElementById("consentCard");
    const boton = document.getElementById("btnIniciar");
    const mensaje = document.getElementById("mensaje");

    if (!consentimiento || !consentCard || !boton || !mensaje) {
        console.error("No se pudieron inicializar los controles de consentimiento.");
        return;
    }

    consentCard.addEventListener("click", (event) => {
        if (event.target.closest("input, label")) {
            return;
        }

        consentimiento.checked = !consentimiento.checked;
    });

    boton.addEventListener("click", async () => {
        if (!consentimiento.checked) {
            mensaje.innerText =
                "Debes aceptar el consentimiento para continuar.";
            return;
        }

        boton.disabled = true;
        mensaje.innerText = "Registrando consentimiento...";

        try {
            const response = await secureFetch("/evaluacion/consentimiento", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    aceptado: true
                })
            });

            await getJsonOrThrow(response);

            window.location.assign("/evaluacion/cognitiva");
        } catch (error) {
            console.error("Error al registrar consentimiento:", error);

            mensaje.innerText =
                error.message || "No se pudo iniciar la evaluación.";

            boton.disabled = false;
        }
    });
});