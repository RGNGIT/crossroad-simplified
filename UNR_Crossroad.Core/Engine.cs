﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace UNR_Crossroad.Core
{
    // Главный класс симуляции
    public static class Engine
    {
        // База машин
        public static List<Car> Cars { get; set; }
        // База пешеходов
        public static List<People> Peoples { get; set; }
        // Готов ли движок?
        public static bool IsReady { get; set; }
        // Сброшена ли симуляция?
        public static bool Clear { get; set; }
        // Удалятор подвижных сущностей (машин, пешеходов)
        public static List<IMovementMember> Deleter { get; set; }
        // Класс рандом для рандомизации событий
        public static Random R { get; set; }
        // Текущая дорога
        public static Road CurrentRoad { get; set; }
        // Таймер генерации машин
        public static Timer GenCarTimer { get; set; }
        // Таймер генерации пешеходов
        public static Timer GenPeopleTimer { get; set; }
        // Таймер движения машин
        public static Timer MoveTimer { get; set; }
        // Таймер движения пешеходов
        public static Timer PMoveTimer { get; set; }
        // Таймер симуляции
        public static Timer WorkTimer { get; set; }
        // Интервал светофоров
        public static int LightsInterval1 { get; set; } 
        public static int LightsInterval2 { get; set; }
        // Таймер светофоров
        public static Timer LightsTimer { get; set; }
        // Панель управления юзера
        public static Panel UserPanel { get; set; }
        // Классы поворотов
        public static RightTurn RightTurn { get; set; }
        public static LeftTurn LeftTurn { get; set; }
        // Текстбоксы визуальные по количествам машин
        public static TextBox CarCount { get; set; }
        public static TextBox CurrentlyCarCount { get; set; }
        // Другие текстбоксы визуальные по времени работы
        public static TextBox WorkTime { get; set; }
        public static TextBox Cpm { get; set; }
        public static int WorkTm { get; set; }
        // Ссылки на светофоры
        public static TrafficLight[] TrafficLights { get; set; }
        public static RoadTransit CurrentRoadTransit { get; set; }
        // Запуск симуляции. Просто стартуем все методы симулятора
        public static void Start()
        {
            MoveTimer.Start();
            PMoveTimer.Start();
            GenCarTimer.Start();
            WorkTimer.Start();
            GenPeopleTimer.Start();
            LightsTimer.Start();
            TrafficLight.CreateLight();
            IsReady = true;
        }
        // Пауза симуляции. Идентично
        public static void Pause()
        {
            MoveTimer.Stop();
            PMoveTimer.Stop();
            GenCarTimer.Stop();
            WorkTimer.Stop();
            GenPeopleTimer.Stop();
            LightsTimer.Stop();
        }
        // Стоп симуляции. Помимо паузы еще и чистим вилкой все данные сессии
        public static void Stop()
        {
            Cars.Clear();
            Peoples.Clear();
            Deleter.Clear();
            MoveTimer.Stop();
            PMoveTimer.Stop();
            LightsTimer.Stop();
            GenCarTimer.Stop();
            GenPeopleTimer.Stop();
            WorkTimer.Stop();
            CarCount.Text= "0";
            CurrentlyCarCount.Text = "0";
            WorkTime.Text = "0 c";
            Cpm.Text = "0";
            WorkTm = 0;
            UserPanel.ResetBackColor();
            UserPanel.Invalidate();
        }
        // Инициализация на старте всякими данными, что нам нужны в работе
        public static void Initialization()
        {
            Cars = new List<Car>();
            Peoples = new List<People>();
            Deleter = new List<IMovementMember>();
            R = new Random();
            LightsInterval1 = 30;
            LightsInterval2 = 30;
            TrafficLights = new TrafficLight[4];
            RightTurn = new RightTurn();
            LeftTurn = new LeftTurn();
            MoveTimer = new Timer { Interval = 10 };
            MoveTimer.Tick += (sender, e) => Update();
            PMoveTimer = new Timer {Interval = 100};
            PMoveTimer.Tick += (sender, e) => P_Move();
            GenCarTimer = new Timer { Interval = 100 };
            GenCarTimer.Tick += (sender, e) => GenerateCar_Tick();
            GenPeopleTimer = new Timer {Interval = 1000};
            GenPeopleTimer.Tick += (sender, e) => GeneratePeople_Tick();
            WorkTimer = new Timer {Interval = 1000};
            WorkTimer.Tick += (sender, e) => Work_tick();
            LightsTimer = new Timer {Interval = LightsInterval1};
            LightsTimer.Tick += (sender, e) => TrafficLight.SwitchLight();
        }
        // Тик симуляции, или атомарный шаг симуляции. Это такая единица, за которую что-то происходит на экране. Координата меняется или еще что, вобщем все
        public static void Work_tick()
        {
            WorkTm++;
            WorkTime.Text = WorkTm + " c";
            Cpm.Text = Math.Round(Convert.ToDouble(CarCount.Text) * (60/ (double)WorkTm)).ToString(CultureInfo.InvariantCulture);
        }
        // Этот метод вызывается периодически и проверяет текущие состояния. В частности состояния машин и могут ли они ехать или нет и т д
        public static void Update()
        {
            CurrentlyCarCount.Text = Cars.Count.ToString();
            foreach (var c in Cars)
            {
                if (Car.Check(c))
                {
                    c.Speed = 0;
                }
                else
                {
                    c.Speed = Car.CanMove(c);
                    if (c.Turn == CTurn.Right)
                    {
                        RightTurn.StartTurn(c);
                    }
                    else if (c.Turn == CTurn.Left)
                    {
                        LeftTurn.StartTurn(c);
                    }
                }
                Move(c);
                if (c.X < -50 || c.X > UserPanel.Width + 50 || c.Y < -50 || c.Y > UserPanel.Height + 50)
                {
                    Deleter.Add(c);
                }
            }
            if (Deleter.Count != 0)
            {
                foreach (var d in Deleter)
                {
                    if (d is Car)
                    {
                        Cars.Remove((Car)d);
                    }
                    else if (d is People)
                    {
                        Peoples.Remove((People)d);
                    }
                }
            }
            UserPanel.Invalidate();
        }
        // Движение пешеходов со сменой анимации + удаление когда они зайдут за зону пешеходки
        public static void P_Move()
        {
            foreach (var p in Peoples)
            {
                People.ChangeLeg(p);
                Move(p);
                if (p.X < UserPanel.Width / 2 - CurrentRoad.VerticalRoadLeft * 40 - 101 || p.X > UserPanel.Width / 2 + CurrentRoad.VerticalRoadLeft * 40 + 101 || p.Y > UserPanel.Height / 2 + CurrentRoad.HorizontRoadDown * 40 + 101 || p.Y < UserPanel.Height / 2 - CurrentRoad.HorizontRoadUp * 40 - 101) // дописать
                {
                    Deleter.Add(p);
                }
            }
        }
        // Метод передвижения любого двигающегося персонажа машины или пешехода
        public static void Move(IMovementMember mm)
        {
            mm.Y += (int)mm.Direct.Y * mm.Speed;
            mm.X += (int)mm.Direct.X * mm.Speed;
        }

        // Нарисовать карту
        public static void RenderMap(object sender, PaintEventArgs e)
        {
            if (IsReady)
            {
                CurrentRoad.RenderRoad(UserPanel, e);
                CurrentRoadTransit.RenderRoadTransit(UserPanel, e);
                foreach (var light in TrafficLights)
                {
                    TrafficLight.RenderLight(light,UserPanel, e);
                }
                foreach (var c in Cars)
                {
                    e.Graphics.DrawImage(c.Sprite, new Point(c.X, c.Y));
                }
                foreach (var p in Peoples)
                {
                    e.Graphics.DrawImage(p.Sprite, new Point(p.X, p.Y));
                }
            }
            else if (Clear)
            {
                e.Graphics.Clear(Color.AliceBlue);
                Clear = false;
            }
        }
        // Генератор машин
        public static void GenerateCar_Tick()
        {
            CarCount.Text = (Convert.ToInt32(CarCount.Text) + 1).ToString();
            switch (R.Next(1, 5))
            {
                case 1:
                    GenerateMembers.VerticalLeftCar();
                    break;
                case 2:
                    GenerateMembers.VerticalRightCar();
                    break;
                case 3:
                    GenerateMembers.HorizontalUpCar();
                    break;
                case 4:
                    GenerateMembers.HorizontalDownCar();
                    break;
            }
        }
        // Генератор пешеходов
        public static void GeneratePeople_Tick()
        {
            if (TrafficLights[3].CurrLight == TrafficLight.Lights.Green)
            {
                if (CurrentRoadTransit.RightRoad)
                    GenerateMembers.RightPeople();
                if (CurrentRoadTransit.LeftRoad)
                    GenerateMembers.LeftPeople();
            }
            else if (TrafficLights[2].CurrLight == TrafficLight.Lights.Green)
            {
                if (CurrentRoadTransit.UpRoad)
                    GenerateMembers.UpPeople();
                if (CurrentRoadTransit.DownRoad)
                    GenerateMembers.DownPeople();
            }
        }
    }
}
