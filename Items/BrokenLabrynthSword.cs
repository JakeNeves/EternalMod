﻿using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eternal.Items
{
    public class BrokenLabrynthSword : ModItem
    {

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'This sword use to have a soul living in this...'");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 38;
            item.value = Item.sellPrice(platinum: 3);
            item.rare = ItemRarityID.Red;
            item.maxStack = 999;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = EternalColor.Teal;
                }
            }
        }
    }
}
