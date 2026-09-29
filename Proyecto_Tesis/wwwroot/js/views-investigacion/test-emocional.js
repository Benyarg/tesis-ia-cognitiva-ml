"use strict";

const preguntas = [
    "Me ha costado relajarme",
    "He sentido la boca seca",
    "No podía experimentar ningún sentimiento positivo",
    "Se me hizo difícil respirar",
    "Me resultó difícil tomar la iniciativa",
    "He reaccionado exageradamente",
    "He sentido temblores",
    "He sentido que estaba usando mucha energía nerviosa",
    "Me preocupé por situaciones en las que podría entrar en pánico",
    "Sentí que no tenía nada que esperar",
    "Me he sentido agitado",
    "Me resultó difícil relajarme",
    "Me sentí triste y deprimido",
    "No toleré nada que me impidiera continuar",
    "Sentí que estaba a punto de entrar en pánico",
    "No pude entusiasmarme por nada",
    "Sentí que no valía mucho como persona",
    "Sentí que estaba muy irritable",
    "Sentí los latidos de mi corazón sin hacer esfuerzo",
    "Tuve miedo sin razón",
    "Sentí que la vida no tenía sentido"
];

document.addEventListener("DOMContentLoaded", () => {
    const contenedor = document.getElementById("preguntas");
    const formulario = document.getElementById("formDASS");
    const resultado = document.getElementById("resultado");

    if (!contenedor || !formulario || !resultado) {
        console.error("No se pudieron inicializar los controles de DASS-21.");
        return;
    }

    preguntas.forEach((pregunta, i) => {
        contenedor.insertAdjacentHTML(
            "beforeend",
            `
            <div class="pregunta-card" id="pregunta-${i}">
                <span class="pregunta-texto">
                    ${i + 1}. ${pregunta}
                </span>

                <div class="opciones-group">
                    ${[0, 1, 2, 3]
                        .map(
                            valor => `
                            <input
                                type="radio"
                                name="q${i}"
                                id="q${i}_${valor}"
                                value="${valor}"
                            >

                            <label
                                for="q${i}_${valor}"
                                class="opcion-label">
                                ${valor}
                            </label>
                        `
                        )
                        .join("")}
                </div>
            </div>
        `
        );
    });

    formulario.addEventListener("submit", async event => {
        event.preventDefault();

        const boton = formulario.querySelector(
            'button[type="submit"]'
        );

        const respuestas = [];

        for (let i = 0; i < preguntas.length; i++) {
            const seleccionada = document.querySelector(
                `input[name="q${i}"]:checked`
            );

            if (!seleccionada) {
                resultado.innerText =
                    `Debes responder la pregunta ${i + 1} antes de continuar.`;

                const tarjeta = document.getElementById(
                    `pregunta-${i}`
                );

                tarjeta?.scrollIntoView({
                    behavior: "smooth",
                    block: "center"
                });

                return;
            }

            respuestas.push(Number(seleccionada.value));
        }

        const estresIdx = [0, 5, 7, 10, 12, 15, 18];
        const ansiedadIdx = [1, 3, 6, 8, 14, 16, 19];
        const depresionIdx = [2, 4, 9, 11, 13, 17, 20];

        const sumar = indices =>
            indices.reduce(
                (total, indice) => total + respuestas[indice],
                0
            );

        boton.disabled = true;
        resultado.innerText = "Guardando respuestas...";

        try {
            const response = await secureFetch(
                "/evaluacion/emocional",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        estres: sumar(estresIdx),
                        ansiedad: sumar(ansiedadIdx),
                        depresion: sumar(depresionIdx)
                    })
                }
            );

            await getJsonOrThrow(response);

            resultado.innerText =
                "Respuestas registradas correctamente.";

            void enviarLog("DASS21", 0, 21);

            window.location.assign("/evaluacion/uso-ia");
        } catch (error) {
            console.error(
                "Error al guardar DASS-21:",
                error
            );

            resultado.innerText =
                error.message ||
                "No se pudieron guardar las respuestas.";

            boton.disabled = false;
        }
    });
});