﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Serialize.Linq.Extensions
{
    internal static class ExpressionExtensions
    {
        public static IEnumerable<Expression> GetLinkNodes(this Expression expression)
        {
            if (expression is LambdaExpression)
            {
                var lambdaExpression = (LambdaExpression)expression;

                yield return lambdaExpression.Body;
                foreach (var parameter in lambdaExpression.Parameters)
                    yield return parameter;
            }
            else if (expression is BinaryExpression)
            {
                var binaryExpression = (BinaryExpression)expression;

                yield return binaryExpression.Left;
                yield return binaryExpression.Right;
            }
            else if (expression is ConditionalExpression)
            {
                var conditionalExpression = (ConditionalExpression)expression;

                yield return conditionalExpression.IfTrue;
                yield return conditionalExpression.IfFalse;
                yield return conditionalExpression.Test;
            }
            else if (expression is InvocationExpression)
            {
                var invocationExpression = (InvocationExpression)expression;
                yield return invocationExpression.Expression;
                foreach (var argument in invocationExpression.Arguments)
                    yield return argument;                
            }
            else if (expression is ListInitExpression)
            {
                yield return (expression as ListInitExpression).NewExpression;
            }
            else if (expression is MemberExpression)
            {
                yield return (expression as MemberExpression).Expression;
            }
            else if (expression is MemberInitExpression)
            {
                yield return (expression as MemberInitExpression).NewExpression;
            }
            else if (expression is MethodCallExpression)
            {
                var methodCallExpression = (MethodCallExpression)expression;
                foreach (var argument in methodCallExpression.Arguments)
                    yield return argument;
                if (methodCallExpression.Object != null)                
                    yield return methodCallExpression.Object;                
            }
            else if (expression is NewArrayExpression)
            {
                foreach (var item in (expression as NewArrayExpression).Expressions)
                    yield return item;
            }
            else if (expression is NewExpression)
            {
                foreach (var item in (expression as NewExpression).Arguments)
                    yield return item;
            }
            else if (expression is TypeBinaryExpression)
            {
                yield return (expression as TypeBinaryExpression).Expression;
            }
            else if (expression is UnaryExpression)
            {
                yield return (expression as UnaryExpression).Operand;
            }
        }

        public static IEnumerable<Expression> GetNodes(this Expression expression)
        {
            foreach (var node in expression.GetLinkNodes())
            {
                foreach (var subNode in node.GetNodes())
                    yield return subNode;
            }
            yield return expression;
        }

        public static IEnumerable<TExpression> GetNodes<TExpression>(this Expression expression) where TExpression : Expression
        {
            return expression.GetNodes().OfType<TExpression>();
        }
    }
}
