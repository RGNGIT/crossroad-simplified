using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using UNR_Crossroad.Core;
using UNR_Crossroad.Data;

namespace UNR_Crossroad.Forms
{
    public partial class MainForm : Form
    {
        // Ссылки на текущего юзера и дороги
        private User myUser;
        private Dictionary<int, string> Roads;

        // Инициализация главной формы
        public MainForm()
        {
            InitializeComponent();
            // Установка и зарисовка картинок с дорогами, машинками и т д
            panel_user.Paint += Engine.RenderMap;
            panel_admin.Paint += Engine.RenderMap;
            // Установка изначальных параметров (количество машин, время работы и т д)
            Engine.UserPanel = panel_user;
            Engine.CarCount = tbAll;
            Engine.CurrentlyCarCount = tbCur;
            Engine.WorkTime = tbTime;
            Engine.Cpm = tbCpm;
            Engine.Initialization();
            // Загрузить картинки
            new Thread(() => Sprite.LoadCarSpriteLib()).Start();
            Thread.Sleep(10000);
            // Произвести автологин (перед этим создать админа)
            AutoAuthWorks();
        }

        void AutoAuthWorks()
        {
            // Тут создаем админа и автологинимся за ним
            DbEngine.Connect();
            if (!DbEngine.LoginCheck("admin"))
                DbEngine.AddUser("admin", "123", (int)AccLevel.Admin);

            myUser = new User("admin", "123", AccLevel.Admin);

            DbEngine.Close();
        }

        private void btn_ch_road_Click(object sender, EventArgs e)
        {
            // Запуск симуляции с кнопки
            Engine.Start();
            button9.Enabled = false;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            // Остановка симуляции с кнопки
            Engine.Pause();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Сброс симуляции с настройками с кнопки
            Engine.Stop();
            panel_user.Controls.Clear();
            Engine.IsReady = false;
            Engine.Clear = true;
            panel_user.Invalidate();
            TrafficLight.Clear();
            button9.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Предпросмотр карты симуляции с вкладки администратора
            Engine.CurrentRoad = new Road((int)numericUpDown2.Value, (int)numericUpDown1.Value, (int)numericUpDown3.Value, (int)numericUpDown4.Value);
            Engine.CurrentRoadTransit = new RoadTransit(checkBox2.Checked, checkBox1.Checked, checkBox3.Checked, checkBox4.Checked, (int)numericUpDown2.Value, (int)numericUpDown1.Value, (int)numericUpDown3.Value, (int)numericUpDown4.Value);
            Engine.IsReady = true;
            Engine.LightsInterval1 = (int)numericUpDown6.Value*1000;
            Engine.LightsInterval2 = (int)numericUpDown5.Value*1000;
            TrafficLight.CreateLight();
            panel_admin.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Сброс настроек симуляции с вкладки администратора
            panel_admin.Controls.Clear();
            Engine.IsReady = false;
            Engine.Clear = true;
            panel_admin.Invalidate();
            TrafficLight.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Обновление таймеров машин и пешеходов с кнопки
            Engine.GenCarTimer.Interval = (int)numericUpDown8.Value;
            Engine.GenPeopleTimer.Interval = (int)numericUpDown7.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Вызов формы с вводом названия сохраняемой карты + сохранение карты
            var namef = new NameForm();
            if (namef.ShowDialog() == DialogResult.OK)
            {
                var name = namef.GetName();
                DbEngine.Connect();
                DbEngine.SaveRoad(name, (int) numericUpDown1.Value, (int) numericUpDown2.Value,
                    (int) numericUpDown3.Value, (int) numericUpDown4.Value, (int) numericUpDown6.Value,
                    (int) numericUpDown5.Value, checkBox4.Checked, checkBox3.Checked, checkBox2.Checked,
                    checkBox1.Checked);
                DbEngine.Close();
            }
            FillGrid(dataGridView1);
        }

