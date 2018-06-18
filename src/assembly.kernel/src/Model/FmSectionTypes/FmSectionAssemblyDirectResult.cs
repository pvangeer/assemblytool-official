﻿#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 
// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Ringtoets is free software: you can redistribute it and/or modify
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

using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.FmSectionTypes {
    /// <summary>
    /// Failure mechanism assessment translation result for direct failure mechanisms.
    /// </summary>
    public class FmSectionAssemblyDirectResult : FmSectionAssemblyResult {
        /// <summary>
        /// The Failure mechanism section category as result of the assessment result translation.
        /// </summary>
        public EFmSectionCategory Result { get; }
        /// <summary>
        /// Optional failure probability originating from the failure mechanism section assessment result.
        /// This field can be null!
        /// </summary>
        public double FailureProbability { get; }

        /// <summary>
        /// Constructor of the direct failure mechanism assembly result.
        /// The failure probability will be null when this constructor is used.
        /// </summary>
        /// <param name="result">The translated category type of the result</param>
        public FmSectionAssemblyDirectResult(EFmSectionCategory result) : 
                base(EAssembledAssessmentResultType.AssessmentCategoryWithoutFailureProbability) {    
            Result = result;
            FailureProbability = double.NaN;
        }

        /// <summary>
        /// Constructor of the direct failure mechanism assembly result with failure probability.
        /// </summary>
        /// <param name="result">The translated category type of the result</param>
        /// <param name="failureProbability">The failure probability of the failure mechanism section</param>
        /// <exception cref="AssemblyException">Thrown when failure probability is &lt;0 or &gt;1</exception>
        public FmSectionAssemblyDirectResult(EFmSectionCategory result, double failureProbability) : 
                base(EAssembledAssessmentResultType.AssessmentCategoryWithFailureProbability) {

            if (failureProbability < 0.0 || failureProbability > 1.0) {
                throw new AssemblyException("FmSectionAssemblyDirectResult", 
                    EAssemblyErrors.FailureProbabilityOutOfRange);
            }

            Result = result;
            FailureProbability = failureProbability;
        }

        /// <inheritdoc />
        public override FmSectionAssemblyResult Clone() {
            return double.IsNaN(FailureProbability)
                ? new FmSectionAssemblyDirectResult(Result)
                : new FmSectionAssemblyDirectResult(Result, FailureProbability);
        }

        /// <summary>
        /// Convert to string
        /// </summary>
        /// <returns>String of the object</returns>
        public override string ToString() {
            return "FmSectionAssemblyDirectResult [" + Result + " P: " + FailureProbability + "]";
        }

        /// <inheritdoc />
        public override bool HasResult() {
            return Result != EFmSectionCategory.Gr;
        }
    }
}