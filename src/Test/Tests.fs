module Tests

open Xunit

open Shared
open Server

[<Fact>]
let ``create new Id`` () =
    let model = [{ Id = 2; Description = "Todo 1"; Completed = false}]
    let result = Todos.newId model
    Assert.Equal(3, result)

[<Fact>]
let ``add todo`` () =
    let model = [{ Id = 1; Description = "Todo 1"; Completed = false}]
    let result = Todos.addTodo model "Todo 2"
    let expected = { Id = 2; Description = "Todo 2"; Completed = false}
    Assert.Contains(expected, result)

[<Fact>]
let ``Shall toggle Complete State`` () =
    let model = [{ Id = 1; Description = "Description"; Completed = false}]
    let result = Todos.toggleComplete model 1
    let expected = [{ Id = 1; Description = "Description"; Completed = true}]
    Assert.Equal<Todo list>(expected, result)
