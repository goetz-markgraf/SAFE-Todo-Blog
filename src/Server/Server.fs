module Server

open Giraffe
open Saturn

open Shared

let mutable database = [
    {
        Id = 1
        Description = "Read all todos"
        Completed = true
    }
    {
        Id = 2
        Description = "Add a new todo"
        Completed = false
    }
]

let webApp =
    router {
        get Route.todos (fun next ctx -> json database next ctx)
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
