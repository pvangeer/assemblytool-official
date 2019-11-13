﻿using System.IO;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.io.Readers;
using Assembly.Kernel.Model;
using DocumentFormat.OpenXml.Packaging;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.io.tests.Readers
{
    [TestFixture]
    public class SafetyAssessmentResultReaderTest : TestFileReaderTestBase
    {
        [Test]
        public void ReaderReadsInformationCorrectly()
        {
            var testFile = Path.Combine(GetTestDir(), "Benchmarktool Excel assemblagetool (v1_0_1_0) 0_03.xlsm");

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {

                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var workSheetPart = workSheetParts["Gecombineerd veiligheidsoordeel"];

                var reader = new SafetyAssessmentFinalResultReader(workSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                var assemblyResult = result.ExpectedSafetyAssessmentAssemblyResult;
                Assert.AreEqual(0.58, assemblyResult.CombinedFailureMechanismProbabilitySpace, 0.001);
                Assert.AreEqual(EFailureMechanismCategory.VIIt, assemblyResult.ExpectedAssemblyResultGroups1and2);
                Assert.AreEqual(double.NaN, assemblyResult.ExpectedAssemblyResultGroups1and2Probability, 1e-6);
                Assert.AreEqual(EFailureMechanismCategory.VIIt, assemblyResult.ExpectedAssemblyResultGroups3and4);
                Assert.AreEqual(EAssessmentGrade.Ngo, assemblyResult.ExpectedSafetyAssessmentAssemblyResult);
                Assert.AreEqual(EFailureMechanismCategory.IIIt, assemblyResult.ExpectedAssemblyResultGroups1and2Temporal);
                Assert.AreEqual(4.24e-4, assemblyResult.ExpectedAssemblyResultGroups1and2ProbabilityTemporal, 1e-6);
                Assert.AreEqual(EFailureMechanismCategory.Vt, assemblyResult.ExpectedAssemblyResultGroups3and4Temporal);
                Assert.AreEqual(EAssessmentGrade.C, assemblyResult.ExpectedSafetyAssessmentAssemblyResultTemporal);

                var categories = assemblyResult.ExpectedCombinedFailureMechanismCategoriesGroup1and2.Categories;
                Assert.AreEqual(6, categories.Length);
                AssertAreEqualCategories(EFailureMechanismCategory.It, 0.0, 6.44e-6, categories[0]);
                AssertAreEqualCategories(EFailureMechanismCategory.IIt, 6.44e-6, 1.93e-4, categories[1]);
                AssertAreEqualCategories(EFailureMechanismCategory.IIIt, 1.93e-4, 5.80e-4, categories[2]);
                AssertAreEqualCategories(EFailureMechanismCategory.IVt, 5.80e-4, 1.00e-3, categories[3]);
                AssertAreEqualCategories(EFailureMechanismCategory.Vt, 1.00e-3, 3.00e-2, categories[4]);
                AssertAreEqualCategories(EFailureMechanismCategory.VIt, 3.00e-2, 1.00, categories[5]);
            }
        }

        
    }
}