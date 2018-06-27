﻿#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Assembly kernel is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program. If not, see <http://www.gnu.org/licenses/>.
// //
// // All names, logos, and references to "Technolution BV" are registered trademarks of
// // Technolution BV and remain full property of Technolution BV at all times.
// // All rights reserved.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class FailureMechanismResultAssemblerTests
    {
        [SetUp]
        public void Init()
        {
            assembler = new FailureMechanismResultAssembler();
        }

        private delegate double RoundDouble(double input, int decimals);

        private static readonly RoundDouble RoundedDouble =
            (input, decimals) => Math.Round(input, decimals, MidpointRounding.AwayFromZero);

        private readonly AssessmentSection assessmentSectionAmeland =
            new AssessmentSection(20306, 1.0 / 1000.0, 1.0 / 300.0);

        private readonly FailureMechanism testFailureMechanism1 = new FailureMechanism(14.4, 0.04);

        private readonly AssessmentSection assessmentSectionTest =
            new AssessmentSection(10000, 1.0 / 3000.0, 1.0 / 300.0);

        private readonly FailureMechanism testFailureMechanism2 = new FailureMechanism(10, 0.1);

        public enum EAssemblyType
        {
            Full,
            Partial
        }

        private IFailureMechanismResultAssembler assembler;

        public class AssembleFailureMechanismTestData
        {
            public static IEnumerable Wbi1B1
            {
                get
                {
                    yield return new TestCaseData(new[]
                            {
                                0.0,
                                0.1
                            },
                            EAssemblyType.Full)
                        .Returns(0.1);

                    yield return new TestCaseData(new[]
                            {
                                0.0005,
                                0.00005
                            },
                            EAssemblyType.Full)
                        .Returns(RoundedDouble(0.000549975, 10));
                }
            }

            public static IEnumerable DirectTestCases
            {
                get
                {
                    yield return new TestCaseData(
                            new[]
                            {
                                EFmSectionCategory.Iv,
                                EFmSectionCategory.IIv,
                                EFmSectionCategory.IIIv,
                                EFmSectionCategory.IVv,
                                EFmSectionCategory.Vv,
                                EFmSectionCategory.VIv,
                                EFmSectionCategory.VIIv
                            },
                            EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.VIIt);

                    yield return new TestCaseData(
                            new[]
                            {
                                EFmSectionCategory.Iv,
                                EFmSectionCategory.IIv,
                                EFmSectionCategory.IIIv,
                                EFmSectionCategory.IVv,
                                EFmSectionCategory.IIIv,
                                EFmSectionCategory.IIIv,
                                EFmSectionCategory.IIv
                            },
                            EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.IVt);

                    yield return new TestCaseData(
                            new[]
                            {
                                EFmSectionCategory.Iv,
                                EFmSectionCategory.IIv
                            },
                            EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.IIt);

                    yield return new TestCaseData(
                            new[]
                            {
                                EFmSectionCategory.Iv,
                                EFmSectionCategory.Iv,
                                EFmSectionCategory.Iv,
                                EFmSectionCategory.Iv
                            },
                            EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.It);

                    yield return new TestCaseData(
                            new[]
                            {
                                EFmSectionCategory.Iv,
                                EFmSectionCategory.IIv,
                                EFmSectionCategory.IIIv,
                                EFmSectionCategory.IVv,
                                EFmSectionCategory.Vv,
                                EFmSectionCategory.VIv,
                                EFmSectionCategory.Gr
                            },
                            EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.Gr);

                    yield return new TestCaseData(
                            new[]
                            {
                                EFmSectionCategory.Iv,
                                EFmSectionCategory.IIv,
                                EFmSectionCategory.IIIv,
                                EFmSectionCategory.NotApplicable
                            },
                            EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.IIIt);

                    yield return new TestCaseData(
                            new[]
                            {
                                EFmSectionCategory.NotApplicable,
                                EFmSectionCategory.NotApplicable
                            },
                            EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.Nvt);

                    yield return new TestCaseData(
                            Generate250DirectCategories(),
                            EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.IIt);

                    yield return new TestCaseData(
                            new[]
                            {
                                EFmSectionCategory.IIv,
                                EFmSectionCategory.VIIv,
                                EFmSectionCategory.NotApplicable
                            },
                            EAssemblyType.Partial)
                        .Returns(EFailureMechanismCategory.IIt);

                    yield return new TestCaseData(
                            new[]
                            {
                                EFmSectionCategory.IIIv,
                                EFmSectionCategory.Gr,
                                EFmSectionCategory.NotApplicable
                            },
                            EAssemblyType.Partial)
                        .Returns(EFailureMechanismCategory.Gr);

                    yield return new TestCaseData(
                            new[]
                            {
                                EFmSectionCategory.IIIv,
                                EFmSectionCategory.VIIv,
                                EFmSectionCategory.NotApplicable
                            },
                            EAssemblyType.Partial)
                        .Returns(EFailureMechanismCategory.IIIt);
                }
            }

            public static IEnumerable IndirectTestCases
            {
                get
                {
                    yield return new TestCaseData(
                            new[]
                            {
                                EIndirectAssessmentResult.Nvt,
                                EIndirectAssessmentResult.FvEt,
                                EIndirectAssessmentResult.FvGt,
                                EIndirectAssessmentResult.FvTom,
                                EIndirectAssessmentResult.FactoredInOtherFailureMechanism
                            },
                            EAssemblyType.Full)
                        .Returns(EIndirectAssessmentResult.FactoredInOtherFailureMechanism);

                    yield return new TestCaseData(
                            new[]
                            {
                                EIndirectAssessmentResult.Nvt,
                                EIndirectAssessmentResult.FvEt,
                                EIndirectAssessmentResult.FvGt,
                                EIndirectAssessmentResult.FvTom,
                                EIndirectAssessmentResult.Nvt
                            },
                            EAssemblyType.Full)
                        .Returns(EIndirectAssessmentResult.FvTom);

                    yield return new TestCaseData(
                            new[]
                            {
                                EIndirectAssessmentResult.Nvt,
                                EIndirectAssessmentResult.FvEt,
                                EIndirectAssessmentResult.FvGt,
                                EIndirectAssessmentResult.FvEt,
                                EIndirectAssessmentResult.Nvt
                            },
                            EAssemblyType.Full)
                        .Returns(EIndirectAssessmentResult.FvGt);

                    yield return new TestCaseData(
                            new[]
                            {
                                EIndirectAssessmentResult.Nvt,
                                EIndirectAssessmentResult.FvEt
                            },
                            EAssemblyType.Full)
                        .Returns(EIndirectAssessmentResult.FvEt);

                    yield return new TestCaseData(
                            Generate250IndirectCategories(),
                            EAssemblyType.Full)
                        .Returns(EIndirectAssessmentResult.FvEt);

                    yield return new TestCaseData(
                            new[]
                            {
                                EIndirectAssessmentResult.Nvt,
                                EIndirectAssessmentResult.Nvt
                            },
                            EAssemblyType.Full)
                        .Returns(EIndirectAssessmentResult.Nvt);

                    yield return new TestCaseData(
                            new[]
                            {
                                EIndirectAssessmentResult.Nvt,
                                EIndirectAssessmentResult.FvEt,
                                EIndirectAssessmentResult.Gr,
                                EIndirectAssessmentResult.FvTom,
                                EIndirectAssessmentResult.FactoredInOtherFailureMechanism
                            },
                            EAssemblyType.Full)
                        .Returns(EIndirectAssessmentResult.Gr);

                    yield return new TestCaseData(
                            new[]
                            {
                                EIndirectAssessmentResult.Ngo,
                                EIndirectAssessmentResult.Nvt,
                                EIndirectAssessmentResult.FvEt,
                                EIndirectAssessmentResult.FvGt,
                                EIndirectAssessmentResult.FvTom,
                                EIndirectAssessmentResult.FactoredInOtherFailureMechanism
                            },
                            EAssemblyType.Full)
                        .Returns(EIndirectAssessmentResult.Ngo);

                    yield return new TestCaseData(
                            new[]
                            {
                                EIndirectAssessmentResult.Ngo,
                                EIndirectAssessmentResult.Nvt,
                                EIndirectAssessmentResult.FvEt,
                                EIndirectAssessmentResult.FvGt,
                                EIndirectAssessmentResult.FvTom,
                                EIndirectAssessmentResult.FactoredInOtherFailureMechanism
                            },
                            EAssemblyType.Partial)
                        .Returns(EIndirectAssessmentResult.FactoredInOtherFailureMechanism);
                }
            }

            private static IEnumerable<EFmSectionCategory> Generate250DirectCategories()
            {
                var categories = new List<EFmSectionCategory>();
                for (var i = 0; i < 250; i++)
                {
                    categories.Add(EFmSectionCategory.IIv);
                }

                return categories;
            }

            private static IEnumerable<EIndirectAssessmentResult> Generate250IndirectCategories()
            {
                var categories = new List<EIndirectAssessmentResult>();
                for (var i = 0; i < 250; i++)
                {
                    categories.Add(EIndirectAssessmentResult.FvEt);
                }

                return categories;
            }
        }

        [Test]
        public void Wbi1A1ExceptionDifferentTypesTest()
        {
            try
            {
                assembler.AssembleFailureMechanismWbi1A1(
                    new[]
                    {
                        new FmSectionAssemblyDirectResult(EFmSectionCategory.Iv),
                        new FmSectionAssemblyDirectResult(EFmSectionCategory.Iv, 0.5)
                    }, false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.InputNotTheSameType, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void Wbi1A1ExceptionEmptyListTest()
        {
            try
            {
                assembler.AssembleFailureMechanismWbi1A1(new List<FmSectionAssemblyDirectResult>(), false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.FailureMechanismAssemblerInputInvalid, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void Wbi1A1ExceptionNullTest()
        {
            try
            {
                assembler.AssembleFailureMechanismWbi1A1(null, false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test, TestCaseSource(
             typeof(AssembleFailureMechanismTestData),
             nameof(AssembleFailureMechanismTestData.DirectTestCases))]
        public EFailureMechanismCategory? Wbi1A1Test(
            IEnumerable<EFmSectionCategory> fmSectionAssemblyResults, EAssemblyType assemblyType)
        {
            return assembler.AssembleFailureMechanismWbi1A1(
                fmSectionAssemblyResults.Select(category => new FmSectionAssemblyDirectResult(category)),
                assemblyType == EAssemblyType.Partial);
        }

        [Test]
        public void Wbi1A2ExceptionEmptyListTest()
        {
            try
            {
                assembler.AssembleFailureMechanismWbi1A2(new List<FmSectionAssemblyIndirectResult>(), false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.FailureMechanismAssemblerInputInvalid, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void Wbi1A2ExceptionNullTest()
        {
            try
            {
                assembler.AssembleFailureMechanismWbi1A2(null, false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test, TestCaseSource(
             typeof(AssembleFailureMechanismTestData),
             nameof(AssembleFailureMechanismTestData.IndirectTestCases))]
        public EIndirectAssessmentResult? Wbi1A2Test(
            IEnumerable<EIndirectAssessmentResult> fmSectionAssemblyResults, EAssemblyType assemblyType)
        {
            return assembler.AssembleFailureMechanismWbi1A2(
                fmSectionAssemblyResults.Select(category => new FmSectionAssemblyIndirectResult(category)),
                assemblyType == EAssemblyType.Partial);
        }

        [Test]
        public void Wbi1B1Category()
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(assessmentSectionTest, testFailureMechanism2,
                new[]
                {
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIIv, 0.0001),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIIv, 0.00026)
                },
                false);

            Assert.IsFalse(double.IsNaN(result.FailureProbability));
            Assert.AreEqual(3.6E-4, RoundedDouble(result.FailureProbability, 5));
            Assert.AreEqual(EFailureMechanismCategory.IVt, result.Category);
        }

        [Test]
        public void Wbi1B1ExceptionNoFailureProb()
        {
            try
            {
                assembler.AssembleFailureMechanismWbi1B1(
                    assessmentSectionAmeland,
                    testFailureMechanism1,
                    new[]
                    {
                        new FmSectionAssemblyDirectResult(EFmSectionCategory.Iv)
                    },
                    false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                Assert.AreEqual(1, e.Errors.Count());
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test, TestCaseSource(
             typeof(AssembleFailureMechanismTestData),
             nameof(AssembleFailureMechanismTestData.Wbi1B1))]
        public double Wbi1B1FailureProbabilityTests(IEnumerable<double> failureProbabilities,
            EAssemblyType assemblyType)
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(assessmentSectionAmeland, testFailureMechanism1,
                failureProbabilities.Select(failureProbability =>
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.Iv, failureProbability)),
                assemblyType == EAssemblyType.Partial);

            Assert.NotNull(result.FailureProbability);
            return RoundedDouble(result.FailureProbability, 10);
        }

        [Test]
        public void Wbi1B1LengthEffectFactor()
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(assessmentSectionTest, testFailureMechanism2,
                new[]
                {
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.VIv, 0.9),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIv, 0.000026),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIv, 0.000010),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIv, 0.0000011),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIv, 0.000015),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIv, 0.000009)
                },
                false);

            Assert.NotNull(result.FailureProbability);
            Assert.AreEqual(0.9, RoundedDouble(result.FailureProbability, 1));
            Assert.AreEqual(EFailureMechanismCategory.VIt, result.Category);
        }

        [Test]
        public void Wbi1B1NoJudgementYet()
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(assessmentSectionTest, testFailureMechanism2,
                new[]
                {
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.VIIv),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIIv, 0.00026),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIIv, 0.00026)
                },
                false);

            Assert.IsNaN(result.FailureProbability);
            Assert.AreEqual(EFailureMechanismCategory.VIIt, result.Category);
        }

        [Test]
        public void Wbi1B1NoResult()
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(assessmentSectionTest, testFailureMechanism2,
                new[]
                {
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.Gr),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIIv, 0.00026),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIIv, 0.00026)
                },
                false);

            Assert.IsNaN(result.FailureProbability);
            Assert.AreEqual(EFailureMechanismCategory.Gr, result.Category);
        }

        [Test]
        public void Wbi1B1NotApplicable()
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(assessmentSectionTest, testFailureMechanism2,
                new[]
                {
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.NotApplicable),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.NotApplicable),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.NotApplicable)
                },
                false);

            Assert.IsNaN(result.FailureProbability);
            Assert.AreEqual(EFailureMechanismCategory.Nvt, result.Category);
        }

        [Test]
        public void Wbi1B1Partial()
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(assessmentSectionTest, testFailureMechanism2,
                new[]
                {
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.VIv, 0.9),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIv, 0.000026),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIv, 0.000010),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIv, 0.0000011),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIv, 0.000015),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.IIv, 0.000009),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.VIIv),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.Gr),
                    new FmSectionAssemblyDirectResult(EFmSectionCategory.NotApplicable)
                },
                true);

            Assert.NotNull(result.FailureProbability);
            Assert.AreEqual(0.9, RoundedDouble(result.FailureProbability, 1));
            Assert.AreEqual(EFailureMechanismCategory.VIt, result.Category);
        }
    }
}