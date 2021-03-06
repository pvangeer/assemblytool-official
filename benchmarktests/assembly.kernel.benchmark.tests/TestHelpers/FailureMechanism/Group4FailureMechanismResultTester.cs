#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.
#endregion

using System.Linq;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.data.Result;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.TestHelpers.FailureMechanism
{
    /// <summary>
    /// Result tester for failure mechanisms in group 4.
    /// </summary>
    public class Group4FailureMechanismResultTester : FailureMechanismResultTesterBase<Group4Or5ExpectedFailureMechanismResult>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Group4FailureMechanismResultTester"/>.
        /// </summary>
        /// <param name="methodResults">The method results.</param>
        /// <param name="expectedFailureMechanismResult">The expected failure mechanism results.</param>
        public Group4FailureMechanismResultTester(MethodResultsListing methodResults,
                                                  IExpectedFailureMechanismResult expectedFailureMechanismResult)
            : base(methodResults, expectedFailureMechanismResult) {}

        protected override void TestSimpleAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var group4FailureMechanismSection = section as Group4FailureMechanismSection;
                if (group4FailureMechanismSection != null)
                {
                    // WBI-0E-1
                    FmSectionAssemblyDirectResult result = assembler.TranslateAssessmentResultWbi0E1(group4FailureMechanismSection.SimpleAssessmentResult);
                    var expectedResult = group4FailureMechanismSection.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyDirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        protected override void TestDetailedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var group4FailureMechanismSection = section as Group4FailureMechanismSection;
                if (group4FailureMechanismSection != null)
                {
                    // WBI-0G-1
                        FmSectionAssemblyDirectResult result = assembler.TranslateAssessmentResultWbi0G1(group4FailureMechanismSection.DetailedAssessmentResult);

                    var expectedResult =
                        group4FailureMechanismSection.ExpectedDetailedAssessmentAssemblyResult as
                            FmSectionAssemblyDirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        protected override void TestTailorMadeAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var group4FailureMechanismSection = section as Group4FailureMechanismSection;
                if (group4FailureMechanismSection != null)
                {
                    // WBI-0T-1
                    var result = assembler.TranslateAssessmentResultWbi0T1(
                        group4FailureMechanismSection.TailorMadeAssessmentResult);

                    var expectedResult = group4FailureMechanismSection.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyDirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        protected override void TestCombinedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            if (ExpectedFailureMechanismResult != null)
            {
                foreach (var section in ExpectedFailureMechanismResult.Sections.OfType<Group4FailureMechanismSection>())
                {
                    // WBI-0A-1 (direct with probability)
                    var result = assembler.TranslateAssessmentResultWbi0A1(
                        section.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyDirectResult,
                        section.ExpectedDetailedAssessmentAssemblyResult as FmSectionAssemblyDirectResult,
                        section.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyDirectResult);

                    Assert.IsInstanceOf<FmSectionAssemblyDirectResult>(result);
                    Assert.AreEqual(section.ExpectedCombinedResult, result.Result);
                }
            }
        }

        protected override void TestAssessmentSectionResultInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1A-1
            EFailureMechanismCategory result = assembler.AssembleFailureMechanismWbi1A1(
                ExpectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResult),
                false
            );

            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResult, result);
        }

        protected override void TestAssessmentSectionResultTemporalInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1A-1
            EFailureMechanismCategory result = assembler.AssembleFailureMechanismWbi1A1(
                ExpectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResult),
                true
            );

            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResultTemporal, result);
        }

        protected override void SetSimpleAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0E1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi0E1, result);
        }

        protected override void SetDetailedAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0G1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi0G1, result);
        }

        protected override void SetTailorMadeAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0T1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi0T1, result);
        }

        protected override void SetCombinedAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0A1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi0A1, result);
        }

        protected override void SetAssessmentSectionMethodResult(bool result)
        {
            MethodResults.Wbi1A1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi1A1, result);
        }

        protected override void SetAssessmentSectionMethodResultTemporal(bool result)
        {
            MethodResults.Wbi1A1T = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi1A1T, result);
        }

        private FmSectionAssemblyDirectResult CreateFmSectionAssemblyDirectResult(IFailureMechanismSection section)
        {
            var directMechanismSection = section as FailureMechanismSectionBase<EFmSectionCategory>;
            return new FmSectionAssemblyDirectResult(directMechanismSection.ExpectedCombinedResult);
        }
    }
}