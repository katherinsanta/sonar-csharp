/*
 * SonarAnalyzer for .NET
 * Copyright (C) 2015-2017 SonarSource SA
 * mailto: contact AT sonarsource DOT com
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SonarAnalyzer.Helpers;

namespace SonarAnalyzer.Metrics.CSharp
{
    public class ExecutableLinesCalculator
    {
        public int CountLines(SyntaxTree tree)
        {
            var nodesToVisit = new Queue<SyntaxNode>(tree.GetRoot().ChildNodes());
            var executableLineNumbers = new HashSet<int>();

            while (nodesToVisit.Count > 0)
            {
                Visit(nodesToVisit, executableLineNumbers);
            }

            return executableLineNumbers.Count;
        }

        private static void Visit(Queue<SyntaxNode> nodesToVisit, HashSet<int> executableLineNumbers)
        {
            var node = nodesToVisit.Dequeue();

            foreach (var n in node.ChildNodes())
            {
                nodesToVisit.Enqueue(n);
            }

            var kind = node.Kind();
            switch (kind)
            {
                case SyntaxKind.CheckedStatement:
                case SyntaxKind.UncheckedStatement:

                case SyntaxKind.LockStatement:
                case SyntaxKind.FixedStatement:
                case SyntaxKind.UnsafeStatement:
                case SyntaxKind.UsingStatement:

                case SyntaxKind.EmptyStatement:
                case SyntaxKind.ExpressionStatement:

                case SyntaxKind.DoStatement:
                case SyntaxKind.ForEachStatement:
                case SyntaxKind.ForStatement:
                case SyntaxKind.WhileStatement:

                case SyntaxKind.IfStatement:
                case SyntaxKind.LabeledStatement:
                case SyntaxKind.SwitchStatement:
                case SyntaxKind.ConditionalAccessExpression:
                case SyntaxKind.ConditionalExpression:

                case SyntaxKind.GotoStatement:
                case SyntaxKind.ThrowStatement:
                case SyntaxKind.ReturnStatement:
                case SyntaxKind.BreakStatement:

                case SyntaxKind.YieldBreakStatement:
                case SyntaxKind.YieldReturnStatement:

                case SyntaxKind.SimpleMemberAccessExpression:
                case SyntaxKind.InvocationExpression:

                case SyntaxKind.ArrayInitializerExpression:
                    executableLineNumbers.Add(node.GetLocation().GetLineNumberToReport());
                    return;

                case SyntaxKind.ObjectInitializerExpression:
                    EnqeueChildExpressions(node, nodesToVisit);
                    return;

                default:
                    return;
            }
        }

        private static void EnqeueChildExpressions(SyntaxNode node, Queue<SyntaxNode> nodesToVisit)
        {
            var childExpressions = ((InitializerExpressionSyntax)node).Expressions;
            foreach (var expression in childExpressions)
            {
                nodesToVisit.Enqueue(expression);
            }
        }
    }
}
