#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace RTSPrototype
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Texture2D TreeTexture;
        public Rectangle TreePosition;
        List<Structure> TreeList = new List<Structure>();
        Texture2D GridTexture;
        Texture2D Cursor;
        public Rectangle CursorPosition;
        public Rectangle CursorPlacer;

        public Rectangle[,] Grid;

        Vector2 ScreenSize = new Vector2(1024, 1280);
        public Vector2 MapSize = new Vector2(100, 50);

        Vector2 Camera = Vector2.Zero;
        Vector2 CameraSpeed = Vector2.Zero;

        MouseState PrevMouse = Mouse.GetState();


        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
            graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;

            graphics.IsFullScreen = true;
            IsMouseVisible = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GridTexture = Content.Load<Texture2D>("dirt");
            Cursor = Content.Load<Texture2D>("cursor");
            TreeTexture = Content.Load<Texture2D>("tree");
            TreePosition = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 32, 32);

            Grid = new Rectangle[(int)MapSize.X, (int)MapSize.Y];
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CursorPosition = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 32, 32);





            for (int i = 0; i < (int)MapSize.X; i++)
                for (int j = 0; j < (int)MapSize.Y; j++)
                {
                    Grid[i, j] = new Rectangle((int)Camera.X + 32 * i, (int)Camera.Y + 32 * j, 32, 32);

                    CursorPlacer = new Rectangle(CursorPosition.X + Grid[i, j].X, CursorPosition.Y + Grid[i, j].Y, 32, 32);

                    if (Mouse.GetState().LeftButton == ButtonState.Released && PrevMouse.LeftButton == ButtonState.Pressed)
                    {
                        if (TreePosition == Grid[i, j])
                        {
                            Structure Tree = new Structure();
                            TreeList.Add(Tree);
                        }
                    }
                    else if (Mouse.GetState().LeftButton == ButtonState.Released)
                    {
                        if (CursorPosition.X >= Grid[i, j].X && CursorPosition.Y >= Grid[i, j].Y)
                        {
                            TreePosition.X = Grid[i, j].X;
                            TreePosition.Y = Grid[i, j].Y;
                        }
                    }

                    PrevMouse = Mouse.GetState();

                }

            if (CursorPosition.X <= -ScreenSize.X + ScreenSize.X)
            {
                if (CameraSpeed.X < 5)
                    CameraSpeed.X += 1f;
            }
            else if (CursorPosition.X >= ScreenSize.X + 200)
            {
                if (CameraSpeed.X > -5)
                    CameraSpeed.X -= 1;
            }
            else if (CursorPosition.Y <= -ScreenSize.Y + ScreenSize.Y)
            {
                if (CameraSpeed.Y < 5)
                    CameraSpeed.Y += 1f;
            }
            else if (CursorPosition.Y >= ScreenSize.Y - 300)
            {
                if (CameraSpeed.Y > -5)
                    CameraSpeed.Y -= 1f;
            }
            else
            {
                if (CameraSpeed.X > 0)
                    CameraSpeed.X -= 1f;
                else if (CameraSpeed.X < 0)
                    CameraSpeed.X += 1f;
                else if (CameraSpeed.Y > 0)
                    CameraSpeed.Y -= 1f;
                else if (CameraSpeed.Y < 0)
                    CameraSpeed.Y += 1f;
                else
                {
                    CameraSpeed = Vector2.Zero;
                }
            }

            Camera += CameraSpeed * gameTime.ElapsedGameTime.Milliseconds / 5;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            foreach (Rectangle grid in Grid)
                spriteBatch.Draw(GridTexture, grid, Color.White);

            spriteBatch.Draw(TreeTexture, TreePosition, Color.White);

            for (int i = 0; i < (int)MapSize.X; i++)
                for (int j = 0; j < (int)MapSize.Y; j++)
                {
                        foreach (Structure tree in TreeList)
                            tree.Draw(spriteBatch, this, new Vector2(Grid[i, j].X, Grid[i, j].Y));
                }

            spriteBatch.Draw(Cursor, CursorPosition, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
