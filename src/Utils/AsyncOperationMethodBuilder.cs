namespace NearShare.Windows.Utils;

// https://github.com/microsoft/CsWinRT/issues/2044

/// <summary>
/// Provides a builder for asynchronous methods that return <see cref="global::Windows.Foundation.IAsyncOperation{TResult}"/>.
/// This type is intended for compiler use only.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="AsyncOperationMethodBuilder{TResult}"/> struct.
/// </remarks>
/// <param name="builder">The underlying builder.</param>
public struct AsyncOperationMethodBuilder<TResult>()
{
    /// <summary>
    /// The underlying builder.
    /// </summary>
    private global::System.Runtime.CompilerServices.AsyncTaskMethodBuilder<TResult> _builder = global::System.Runtime.CompilerServices.AsyncTaskMethodBuilder<TResult>.Create();

    /// <summary>
    /// Initializes a new <see cref="AsyncOperationMethodBuilder{TResult}"/>.
    /// </summary>
    /// <returns>The initialized <see cref="AsyncOperationMethodBuilder{TResult}"/>.</returns>
    public static AsyncOperationMethodBuilder<TResult> Create() => default;

    /// <summary>
    /// Initiates the builder's execution with the associated state machine.
    /// </summary>
    /// <typeparam name="TStateMachine">Specifies the type of the state machine.</typeparam>
    /// <param name="stateMachine">The state machine instance, passed by reference.</param>
    [global::System.Diagnostics.DebuggerStepThrough]
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : global::System.Runtime.CompilerServices.IAsyncStateMachine
    {
        _builder.Start(ref stateMachine);
    }

    /// <summary>
    /// Associates the builder with the state machine it represents.
    /// </summary>
    /// <param name="stateMachine">The heap-allocated state machine object.</param>
    /// <exception cref="global::System.ArgumentNullException">The <paramref name="stateMachine"/> argument was null (<see langword="Nothing" /> in Visual Basic).</exception>
    /// <exception cref="global::System.InvalidOperationException">The builder is incorrectly initialized.</exception>
    public void SetStateMachine(global::System.Runtime.CompilerServices.IAsyncStateMachine stateMachine)
    {
        _builder.SetStateMachine(stateMachine);
    }

    /// <summary>
    /// Schedules the specified state machine to be pushed forward when the specified awaiter completes.
    /// </summary>
    /// <typeparam name="TAwaiter">Specifies the type of the awaiter.</typeparam>
    /// <typeparam name="TStateMachine">Specifies the type of the state machine.</typeparam>
    /// <param name="awaiter">The awaiter.</param>
    /// <param name="stateMachine">The state machine.</param>
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : global::System.Runtime.CompilerServices.INotifyCompletion
        where TStateMachine : global::System.Runtime.CompilerServices.IAsyncStateMachine
    {
        _builder.AwaitOnCompleted(ref awaiter, ref stateMachine);
    }

    /// <summary>
    /// Schedules the specified state machine to be pushed forward when the specified awaiter completes.
    /// </summary>
    /// <typeparam name="TAwaiter">Specifies the type of the awaiter.</typeparam>
    /// <typeparam name="TStateMachine">Specifies the type of the state machine.</typeparam>
    /// <param name="awaiter">The awaiter.</param>
    /// <param name="stateMachine">The state machine.</param>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : global::System.Runtime.CompilerServices.ICriticalNotifyCompletion
        where TStateMachine : global::System.Runtime.CompilerServices.IAsyncStateMachine
    {
        _builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
    }

    /// <summary>
    /// Gets the <see cref="global::Windows.Foundation.IAsyncOperation{TResult}"/> for this builder.
    /// </summary>
    /// <returns>The <see cref="global::Windows.Foundation.IAsyncOperation{TResult}"/> representing the builder's asynchronous operation.</returns>
    public global::Windows.Foundation.IAsyncOperation<TResult> Task
    {
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        get
        {
            return global::System.WindowsRuntimeSystemExtensions.AsAsyncOperation<TResult>(_builder.Task);
        }
    }

    /// <summary>
    /// Completes the <see cref="global::Windows.Foundation.IAsyncAction"/> in the
    /// <see cref="TaskStatus">RanToCompletion</see> state.
    /// </summary>
    /// <param name="result">The result to use to complete the task.</param>
    /// <exception cref="global::System.InvalidOperationException">The task has already completed.</exception>
    public void SetResult(TResult result)
    {
        _builder.SetResult(result);
    }

    /// <summary>
    /// Completes the <see cref="global::Windows.Foundation.IAsyncAction"/> in the
    /// <see cref="TaskStatus">Faulted</see> state with the specified exception.
    /// </summary>
    /// <param name="exception">The <see cref="global::System.Exception"/> to use to fault the task.</param>
    /// <exception cref="global::System.ArgumentNullException">The <paramref name="exception"/> argument is null (<see langword="Nothing" /> in Visual Basic).</exception>
    /// <exception cref="global::System.InvalidOperationException">The task has already completed.</exception>
    public void SetException(global::System.Exception exception)
    {
        _builder.SetException(exception);
    }
}
