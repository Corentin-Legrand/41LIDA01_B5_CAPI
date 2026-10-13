![CI](https://github.com/Corentin-Legrand/41LIDA01_B5_CAPI/actions/workflows/ci.yml/badge.svg)

# CAPI

Application de planning poker (C# .NET 10 / WPF).

## Prérequis

- SDK .NET 10 (`dotnet --list-sdks` doit afficher une version 10.0.x)
- Windows pour lancer l'application (WPF)

## Compiler et lancer l'application

Depuis la racine du dépôt :

```sh
dotnet run --project CAPI/CAPI_APP/CAPI_APP.csproj
```

## Lancer les tests

Depuis la racine du dépôt :

```sh
dotnet test CAPI/CAPI_TESTS/CAPI_TESTS.csproj
```