module Server

open Giraffe
open Saturn
open FSharp.Control.Tasks.V2.ContextInsensitive

open Shared

module Database =

    let mutable private database: Todo list = []
    
    let getAll () =
        database

    let init() =
        database <- [
        {
            Id = 1
            Description = "Read all todos"
            Completed = true
        }
        {
            Id = 2
            Description = "Add a new todo"
            Completed = true
        }
        {
            Id = 3
            Description = "Toggle State"
            Completed = false
        }
    ]

    let save model =
        database <- model

    do init()




module Todos =

    let newId model =
        model
            |> List.map (fun each -> each.Id)
            |> List.max
            |> (+) 1

    let addTodo model description =
        let id = newId model
        let newTodo = { Description = description; Completed = false; Id = id }
        newTodo :: model
        






let webApp =
    router {
        get Route.todos (fun next ctx ->
            json (Database.getAll()) next ctx)
        post Route.todos (fun next ctx -> 
            task {
                let! description = ctx.BindModelAsync<string>()
                let model = Todos.addTodo (Database.getAll()) description
                Database.save model
                return! json model next ctx
            })
    }

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router webApp
        memory_cache
        use_static "public"
        use_json_serializer (Thoth.Json.Giraffe.ThothSerializer())
        use_gzip
    }


run app