        private void FillGrid(DataGridView dgv)
        {
            // Метод для заполнения таблиц с дорогами
            dgv.Rows.Clear();
            Roads = DbEngine.GetRoads();
            // Создание столбцов
            var column1 = new DataGridViewColumn
            {
                HeaderText = "ID",
                Width = (int)(dgv.Width*0.1),
                ReadOnly = true,
                Frozen = true,
                CellTemplate = new DataGridViewTextBoxCell()
            };
            var column2 = new DataGridViewColumn
            {
                HeaderText = "Название",
                Width = (int)(dgv.Width * 0.9),
                ReadOnly = true,
                Frozen = true,
                CellTemplate = new DataGridViewTextBoxCell()
            };
            dgv.Columns.Add(column1);
            dgv.Columns.Add(column2);
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.MultiSelect = false;
            // Заполняем данными
            foreach (var road in Roads)
            {
                dgv.Rows.Add(road.Key,road.Value);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Обновить список дорог
            button6.Enabled = true;
            FillGrid(dataGridView1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Удалить дорогу из базы данных
            if (dataGridView1.SelectedCells[0].ColumnIndex == 0)
            {
                DbEngine.DeleteSelectedId(Convert.ToInt32(dataGridView1.SelectedCells[0].Value));
                FillGrid(dataGridView1);
            }
            if (dataGridView1.SelectedCells[0].ColumnIndex == 1)
            {
                DbEngine.DeleteSelectedName((dataGridView1.SelectedCells[0].Value).ToString());
                FillGrid(dataGridView1);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Показать список дорог во вкладке пользователя
            button9.Enabled = true;
            FillGrid(dataGridView2);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Загрузить перекресток для последующей симуляции
            if (dataGridView2.SelectedCells[0].ColumnIndex == 0)
            {
                // Тут и дальше просто загружаем всякую инфу о перекрестке из бд в сессию. По названиям переменных можно понять что именно
                Engine.CurrentRoad = DbEngine.LoadRoadSelectedId(Convert.ToInt32(dataGridView2.SelectedCells[0].Value));
                Engine.CurrentRoadTransit =
                    DbEngine.LoadTransitSelectedId(Convert.ToInt32(dataGridView2.SelectedCells[0].Value));
                Engine.LightsInterval1 = DbEngine.LoadIntervalId(Convert.ToInt32(dataGridView2.SelectedCells[0].Value), 1)*1000;
                Engine.LightsInterval2 = DbEngine.LoadIntervalId(Convert.ToInt32(dataGridView2.SelectedCells[0].Value), 2)*1000;
                if (Engine.CurrentRoad == null || Engine.CurrentRoadTransit == null)
                {
                    MessageBox.Show("Ошибка!");
                }
                else
                {
                    MessageBox.Show("Перекресток загружен!");
                    btn_start.Enabled = true;
                }
            }
            if (dataGridView2.SelectedCells[0].ColumnIndex == 1)
            {
                // Тут и дальше просто загружаем всякую инфу о перекрестке из бд в сессию. По названиям переменных можно понять что именно
                Engine.CurrentRoad = DbEngine.LoadRoadSelectedName((dataGridView2.SelectedCells[0].Value).ToString());
                Engine.CurrentRoadTransit =
                    DbEngine.LoadTransitSelectedName((dataGridView2.SelectedCells[0].Value).ToString());
                Engine.LightsInterval1 = DbEngine.LoadIntervalName((dataGridView2.SelectedCells[0].Value).ToString(), 1) * 1000;
                Engine.LightsInterval2 = DbEngine.LoadIntervalName((dataGridView2.SelectedCells[0].Value).ToString(), 2) * 1000;
                if (Engine.CurrentRoad == null || Engine.CurrentRoadTransit == null)
                {
                    MessageBox.Show("Ошибка!");
                }
                else
                {
                    MessageBox.Show("Перекресток загружен!");
                    btn_start.Enabled = true;
                }
            }
        }
    }
}
