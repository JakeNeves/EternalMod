﻿using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Eternal.Tiles;
using System;
using System.IO;
using System.Collections.Generic;

namespace Eternal
{
    class EternalWorld : ModWorld
    {
        #region BiomeTiles
        public static int DuneTiles;
        #endregion

        public static bool hellMode = false;

        #region DownedBosses
        public static bool downedCarmaniteScouter = false;
        public static bool downedDunekeeper = false;
        public static bool downedIncinerius = false;
        public static bool downedSubzeroElemental = false;
        #endregion

        public override void TileCountsAvailable(int[] tileCounts)
        {
            DuneTiles = tileCounts[TileType<Dunestone>()] + tileCounts[TileType<Dunestone>()];
            thunderduneBiome = DuneTiles;
            commet = tileCounts[TileType<CometiteOre>()];
        }

        public override void Initialize()
		{
			downedCarmaniteScouter = false;
		}

		public override TagCompound Save()
		{
			var downed = new List<string>();
			if (downedCarmaniteScouter) downed.Add("eternal");
            if (downedDunekeeper) downed.Add("eternal");
            if (downedIncinerius) downed.Add("eternal");
            if (downedSubzeroElemental) downed.Add("eternal");

            return new TagCompound
			{
				{"downed", downed }
			};

		}

        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedCarmaniteScouter = downed.Contains("eternal");
            downedDunekeeper = downed.Contains("eternal");
            downedIncinerius = downed.Contains("eternal");
            downedSubzeroElemental = downed.Contains("eternal");
        }

        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();
            if (loadVersion == 0)
            {
                BitsByte flags = reader.ReadByte();
                #region DownedBossFlags
                downedCarmaniteScouter = flags[0];
                downedDunekeeper = flags[0];
                downedIncinerius = flags[0];
                downedSubzeroElemental = flags[0];
                #endregion
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedCarmaniteScouter;
            flags[0] = downedDunekeeper;
            flags[0] = downedIncinerius;
            flags[0] = downedSubzeroElemental;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            #region DownedBossFlags
            downedCarmaniteScouter = flags[0];
            downedDunekeeper = flags[0];
            downedIncinerius = flags[0];
            downedSubzeroElemental = flags[0];
            #endregion
        }

