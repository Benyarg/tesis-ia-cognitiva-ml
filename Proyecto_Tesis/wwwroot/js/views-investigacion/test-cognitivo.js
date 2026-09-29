let startTime = 0;
let esperandoClick = false;
let intentos = [];
let temporizador = null;
let enviando = false;

function iniciarTest() {
    if (enviando) return;

    const box = document.getElementById("box");
    const resultado = document.getElementById("resultado");
    resultado.innerText = "";
    box.style.backgroundColor = "gray";
    box.innerText = "Espera...";
    esperandoClick = false;

    clearTimeout(temporizador);
    const delay = Math.random() * 3000 + 2000;
    temporizador = setTimeout(() => {
        box.style.backgroundColor = "green";
        box.innerText = "¡Toca ahora!";
        startTime = performance.now();
        esperandoClick = true;
    }, delay);
}
document.getElementById("btnIniciarTest")
    ?.addEventListener("click", iniciarTest);

document.getElementById("box").addEventListener("click", async function () {
    if (enviando) return;
    if (!esperandoClick) {
        alert("Espera a que el cuadro cambie de color");
        return;
    }

    const latencia = performance.now() - startTime;
    esperandoClick = false;
    intentos.push(latencia);

    if (intentos.length < 3) {
        temporizador = setTimeout(iniciarTest, 1500);
        return;
    }

    enviando = true;
    const promedio = intentos.reduce((a, b) => a + b, 0) / intentos.length;
    const resultado = document.getElementById("resultado");
    resultado.innerText = "Guardando resultado...";

    try {
        const response = await secureFetch('/evaluacion/cognitiva', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ latenciaMs: promedio })
        });
        await getJsonOrThrow(response);
        resultado.innerText = "Test completado correctamente";
        enviarLog("TestCognitivo", promedio, 3);
        setTimeout(() => window.location.href = '/evaluacion/emocional', 900);
    } catch (error) {
        resultado.innerText = error.message || "No se pudo guardar el resultado.";
        intentos = [];
        enviando = false;
    }
});
