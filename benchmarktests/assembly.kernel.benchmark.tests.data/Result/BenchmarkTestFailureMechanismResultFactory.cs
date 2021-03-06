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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;

namespace assembly.kernel.benchmark.tests.data.Result
{
    /// <summary>
    /// Factory for creating instances of <see cref="BenchmarkFailureMechanismTestResult"/>.
    /// </summary>
    public static class BenchmarkTestFailureMechanismResultFactory
    {
        private static readonly Dictionary<MechanismType, Tuple<string, int>> infos =
            new Dictionary<MechanismType, Tuple<string, int>>
            {
                {MechanismType.STBI, new Tuple<string, int>("Macrostabiliteit binnenwaarts", 2)},
                {MechanismType.STBU, new Tuple<string, int>("Macrostabiliteit buitenwaarts", 4)},
                {MechanismType.STPH, new Tuple<string, int>("Piping", 2)},
                {MechanismType.STMI, new Tuple<string, int>("Microstabiliteit", 4)},
                {MechanismType.AGK, new Tuple<string, int>("Golfklappen op asfaltbekleding", 3)},
                {MechanismType.AWO, new Tuple<string, int>("Wateroverdruk bij asfaltbekleding", 4)},
                {MechanismType.GEBU, new Tuple<string, int>("Grasbekleding erosie buitentalud", 3)},
                {MechanismType.GABU, new Tuple<string, int>("Grasbekleding afschuiven buitentalud", 4)},
                {MechanismType.GEKB, new Tuple<string, int>("Grasbekleding erosie kruin en binnentalud", 1)},
                {MechanismType.GABI, new Tuple<string, int>("Grasbekleding afschuiven binnentalud", 4)},
                {MechanismType.ZST, new Tuple<string, int>("Stabiliteit steenzetting", 3)},
                {MechanismType.DA, new Tuple<string, int>("Duinafslag", 3)},
                {MechanismType.HTKW, new Tuple<string, int>("Hoogte kunstwerk", 1)},
                {MechanismType.BSKW, new Tuple<string, int>("Betrouwbaarheid sluiting kunstwerk", 1)},
                {MechanismType.PKW, new Tuple<string, int>("Piping bij kunstwerk", 4)},
                {MechanismType.STKWp, new Tuple<string, int>("Sterkte en stabiliteit punconstructies", 1)},
                {MechanismType.STKWl, new Tuple<string, int>("Sterkte en stabiliteit langsconstructies", 4)},
                {MechanismType.VLGA, new Tuple<string, int>("Golfafslag voorland", 5)},
                {MechanismType.VLAF, new Tuple<string, int>("Afschuiving voorland", 5)},
                {MechanismType.VLZV, new Tuple<string, int>("Zettingsvloeiing voorland", 5)},
                {MechanismType.NWObe, new Tuple<string, int>("Bebouwing", 5)},
                {MechanismType.NWObo, new Tuple<string, int>("Begroeiing", 5)},
                {MechanismType.NWOkl, new Tuple<string, int>("Kabels en leidingen", 5)},
                {MechanismType.NWOoc, new Tuple<string, int>("Overige constructies", 5)},
                {MechanismType.HAV, new Tuple<string, int>("Havendammen", 5)},
                {MechanismType.INN, new Tuple<string, int>("Technische innovaties", 4)}
            };

        /// <summary>
        /// Creates a default benchmark test result for the specified failure mechanism type.
        /// </summary>
        /// <param name="type">The <see cref="MechanismType"/> to get the result for.</param>
        /// <returns>A new <see cref="BenchmarkFailureMechanismTestResult"/>.</returns>
        public static BenchmarkFailureMechanismTestResult CreateFailureMechanismTestResult(MechanismType type)
        {
            if (!infos.ContainsKey(type))
            {
                throw new InvalidEnumArgumentException();
            }

            Tuple<string, int> mechanismInformation = infos[type];
            return new BenchmarkFailureMechanismTestResult(mechanismInformation.Item1, type, mechanismInformation.Item2);
        }
    }
}