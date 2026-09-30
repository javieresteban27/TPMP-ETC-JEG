# Copilot Instructions

## Directrices del proyecto
- Preferencia: usar MSTest como framework de pruebas por defecto.
- En este proyecto, `Assert.ThrowsException` no está disponible en MSTest. Al generar pruebas de excepciones para este workspace, usa un patrón compatible con la versión instalada, por ejemplo try/catch con `Assert.Fail` si no se confirma otra API.