        //ripped this from tremor, credit to whoever wrote this originally...
        public static void DropComet()
        {
            bool flag = true;
            if (Main.netMode == 1)
            {
                return;
            }
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].active)
                {
                    flag = false;
                    break;
                }
            }
            int num = 0;
            float num2 = Main.maxTilesX / 4200;
            int num3 = (int)(400f * num2);
            for (int j = 5; j < Main.maxTilesX - 5; j++)
            {
                int num4 = 5;
                while (num4 < Main.worldSurface)
                {
                    if (Main.tile[j, num4].active() && Main.tile[j, num4].type == (ushort)TileType<CometiteOre>())
                    {
                        num++;
                        if (num > num3)
                        {
                            return;
                        }
                    }
                    num4++;
                }
            }
            float num5 = 600f;
            while (!flag)
            {
                float num6 = Main.maxTilesX * 0.08f;
                int num7 = Main.rand.Next(150, Main.maxTilesX - 150);
                while (num7 > Main.spawnTileX - num6 && num7 < Main.spawnTileX + num6)
                {
                    num7 = Main.rand.Next(150, Main.maxTilesX - 150);
                }
                int k = (int)(Main.worldSurface * 0.3);
                while (k < Main.maxTilesY)
                {
                    if (Main.tile[num7, k].active() && Main.tileSolid[Main.tile[num7, k].type])
                    {
                        int num8 = 0;
                        int num9 = 15;
                        for (int l = num7 - num9; l < num7 + num9; l++)
                        {
                            for (int m = k - num9; m < k + num9; m++)
                            {
                                if (WorldGen.SolidTile(l, m))
                                {
                                    num8++;
                                    if (Main.tile[l, m].type == 189 || Main.tile[l, m].type == 202)
                                    {
                                        num8 -= 100;
                                    }
                                }
                                else if (Main.tile[l, m].liquid > 0)
                                {
                                    num8--;
                                }
                            }
                        }
                        if (num8 < num5)
                        {
                            num5 -= 0.5f;
                            break;
                        }
                        flag = Comet(num7, k);
                        if (flag)
                        {
                        }
                        break;
                    }
                    k++;
                }
                if (num5 < 100f)
                {
                    return;
                }
            }
        }

        //also ripped this from tremor, credit to whoever wrote this originally...
        public static bool Comet(int i, int j)
        {
            if (i < 50 || i > Main.maxTilesX - 50)
            {
                return false;
            }
            if (j < 50 || j > Main.maxTilesY - 50)
            {
                return false;
            }
            int num = 35;
            Rectangle rectangle = new Rectangle((i - num) * 16, (j - num) * 16, num * 2 * 16, num * 2 * 16);
            for (int k = 0; k < 255; k++)
            {
                if (Main.player[k].active)
                {
                    Rectangle value = new Rectangle((int)(Main.player[k].position.X + Main.player[k].width / 2 - NPC.sWidth / 2 - NPC.safeRangeX), (int)(Main.player[k].position.Y + Main.player[k].height / 2 - NPC.sHeight / 2 - NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    if (rectangle.Intersects(value))
                    {
                        return false;
                    }
                }
            }
            for (int l = 0; l < 200; l++)
            {
                if (Main.npc[l].active)
                {
                    Rectangle value2 = new Rectangle((int)Main.npc[l].position.X, (int)Main.npc[l].position.Y, Main.npc[l].width, Main.npc[l].height);
                    if (rectangle.Intersects(value2))
                    {
                        return false;
                    }
                }
            }
            for (int m = i - num; m < i + num; m++)
            {
                for (int n = j - num; n < j + num; n++)
                {
                    if (Main.tile[m, n].active() && Main.tile[m, n].type == 21)
                    {
                        return false;
                    }
                }
            }
            num = WorldGen.genRand.Next(17, 23);
            for (int num2 = i - num; num2 < i + num; num2++)
            {
                for (int num3 = j - num; num3 < j + num; num3++)
                {
                    if (num3 > j + Main.rand.Next(-2, 3) - 5)
                    {
                        float num4 = Math.Abs(i - num2);
                        float num5 = Math.Abs(j - num3);
                        float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                        if (num6 < num * 0.9 + Main.rand.Next(-4, 5))
                        {
                            if (!Main.tileSolid[Main.tile[num2, num3].type])
                            {
                                Main.tile[num2, num3].active(false);
                            }
                            Main.tile[num2, num3].type = (ushort)TileType<CometiteOre>();
                        }
                    }
                }
            }
            num = WorldGen.genRand.Next(8, 14);
            for (int num7 = i - num; num7 < i + num; num7++)
            {
                for (int num8 = j - num; num8 < j + num; num8++)
                {
                    if (num8 > j + Main.rand.Next(-2, 3) - 4)
                    {
                        float num9 = Math.Abs(i - num7);
                        float num10 = Math.Abs(j - num8);
                        float num11 = (float)Math.Sqrt(num9 * num9 + num10 * num10);
                        if (num11 < num * 0.8 + Main.rand.Next(-3, 4))
                        {
                            Main.tile[num7, num8].active(false);
                        }
                    }
                }
            }
            num = WorldGen.genRand.Next(25, 35);
            for (int num12 = i - num; num12 < i + num; num12++)
            {
                for (int num13 = j - num; num13 < j + num; num13++)
                {
                    float num14 = Math.Abs(i - num12);
                    float num15 = Math.Abs(j - num13);
                    float num16 = (float)Math.Sqrt(num14 * num14 + num15 * num15);
                    if (num16 < num * 0.7)
                    {
                        if (Main.tile[num12, num13].type == 5 || Main.tile[num12, num13].type == 32 || Main.tile[num12, num13].type == 352)
                        {
                            WorldGen.KillTile(num12, num13, false, false, false);
                        }
                        Main.tile[num12, num13].liquid = 0;
                    }
                    if (Main.tile[num12, num13].type == (ushort)TileType<CometiteOre>())
                    {
                        if (!WorldGen.SolidTile(num12 - 1, num13) && !WorldGen.SolidTile(num12 + 1, num13) && !WorldGen.SolidTile(num12, num13 - 1) && !WorldGen.SolidTile(num12, num13 + 1))
                        {
                            Main.tile[num12, num13].active(false);
                        }
                        else if ((Main.tile[num12, num13].halfBrick() || Main.tile[num12 - 1, num13].topSlope()) && !WorldGen.SolidTile(num12, num13 + 1))
                        {
                            Main.tile[num12, num13].active(false);
                        }
                    }
                    WorldGen.SquareTileFrame(num12, num13, true);
                    WorldGen.SquareWallFrame(num12, num13, true);
                }
            }
            num = WorldGen.genRand.Next(23, 32);
            for (int num17 = i - num; num17 < i + num; num17++)
            {
                for (int num18 = j - num; num18 < j + num; num18++)
                {
                    if (num18 > j + WorldGen.genRand.Next(-3, 4) - 3 && Main.tile[num17, num18].active() && Main.rand.Next(10) == 0)
                    {
                        float num19 = Math.Abs(i - num17);
                        float num20 = Math.Abs(j - num18);
                        float num21 = (float)Math.Sqrt(num19 * num19 + num20 * num20);
                        if (num21 < num * 0.8)
                        {
                            if (Main.tile[num17, num18].type == 5 || Main.tile[num17, num18].type == 32 || Main.tile[num17, num18].type == 352)
                            {
                                WorldGen.KillTile(num17, num18, false, false, false);
                            }
                            Main.tile[num17, num18].type = (ushort)TileType<CometiteOre>();
                            WorldGen.SquareTileFrame(num17, num18, true);
                        }
                    }
                }
            }
            num = WorldGen.genRand.Next(30, 38);
            for (int num22 = i - num; num22 < i + num; num22++)
            {
                for (int num23 = j - num; num23 < j + num; num23++)
                {
                    if (num23 > j + WorldGen.genRand.Next(-2, 3) && Main.tile[num22, num23].active() && Main.rand.Next(20) == 0)
                    {
                        float num24 = Math.Abs(i - num22);
                        float num25 = Math.Abs(j - num23);
                        float num26 = (float)Math.Sqrt(num24 * num24 + num25 * num25);
                        if (num26 < num * 0.85)
                        {
                            if (Main.tile[num22, num23].type == 5 || Main.tile[num22, num23].type == 32 || Main.tile[num22, num23].type == 352)
                            {
                                WorldGen.KillTile(num22, num23, false, false, false);
                            }
                            Main.tile[num22, num23].type = (ushort)TileType<CometiteOre>();
                            WorldGen.SquareTileFrame(num22, num23, true);
                        }
                    }
                }
            }
            return true;
        }

        private void GenDune(GenerationProgress progress)
        {
            progress.Message = "Generating Thunderdune...";

        }

        private void EternalOres(GenerationProgress progress)
        {
            progress.Message = "Generating Eternalism";

			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 6E-05); k++)
			{
				int x = WorldGen.genRand.Next(0, Main.maxTilesX);
				int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY);
				WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), TileType<Rudanium>());

            }
		}

        public static int thunderduneBiome = 0;
        public static int commet = 0;

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int shiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (shiniesIndex == -1)
            {
                return;
            }

            tasks.Insert(shiniesIndex + 4, new PassLegacy("Generating Thunderdune...", GenDune));
        }

        //Dune Temple
        private void MakeDuneTemple()
        {
            float widthScale = Main.maxTilesX / 42000f;
            int numberToGenerate = WorldGen.genRand.Next(1, (int)(2f / widthScale));
            for (int k = 0; k < numberToGenerate; k++)
            {
                bool success = false;
                int attempts = 0;
                while (!success)
                {
                    attempts++;
                    if (attempts > 1000)
                    {
                        success = true;
                        continue;
                    }
                    int i = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
                    if (i <= Main.maxTilesX / 2 - 50 || i >= Main.maxTilesX / 2 + 50)
                    {
                        int j = 0;
                        while (!Main.tile[i, j].active() && (double)j < EternalWorld.thunderduneBiome)
                        {
                            j++;
                        }
                        if (Main.tile[i, j].type == TileType<Dunesand>() || Main.tile[i, j].type == TileType<Dunestone>())
                        {
                            j--;
                            if (j > 150)
                            {
                                bool placementOK = true;
                                for (int l = i - 4; l < i + 4; l++)
                                {
                                    for (int m = j - 6; m < j + 20; m++)
                                    {
                                        if (Main.tile[l, m].active())
                                        {
                                            int type = (int)Main.tile[l, m].type;
                                            if (type == TileID.BlueDungeonBrick || type == TileID.GreenDungeonBrick || type == TileID.PinkDungeonBrick || type == TileID.Cloud || type == TileID.RainCloud)
                                            {
                                                placementOK = false;
                                            }
                                        }
                                    }
                                }
                                if (placementOK)
                                {
                                    success = PlaceDuneTemple(i, j);
                                }
                            }
                        }
                    }
                }
            }
        }

        private readonly int[,] _duneTempleShape =
        {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0},
            {1,1,1,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0},
            {1,1,1,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0},
            {1,1,1,0,0,0,0,0,0,0,1,1,1,2,2,2,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,1,1,1,0,0,0,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,1,1,1,0,0,0,1,1,1},
            {1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };

        public bool PlaceDuneTemple(int i, int j)
        {
            if (!WorldGen.SolidTile(i, j + 1))
            {
                return false;
            }

            if(Main.tile[i, j].active())
            {
                return false;
            }
            if (j < 150)
            {
                return false;
            }

            for (int y = 0; y < _duneTempleShape.GetLength(0); y++)
            {
                for (int x = 0; x < _duneTempleShape.GetLength(1); x++)
                {
                    int k = i - 3 + x;
                    int l = j - 6 + y;
                    if(WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, 1);
                        switch (_duneTempleShape[y, x])
                        {
                            case 1:
                                tile.type = (ushort)TileType<Dunestone>();
                                tile.active(true);
                            break;

                            case 2:
                                tile.type = TileID.TeamBlockYellowPlatform;
                                tile.active(true);
                            break;
                        }

                    }
                }
            }

            return true;
        }

    }
}
