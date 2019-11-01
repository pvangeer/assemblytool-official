﻿using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public class Group3FailureMechanismResult : FailureMechanismResultBase, IGroup3FailureMechanismResult
    {
        public Group3FailureMechanismResult(string name, MechanismType type) : base(name)
        {
            Type = type;
        }

        public override MechanismType Type { get; }

        public override int Group => 3;

        public double FailureMechanismProbabilitySpace { get; set; }

        public double LengthEffectFactor { get; set; }

        public CategoriesList<FmSectionCategory> ExpectedFailureMechanismSectionCategories { get; set; }
    }
}