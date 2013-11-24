﻿/*
 *   Nitrogen - Halo Content API
 *   Copyright (c) 2013 Collin Dillinger, Matt Saville and Aaron Dierking
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

using Nitrogen.Core.ContentData;
using Nitrogen.Core.ContentData.Localization;
using Nitrogen.Core.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Nitrogen.Core.ContentData
{
    /// <summary>
    /// Provides an overview of a level (map).
    /// </summary>
    /// <remarks>Represents the 'levl' chunk in a map info BLF file.</remarks>
    public class CampaignShared
        : Campaign
    {
        private const int LanguageCount = 12;
        private const int MaxLevels = 64;

        private int[] mapIDs;
        int unk2;

        /// <summary>
        /// Initializes a new instance of the <see cref="Halo4Level"/> class with default values.
        /// </summary>
        public CampaignShared()
            : base(version: 3)
        {
            Name = new LocalizedNameCampaign(CommonProperties.Languages, "");
            Description = new LocalizedDescription(CommonProperties.Languages, "");

            this.mapIDs = new int[MaxLevels];
            for (int i = 0; i < this.mapIDs.Length; i++)
                mapIDs[i] = -1;
        }

        /// <summary>
        /// Gets or sets the IDs of the maps in the campaign.
        /// </summary>
        public int[] MapIDs
        {
            get { return this.mapIDs; }
            set { this.mapIDs = value; }
        }

        #region Level Members

        protected override void SerializeEndianStreamData(EndianStream s)
        {
            base.SerializeEndianStreamData(s);

            s.Stream(this.mapIDs, 0, this.mapIDs.Length);
            s.Stream(ref this.unk2);
        }

        #endregion

    }
}