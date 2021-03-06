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
#region

using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

#endregion

namespace SeaBotCore.Data.Materials
{
    #region

    using System.Collections.Generic;

    using SeaBotCore.Data.Definitions;

    #endregion

    public class MaterialsData
    {
        public class Item
        {
            [J("def_id")]
            public int DefId { get; set; }

            [J("disposable")]
            public int Disposable { get; set; }

            [J("limited")]
            public int Limited { get; set; }

            [J("name")]
            public string Name { get; set; }

            [J("name_loc")]
            public string NameLoc { get; set; }

            [J("texture")]
            public string Texture { get; set; }
        }

        public class Items
        {
            [J("item")]
            public List<Item> Item { get; set; }
        }

        public class Root : IDefinition
        {
            [J("items")]
            public Items Items { get; set; }
        }
    }
}