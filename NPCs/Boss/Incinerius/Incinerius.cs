﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Eternal.Items.BossBags;
using Eternal.Items.Weapons.Melee;
using Eternal.Items.Weapons.Magic;
using Eternal.Items.Accessories.Hell;
namespace Eternal.NPCs.Boss.Incinerius
{
    [AutoloadBossHead]
    public class Incinerius : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        #region Fundementals
        const int ShootType = ProjectileID.ImpFireball;
        const int ShootDamage = 9;
        const float ShootKnockback = 0f;
        const int ShootDirection = 5;

        const float Speed = 6f;
        const float Acceleration = 2f;
        int Timer;
        #endregion

        public override void SetDefaults()
        {
            npc.width = 99;
            npc.height = 119;
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/FieryBattler");
            npc.aiStyle = -1;
            npc.damage = 12;
            npc.defense = 20;
            npc.lifeMax = 20000;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.buffImmune[BuffID.Electrified] = true;
            npc.lavaImmune = true;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = null;
            npc.DeathSound = SoundID.NPCDeath42;
            bossBag = ItemType<IncineriusBag>();
        }

        public Vector2 bossCenter
        {
            get { return npc.Center; }
            set { npc.position = value - new Vector2(npc.width / 2, npc.height / 2); }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            if (!NPC.downedMoonlord)
            {
                potionType = ItemID.GreaterHealingPotion;
            }
            else
            {
                potionType = ItemID.None;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 22000;
            npc.damage = 14; //(int)(npc.damage + 1.5f);
            npc.defense = 22; //(int)(npc.defense + numPlayers);

            if (EternalWorld.hellMode)
            {
                npc.lifeMax = 24000;
                npc.damage = 16; //(int)(npc.damage + 2.5f);
                npc.defense = 24; //(int)(npc.defense + numPlayers);
            }

        }

        public override void HitEffect(int hitDirection, double damage)
        {
            Main.PlaySound(SoundID.Tink, npc.position);
            if (npc.life <= 0)
            {
                if (!NPC.downedMoonlord)
                {
                    Main.NewText("Ack...", 215, 95, 0);
                }
                else if(NPC.downedMoonlord)
                {
                    Main.NewText("I am not done yet...", 215, 95, 0);
                    Main.NewText("Incinerius reveals his true form!", 175, 75, 255);
                    NPC.NewNPC((int)npc.Center.X - 20, (int)npc.Center.Y, NPCType<TrueIncinerius>());
                }
                
                Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/IncineriusHead"), 1f);
                Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/IncineriusBody"), 1f);
            }
        }

        private Player player;

        public override void NPCLoot()
        {
            if (!NPC.downedMoonlord)
            {
                if (EternalWorld.hellMode)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<ProtectiveFlame>());
                }

                if (Main.expertMode)
                {
                    npc.DropBossBags();
                }
                else
                {
                    if (Main.rand.Next(1) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<FuryFlare>());
                    }
                    if (Main.rand.Next(2) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Incinerator>());
                    }
                    if (Main.rand.Next(3) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<SmotheringInferno>());
                    }
                    if (Main.rand.Next(4) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Pyroyo>());
                    }
                }
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int num1 = (int)(npc.position.X + (npc.width / 2)) / 16;
                int num2 = (int)(npc.position.Y + (npc.height / 2)) / 16;
                int num5 = npc.width / 2 / 16 + 1;
                for (int index2 = num1 - num5; index2 <= num1 + num5; ++index2)
                {
                    for (int index3 = num2 - num5; index3 <= num2 + num5; ++index3)
                    {
                        if ((index2 == num1 - num5 || index2 == num1 + num5 || (index3 == num2 - num5 || index3 == num2 + num5)) && !Main.tile[index2, index3].active())
                        {
                            Main.tile[index2, index3].type = (ushort)TileType<Tiles.ScorchedBrick>();
                            Main.tile[index2, index3].active(true);
                        }
                        Main.tile[index2, index3].lava(false);
                        Main.tile[index2, index3].liquid = 0;
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendTileSquare(-1, index2, index3, 1);
                        else
                            WorldGen.SquareTileFrame(index2, index3, true);
                    }
                }
            }
        }

        public override void AI()
        {
            npc.spriteDirection = npc.direction;

            Lighting.AddLight(npc.position, 0.215f, 0.95f, 0f);

            if (NPC.AnyNPCs(NPCType<RollingFire>()))
            {
                npc.dontTakeDamage = true;
            }
            if (!NPC.AnyNPCs(NPCType<RollingFire>()))
            {
                npc.dontTakeDamage = false;
            }

            npc.TargetClosest(true);
            npc.spriteDirection = npc.direction;
            Player player = Main.player[npc.target];
            if (player.dead || !player.active)
            {
                npc.TargetClosest(false);
                npc.active = false;
            }
            Timer++;
            if(Timer >= 0)
            {
                Vector2 StartPosition = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float DirectionX = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - StartPosition.X;
                float DirectionY = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - 120 - StartPosition.Y;
                float Length = (float)Math.Sqrt(DirectionX * DirectionX + DirectionY * DirectionY);
                float Num = Speed / Length;
                DirectionX = DirectionX * Num;
                DirectionY = DirectionY * Num;
                if (npc.velocity.X < DirectionX)
                {
                    npc.velocity.X = npc.velocity.X + Acceleration;
                    if (npc.velocity.X < 0 && DirectionX > 0)
                        npc.velocity.X = npc.velocity.X + Acceleration;
                }
                else if (npc.velocity.X > DirectionX)
                {
                    npc.velocity.X = npc.velocity.X - Acceleration;
                    if (npc.velocity.X > 0 && DirectionX < 0)
                        npc.velocity.X = npc.velocity.X - Acceleration;
                }
                if (npc.velocity.Y < DirectionY)
                {
                    npc.velocity.Y = npc.velocity.Y + Acceleration;
                    if (npc.velocity.Y < 0 && DirectionY > 0)
                        npc.velocity.Y = npc.velocity.Y + Acceleration;
                }
                else if (npc.velocity.Y > DirectionY)
                {
                    npc.velocity.Y = npc.velocity.Y - Acceleration;
                    if (npc.velocity.Y > 0 && DirectionY < 0)
                        npc.velocity.Y = npc.velocity.Y - Acceleration;
                }
                if (Main.rand.Next(36) == 1)
                {
                    Vector2 StartPosition2 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height / 2));
                    float BossRotation = (float)Math.Atan2(StartPosition2.Y - (Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)), StartPosition2.X - (Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)));
                    npc.velocity.X = (float)(Math.Cos(BossRotation) * 9) * -1;
                    npc.velocity.Y = (float)(Math.Sin(BossRotation) * 9) * -1;
                    npc.netUpdate = true;
                }
            }
            if(Timer == 700)
            {
                IncineriusShot();
                SpawnRollingFire();
            }
            if(Timer == 900)
            {
                SpawnRollingFire();
            }
            if(Timer == 1000)
            {
                IncineriusShot();

                Timer = 0;
            }
        }

        void IncineriusShot()
        {
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, -ShootDirection, 0, ShootType, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, ShootDirection, 0, ShootType, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, 0, ShootDirection, ShootType, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 20, npc.position.Y + 20, 0, -ShootDirection, ShootType, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
        }

        void SpawnRollingFire()
        {
            for (int i = 0; i < 50; i++)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire);
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].velocity *= 0f;
            }
            NPC.NewNPC((int)npc.Center.X - 20, (int)npc.Center.Y, NPCType<RollingFire>());
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int Frame = (int)npc.frameCounter;
            npc.frame.Y = Frame * frameHeight;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        /*public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D drawTexture = Main.npcTexture[npc.type];
            Vector2 origin = new Vector2((drawTexture.Width / 2) * 0.5F, (drawTexture.Height / Main.npcFrameCount[npc.type]) * 0.5F);

            Vector2 drawPos = new Vector2(
                npc.position.X - Main.screenPosition.X + (npc.width / 2) - (Main.npcTexture[npc.type].Width / 2) * npc.scale / 2f + origin.X * npc.scale,
                npc.position.Y - Main.screenPosition.Y + npc.height - Main.npcTexture[npc.type].Height * npc.scale / Main.npcFrameCount[npc.type] + 4f + origin.Y * npc.scale + npc.gfxOffY);

            SpriteEffects effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(drawTexture, drawPos, npc.frame, Color.Orange, npc.rotation, origin, npc.scale, effects, 0);

            return false;
        }*/
    }
}
