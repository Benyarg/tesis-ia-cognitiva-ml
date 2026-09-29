# Debugging corregido

Principales problemas corregidos:
- credenciales históricas fuera del proyecto y configuración preparada para secretos;
- Identity público y registro eliminados;
- rutas públicas y administrativas separadas;
- formularios protegidos con antiforgery;
- validación de orden del flujo;
- doble envío sin duplicar evaluaciones;
- resultado generado por POST;
- errores internos no expuestos;
- tokens de investigación persistidos como hash;
- endpoints admin protegidos por rol;
- paginación y reducción de consultas N+1 en listados;
- carga correcta de JavaScript y rutas de assets.
