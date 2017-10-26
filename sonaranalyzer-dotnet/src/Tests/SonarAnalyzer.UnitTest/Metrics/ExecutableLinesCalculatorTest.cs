﻿/*
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

using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SonarAnalyzer.UnitTest.Common
{
    [TestClass]
    public class ExecutableLinesCalculatorTest
    {
        private static void AssertLinesOfCode(int expectedLineCount, string code)
        {
            var calculator = new Metrics.CSharp.ExecutableLinesCalculator();
            calculator.CountLines(CSharpSyntaxTree.ParseText(code)).Should().Be(expectedLineCount);
        }

        [TestMethod]
        public void Test_01_No_Executable_Lines()
        {
            AssertLinesOfCode(0,
              @"using System;
                using System.Linq;

                namespace Test
                {
                    class Program
                    {
                        static void Main(string[] args)
                        {
                        }
                    }
                }");
        }

        [TestMethod]
        public void Test_02_Checked_Unchecked()
        {
            AssertLinesOfCode(2,
              @"
                static void Main(string[] args)
                {
                    checked // +1
                    {
                        unchecked // +1
                        {
                        }
                    }
                }");
        }


        [TestMethod]
        public void Test_03_Blocks()
        {
            AssertLinesOfCode(4,
              @"
                unsafe static void Main(int[] arr, object obj)
                {
                    lock (obj) { } // +1
                    fixed (int* p = arr) { } // +1
                    unsafe { } // +1
                    using ((IDisposable)obj) { } // +1
                }");
        }


        [TestMethod]
        public void Test_04_Statements()
        {
            AssertLinesOfCode(2,
              @"
                void Foo(int i)
                {
                    ; // +1
                    i++; // +1
                }");
        }



        [TestMethod]
        public void Test_06_Loops()
        {
            AssertLinesOfCode(4,
              @"
                void Foo(int[] arr)
                {
                    do {} while (true); // +1
                    foreach (var a in arr) { }// +1
                    for (;;) { } // +1
                    while (true) { } // +1
                }");
        }

        [TestMethod]
        public void Test_07_Conditionals()
        {
            AssertLinesOfCode(6,
              @"
                void Foo(int? i, string s)
                {
                    if (true) { } // +1
                    label: // +1
                    switch (i) // +1
                    {
                        case 1:
                        case 2:
                        default:
                            break; // +1
                    }
                    var x = s?.Length; // +1
                    var xx = i == 1 ? 1 : 1; // +1
                }");
        }

        [TestMethod]
        public void Test_08_Conditionals()
        {
            AssertLinesOfCode(6,
              @"
                void Foo(Exception ex)
                {
                    goto home; // +1
                    throw ex; // +1
                    home: // +1

                    while (true) // +1
                    {
                        break; // +1
                    }
                    return; // +1
                }");
        }

        [TestMethod]
        public void Test_09_Yields()
        {
            AssertLinesOfCode(2,
              @"
               using System;
               using System.Collections.Generic;
               using System.Linq;

               namespace Test
               {
                   class Program
                   {
                       IEnumerable<string> Foo()
                       {
                           yield return ""; // +1
                           yield break; // +1
                       }
                   }
               }");
        }

        [TestMethod]
        public void Test_10_AccessAndInvocation()
        {
            AssertLinesOfCode(2,
              @"
                static void Main(string[] args)
                {
                    var x = args.Length; // +1
                    args.ToString(); // +1
                }");
        }

        [TestMethod]
        public void Test_11_Initialization()
        {
            AssertLinesOfCode(2,
              @"
                static string GetString() => "";

                static void Main()
                {
                    var arr = new object();
                    var arr2 = new int[] { 1 }; // +1

                    var ex = new Exception()
                    {
                        Source = GetString(), // +1
                        HelpLink = ""
                    };
                }");
        }


        [TestMethod]
        public void Test_12_Property_Set()
        {
            AssertLinesOfCode(1,
              @"
                class Program
                {
                    int Prop { get; set; }

                    void Foo()
                    {
                        Prop = 1; // + 1
                    }
                }");
        }

        [TestMethod]
        public void Test_13_Property_Get()
        {
            AssertLinesOfCode(0,
              @"
                class Program
                {
                    int Prop { get; set; }

                    void Foo()
                    {
                        var x = Prop;
                    }
                }");
        }
        [TestMethod]
        public void Test_14()
        {
            AssertLinesOfCode(2,
             @"using System;
               public void Foo(int x) {
               int i = 0; if (i == 0) {i++;i--;} else
               { while(true){i--;} }
               }");
        }
    }
}
