document.getElementById("formUsoIA").addEventListener("submit", async function (e) {
    e.preventDefault();
    const mensaje = document.getElementById("mensaje");
    const boton = this.querySelector('button[type="submit"]');
    const horas = Number(document.getElementById("horas").value);
    const dias = Number(document.getElementById("dias").value);
    const tipo = document.querySelector('input[name="tipoUso"]:checked');

    if (!tipo) {
        mensaje.innerText = "Selecciona el tipo de uso.";
        return;
    }

    boton.disabled = true;
    mensaje.innerText = "Guardando información...";

    try {
        const response = await secureFetch('/evaluacion/uso-ia', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ horasUsoDiario: horas, diasSemana: dias, tipoUso: tipo.value })
        });
        await getJsonOrThrow(response);
        mensaje.innerText = "Información registrada correctamente";
        enviarLog("UsoIA", 0, 3);
        setTimeout(() => window.location.href = '/evaluacion/usabilidad', 900);
    } catch (error) {
        mensaje.innerText = error.message || "No se pudo guardar la información.";
        boton.disabled = false;
    }
});
