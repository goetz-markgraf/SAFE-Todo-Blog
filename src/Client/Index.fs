module Index

open Elmish
open Thoth.Fetch

open Shared

type Model =
    {
        Todos: Todo list
        Error: string
        Description: string
    }

type Msg =
    | Load
    | Refresh of Todo list
    | Error of exn
    | DescriptionChanged of string
    | AddTodo

let init() =
    { Todos = []; Error = ""; Description = "" }, Cmd.ofMsg Load



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
    | DescriptionChanged s ->
        { model with Description = s}, Cmd.none
    | AddTodo ->
        let newModel description = Fetch.post<string, Todo list> (Route.todos, description)
        let cmd = Cmd.OfPromise.either newModel model.Description Refresh Error
        { model with Description = "" } , cmd
        

open Fable.React
open Fable.React.Props
open Fable.Core.JsInterop


let errorView model =
    match model.Error with
    | "" -> div [] []
    | s -> p [ ] [ str s ]


let todosView model dispatch =
    div [] ( model.Todos |> List.map (fun each ->
        p [ ] [str each.Description]))


let descriptionView model dispatch =
    div [] [
        input [
            Placeholder "What is to be done?"
            Value (model.Description)
            OnChange (fun ev -> !!ev.target?value |> DescriptionChanged |> dispatch)
        ]
        button [
            OnClick (fun _ -> AddTodo |> dispatch)
        ] [ str "Add" ]
    ]

let view model dispatch =
    div [ Style [ TextAlign TextAlignOptions.Center; Padding 40 ] ] [
        div [] [
            img [ Src "favicon.png" ]
            h1 [] [ str (sprintf "Todos: %i" model.Todos.Length) ]
            
            descriptionView model dispatch
            errorView model
            todosView model dispatch            
        ]
    ]
