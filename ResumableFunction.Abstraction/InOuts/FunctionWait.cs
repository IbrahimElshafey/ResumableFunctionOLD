﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ResumableFunction.Abstraction.InOuts
{
    public sealed class FunctionWait : Wait
    {
        public FunctionWait() : base()
        {

        }
        public FunctionWait(string eventIdentifier, Func<IAsyncEnumerable<Wait>> function)
        {
            EventIdentifier = eventIdentifier;
            FunctionName = function.Method.Name;
        }
        public Wait ParentFunctionGroup { get; internal set; }
        
        [ForeignKey(nameof(ParentFunctionGroup))]
        public int? ParentFunctionGroupId { get; internal set; }
        public Wait CurrentWait { get; internal set; }
        public string FunctionName { get; internal set; }
    }
}
