namespace Shared

type Todo =
    {
        Id: int
        Description: string
        Completed: bool
    }

module Route =
    let todos = "/api/todos"