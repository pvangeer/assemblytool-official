﻿using System.Linq;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.data.Result;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.TestHelpers.FailureMechanism
{
    public class Group5NoDetailedAssessmentFailureMechanismTester : FailureMechanismResultTesterBase<Group4Or5ExpectedFailureMechanismResult>
    {
        public Group5NoDetailedAssessmentFailureMechanismTester(MethodResultsListing methodResults, IExpectedFailureMechanismResult expectedFailureMechanismResult) : base(methodResults, expectedFailureMechanismResult)
        {
        }

        protected override void TestSimpleAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var group5NoDetailedAssessmentFailureMechanismSection = section as Group5NoDetailedAssessmentFailureMechanismSection;
                if (group5NoDetailedAssessmentFailureMechanismSection != null)
                {
                    // WBI-0E-2
                    FmSectionAssemblyIndirectResult result = assembler.TranslateAssessmentResultWbi0E2(group5NoDetailedAssessmentFailureMechanismSection.SimpleAssessmentResult);
                    var expectedResult = group5NoDetailedAssessmentFailureMechanismSection.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyIndirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        public override bool? TestDetailedAssessment()
        {
            return null;
        }

        protected override void TestTailorMadeAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var group5NoDetailedAssessmentFailureMechanismSection = section as Group5NoDetailedAssessmentFailureMechanismSection;
                if (group5NoDetailedAssessmentFailureMechanismSection != null)
                {
                    // WBI-0T-2
                    FmSectionAssemblyIndirectResult result = assembler.TranslateAssessmentResultWbi0T2(group5NoDetailedAssessmentFailureMechanismSection.TailorMadeAssessmentResult);

                    var expectedResult = group5NoDetailedAssessmentFailureMechanismSection.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyIndirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        protected override void TestCombinedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            if (ExpectedFailureMechanismResult != null)
            {
                foreach (var section in ExpectedFailureMechanismResult.Sections.OfType<Group5NoDetailedAssessmentFailureMechanismSection>())
                {
                    // WBI-0A-1 (direct with probability)
                    var result = assembler.TranslateAssessmentResultWbi0A1(
                        section.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyIndirectResult,
                        section.ExpectedDetailedAssessmentAssemblyResult as FmSectionAssemblyIndirectResult,
                        section.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyIndirectResult);

                    Assert.IsInstanceOf<FmSectionAssemblyIndirectResult>(result);
                    Assert.AreEqual(section.ExpectedCombinedResult, result.Result);
                }
            }
        }

        protected override void TestAssessmentSectionResultInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1A-2
            var result = assembler.AssembleFailureMechanismWbi1A2(
                ExpectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyIndirectResult),
                false
            );

            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResult, result);
        }

        protected override void TestAssessmentSectionResultTemporalInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1A-2
            var result = assembler.AssembleFailureMechanismWbi1A2(
                ExpectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyIndirectResult),
                true
            );

            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResultTemporal, result);
        }

        protected override void SetSimpleAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0E2 = GetUpdatedMethodResult(MethodResults.Wbi0E2, result);
        }

        protected override void SetTailorMadeAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0T2 = GetUpdatedMethodResult(MethodResults.Wbi0T2, result);
        }

        protected override void SetCombinedAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0A1 = GetUpdatedMethodResult(MethodResults.Wbi0A1, result);
        }

        protected override void SetAssessmentSectionMethodResult(bool result)
        {
            MethodResults.Wbi1A2 = GetUpdatedMethodResult(MethodResults.Wbi1A2, result);
        }

        protected override void SetAssessmentSectionMethodResultTemporal(bool result)
        {
            MethodResults.Wbi1A2T = GetUpdatedMethodResult(MethodResults.Wbi1A2T, result);
        }

        private FmSectionAssemblyIndirectResult CreateFmSectionAssemblyIndirectResult(IFailureMechanismSection section)
        {
            var directMechanismSection = section as FailureMechanismSectionBase<EIndirectAssessmentResult>;
            return new FmSectionAssemblyIndirectResult(directMechanismSection.ExpectedCombinedResult);
        }
    }
}