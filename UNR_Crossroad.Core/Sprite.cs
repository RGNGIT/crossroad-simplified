using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNR_Crossroad.Core
{
    // Класс для загрузки спрайтов (картинок) из проводника в программу
    public static class Sprite
    {
        // Временный битмап
        private static Bitmap _tmpBitmap;
        // Всевозможные цвета для машин и пешеходов
        public static string[] Color =
        {
            "blue", "green", "orange", "purple", "red", "yellow"
        };
        // Цвета волос для пешеходов
        public static string[] Hair =
        {
            "Blond", "Brown", "Black"
        };
        // Массивы с названиями и картинками
        public static string[] CarFileName = new string[42];
        public static Bitmap[] SpriteLibUp = new Bitmap[42];
        public static Bitmap[] SpriteLibDown = new Bitmap[42];
        public static Bitmap[] SpriteLibLeft = new Bitmap[42];
        public static Bitmap[] SpriteLibRight = new Bitmap[42];
        public static Bitmap[] SpriteLibDownLights = new Bitmap[3];
        public static Bitmap[] SpriteLibLeftLights = new Bitmap[3];
        public static Bitmap[] SpriteLibUpLights = new Bitmap[3];
        public static Bitmap[] SpriteLibRightLights = new Bitmap[3];
        public static string[] PeopleFileName1 = new string[18];
        public static string[] PeopleFileName2 = new string[18];
        public static Bitmap[] SpriteLibRightPeople1 = new Bitmap[18];
        public static Bitmap[] SpriteLibRightPeople2 = new Bitmap[18];
        public static Bitmap[] SpriteLibUpPeople1 = new Bitmap[18];
        public static Bitmap[] SpriteLibUpPeople2 = new Bitmap[18];
        public static Bitmap[] SpriteLibDownPeople1 = new Bitmap[18];
        public static Bitmap[] SpriteLibDownPeople2 = new Bitmap[18];
        public static Bitmap[] SpriteLibLeftPeople1 = new Bitmap[18];
        public static Bitmap[] SpriteLibLeftPeople2 = new Bitmap[18];

        public static Label Lb { get; set; }
        public static bool IsDone { get; set; }
        private static int _delay = 1;
        // Метод, который вызывает все методы для загрузки спрайтов
        public static void LoadCarSpriteLib() 
        {
            LoadCarFileName();
            LoadUpSprite();
            LoadRightSprite();
            LoadDownSprite();
            LoadLeftSprite();
            LoadDownLightsSprite();
            LoadLeftLightsSprite();
            LoadRightLightsSprite();
            LoadUpLightsSprite();
            LoadPeopleFileName();
            LoadPeopleLibDown();
            LoadPeopleLibLeft();
            LoadPeopleLibRight();
            LoadPeopleLibUp();
            Thread.Sleep(1000);
            IsDone = true;
        }
        // Загрузка машин
        private static void LoadCarFileName()
        {
            for (int i = 0; i < CarFileName.Length;)
            {
                foreach (var clr in Color)
                {
                    for (int k = 1; k < 8; k++)
                    {
                        CarFileName[i] = "img\\Car\\car" + k + "_" + clr + ".png";
                        i++;
                    }
                }
            }
        }
        // Загрузка пешеходов
        private static void LoadPeopleFileName()
        {
            for (int i = 0; i < PeopleFileName1.Length;)
            {
                foreach (var clr in Color)
                {
                    foreach (var h in Hair)
                    {
                        PeopleFileName1[i] = "img\\People\\Person_" + clr + h + "1" + ".png";
                        PeopleFileName2[i] = "img\\People\\Person_" + clr + h + "2" + ".png";
                        i++;
                    }
                }
            }
        }
        // Далее методы загружают спрайты, при этом переворачивая их по 90 градусов (для разных направлений)
        private static void LoadUpSprite()
        {
            for (int i = 0; i < SpriteLibUp.Length; i++)
            {
                Thread.Sleep(_delay);
                _tmpBitmap = new Bitmap(Image.FromFile(CarFileName[i]));
                SpriteLibUp[i] = _tmpBitmap;
            }
        }

        private static void LoadRightSprite()
        {
            for (int i = 0; i < SpriteLibRight.Length; i++)
            {
                Thread.Sleep(_delay);
                _tmpBitmap = new Bitmap(Image.FromFile(CarFileName[i]));
                _tmpBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                SpriteLibRight[i] = _tmpBitmap;
            }
        }

        private static void LoadDownSprite()
        {
            for (int i = 0; i < SpriteLibDown.Length; i++)
            {
                Thread.Sleep(_delay);
                _tmpBitmap = new Bitmap(Image.FromFile(CarFileName[i]));
                _tmpBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                SpriteLibDown[i] = _tmpBitmap;
            }
        }

        private static void LoadLeftSprite()
        {
            for (int i = 0; i < SpriteLibLeft.Length; i++)
            {
                Thread.Sleep(_delay);
                _tmpBitmap = new Bitmap(Image.FromFile(CarFileName[i]));
                _tmpBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                SpriteLibLeft[i] = _tmpBitmap;
            }
        }
        // Далее загружаем изображения светофоров, переворачивая их по 90 градусов
        private static void LoadDownLightsSprite()
        {
            SpriteLibDownLights[0] = new Bitmap(Image.FromFile("img\\Lights\\Red.png"));
            SpriteLibDownLights[1] = new Bitmap(Image.FromFile("img\\Lights\\Yellow.png"));
            SpriteLibDownLights[2] = new Bitmap(Image.FromFile("img\\Lights\\Green.png"));
            Thread.Sleep(_delay * 5);
        }
        private static void LoadUpLightsSprite()
        {
            _tmpBitmap = new Bitmap(Image.FromFile("img\\Lights\\Red.png"));
            _tmpBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            SpriteLibUpLights[0] = _tmpBitmap;
            _tmpBitmap = new Bitmap(Image.FromFile("img\\Lights\\Yellow.png"));
            _tmpBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            SpriteLibUpLights[1] = _tmpBitmap;
            _tmpBitmap = new Bitmap(Image.FromFile("img\\Lights\\Green.png"));
            _tmpBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            SpriteLibUpLights[2] = _tmpBitmap;
            Thread.Sleep(_delay * 5);
        }

        private static void LoadRightLightsSprite()
        {
            _tmpBitmap = new Bitmap(Image.FromFile("img\\Lights\\Red.png"));
            _tmpBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
            SpriteLibRightLights[0] = _tmpBitmap;
            _tmpBitmap = new Bitmap(Image.FromFile("img\\Lights\\Yellow.png"));
            _tmpBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
            SpriteLibRightLights[1] = _tmpBitmap;
            _tmpBitmap = new Bitmap(Image.FromFile("img\\Lights\\Green.png"));
            _tmpBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
            SpriteLibRightLights[2] = _tmpBitmap;
            Thread.Sleep(_delay * 5);
        }

        private static void LoadLeftLightsSprite()
        {
            _tmpBitmap = new Bitmap(Image.FromFile("img\\Lights\\Red.png"));
            _tmpBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            SpriteLibLeftLights[0] = _tmpBitmap;
            _tmpBitmap = new Bitmap(Image.FromFile("img\\Lights\\Yellow.png"));
            _tmpBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            SpriteLibLeftLights[1] = _tmpBitmap;
            _tmpBitmap = new Bitmap(Image.FromFile("img\\Lights\\Green.png"));
            _tmpBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            SpriteLibLeftLights[2] = _tmpBitmap;
            Thread.Sleep(_delay * 5); 
        }
        // Далее загружаем пешеходов, переворачивая их по 90 градусов
        private static void LoadPeopleLibLeft()
        {
            for (int i = 0; i < SpriteLibLeftPeople1.Length; i++)
            {
                Thread.Sleep(_delay);
                SpriteLibLeftPeople1[i] = new Bitmap(Image.FromFile(PeopleFileName1[i]));
                SpriteLibLeftPeople2[i] = new Bitmap(Image.FromFile(PeopleFileName2[i]));
            }
        }

        private static void LoadPeopleLibRight()
        {
            for (int i = 0; i < SpriteLibLeftPeople1.Length; i++)
            {
                Thread.Sleep(_delay);
                _tmpBitmap = new Bitmap(Image.FromFile(PeopleFileName1[i]));
                _tmpBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                SpriteLibRightPeople1[i] = _tmpBitmap;
                _tmpBitmap = new Bitmap(Image.FromFile(PeopleFileName2[i]));
                _tmpBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                SpriteLibRightPeople2[i] = _tmpBitmap;
            }
        }
        private static void LoadPeopleLibUp()
        {
            for (int i = 0; i < SpriteLibLeftPeople1.Length; i++)
            {
                Thread.Sleep(_delay);
                _tmpBitmap = new Bitmap(Image.FromFile(PeopleFileName1[i]));
                _tmpBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                SpriteLibUpPeople1[i] = _tmpBitmap;
                _tmpBitmap = new Bitmap(Image.FromFile(PeopleFileName2[i]));
                _tmpBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                SpriteLibUpPeople2[i] = _tmpBitmap;
            }
        }

        private static void LoadPeopleLibDown()
        {
            for (int i = 0; i < SpriteLibLeftPeople1.Length; i++)
            {
                Thread.Sleep(_delay);
                _tmpBitmap = new Bitmap(Image.FromFile(PeopleFileName1[i]));
                _tmpBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                SpriteLibDownPeople1[i] = _tmpBitmap;
                _tmpBitmap = new Bitmap(Image.FromFile(PeopleFileName2[i]));
                _tmpBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                SpriteLibDownPeople2[i] = _tmpBitmap;
            }
        }
    }
}