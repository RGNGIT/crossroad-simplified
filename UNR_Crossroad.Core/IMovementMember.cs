using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UNR_Crossroad.Core
{
    // Интерфейс для всех подвижных сущностей (машина, пешеход)
    public interface IMovementMember
    {
        // Координата X
        int X { get; set; }
        // Координата Y
        int Y { get; set; }
        // Скорость
        int Speed { get; set; }
        // Изображение
        Bitmap Sprite { get; set; }
        // Направление
        Vector Direct { get; set; }
    }
    // Перечисление возможных направлений
    public enum Side
    {
        Right, Left, Up, Down
    }
    // Перечивление поворотов
    public enum CTurn
    {
        Right, Left, No
    }
}
