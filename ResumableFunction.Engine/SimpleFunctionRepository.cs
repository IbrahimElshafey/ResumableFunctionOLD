﻿using ResumableFunction.Engine.Abstraction;

namespace ResumableFunction.Engine
{
    public class SimpleFunctionRepository : IFunctionRepository
    {
        public Task<FunctionData> GetFunctionData<FunctionData>(Guid instanceId, string functionName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsFunctionRegistred(Type functionType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveFunctionData<FunctionData>(FunctionData args, Guid instanceId, string FunctionName)
        {
            throw new NotImplementedException();
        }
    }
}
