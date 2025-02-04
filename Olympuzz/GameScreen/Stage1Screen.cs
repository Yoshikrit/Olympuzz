﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Olympuzz.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Olympuzz.Singleton;

namespace Olympuzz.GameScreen
{
    class Stage1Screen : PlayScreen
    {
        //song and sfx
        private Song poseidonTheme;

        private bool isBallHolderDie = false;
        private bool skillActive = false;
        private bool hasSwitched = false;
        private bool isWave = false;

        private float timeBossSkillActive = 10f; // Time of skill is in Active          // period of active skill
        private float timeCDBossSkillActive = 5f; // Chance of skill that will Active   // Time of next boss skill active 
        
        private float timeKillBallHolder = 5f;  // period of BallHolder die
        private float timeCDBallHoldkill = 10f; // Time of next BallHolder kill active

        private int switchSkill;

        private Random rand = new Random();

        private Color waveColor;
        //sound
        private bool isSFXPlay = false;

        public override void Initial()
        {
            //all button
            pauseButton = new Button(pauseButtonPic, new Vector2(68, 50), new Vector2(148, 60));//create button object on playscreen
            //sound
            waveColor = new Color(58, 172, 212);
            _scrollSpd = 3f;

            timeAttack = 180f;

            base.Initial();
        }
        public override void LoadContent()
        {
            base.LoadContent();

            //stage map
            stageBGPic = content.Load<Texture2D>("Stage1/PoseidonStage");
            boardBGPic = content.Load<Texture2D>("Stage1/Board1");
            //all button on playscreen
            pauseButtonPic = content.Load<Texture2D>("Stage1/Pause1");
            //poseidon trident pic
            poseidonTridentPic = content.Load<Texture2D>("Stage1/PoseidonTrident");

            //bg music and sfx
            poseidonTheme = content.Load<Song>("Sounds/PoseidonTheme");
            MediaPlayer.Play(poseidonTheme);

            tridentSound = content.Load<SoundEffect>("Sounds/PoseidonTridentSound");

            Initial();
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            waveSoundInstance.Volume = masterSFX;
            if (!notPlay)
            {
                timeAttack -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                if (!hasSwitched)
                {
                    switchSkill = rand.Next(2);
                    hasSwitched = true;
                }
                
                if (!dionysusSkilled)

                {
                    switch (switchSkill)
                    {
                        case 0:
                            timeCDBossSkillActive -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            if (timeCDBossSkillActive < 0 && !skillActive)
                            {
                                int chance = rand.Next(3);
                                if (chance == 1)
                                {
                                    Singleton.Instance.speed = -300;
                                    isWave = true;
                                    skillActive = true;
                                }
                            }
                            break;

                        case 1:
                            timeCDBallHoldkill -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            if (timeCDBallHoldkill < 0 && !skillActive)
                            {
                                int chance = rand.Next(2);
                                if (chance == 1)
                                {
                                    isBallHolderDie = true;
                                    skillActive = true;
                                }
                            }
                            break;
                    }
                }
                
                if (skillActive) // skill was Active now this is not skillCool
                {
                    switch (switchSkill)
                    {
                        case 0:
                            timeBossSkillActive -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            break;

                        case 1:
                            timeKillBallHolder -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            break;
                    }
                }

                if (timeBossSkillActive < 0 || timeKillBallHolder < 0) //if 
                {
                    Singleton.Instance.speed = -1400;
                    isBallHolderDie = false;
                    skillActive = false;
                    hasSwitched = false;
                    isWave = false;
                    isSFXPlay = false;
                    timeBossSkillActive = 10f;
                    timeKillBallHolder = 5f;
                    timeCDBossSkillActive = 5;
                    timeCDBallHoldkill = 10;
                }

                //poseidon wave
                if (isWave && !isSFXPlay)
                {
                    waveSoundInstance.Play(); 
                    waveSoundInstance.Resume();
                    isSFXPlay = true;
                }
                if (!isWave)
                {
                    waveSoundInstance.Stop();
                }
                
            }
            else
            {
                waveSoundInstance.Pause();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bossSkillPic, Vector2.Zero, Color.Black);
            spriteBatch.Draw(boardBGPic, new Vector2(336, 54), Color.White);
            spriteBatch.Draw(stageBGPic, Vector2.Zero, Color.White);

            //time
            fontSize = smallfonts.MeasureString(((int)timeAttack).ToString());
            spriteBatch.DrawString(smallfonts, ((int)timeAttack).ToString(), new Vector2(1048, 128), new Color(33, 35, 60, 255));

            if (!isBallHolderDie)
            {
                shooter.GetBubbleNext().Draw(spriteBatch);
                spriteBatch.Draw(holderAlivePic, new Vector2(410, 606), Color.White);
                isSFXPlay = false;
            }
            else
            {
                spriteBatch.Draw(holderDeathPic, new Vector2(410, 606), Color.White);
                if (!isSFXPlay)
                {
                    isSFXPlay = true;
                    tridentSound.Play(volume: Singleton.Instance.soundMasterVolume, 0, 0);
                }
                spriteBatch.Draw(poseidonTridentPic, new Vector2(390, 520), Color.White);
            }

            if (skillCooldown || athenaSkilled)
            {
                switch (Singleton.Instance.charState)
                {
                    case CharState.ATHENA:
                        spriteBatch.Draw(athenaSkillPic, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.HERMES:
                        spriteBatch.Draw(hermesSkillPic, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.DIONYSUS:
                        spriteBatch.Draw(dionysusSkillPic, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.HEPHAESTUS:
                        spriteBatch.Draw(hephaestusSkillPic, new Vector2(106, 466), Color.White);
                        break;
                }
                if (!godSkill.GetAnimationStop())
                {
                    godSkill.Draw(spriteBatch);
                }
            }
            else
            {
                switch (Singleton.Instance.charState)
                {
                    case CharState.ATHENA:
                        spriteBatch.Draw(athenaReadyPic, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.HERMES:
                        spriteBatch.Draw(hermesReadyPic, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.DIONYSUS:
                        spriteBatch.Draw(dionysusSkillPic, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.HEPHAESTUS:
                        spriteBatch.Draw(hephaestusSkillPic, new Vector2(106, 466), Color.White);
                        break;
                }
            }

            //draw god skill
            switch (Singleton.Instance.charState)
            {
                case CharState.ATHENA:
                    godSkill.Draw(spriteBatch);
                    break;
                case CharState.DIONYSUS:
                    if (dionysusSkilled)
                    {
                        switch (Singleton.Instance.levelState)
                        {
                            case LevelState.POSEIDON:
                                spriteBatch.Draw(dionysusMutePic, new Vector2(1029, 398), Color.White);
                                break;
                            case LevelState.HADES:
                                spriteBatch.Draw(dionysusMutePic, new Vector2(994, 394), Color.White);
                                break;
                            case LevelState.ZEUS:
                                spriteBatch.Draw(dionysusMutePic, new Vector2(1075, 290), Color.White);
                                break;
                        }
                    }
                    break;
                case CharState.HEPHAESTUS:
                    if (hammerSkill)
                    {
                        godSkill.Draw(spriteBatch);
                    }
                    break;
            }

            shooter.Draw(spriteBatch);

            base.Draw(spriteBatch);

            if (isWave && eventScreen != EventScreen.PAUSE && eventScreen != EventScreen.WIN && eventScreen != EventScreen.LOSE)
            {
                spriteBatch.Draw(bossSkillPic, Vector2.Zero, waveColor * 0.2f);
            }

        }
    }
}
