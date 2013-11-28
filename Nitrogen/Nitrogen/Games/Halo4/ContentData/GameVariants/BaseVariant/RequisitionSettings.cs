﻿/*
 *   Nitrogen - Halo Content API
 *   Copyright © 2013 The Nitrogen Authors. All rights reserved.
 * 
 *   This file is part of Nitrogen.
 *
 *   Nitrogen is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   Nitrogen is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with Nitrogen.  If not, see <http://www.gnu.org/licenses/>.
 */

using Nitrogen.IO;
using System;
using System.Collections.Generic;

namespace Nitrogen.Games.Halo4.ContentData.GameVariants.BaseVariant
{
    /// <summary>
    /// Represents a set of settings in a multiplayer variant for some kind of requisition-related
    /// feature which was cut from the release version of Halo 4.
    /// </summary>
    public class RequisitionSettings
        : ISerializable<BitStream>
    {
        private float unk0;
        private int unk1, unk2;
        private List<RequisitionData> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequisitionSettings"/> class with default values.
        /// </summary>
        public RequisitionSettings()
        {
            this.data = new List<RequisitionData>();
        }

        #region ISerializable<BitStream>

        public void Serialize(BitStream s)
        {
            s.Stream(ref this.unk0);
            s.Stream(ref this.unk1);

            int count = this.data.Count;
            s.Stream(ref count, 7);

            s.Serialize(this.data, 0, count);
            s.Stream(ref this.unk2);
        }

        #endregion
    }
}