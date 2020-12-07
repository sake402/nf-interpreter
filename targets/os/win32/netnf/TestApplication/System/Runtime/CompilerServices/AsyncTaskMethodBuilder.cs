
using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    public struct AsyncTaskMethodBuilder
    {
        public static AsyncTaskMethodBuilder Create()
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:Create");
            return new AsyncTaskMethodBuilder();
        }

        Task task;
        public Task Task => task;
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:AwaitOnCompleted");
            var _stateMachine = stateMachine;
            awaiter.OnCompleted(() =>
            {
                Debug.WriteLine($"AsyncTaskMethodBuilder:OnCompleted");
                _stateMachine.MoveNext();
            });
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:AwaitUnsafeOnCompleted");
            var _stateMachine = stateMachine;
            awaiter.OnCompleted(() =>
            {
                Debug.WriteLine($"AsyncTaskMethodBuilder:OnCompleted");
                _stateMachine.MoveNext();
            });
        }

        public void SetException(Exception exception)
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:SetException");
        }
        public void SetResult()
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:SetResult");
        }
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:SetStateMachine");
        }
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:Start");
            task = new Task();
            stateMachine.MoveNext();
        }
    }

    public struct AsyncTaskMethodBuilder<TResult>
    {
        public static AsyncTaskMethodBuilder<TResult> Create()
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:Create");
            var m = new AsyncTaskMethodBuilder<TResult>();
            Debug.WriteLine($"AsyncTaskMethodBuilder:Create:End");
            return m;
        }

        Task<TResult> task;
        public Task<TResult> Task => task;
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:AwaitOnCompleted");
            var _stateMachine = stateMachine;
            awaiter.OnCompleted(() =>
            {
                Debug.WriteLine($"AsyncTaskMethodBuilder:OnCompleted");
                _stateMachine.MoveNext();
            });
        }
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:AwaitUnsafeOnCompleted");
            var _stateMachine = stateMachine;
            awaiter.OnCompleted(() =>
            {
                Debug.WriteLine($"AsyncTaskMethodBuilder:OnCompleted");
                _stateMachine.MoveNext();
            });
        }
        public void SetException(Exception exception)
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:SetException");
            task?.CompleteWithException(exception);
        }
        public void SetResult(TResult result)
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:SetResult");
            task?.Complete(result);
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:SetStateMachine");

        }
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            Debug.WriteLine($"AsyncTaskMethodBuilder:Start");
            task = new Task<TResult>();
            stateMachine.MoveNext();
        }
    }
}
