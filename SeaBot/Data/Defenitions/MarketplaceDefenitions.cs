﻿// SeaBotCore
// Copyright (C) 2018 - 2019 Weespin
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Defenitions
{
    public class MarketplaceDefenitions
    {
        public class Root
        {
            [J("items")] public Items Items { get; set; }
        }

        public class Items
        {
            [J("item")] public List<Item> Item { get; set; }
        }

        public class Item
        {
            [J("def_id")] public long DefId { get; set; }
            [J("version_id")] public long VersionId { get; set; }
            [J("name")] public string Name { get; set; }
            [J("name_loc")] public string NameLoc { get; set; }
            [J("speedupable")] public long Speedupable { get; set; }
            [J("sailors")] public long Sailors { get; set; }
            [J("slots")] public long Slots { get; set; }
            [J("travel_time")] public long TravelTime { get; set; }
            [J("outpost_id")] public long OutpostId { get; set; }
            [J("contractor_id")] public long ContractorId { get; set; }
            [J("map_x")] public long MapX { get; set; }
            [J("map_y")] public long MapY { get; set; }
            [J("atlas")] public string Atlas { get; set; }
            [J("texture")] public string Texture { get; set; }
            [J("iso_width")] public long IsoWidth { get; set; }
            [J("iso_height")] public long IsoHeight { get; set; }
            [J("points")] public string Points { get; set; }
            [J("controls")] public string Controls { get; set; }
            [J("materials")] public Materials Materials { get; set; }
        }

        public class Materials
        {
            [J("material")] public List<Material> Material { get; set; }
        }

        public class Material
        {
            [J("desc_loc")] public string DescLoc { get; set; }
            [J("contractor_id")] public long ContractorId { get; set; }
            [J("id")] public long Id { get; set; }
            [J("input_id")] public long InputId { get; set; }
            [J("input_koef")] public long InputKoef { get; set; }
            [J("output_id")] public long OutputId { get; set; }
            [J("output_type")] public string OutputType { get; set; }
            [J("amount")] public long Amount { get; set; }
            [J("event_id")] public long EventId { get; set; }
        }
    }
}