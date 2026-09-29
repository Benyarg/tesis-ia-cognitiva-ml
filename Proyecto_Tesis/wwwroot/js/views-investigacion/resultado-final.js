async function obtenerResultado() {
    const box = document.getElementById("resultadoBox");
    box.style.display = "block";
    box.innerHTML = '<div class="res-text-sub">Procesando evaluación...</div>';

    try {
        const response = await secureFetch('/evaluacion/resultado', { method: 'POST' });
        const data = await getJsonOrThrow(response);
        const alto = data.riesgo === "ALTO";

        box.style.background = alto ? "#fff7ed" : "#f0fdf4";
        box.style.color = alto ? "#9a3412" : "#166534";
        box.style.borderColor = alto ? "#fed7aa" : "#bbf7d0";

        const titulo = alto ? "Nivel observado: Alto" : "Nivel observado: Bajo";
        box.innerHTML = `
            <div class="res-text-main">${titulo}</div>
            <div class="res-text-sub">${data.mensaje}</div>
            <div class="res-text-sub" style="margin-top:12px;font-weight:600;">
                Este resultado corresponde a una evaluación predictiva y de detección temprana. No constituye un diagnóstico clínico ni sustituye una evaluación profesional especializada.
            </div>`;
    } catch (error) {
        box.style.background = "#fef2f2";
        box.style.color = "#991b1b";
        box.style.borderColor = "#fecaca";
        box.innerHTML = `
            <div class="res-text-main">Resultado no disponible</div>
            <div class="res-text-sub">${error.message || "No se pudo generar el resultado."}</div>`;
    }
}

document.addEventListener('DOMContentLoaded', obtenerResultado);
