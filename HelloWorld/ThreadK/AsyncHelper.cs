﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorld.ThreadK
{
    /// <summary>
    /// 异步调用完成后的回调委托
    /// </summary>
    /// <typeparam name="TAsyncResult"></typeparam>
    /// <param name="arg1">自定义方法（同步执行）</param>
    /// <param name="arg2">异步调用结果</param>
    public delegate void AsyncAction<TAsyncResult>(Action arg1, TAsyncResult arg2)
    where TAsyncResult : AsyncResult;

    /// <summary>
    /// 异步调用完成后的回调委托
    /// </summary>
    /// <typeparam name="TAsyncResult"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="arg1">自定义方法（同步执行）</param>
    /// <param name="arg2">异步调用结果</param>
    public delegate void AsyncAction<TAsyncResult, T>(Func<T> arg1, TAsyncResult arg2)
        where TAsyncResult : AsyncResult;

    /// <summary>
    /// 异步结果事件
    /// </summary>
    public class AsyncResult
    {
        /// <summary>
        /// 异步调用产生的异常
        /// </summary>
        public Exception AsyncException { get; } = null;
        /// <summary>
        /// 异步调用完成后的同步调用产生的异常
        /// </summary>
        public Exception SyncException { get; } = null;
        /// <summary>
        /// 结果
        /// </summary>
        public AsyncResultOptions Result { get; }
        public object Data { get; } = null;
        /// <summary>
        /// 预留
        /// </summary>
        public object Tag { get; } = null;

        public AsyncResult(AsyncResultOptions resultOptions)
        {
            Result = resultOptions;
        }
    }

    /// <summary>
    /// 异步结果
    /// </summary>
    public enum AsyncResultOptions
    {
        Completed = 0,
        Cancled = 1,
        Running = 2,
        Fault = 3,
        Unknown = 4,
    }

    public static class AsyncHelper
    {

        private static ConcurrentDictionary<string, AsyncOperation> _asyncOpDic = new ConcurrentDictionary<string, AsyncOperation>();

        /// <summary>
        /// 运行异步方法，完成事件与调用线程运上下文一致
        /// Task实现
        /// </summary>
        /// <param name="action"></param>
        /// <param name="callBack"></param>
        public static void RunAsync(Action action, Action<Action, AsyncResult> callBack = null)
        {
            if (action == null)
                return;
            try
            {
                string guid = null;
                if (callBack != null)
                {
                    var op = AsyncOperationManager.CreateOperation(callBack);
                    AddNewAsyncOperation(op, out guid);
                }
                var task = new Task(() =>
                {
                    action?.Invoke();
                });
                if (callBack != null)
                {
                    task.ContinueWith((t, obj) =>
                    {
                        InvokeSyncOperation(guid);
                    }, task);
                }
                task.Start();
            }
            catch (Exception ex)
            {
                //TODO:
                //记录runasync 异常
            }
        }

        private static void InvokeSynvOperation(string key, Task t)
        {
            try
            {
                AsyncOperation op;
                if (TakeAsyncOperation(key, out op))
                {
                    if (op.UserSuppliedState is Action)
                    {
                        op.Post((obj) => { (op.UserSuppliedState as Action).Invoke(); }, null);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:record log
            }
        }

        private static void InvokeSyncOperation(string key)
        {
            try
            {
                AsyncOperation op;
                if (TakeAsyncOperation(key, out op))
                {
                    if (op.UserSuppliedState is Action)
                    {
                        op.Post((obj) => { (op.UserSuppliedState as Action).Invoke(); }, null);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:record log
            }
        }

        private static void InvokeSyncOperation(string key, Action callBack)
        {
            try
            {
                AsyncOperation op2;
                if (TakeAsyncOperation(key, out op2))
                {
                    op2.Post((obj) => { callBack(); }, null);
                }
            }
            catch (Exception ex)
            {
                //TODO:record log
            }
        }

        public static void AddNewAsyncOperation(AsyncOperation op, out string guid)
        {
            guid = null;
            if (op != null)
            {
                guid = Guid.NewGuid().ToString();
                if (!_asyncOpDic.TryAdd(guid, op))
                {
                    AddNewAsyncOperation(op, out guid);
                }
            }
        }

        public static bool TakeAsyncOperation(string key, out AsyncOperation operation)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return _asyncOpDic.TryRemove(key, out operation);
            }
            operation = null;
            return false;
        }

        public static bool FindAsyncOpeartion(string key, out AsyncOperation operation)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return _asyncOpDic.TryGetValue(key, out operation);
            }
            operation = null;
            return false;
        }

        public static void RemoveOpeartion(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                AsyncOperation op;
                _asyncOpDic.TryRemove(key, out op);
            }
        }

        public static void RunAsync_Threadpool(Action action, Action callBack = null)
        {

        }

        private static ConcurrentDictionary<string, EventHandler> _eventDic;

        public static void Run(Action action, object para, Action<object> callback)
        {

        }

        /// <summary>
        /// 异步执行一个具有TResult返回值的方法
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Task<TResult> Run<TResult>(Func<TResult> func)
        {
            var taskc = new TaskCompletionSource<TResult>();
            Thread t = new Thread(() =>
            {
                try
                {
                    TResult result;
                    if (func != null)
                    {
                        result = func.Invoke();
                        taskc.SetResult(result);
                    }
                }
                catch (Exception ex)
                {
                    taskc.SetException(ex);
                }
            });
            t.IsBackground = true;
            t.Start();
            return taskc.Task;
        }
    }

}
