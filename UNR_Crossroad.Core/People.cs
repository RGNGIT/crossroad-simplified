using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNR_Crossroad.Core
{
    // Класс пешехода
    public class People : IMovementMember
    {
        // Координата X
        public int X { get; set; }
        // Координата Y
        public int Y { get; set; }
        // Скорость
        public int Speed { get; set; }
        // Изображение
        public Bitmap Sprite { get; set; }
        // Номер спрайта
        public int NSprite { get; set; }
        // Направление
        public Vector Direct { get; set; }
        // Сторона (куда идет)
        public Side Pside { get; set; }
        // Выставлена ли правая нога (ну типа в один момент у нас может быть выставлена только левая или только правая нога, значит 0 или 1)
        public bool IsRightLeg { get; set; }

        // Конструктор класса. Заполняем начальными данными
        public People(int x, int y, int speed, Bitmap sprite,int ns, Vector dir, Side cr)
        {
            X = x;
            Y = y;
            Speed = speed;
            Sprite = sprite;
            Direct = dir;
            Pside = cr;
            IsRightLeg = true;
            NSprite = ns;
        }
        // Сменить выставленную вперед ногу (поменять спрайт)
        public static void ChangeLeg(People p)
        {
            switch (p.Pside)
            {
                case Side.Right:
                    p.Sprite = p.IsRightLeg ? Core.Sprite.SpriteLibRightPeople2[p.NSprite] : Core.Sprite.SpriteLibRightPeople1[p.NSprite];
                    p.IsRightLeg = !p.IsRightLeg;
                    break;
                case Side.Left:
                    p.Sprite = p.IsRightLeg ? Core.Sprite.SpriteLibLeftPeople2[p.NSprite] : Core.Sprite.SpriteLibLeftPeople1[p.NSprite];
                    p.IsRightLeg = !p.IsRightLeg;
                    break;
                case Side.Up:
                    p.Sprite = p.IsRightLeg ? Core.Sprite.SpriteLibUpPeople2[p.NSprite] : Core.Sprite.SpriteLibUpPeople1[p.NSprite];
                    p.IsRightLeg = !p.IsRightLeg;
                    break;
                case Side.Down:
                    p.Sprite = p.IsRightLeg ? Core.Sprite.SpriteLibDownPeople2[p.NSprite] : Core.Sprite.SpriteLibDownPeople1[p.NSprite];
                    p.IsRightLeg = !p.IsRightLeg;
                    break;
            }
        }
    }
}
