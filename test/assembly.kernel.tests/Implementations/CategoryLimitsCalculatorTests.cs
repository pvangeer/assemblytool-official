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
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class CategoryLimitsCalculatorTests
    {
        [SetUp]
        public void Init()
        {
            categoryLimitsCalculator = new CategoryLimitsCalculator();
        }

        private CategoryLimitsCalculator categoryLimitsCalculator;

        private delegate double RoundDouble(double input, int decimals);

        private readonly RoundDouble roundDouble =
            (input, decimals) => Math.Round(input, decimals, MidpointRounding.AwayFromZero);

        [Test]
        public void CalculateWbi01Exceptions()
        {
            const double SignallingLimit = 1.0 / 1000.0;
            const double LowerLimit = 1.0 / 300.0;
            const double FailurePobabilityMarginFactor = 1;
            const double LengthEffectFactor = 1;

            var section = new AssessmentSection(20306, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            try
            {
                categoryLimitsCalculator.CalculateFmSectionCategoryLimitsWbi01(section, failureMechanism);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);

                Assert.AreEqual(2, e.Errors.Count());
                List<AssemblyErrorMessage> messages = e.Errors.ToList();
                Assert.NotNull(messages[0]);
                Assert.NotNull(messages[1]);

                Assert.AreEqual(EAssemblyErrors.PsigDsnAbovePsig, messages[0].ErrorCode);
                Assert.AreEqual(EAssemblyErrors.PlowDsnAbovePlow, messages[1].ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected Exception not thrown");
        }

        [Test]
        public void CalculateWbi01MaximizeTest()
        {
            const double SignallingLimit = 1.0 / 1000.0;
            const double LowerLimit = 1.0 / 30.0;
            const double FailurePobabilityMarginFactor = 0.04;
            const double LengthEffectFactor = 14.4;

            var section = new AssessmentSection(20306, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            IEnumerable<FmSectionCategoryLimits> results =
                categoryLimitsCalculator.CalculateFmSectionCategoryLimitsWbi01(section, failureMechanism);

            List<FmSectionCategoryLimits> calculationResult = results.ToList();
            Assert.AreEqual(6, calculationResult.Count);

            foreach (var limitResults in calculationResult)
            {
                switch (limitResults.Category)
                {
                    case EFmSectionCategory.Iv:
                        Assert.AreEqual(0, roundDouble(limitResults.LowerLimit, 0));
                        Assert.AreEqual(9.26E-8, roundDouble(limitResults.UpperLimit, 10));
                        break;
                    case EFmSectionCategory.IIv:
                        Assert.AreEqual(9.26E-8, roundDouble(limitResults.LowerLimit, 10));
                        Assert.AreEqual(2.78E-6, roundDouble(limitResults.UpperLimit, 8));
                        break;
                    case EFmSectionCategory.IIIv:
                        Assert.AreEqual(2.78E-6, roundDouble(limitResults.LowerLimit, 8));
                        Assert.AreEqual(9.26E-5, roundDouble(limitResults.UpperLimit, 7));
                        break;
                    case EFmSectionCategory.IVv:
                        Assert.AreEqual(9.26E-5, roundDouble(limitResults.LowerLimit, 7));
                        Assert.AreEqual(3.33E-2, roundDouble(limitResults.UpperLimit, 4));
                        break;
                    case EFmSectionCategory.Vv:
                        Assert.AreEqual(3.33E-2, roundDouble(limitResults.LowerLimit, 4));
                        Assert.AreEqual(1.0, roundDouble(limitResults.UpperLimit, 1));
                        break;
                    case EFmSectionCategory.VIv:
                        Assert.AreEqual(1.0, roundDouble(limitResults.LowerLimit, 1));
                        Assert.AreEqual(1.0, roundDouble(limitResults.UpperLimit, 1));
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        /// <summary>
        /// Test data is from section 2-1 (Ameland (2))
        /// </summary>
        [Test]
        public void CalculateWbi01Test()
        {
            const double SignallingLimit = 1.0 / 1000.0;
            const double LowerLimit = 1.0 / 300.0;
            const double FailurePobabilityMarginFactor = 0.04;
            const double LengthEffectFactor = 14.4;

            var section = new AssessmentSection(20306, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            IEnumerable<FmSectionCategoryLimits> results =
                categoryLimitsCalculator.CalculateFmSectionCategoryLimitsWbi01(section, failureMechanism);

            List<FmSectionCategoryLimits> calculationResult = results.ToList();
            Assert.AreEqual(6, calculationResult.Count);

            foreach (var limitResults in calculationResult)
            {
                switch (limitResults.Category)
                {
                    case EFmSectionCategory.Iv:
                        Assert.AreEqual(0, roundDouble(limitResults.LowerLimit, 0));
                        Assert.AreEqual(9.26E-8, roundDouble(limitResults.UpperLimit, 10));
                        break;
                    case EFmSectionCategory.IIv:
                        Assert.AreEqual(9.26E-8, roundDouble(limitResults.LowerLimit, 10));
                        Assert.AreEqual(2.78E-6, roundDouble(limitResults.UpperLimit, 8));
                        break;
                    case EFmSectionCategory.IIIv:
                        Assert.AreEqual(2.78E-6, roundDouble(limitResults.LowerLimit, 8));
                        Assert.AreEqual(9.26E-6, roundDouble(limitResults.UpperLimit, 8));
                        break;
                    case EFmSectionCategory.IVv:
                        Assert.AreEqual(9.26E-6, roundDouble(limitResults.LowerLimit, 8));
                        Assert.AreEqual(3.33E-3, roundDouble(limitResults.UpperLimit, 5));
                        break;
                    case EFmSectionCategory.Vv:
                        Assert.AreEqual(3.33E-3, roundDouble(limitResults.LowerLimit, 5));
                        Assert.AreEqual(0.1, roundDouble(limitResults.UpperLimit, 1));
                        break;
                    case EFmSectionCategory.VIv:
                        Assert.AreEqual(0.1, roundDouble(limitResults.LowerLimit, 1));
                        Assert.AreEqual(1, roundDouble(limitResults.UpperLimit, 0));
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        [Test]
        public void CalculateWbi02Exceptions()
        {
            const double SignallingLimit = 1.0 / 1000.0;
            const double LowerLimit = 1.0 / 300.0;
            const double FailurePobabilityMarginFactor = 1;
            const double LengthEffectFactor = 1;

            var section = new AssessmentSection(20306, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            try
            {
                categoryLimitsCalculator.CalculateFmSectionCategoryLimitsWbi02(section, failureMechanism);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);

                Assert.AreEqual(2, e.Errors.Count());
                List<AssemblyErrorMessage> messages = e.Errors.ToList();
                Assert.NotNull(messages[0]);
                Assert.NotNull(messages[1]);

                Assert.AreEqual(EAssemblyErrors.PsigDsnAbovePsig, messages[0].ErrorCode);
                Assert.AreEqual(EAssemblyErrors.PlowDsnAbovePlow, messages[1].ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected Exception not thrown");
        }

        [Test]
        public void CalculateWbi02MaximizeTest()
        {
            const double SignallingLimit = 0.00003;
            const double LowerLimit = 0.068;
            const double FailurePobabilityMarginFactor = 0.1;
            const double LengthEffectFactor = 2;

            var section = new AssessmentSection(10000, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            IEnumerable<FmSectionCategoryLimits> results =
                categoryLimitsCalculator.CalculateFmSectionCategoryLimitsWbi02(section, failureMechanism);

            List<FmSectionCategoryLimits> calculationResult = results.ToList();
            Assert.AreEqual(6, calculationResult.Count);

            foreach (var limitResults in calculationResult)
            {
                switch (limitResults.Category)
                {
                    case EFmSectionCategory.Iv:
                        Assert.AreEqual(0, roundDouble(limitResults.LowerLimit, 0));
                        Assert.AreEqual(0.0000005, roundDouble(limitResults.UpperLimit, 7));
                        break;
                    case EFmSectionCategory.IIv:
                        Assert.AreEqual(0.0000005, roundDouble(limitResults.LowerLimit, 7));
                        Assert.AreEqual(0.000015, roundDouble(limitResults.UpperLimit, 6));
                        break;
                    case EFmSectionCategory.IIIv:
                        Assert.AreEqual(0.000015, roundDouble(limitResults.LowerLimit, 6));
                        Assert.AreEqual(0.034, roundDouble(limitResults.UpperLimit, 5));
                        break;
                    case EFmSectionCategory.IVv:
                        Assert.AreEqual(0.034, roundDouble(limitResults.LowerLimit, 5));
                        Assert.AreEqual(0.068, roundDouble(limitResults.UpperLimit, 4));
                        break;
                    case EFmSectionCategory.Vv:
                        Assert.AreEqual(0.068, roundDouble(limitResults.LowerLimit, 4));
                        Assert.AreEqual(1.0, roundDouble(limitResults.UpperLimit, 1));
                        break;
                    case EFmSectionCategory.VIv:
                        Assert.AreEqual(1.0, roundDouble(limitResults.LowerLimit, 1));
                        Assert.AreEqual(1.0, roundDouble(limitResults.UpperLimit, 1));
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        [Test]
        public void CalculateWbi02Test()
        {
            const double SignallingLimit = 0.00003;
            const double LowerLimit = 0.0003;
            const double FailurePobabilityMarginFactor = 0.1;
            const double LengthEffectFactor = 2;

            var section = new AssessmentSection(10000, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            IEnumerable<FmSectionCategoryLimits> results =
                categoryLimitsCalculator.CalculateFmSectionCategoryLimitsWbi02(section, failureMechanism);

            List<FmSectionCategoryLimits> calculationResult = results.ToList();
            Assert.AreEqual(6, calculationResult.Count);

            foreach (var limitResults in calculationResult)
            {
                switch (limitResults.Category)
                {
                    case EFmSectionCategory.Iv:
                        Assert.AreEqual(0, roundDouble(limitResults.LowerLimit, 0));
                        Assert.AreEqual(0.0000005, roundDouble(limitResults.UpperLimit, 7));
                        break;
                    case EFmSectionCategory.IIv:
                        Assert.AreEqual(0.0000005, roundDouble(limitResults.LowerLimit, 7));
                        Assert.AreEqual(0.000015, roundDouble(limitResults.UpperLimit, 6));
                        break;
                    case EFmSectionCategory.IIIv:
                        Assert.AreEqual(0.000015, roundDouble(limitResults.LowerLimit, 6));
                        Assert.AreEqual(0.00015, roundDouble(limitResults.UpperLimit, 5));
                        break;
                    case EFmSectionCategory.IVv:
                        Assert.AreEqual(0.00015, roundDouble(limitResults.LowerLimit, 5));
                        Assert.AreEqual(0.0003, roundDouble(limitResults.UpperLimit, 4));
                        break;
                    case EFmSectionCategory.Vv:
                        Assert.AreEqual(0.0003, roundDouble(limitResults.LowerLimit, 4));
                        Assert.AreEqual(0.009, roundDouble(limitResults.UpperLimit, 3));
                        break;
                    case EFmSectionCategory.VIv:
                        Assert.AreEqual(0.009, roundDouble(limitResults.LowerLimit, 3));
                        Assert.AreEqual(1, roundDouble(limitResults.UpperLimit, 0));
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        [Test]
        public void CalculateWbi11MaximizeTest()
        {
            const double SignallingLimit = 0.0003;
            const double LowerLimit = 0.034;
            const double FailurePobabilityMarginFactor = 0.1;
            const double LengthEffectFactor = 1;

            var section = new AssessmentSection(10000, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            IEnumerable<FailureMechanismCategoryLimits> results =
                categoryLimitsCalculator.CalculateFailureMechanismCategoryLimitsWbi11(section, failureMechanism);

            List<FailureMechanismCategoryLimits> calculationResult = results.ToList();
            Assert.AreEqual(6, calculationResult.Count);

            foreach (var limitResults in calculationResult)
            {
                switch (limitResults.Category)
                {
                    case EFailureMechanismCategory.It:
                        Assert.AreEqual(0, roundDouble(limitResults.LowerLimit, 0));
                        Assert.AreEqual(0.000001, roundDouble(limitResults.UpperLimit, 6));
                        break;
                    case EFailureMechanismCategory.IIt:
                        Assert.AreEqual(0.000001, roundDouble(limitResults.LowerLimit, 6));
                        Assert.AreEqual(0.00003, roundDouble(limitResults.UpperLimit, 5));
                        break;
                    case EFailureMechanismCategory.IIIt:
                        Assert.AreEqual(0.00003, roundDouble(limitResults.LowerLimit, 5));
                        Assert.AreEqual(0.0034, roundDouble(limitResults.UpperLimit, 4));
                        break;
                    case EFailureMechanismCategory.IVt:
                        Assert.AreEqual(0.0034, roundDouble(limitResults.LowerLimit, 4));
                        Assert.AreEqual(0.034, roundDouble(limitResults.UpperLimit, 3));
                        break;
                    case EFailureMechanismCategory.Vt:
                        Assert.AreEqual(0.034, roundDouble(limitResults.LowerLimit, 3));
                        Assert.AreEqual(1.0, roundDouble(limitResults.UpperLimit, 1));
                        break;
                    case EFailureMechanismCategory.VIt:
                        Assert.AreEqual(1.0, roundDouble(limitResults.LowerLimit, 1));
                        Assert.AreEqual(1.0, roundDouble(limitResults.UpperLimit, 1));
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        [Test]
        public void CalculateWbi11Test()
        {
            const double SignallingLimit = 0.0003;
            const double LowerLimit = 0.003;
            const double FailurePobabilityMarginFactor = 0.1;
            const double LengthEffectFactor = 1;

            var section = new AssessmentSection(10000, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            IEnumerable<FailureMechanismCategoryLimits> results =
                categoryLimitsCalculator.CalculateFailureMechanismCategoryLimitsWbi11(section, failureMechanism);

            List<FailureMechanismCategoryLimits> calculationResult = results.ToList();
            Assert.AreEqual(6, calculationResult.Count);

            foreach (var limitResults in calculationResult)
            {
                switch (limitResults.Category)
                {
                    case EFailureMechanismCategory.It:
                        Assert.AreEqual(0, roundDouble(limitResults.LowerLimit, 0));
                        Assert.AreEqual(0.000001, roundDouble(limitResults.UpperLimit, 6));
                        break;
                    case EFailureMechanismCategory.IIt:
                        Assert.AreEqual(0.000001, roundDouble(limitResults.LowerLimit, 6));
                        Assert.AreEqual(0.00003, roundDouble(limitResults.UpperLimit, 5));
                        break;
                    case EFailureMechanismCategory.IIIt:
                        Assert.AreEqual(0.00003, roundDouble(limitResults.LowerLimit, 5));
                        Assert.AreEqual(0.0003, roundDouble(limitResults.UpperLimit, 4));
                        break;
                    case EFailureMechanismCategory.IVt:
                        Assert.AreEqual(0.0003, roundDouble(limitResults.LowerLimit, 4));
                        Assert.AreEqual(0.003, roundDouble(limitResults.UpperLimit, 3));
                        break;
                    case EFailureMechanismCategory.Vt:
                        Assert.AreEqual(0.003, roundDouble(limitResults.LowerLimit, 3));
                        Assert.AreEqual(0.09, roundDouble(limitResults.UpperLimit, 2));
                        break;
                    case EFailureMechanismCategory.VIt:
                        Assert.AreEqual(0.09, roundDouble(limitResults.LowerLimit, 2));
                        Assert.AreEqual(1, roundDouble(limitResults.UpperLimit, 0));
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        [Test]
        public void CalculateWbi21MaximizeTest()
        {
            const double SignallingLimit = 0.003;
            const double LowerLimit = 0.034;

            var section = new AssessmentSection(10000, SignallingLimit, LowerLimit);

            IEnumerable<AssessmentSectionCategoryLimits> results =
                categoryLimitsCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(section);

            List<AssessmentSectionCategoryLimits> calculationResult = results.ToList();
            Assert.AreEqual(5, calculationResult.Count);

            foreach (var limitResults in calculationResult)
            {
                switch (limitResults.Category)
                {
                    case EAssessmentGrade.APlus:
                        Assert.AreEqual(0, roundDouble(limitResults.LowerLimit, 0));
                        Assert.AreEqual(0.0001, roundDouble(limitResults.UpperLimit, 4));
                        break;
                    case EAssessmentGrade.A:
                        Assert.AreEqual(0.0001, roundDouble(limitResults.LowerLimit, 4));
                        Assert.AreEqual(0.003, roundDouble(limitResults.UpperLimit, 3));
                        break;
                    case EAssessmentGrade.B:
                        Assert.AreEqual(0.003, roundDouble(limitResults.LowerLimit, 3));
                        Assert.AreEqual(0.034, roundDouble(limitResults.UpperLimit, 4));
                        break;
                    case EAssessmentGrade.C:
                        Assert.AreEqual(0.034, roundDouble(limitResults.LowerLimit, 4));
                        Assert.AreEqual(1.0, roundDouble(limitResults.UpperLimit, 1));
                        break;
                    case EAssessmentGrade.D:
                        Assert.AreEqual(1.0, roundDouble(limitResults.LowerLimit, 1));
                        Assert.AreEqual(1.0, roundDouble(limitResults.UpperLimit, 1));
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        [Test]
        public void CalculateWbi21Test()
        {
            const double SignallingLimit = 0.003;
            const double LowerLimit = 0.03;

            var section = new AssessmentSection(10000, SignallingLimit, LowerLimit);

            IEnumerable<AssessmentSectionCategoryLimits> results =
                categoryLimitsCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(section);

            List<AssessmentSectionCategoryLimits> calculationResult = results.ToList();
            Assert.AreEqual(5, calculationResult.Count);

            foreach (var limitResults in calculationResult)
            {
                switch (limitResults.Category)
                {
                    case EAssessmentGrade.APlus:
                        Assert.AreEqual(0, roundDouble(limitResults.LowerLimit, 0));
                        Assert.AreEqual(0.0001, roundDouble(limitResults.UpperLimit, 4));
                        break;
                    case EAssessmentGrade.A:
                        Assert.AreEqual(0.0001, roundDouble(limitResults.LowerLimit, 4));
                        Assert.AreEqual(0.003, roundDouble(limitResults.UpperLimit, 3));
                        break;
                    case EAssessmentGrade.B:
                        Assert.AreEqual(0.003, roundDouble(limitResults.LowerLimit, 3));
                        Assert.AreEqual(0.03, roundDouble(limitResults.UpperLimit, 3));
                        break;
                    case EAssessmentGrade.C:
                        Assert.AreEqual(0.03, roundDouble(limitResults.LowerLimit, 2));
                        Assert.AreEqual(0.9, roundDouble(limitResults.UpperLimit, 3));
                        break;
                    case EAssessmentGrade.D:
                        Assert.AreEqual(0.9, roundDouble(limitResults.LowerLimit, 1));
                        Assert.AreEqual(1, roundDouble(limitResults.UpperLimit, 0));
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }
    }
}