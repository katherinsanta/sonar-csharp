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
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SonarAnalyzer.Common;

namespace SonarAnalyzer.Helpers
{
    public sealed class ExecutableLinesWalker : CSharpSyntaxWalker, IMetricsWalker
    {
        private readonly Dictionary<int, SecondaryLocation> executableLines = new Dictionary<int, SecondaryLocation>();

        public int MetricValue => executableLines.Count;

        public bool Success => true;

        public IEnumerable<SecondaryLocation> Locations => executableLines.Values;

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            AddExecutableLocations(node
                .Declaration
                .Variables
                .Where(v => v.Initializer != null)
                .Select(v => v.GetLocation()));

            base.VisitLocalDeclarationStatement(node);
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            base.VisitExpressionStatement(node);
        }

        public override void VisitGotoStatement(GotoStatementSyntax node)
        {
            base.VisitGotoStatement(node);
        }

        public override void VisitBreakStatement(BreakStatementSyntax node)
        {
            AddExecutableLocation(node.GetLocation());
            base.VisitBreakStatement(node);
        }

        public override void VisitContinueStatement(ContinueStatementSyntax node)
        {
            AddExecutableLocation(node.GetLocation());
            base.VisitContinueStatement(node);
        }

        public override void VisitReturnStatement(ReturnStatementSyntax node)
        {
            AddExecutableLocation(node.GetLocation());
            base.VisitReturnStatement(node);
        }

        public override void VisitThrowStatement(ThrowStatementSyntax node)
        {
            base.VisitThrowStatement(node);
        }

        public override void VisitYieldStatement(YieldStatementSyntax node)
        {
            base.VisitYieldStatement(node);
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            AddExecutableLocation(node.GetLocation());
            base.VisitWhileStatement(node);
        }

        public override void VisitDoStatement(DoStatementSyntax node)
        {
            AddExecutableLocation(node.GetLocation());
            base.VisitDoStatement(node);
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            AddExecutableLocation(node.GetLocation());
            base.VisitForStatement(node);
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            AddExecutableLocation(node.GetLocation());
            base.VisitForEachStatement(node);
        }

        public override void VisitLockStatement(LockStatementSyntax node)
        {
            base.VisitLockStatement(node);
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            base.VisitIfStatement(node);
        }

        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            base.VisitSwitchStatement(node);
        }

        public override void VisitTryStatement(TryStatementSyntax node)
        {
            base.VisitTryStatement(node);
        }

        private void AddExecutableLocations(IEnumerable<Location> locations)
        {
            foreach (var location in locations)
            {
                AddExecutableLocation(location);
            }
        }

        private void AddExecutableLocation(Location location)
        {
            executableLines[location.GetLineNumberToReport()] = new SecondaryLocation(location, string.Empty);
        }
    }
}
