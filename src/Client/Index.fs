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
    | ToggleCompleted of int

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
    | ToggleCompleted id ->
        let url = sprintf "/api/todos/%i/toggle_complete" id
        let newModel id = Fetch.put<unit, Todo list> url
        let cmd = Cmd.OfPromise.either newModel () Refresh Error
        model, cmd
        

open Fable.React
open Fable.React.Props
open Fable.Core.JsInterop


let errorView model =
    match model.Error with
    | "" -> div [] []
    | s -> p [ ] [ str s ]



let todoView todo dispatch =
    div [] [
        input [
            Type "checkbox"
            Checked todo.Completed
            OnClick (fun _ -> todo.Id |> ToggleCompleted |> dispatch)
        ]
        label [ ] [str todo.Description]
    ]

let todosView model dispatch =
    div [] ( model.Todos |> List.map (fun each ->
        todoView each dispatch))


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
