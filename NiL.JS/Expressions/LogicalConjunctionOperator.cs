﻿using System;
using System.Collections.Generic;
using NiL.JS.Core;

namespace NiL.JS.Expressions
{
#if !PORTABLE
    [Serializable]
#endif
    public sealed class LogicalConjunctionOperator : Expression
    {
        protected internal override PredictedType ResultType
        {
            get
            {
                return PredictedType.Bool;
            }
        }

        internal override bool ResultInTempContainer
        {
            get { return false; }
        }

        public LogicalConjunctionOperator(Expression first, Expression second)
            : base(first, second, false)
        {

        }

        public override JSValue Evaluate(Context context)
        {
            var left = first.Evaluate(context);
            if (!(bool)left)
                return left;
            else
                return second.Evaluate(context);
        }

        internal protected override bool Build(ref CodeNode _this, int expressionDepth, List<string> scopeVariables, System.Collections.Generic.Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, CompilerMessageCallback message, FunctionStatistics stats, Options opts)
        {
            if (message != null && expressionDepth <= 1)
                message(MessageLevel.Warning, new CodeCoordinates(0, Position, 0), "Do not use logical operator as a conditional statement");
            return base.Build(ref _this, expressionDepth, scopeVariables, variables, codeContext | CodeContext.Conditional, message, stats, opts);
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + first + " && " + second + ")";
        }
    }
}