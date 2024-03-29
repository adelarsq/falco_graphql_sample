# Falco GraphQL Sample using .NET 8

[Falco](https://www.falcoframework.com/) is a toolkit for building secure, fast, functional-first and fault-tolerant web applications using [F#](https://fsharp.org).

[GraphQL](https://graphql.org) is an open-source data query and manipulation language for APIs.

This project is a sample showing how to use GraphQL on Falco using [.NET 8](https://dotnet.microsoft.com).

For .NET 6 check [dotnet_6](https://github.com/adelarsq/falco_graphql_sample/tree/dotnet_6) branch.

## How to use?

On the terminal run with:

```
cd src/HelloWorld
dotnet run
```

The server will start on the address `http://127.0.0.1:8080`.

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


