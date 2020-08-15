﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Eternal.Items;
using static Terraria.ModLoader.ModContent;
using System.Linq;
using Eternal.Tiles;

namespace Eternal.NPCs
{
    class CosmigeldeonSlime : ModNPC
    {
		public override void SetStaticDefaults()
		{
			//Main.npcFrameCount[npc.type] = 2;
		}

		public override void SetDefaults()
		{
			npc.width = 46;
			npc.damage = 20;
			npc.height = 72;
			npc.aiStyle = 1;
			npc.defense = 10;
			npc.lifeMax = 6900;
			npc.knockBackResist = 0f;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 25f;
			npc.rarity = 3;
			aiType = NPCID.RainbowSlime;
			animationType = NPCID.RainbowSlime;
			Main.npcFrameCount[npc.type] = 4;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
			{
				int[] TileArray2 = { ModContent.TileType<CometiteOre>(), TileID.Grass, TileID.Dirt, TileID.Stone, TileID.Sand, TileID.SnowBlock, TileID.IceBlock };
				return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMoonlord && player.ZoneOverworldHeight ? 2.09f : 0f;
			}
			return SpawnCondition.OverworldNightMonster.Chance * 0.5f;
		}

        /*public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            return NPC.downedMoonlord = true;
        }*/

        public override void NPCLoot()
		{
			if (Main.rand.Next(5) == 0)
			{	
			}
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Astragel>(), Main.rand.Next(20, 48));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<StarmetalBar>(), Main.rand.Next(10, 75));
		}
	}
}
