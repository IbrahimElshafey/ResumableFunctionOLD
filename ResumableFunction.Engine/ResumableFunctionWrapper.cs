﻿using ResumableFunction.Abstraction;
using ResumableFunction.Abstraction.InOuts;
using ResumableFunction.Engine.InOuts;
using System.Linq.Expressions;
using System.Reflection;

namespace ResumableFunction.Engine
{
    public class ResumableFunctionWrapper : IResumableFunction<object>
    {
        private readonly EventWait _currentWait;
        private object _activeRunner;
        public ResumableFunctionWrapper(EventWait eventWait)
        {
            _currentWait = eventWait;
            var functionClassType = _currentWait.ParentFunctionState.InitiatedByClass;
            var isResumableFunctionClass = functionClassType.IsSubclassOfRawGeneric(typeof(ResumableFunction<>));
            if (isResumableFunctionClass is false)
                throw new Exception("functionClassType must inherit ResumableFunction<>");

            FunctionClassInstance = Activator.CreateInstance(functionClassType);
            if (FunctionClassInstance is null)
                throw new Exception($"Can't create instance of `{functionClassType}` with .ctor()"); ;
            //if (FunctionClassInstance.Data == null)
            //{
            //    var propType = functionClassType.GetProperty("Data").PropertyType;
            //    Data = Activator.CreateInstance(propType);
            //}
            FunctionState = eventWait.ParentFunctionState;
            Data = eventWait.ParentFunctionState.Data;
        }


        public dynamic Data
        {
            get => FunctionClassInstance.Data;
            private set => FunctionClassInstance.Data = value;
        }

        public ResumableFunctionState FunctionState
        {
            get => FunctionClassInstance.FunctionState;
            private set => FunctionClassInstance.FunctionState = value;
        }

        public dynamic FunctionClassInstance { get; private set; }

        public Task OnFunctionEnd()
        {
            return FunctionClassInstance.OnFunctionEnd();
        }

        /// <summary>
        /// called by the engine after event received
        /// </summary>
        public void UpdateDataWithEventData()
        {
            _currentWait.SetDataProp();
            Data = _currentWait.ParentFunctionState.Data;
            FunctionState.Data = Data;
        }

        /// <summary>
        /// called by the engine to resume function execution
        /// </summary>
        public async Task<NextWaitResult> GetNextWait()
        {
            var functionRunner = GetCurrentRunner();

            if (functionRunner is null)
                throw new Exception(
                    $"Can't initiate runner `{_currentWait.InitiatedByFunction}` for {_currentWait.EventDataType.FullName}");
            SetActiveRunnerState(CurrentRunnerLastState());
            bool waitExist = await functionRunner.MoveNextAsync();
            //after function resumed data may be changed (for example user set some props)
            FunctionState.Data = Data;
            //update current runner state
            if (FunctionState.FunctionsStates.ContainsKey(_currentWait.InitiatedByFunction))
                FunctionState.FunctionsStates[_currentWait.InitiatedByFunction] = GetActiveRunnerState();
            else
                FunctionState.FunctionsStates.Add(_currentWait.InitiatedByFunction, GetActiveRunnerState());

            if (waitExist)
                return new NextWaitResult(functionRunner.Current, false, false);
            else
            {
                //if current Function runner name is the main function start
                if (_currentWait.InitiatedByFunction == nameof(Start))
                {
                    await OnFunctionEnd();
                    return new NextWaitResult(null, true, true);
                }
                return new NextWaitResult(null, false, true);
            }
        }



        private void SetActiveRunnerState(int state)
        {
            if (_activeRunner != null || GetCurrentRunner() != null)
            {
                var stateField = _activeRunner?.GetType().GetField("<>1__state");
                if (stateField != null)
                {

                    stateField.SetValue(_activeRunner, state);
                }
            }
        }
        private int GetActiveRunnerState()
        {
            if (_activeRunner != null || GetCurrentRunner() != null)
            {
                var stateField = _activeRunner?.GetType().GetField("<>1__state");
                if (stateField != null)
                {
                    return (int)stateField.GetValue(_activeRunner);
                }
            }
            return int.MinValue;
        }

        private IAsyncEnumerator<Wait>? GetCurrentRunner()
        {
            if (_activeRunner == null)
            {
                var FunctionRunnerType = FunctionState.InitiatedByClass
                   .GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SuppressChangeType)
                   .FirstOrDefault(x => x.Name.StartsWith(CurrentRunnerName()));

                if (FunctionRunnerType == null) return null;
                ConstructorInfo? ctor = FunctionRunnerType.GetConstructor(
                   BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance,
                   new Type[] { typeof(int) });

                if (ctor == null) return null;
                _activeRunner = ctor.Invoke(new object[] { -2 });

                if (_activeRunner == null) return null;
                //set parent class who call
                var thisField = FunctionRunnerType.GetFields().FirstOrDefault(x => x.Name.EndsWith("__this"));
                //var thisField = FunctionRunnerType.GetField("<>4__this");
                thisField?.SetValue(_activeRunner, FunctionClassInstance);

                //set in start state
                SetActiveRunnerState(int.MinValue);
                return _activeRunner as IAsyncEnumerator<Wait>;
            }
            return _activeRunner as IAsyncEnumerator<Wait>;


        }

        private string CurrentRunnerName()
        {
            return $"<{_currentWait.InitiatedByFunction}>";
        }

        private int CurrentRunnerLastState()
        {
            int result = int.MinValue;
            FunctionState?.FunctionsStates.TryGetValue(_currentWait.InitiatedByFunction, out result);
            return result;
        }

        public IAsyncEnumerable<Wait> Start()
        {
            return FunctionClassInstance.Start();
        }
    }
}