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

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SonarAnalyzer.Common;
using SonarAnalyzer.Helpers;
using SonarAnalyzer.Helpers.FlowAnalysis.CSharp;

namespace SonarAnalyzer.UnitTest.Helpers
{
    [TestClass]
    public class ExecutableLinesWalkerTest
    {
        [TestMethod]
        [TestCategory("Rule")]
        public void ExecutableLinesWalker()
        {
            Verifier.VerifyAnalyzer(@"TestCases\ExecutableLinesWalker.cs", new DummyAnalyzer());
        }
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DummyAnalyzer : SonarDiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor rule =
            new DiagnosticDescriptor("S7777", "Executable lines", "The executable lines are {0}", "Design", DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(rule);

        protected override void Initialize(SonarAnalysisContext context)
        {
            context.RegisterSyntaxTreeAction(
                c =>
                {
                    if (SonarAnalyzer.Helpers.CSharp.GeneratedCodeRecognizer.Instance.IsGenerated(c.Tree))
                    {
                        return;
                    }

                    var cfg = ControlFlowGraph.Create();

                    var walker = new ExecutableLinesWalker();

                    walker.Visit(c.Tree.GetRoot());

                    var diagnostic = Diagnostic.Create(rule, c.Tree.GetRoot().GetLocation(),
                        additionalLocations: walker.Locations.ToAdditionalLocations(),
                        properties: walker.Locations.ToProperties(),
                        messageArgs: walker.MetricValue);

                    c.ReportDiagnostic(diagnostic);
                });
        }
    }
}
