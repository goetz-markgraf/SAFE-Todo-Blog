module Index

open Elmish
open Thoth.Fetch

open Shared

type Model =
    {
        Todos: Todo list
        Error: string
    }

type Msg =
    | Load
    | Refresh of Todo list
    | Error of exn

let init() =
    { Todos = []; Error = "" }, Cmd.ofMsg Load


let update msg model =
    match msg with
    | Load ->
        let loadTodos() = Fetch.get<unit, Todo list> Route.todos
        let cmd = Cmd.OfPromise.either loadTodos () Refresh Error
        model, cmd
    | Refresh todos ->
        { model with Todos = todos}, Cmd.none
    | Error err ->
        { model with Error = err.Message }, Cmd.none
        

open Fable.React
open Fable.React.Props

let view model dispatch =
    div [ Style [ TextAlign TextAlignOptions.Center; Padding 40 ] ] [
        div [] [
            img [ Src "favicon.png" ]
            h1 [] [ str (sprintf "Todos: %i" model.Todos.Length) ]
            match model.Error with
            | "" -> div [] []
            | s -> p [ ] [ str s ]
            div [] ( model.Todos |> List.map (fun each -> p [] [str each.Description]))
        ]
    ]
