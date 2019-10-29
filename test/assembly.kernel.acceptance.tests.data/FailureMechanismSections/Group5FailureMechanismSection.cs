﻿using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentResultTypes;

namespace assembly.kernel.acceptance.tests.data.FailureMechanismSections
{
    public class Group5FailureMechanismSection : FailureMechanismSectionBase<EIndirectAssessmentResult>
    {
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        public EAssessmentResultTypeG1 DetailedAssessmentResult { get; set; }

        public EAssessmentResultTypeT2 TailorMadeAssessmentResult { get; set; }
    }
}
