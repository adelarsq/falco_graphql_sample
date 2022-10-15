# Falco GraphQL Sample using .NET 6

[Falco](https://www.falcoframework.com/) is a toolkit for building secure, fast, functional-first and fault-tolerant web applications using [F#](https://fsharp.org).

[GraphQL](https://graphql.org) is an open-source data query and manipulation language for APIs.

This project is a sample showing how to use GraphQL on Falco using [.NET 6](https://dotnet.microsoft.com).

## How to use?

On the terminal run with:

```
cd src/HelloWorld
dotnet run
```

The server with start on the address `http://127.0.0.1:8080`.

## Queries

With a GraphQL client you can test with the follow queries:

```graphql
query {
    viewer {
      id
      name
      age
    }
}
```

![image](https://user-images.githubusercontent.com/430272/195999560-20927bd0-d948-4815-b723-a0baa452036f.png)

Query with fragments:

```graphql
fragment userFragment on User {
    widgets(first: 10) {
        edges {
            node {
                id
                name
            }
        }
    }
}
  
query {
    viewer {
        ...userFragment
    }
}
```

![image](https://user-images.githubusercontent.com/430272/195999585-37f566db-bab0-4aca-8caf-32e25c60d3e3.png)

## Acknowledgments

- [Falco](https://www.falcoframework.com)
- [FSharp.Data.GraphQL](https://github.com/fsprojects/FSharp.Data.GraphQL)


