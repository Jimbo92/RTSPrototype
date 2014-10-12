#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
#endregion

namespace RTSPrototype
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Texture2D TreeTexture;
        public Rectangle TreePosition;
        public Texture2D PlacerTexture;
        List<Structure> TreeList = new List<Structure>();
        Texture2D GridTexture;
        Texture2D Cursor;
        public Rectangle CursorPosition;
        public Rectangle CursorPlacer;

        public int[,] GridData;
        public string[] index = new string[0];
        Rectangle DrawTile;

        public int TileSize = 32;

        Vector2 ScreenSize = new Vector2(800, 600);
        public Vector2 MapSize = new Vector2(20, 10);

        public Vector2 Camera = Vector2.Zero;
        Vector2 CameraSpeed = Vector2.Zero;

        MouseState PrevMouse = Mouse.GetState();

        int PrevScrollValue = Mouse.GetState().ScrollWheelValue;

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

            graphics.IsFullScreen = false;
            IsMouseVisible = false;

            for (int i = 0; i < (int)MapSize.X; i++)
                for (int j = 0; j < (int)MapSize.Y; j++)
                {
                    GridData = new int[j, i];
                }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GridTexture = Content.Load<Texture2D>("grid");
            Cursor = Content.Load<Texture2D>("cursor");
            TreeTexture = Content.Load<Texture2D>("tree");
            PlacerTexture = Content.Load<Texture2D>("placer");
            TreePosition = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 32, 32);

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.F12))
            {
                StreamWriter sw = new StreamWriter("file.txt");

                int[] Rows = new int[(int)MapSize.X];

                foreach(int row in Rows) 
                {
                    foreach (int data in GridData)
                    {
                        sw.Write(data + ",");
                    }
                    sw.WriteLine(); 
                }
                sw.Close();
            }


            CursorPosition = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);
            CursorPlacer.Width = 32;
            CursorPlacer.Height = 32;

            if (Mouse.GetState().ScrollWheelValue < PrevScrollValue)
            {
                TileSize -= 2;
            }
            if (Mouse.GetState().ScrollWheelValue > PrevScrollValue)
            {
                TileSize += 2;
            }
            PrevScrollValue = Mouse.GetState().ScrollWheelValue;

            if (TileSize <= 12)
                TileSize = 12;

            if (CursorPosition.X <= -ScreenSize.X + ScreenSize.X)
            {
                if (CameraSpeed.X < 5)
                    CameraSpeed.X += 1f;
            }
            else if (CursorPosition.X >= ScreenSize.X - 1)
            {
                if (CameraSpeed.X > -5)
                    CameraSpeed.X -= 1;
            }
            else if (CursorPosition.Y <= -ScreenSize.Y + ScreenSize.Y)
            {
                if (CameraSpeed.Y < 5)
                    CameraSpeed.Y += 1f;
            }
            else if (CursorPosition.Y >= ScreenSize.Y - 1)
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
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            for (int i = 0; i < GridData.GetLength(1); i++)
                for (int j = 0; j < GridData.GetLength(0); j++)
                {
                    DrawTile = new Rectangle((int)Camera.X + TileSize * i, (int)Camera.Y + TileSize * j, TileSize, TileSize);

                    if (GridData[j, i] < 1)
                        spriteBatch.Draw(GridTexture, DrawTile, Color.White);

                    if (GridData[j, i] == 1)
                        spriteBatch.Draw(TreeTexture, DrawTile, Color.White);

                    if (DrawTile.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        CursorPlacer = DrawTile;

                        spriteBatch.Draw(PlacerTexture, CursorPlacer, Color.White);

                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            GridData[j, i] = 1;
                        }

                        if (Mouse.GetState().RightButton == ButtonState.Pressed)
                        {
                            GridData[j, i] = 0;
                        }
                    }

                }


            spriteBatch.Draw(Cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 32, 32), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
