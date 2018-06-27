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

using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Failure mechanism section with assessment category.
    /// </summary>
    public abstract class FmSectionWithCategory
    {
        /// <summary>
        /// Failure mechanism with category constructor.
        /// </summary>
        /// <param name="sectionStart">The start of the section in meters from the beginning of the assessment section.
        ///  Must be greater than 0</param>
        /// <param name="sectionEnd">The end of the section in meters from the beginning of the assessment section.
        ///  Must be greater than 0 and greater than the start of the section</param>
        /// <param name="type">The type of failure mechanism (direct or indirect)</param>
        /// <exception cref="AssemblyException">Thrown when start of end are below zero and 
        /// when end is before the start</exception>
        protected FmSectionWithCategory(double sectionStart, double sectionEnd, EAssembledAssessmentResultType type)
        {
            if (sectionStart < 0.0 || sectionEnd < 0.0 || sectionEnd <= sectionStart)
            {
                throw new AssemblyException("FmSectionWithCategory", EAssemblyErrors.FmSectionSectionStartEndInvalid);
            }

            SectionStart = sectionStart;
            SectionEnd = sectionEnd;
            Type = type;
        }

        /// <summary>
        /// The start of the section in meters from the beginning of the assessment section.
        /// </summary>
        public double SectionStart { get; }

        /// <summary>
        /// The end of the section in meters from the beginning of the assessment section.
        /// </summary>
        public double SectionEnd { get; }

        /// <summary>
        /// Whether this section contains direct or indirect results.
        /// </summary>
        public EAssembledAssessmentResultType Type { get; }
    }
}