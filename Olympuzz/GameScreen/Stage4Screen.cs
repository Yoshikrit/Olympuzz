﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olympuzz.GameScreen
{
    class Stage4Screen : PlayScreen
    {
        //song and sfx
        protected Song endlessTheme;
        public override void LoadContent()
        {
            base.LoadContent();

            //stage map
            stageBGPic = content.Load<Texture2D>("Stag_1/Poseidon Stage");
            boardBGPic = content.Load<Texture2D>("Stag_1/board");

            //bg music
            endlessTheme = content.Load<Song>("Sounds/PoseidonTheme");
            MediaPlayer.Play(endlessTheme);
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
