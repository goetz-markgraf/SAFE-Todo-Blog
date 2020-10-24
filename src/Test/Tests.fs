namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting

open Server
open Shared

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.CreateNewId () =
        let model = [{ Id = 2; Description = "Todo 1"; Completed = false}]
        let result = Todos.newId model
        Assert.AreEqual(3, result)

    [<TestMethod>]
    member this.AddTodo () =
        let model = [{ Id = 1; Description = "Todo 1"; Completed = false}]
        let result = Todos.addTodo model "Todo 2"
        printfn "Model %A" result
        Assert.AreEqual(2, List.length result)
    
    [<TestMethod>]
    member this.``Shall toggle Complete State`` () =
        let model = [{ Id = 1; Description = "Description"; Completed = false}]
        let result = Todos.toggleComplete model 1
        let expected = [{ Id = 1; Description = "Description"; Completed = true}]
        Assert.AreEqual(expected, result)
