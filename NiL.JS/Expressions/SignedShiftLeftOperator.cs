﻿using System;
using System.Collections.Generic;
using NiL.JS.Core;

namespace NiL.JS.Expressions
{
#if !PORTABLE
    [Serializable]
#endif
    public sealed class SignedShiftLeftOperator : Expression
    {
        protected internal override PredictedType ResultType
        {
            get
            {
                return PredictedType.Int;
            }
        }

        internal override bool ResultInTempContainer
        {
            get { return true; }
        }

        public SignedShiftLeftOperator(Expression first, Expression second)
            : base(first, second, true)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            tempContainer.iValue = (int)(Tools.JSObjectToInt32(first.Evaluate(context)) << Tools.JSObjectToInt32(second.Evaluate(context)));
            tempContainer.valueType = JSValueType.Int;
            return tempContainer;
        }

        internal protected override bool Build(ref CodeNode _this, int expressionDepth, List<string> scopeVariables, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, CompilerMessageCallback message, FunctionStatistics stats, Options opts)
        {
            var res = base.Build(ref _this, expressionDepth, scopeVariables, variables, codeContext, message, stats, opts);
            if (!res && _this == this)
            {
                try
                {
                    if (first.ContextIndependent && Tools.JSObjectToInt32((first).Evaluate(null)) == 0)
                        _this = new ConstantDefinition(0);
                    else if (second.ContextIndependent && Tools.JSObjectToInt32((second).Evaluate(null)) == 0)
                        _this = new ToIntegerOperator(first);
                }
                catch
                {

                }
            }
            return res;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + first + " << " + second + ")";
        }
    }
}