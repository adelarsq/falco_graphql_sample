module HelloWorld.Program

open System

// Data ----------------------------------------------------------------------------------------------------------------

type Widget =
    { Id: string;
      Name: string }

type User =
    { Id: string;
      Name: string;
      Age: int;
      Widgets: Widget list }

let viewer = {
    Id = "1"
    Name = "John"
    Age = 32
    Widgets = [
        { Id = "1"; Name = "Hammer" }
        { Id = "2"; Name = "Key" }
        { Id = "3"; Name = "Master Key" } ] }

let getUser id = if viewer.Id = id then Some viewer else None
let getWidget id = viewer.Widgets |> List.tryFind (fun w -> w.Id = id)

// Schema Definition ---------------------------------------------------------------------------------------------------

open FSharp.Data.GraphQL
open FSharp.Data.GraphQL.Types
open FSharp.Data.GraphQL.Relay

let rec Widget =
    Define.Object<Widget>(
        name = "Widget",
        description = "A shiny widget",
        interfaces = [ Node ],
        fields = [
            Define.GlobalIdField(fun _ w -> w.Id)
            Define.Field("name", String, fun _ w -> w.Name)])

and User =
    Define.Object<User>(
        name = "User",
        description = "A person who uses our app",
        interfaces = [ Node ],
        fields = [
            Define.GlobalIdField(fun _ w -> w.Id)
            Define.Field("name", String, fun _ w -> w.Name)
            Define.Field("age", Int, fun _ w -> w.Age)
            Define.Field("widgets", ConnectionOf Widget, "A person's collection of widgets", Connection.allArgs, fun ctx user ->
                let widgets = user.Widgets |> List.toArray
                Connection.ofArray widgets )])

and Node = Define.Node<obj>(fun () -> [ User; Widget ])

let Query = Define.Object("Query", [
    Define.NodeField(Node, fun ctx () id ->
        match id with
        | GlobalId("User", i) -> getUser i |> Option.map box
        | GlobalId("Widget", i) -> getWidget i |> Option.map box
        | _ -> None)
    Define.Field("viewer", User, fun _ () -> viewer)])

let schema = Schema(query = Query, config = { SchemaConfig.Default with Types = [ User; Widget ]})

// Utils ---------------------------------------------------------------------------------------------------------------

open Newtonsoft.Json

let settings = JsonSerializerSettings()
settings.ContractResolver <- Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
let json o = JsonConvert.SerializeObject(o, settings)

let execute (query:string) =
    async {
        let q = query.Trim().Replace("\r\n", " ")
        let! result = Executor(schema).AsyncExecute(q)
        let serialized = json result
        return serialized
    }

// Endpoints -----------------------------------------------------------------------------------------------------------

open Falco
open Falco.Routing
open Falco.HostBuilder

/// GET /
let handleHome : HttpHandler =
    Response.ofPlainText "It's alive!"

/// GraphQL endpoint
let handleGraphQL : HttpHandler = fun ctx ->
    let task = Request.getBodyString ctx
    task.Wait()
    let body = task.Result
    let response = execute body |> Async.RunSynchronously
    Response.ofPlainText $"{response}" ctx

[<EntryPoint>]
let main args =
    webHost args {
        endpoints [
            get "/" handleHome            // http://localhost:8080
            post "/graphql" handleGraphQL // http://localhost:8080/graphql
        ]
    }

    0 // Exit code
