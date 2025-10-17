# Prueba Técnica Carsales

Este repositorio contiene dos proyectos principales:

1. **Backend:** .NET 8 Web API  
2. **Frontend:** Angular 20 Standalone

A continuación encontrarás instrucciones para configurar, ejecutar y probar ambos proyectos.

---

## Requisitos Previos

Antes de comenzar, asegúrate de tener instaladas las siguientes herramientas:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/) (puedes usar [nvm](https://github.com/coreybutler/nvm-windows/releases) para gestionar versiones)
- [Angular CLI 20+](https://angular.io/cli)
  
  ```bash
  npm install -g @angular/cli

## Ejecutar proyecto Backend
1. Restaurar paquetes NuGet
```bash
cd Backend
```
```bash
dotnet restore
```

2. Costruir proyecto
```bash
dotnet build
```
3. Por ultimo, se recomienda entrar a Visual Studio, Abrir la solución del proyecto y ejecutar el proyecto con el perfil **http**.

## Ejecutar proyecto Frontend
1. Ingresar al proyecto
```bash
cd Frontend
```
2. Cambiar a v20 de node.js (si usas nvm cambiar a la version 20 de node o sup)
```bash
nvm use 20
```
3. Instalar dependencias
```bash
npm install
```
4. Iniciar aplicación
```bash
ng serve
```
