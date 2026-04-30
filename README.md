# Pokédex

Aplicação web para pesquisar Pokémons por nome, habitat ou tipo, consumindo a [PokéAPI](https://pokeapi.co/).

Desenvolvida em **ASP.NET Core MVC (.NET 6)** com suporte a Docker.

---

## Funcionalidades

- Busca de Pokémon por **nome ou ID**
- Listagem por **habitat** (cave, forest, sea, etc.)
- Listagem por **tipo** (fire, water, grass, etc.)
- Paginação nos resultados
- Design responsivo via Bootstrap

---

## Rodando com Docker (recomendado)

Não precisa de .NET instalado localmente.

```bash
docker compose up --build
```

Acesse: [http://localhost:8080](http://localhost:8080)

```bash
docker compose down   # parar
```

---

## Rodando localmente

Requer [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

```bash
dotnet restore
dotnet run
```

Acesse: [https://localhost:7238](https://localhost:7238)

---

## Stack

| Tecnologia | Uso |
|---|---|
| ASP.NET Core MVC | Framework web |
| C# / .NET 6 | Linguagem e runtime |
| PokéAPI | Fonte de dados |
| Newtonsoft.Json | Parse das respostas da API |
| Bootstrap | Layout e responsividade |
| Docker | Containerização |
