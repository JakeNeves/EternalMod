﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Eternal.NPCs.Boss.Empraynia
{
    [AutoloadBossHead]
    public class Empraynia : ModNPC
    {
        int timer;

        private Player player;

        bool expert = Main.expertMode;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
            NPCID.Sets.TrailCacheLength[npc.type] = 8;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 78000;
            npc.width = 221;
            npc.height = 227;
            npc.damage = 180;
            npc.defense = 64;
            npc.knockBackResist = -1f;
            npc.boss = true;
            npc.noTileCollide = true;
            music = MusicID.Boss4;
            npc.noGravity = true;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.Confused] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.HitSound = SoundID.NPCHit12;
            npc.DeathSound = SoundID.NPCDeath5;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < damage / npc.lifeMax * 50; k++)
                Dust.NewDust(npc.position, npc.width, npc.height, 27, 2.5f * hitDirection, -2.5f, 0, default, 1.7f);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 80000;
            npc.damage = 182;
            if (EternalWorld.hellMode)
            {
                npc.lifeMax = 82000;
                npc.damage = 184;
            }
        }

        public override bool PreAI()
        {
            npc.netUpdate = true;
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                npc.velocity.Y = -100;
            }
            if (npc.ai[0] == 0)
            {
                #region Flying Movement
                float speed;
                if (EternalWorld.hellMode)
                {
                    speed = 10f;
                }
                else
                {
                    speed = 8f;
                }
                float acceleration = 0.10f;
                Vector2 vector2 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float xDir = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector2.X;
                float yDir = (float)(Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - 120) - vector2.Y;
                float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);
                if (length > 400 && Main.expertMode)
                {
                    ++speed;
                    acceleration += 0.05F;
                    if (length > 600)
                    {
                        ++speed;
                        acceleration += 0.05F;
                        if (length > 800)
                        {
                            ++speed;
                            acceleration += 0.05F;
                        }
                    }
                }
                float num10 = speed / length;
                xDir = xDir * num10;
                yDir = yDir * num10;
                if (npc.velocity.X < xDir)
                {
                    npc.velocity.X = npc.velocity.X + acceleration;
                    if (npc.velocity.X < 0 && xDir > 0)
                        npc.velocity.X = npc.velocity.X + acceleration;
                }
                else if (npc.velocity.X > xDir)
                {
                    npc.velocity.X = npc.velocity.X - acceleration;
                    if (npc.velocity.X > 0 && xDir < 0)
                        npc.velocity.X = npc.velocity.X - acceleration;
                }
                if (npc.velocity.Y < yDir)
                {
                    npc.velocity.Y = npc.velocity.Y + acceleration;
                    if (npc.velocity.Y < 0 && yDir > 0)
                        npc.velocity.Y = npc.velocity.Y + acceleration;
                }
                else if (npc.velocity.Y > yDir)
                {
                    npc.velocity.Y = npc.velocity.Y - acceleration;
                    if (npc.velocity.Y > 0 && yDir < 0)
                        npc.velocity.Y = npc.velocity.Y - acceleration;
                }
                #endregion


                if (npc.ai[1] % 50 == 0)
                {
                    Vector2 dir = Main.player[npc.target].Center - npc.Center;
                    dir += new Vector2(Main.rand.Next(-40, 41), Main.rand.Next(-40, 41));
                    dir.Normalize();
                    dir *= 12;
                    int newNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<EmprayniaRoller>(), npc.whoAmI);
                    Main.npc[newNPC].velocity = dir;
                }

            }
                return true;
        }

        public override void AI()
        {

            npc.netUpdate = true;
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];

            timer++;
            if ((timer == 200 || timer == 400 && npc.life >= (npc.lifeMax / 2)))
            {
                //Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 73);

                //Main.PlaySound(SoundID.DD2_LightningBugZap, npc.position);
                Main.PlayTrackedSound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);
                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                direction.Normalize();
                direction.X *= 8.5f;
                direction.Y *= 8.5f;

                int amountOfProjectiles = Main.rand.Next(8, 11);
                for (int i = 0; i < amountOfProjectiles; ++i)
                {
                    float A = (float)Main.rand.Next(-200, 200) * 0.01f;
                    float B = (float)Main.rand.Next(-200, 200) * 0.01f;
                    int damage = expert ? 15 : 17;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ProjectileID.DD2DrakinShot, damage, 1, Main.myPlayer, 0, 0);
                }
            }
            else if ((timer == 600 || timer == 650 || timer == 700 || timer == 800 || timer == 850 || timer == 880))
            {
                //Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 73);
                Main.PlayTrackedSound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    int damage = expert ? 15 : 19;
                    if (EternalWorld.hellMode)
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X * 14f, direction.Y * 14f, ProjectileID.ShadowBeamHostile, damage, 1, Main.myPlayer, 0, 0);
                    }
                    else {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X * 14f, direction.Y * 14f, ProjectileID.DD2DarkMageBolt, damage, 1, Main.myPlayer, 0, 0);
                    }
                }
            }
            else if ((timer == 900 || timer == 950))
            {
                if (Main.rand.Next(50) == 5)
                {
                    Main.PlayTrackedSound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, -12, 0, ProjectileID.ShadowBeamHostile, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 12, 0, ProjectileID.ShadowBeamHostile, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 0, 12, ProjectileID.ShadowBeamHostile, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 0, -12, ProjectileID.ShadowBeamHostile, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, -12, -12, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 12, -12, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, -12, 12, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 12, 12, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
                }
                else
                {
                    Main.PlayTrackedSound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, -12, 0, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 12, 0, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 0, 12, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 0, -12, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, -12, -12, ProjectileID.ShadowBeamHostile, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 12, -12, ProjectileID.ShadowBeamHostile, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, -12, 12, ProjectileID.ShadowBeamHostile, 6, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 12, 12, ProjectileID.ShadowBeamHostile, 6, 0, Main.myPlayer, 0f, 0f);
                }
            }
            else if (timer == 1000)
            {
                timer = 0;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 0.24f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override void NPCLoot()
        {
            Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, -12, 0, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 12, 0, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 0, 12, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 0, -12, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, -12, -12, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 12, -12, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, -12, 12, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 80, npc.position.Y + 80, 12, 12, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);

            Projectile.NewProjectile(npc.position.X + 40, npc.position.Y + 40, -8, 0, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 40, npc.position.Y + 40, 8, 0, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 40, npc.position.Y + 40, 0, 8, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 40, npc.position.Y + 40, 0, -8, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 40, npc.position.Y + 40, -8, -8, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 40, npc.position.Y + 40, 8, -8, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 40, npc.position.Y + 40, -8, 8, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(npc.position.X + 40, npc.position.Y + 40, 8, 8, ProjectileID.DD2DarkMageBolt, 6, 0, Main.myPlayer, 0f, 0f);

            if (Main.expertMode)
            {
                //npc.DropBossBags();
            }
            else
            {
                
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
            lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
            => GlowMaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/Empraynia/Empraynia_Glow"));

        private bool AliveCheck(Player player)
        {
            if (player.dead)
            {
                if (npc.timeLeft > 30)
                    npc.timeLeft = 30;
                npc.velocity.Y -= 1f;
                return false;
            }
            return true;
        }

    }
}
