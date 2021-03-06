﻿#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
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

using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    /// <summary>
    /// Interface for expected failure mechanism result for group 3.
    /// </summary>
    public interface IGroup3ExpectedFailureMechanismResult : IExpectedFailureMechanismResult
    {
        /// <summary>
        /// The probability space for this failure mechanism (a number between 0 and 1).
        /// </summary>
        double FailureMechanismProbabilitySpace { get; set; }

        /// <summary>
        /// The length-effect factor (number >= 1)
        /// </summary>
        double LengthEffectFactor { get; set; }

        /// <summary>
        /// The expected categories for this failure mechanism at failure mechanism section level.
        /// </summary>
        CategoriesList<FmSectionCategory> ExpectedFailureMechanismSectionCategories { get; set; }
    }
}