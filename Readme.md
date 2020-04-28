## Awaitables

This is an experimental repo designed to highlight both the possibilities, and limitations, of using async await for monads other than Task like types.

### Async State Machinery

You can find documentation on task like types at https://github.com/dotnet/roslyn/blob/master/docs/features/task-types.md. Whilst the extension points to allow a task to be awaited or used as the return type of an async method are designed around task based scenarios, they can be abused for a much larger selection of monads.

### Implemented Awaitables

- `Option<T>`
- `Result<T>`
- `AwaitableEnumerable<T>`

### Examples

You can see example dummy programs here:

https://github.com/YairHalberstadt/awaitables/blob/master/Awaitables.Option.Example/Program.cs
https://github.com/YairHalberstadt/awaitables/blob/master/Awaitables.Result.Example/Program.cs
https://github.com/YairHalberstadt/awaitables/blob/master/Awaitables.Enumerable.Example/Program.cs

### Limitations

All three implementations have only limited testing and carry out only limited validation, as they were designed to highlight the possibilities of state machine based rewriting in C#, rather than to be used in production.

However all three of them *almost work*. `Result<T>` and `Option<T>` are allocation free and highly efficient, and `AwaitableEnumerable<T>` only allocates the enumerable required to store the result of the method. There is just one critical limitation they all share, and which the language currently provides no way around: they do not respect the semantics of try/catch/finally or using blocks.

The `AwaitableEnumerable<T>` returned from an async method is eager rather than lazy - currently making it lazy would require a significantly greater number of allocations, though would be possible.

### Contributing

Feel free to contribute your own awaitables, or to address any of the existing limitations!
