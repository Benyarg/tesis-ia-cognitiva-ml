const preguntasSUS = [
    "Creo que me gustaría usar este sistema con frecuencia", "Encontré el sistema innecesariamente complejo",
    "Pensé que el sistema era fácil de usar", "Creo que necesitaría ayuda técnica para usar el sistema",
    "Encontré que las funciones estaban bien integradas", "Encontré demasiada inconsistencia en el sistema",
    "Imagino que la mayoría de las personas aprenderían a usar este sistema rápidamente", "Encontré el sistema difícil de usar",
    "Me sentí muy confiado usando el sistema", "Necesité aprender muchas cosas antes de poder usar el sistema"
];

const contenedor = document.getElementById("preguntasSUS");
preguntasSUS.forEach((p, i) => {
    contenedor.insertAdjacentHTML("beforeend", `
        <div class="pregunta-card">
            <span class="pregunta-texto">${i + 1}. ${p}</span>
            <div class="opciones-group">
                ${[1, 2, 3, 4, 5].map(val => `
                    <input type="radio" name="q${i}" id="sus_q${i}_${val}" value="${val}" required>
                    <label for="sus_q${i}_${val}" class="opcion-label">${val}</label>
                `).join('')}
            </div>
        </div>`);
});

document.getElementById("formSUS").addEventListener("submit", async function (e) {
    e.preventDefault();
    const respuestas = [];
    const resultado = document.getElementById("resultado");
    const boton = this.querySelector('button[type="submit"]');

    for (let i = 0; i < 10; i++) {
        const val = document.querySelector(`input[name="q${i}"]:checked`);
        if (!val) {
            alert("Por favor, responde todas las preguntas antes de enviar.");
            return;
        }
        respuestas.push(Number(val.value));
    }

    let suma = 0;
    respuestas.forEach((valor, i) => suma += i % 2 === 0 ? valor - 1 : 5 - valor);
    const puntajeFinal = suma * 2.5;

    boton.disabled = true;
    resultado.innerText = "Guardando evaluación...";

    try {
        const response = await secureFetch('/evaluacion/usabilidad', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ puntajeTotal: puntajeFinal })
        });
        await getJsonOrThrow(response);
        resultado.innerText = "Evaluación completada correctamente";
        enviarLog("SUS", 0, 10);
        setTimeout(() => window.location.href = '/evaluacion/resultado', 900);
    } catch (error) {
        resultado.innerText = error.message || "No se pudo guardar la evaluación.";
        boton.disabled = false;
    }
});
