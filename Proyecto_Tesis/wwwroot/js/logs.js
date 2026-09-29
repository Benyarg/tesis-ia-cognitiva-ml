async function enviarLog(action, latency, size) {
    try {
        const response = await secureFetch('/evaluacion/log', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                actionType: action,
                latencyMs: latency,
                inputSize: size
            })
        });

        if (!response.ok) {
            console.warn("No se pudo registrar el evento de interacción.");
        }
    } catch {
        console.warn("No se pudo registrar el evento de interacción.");
    }
}
