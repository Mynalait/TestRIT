using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestRIT
{
    public partial class Form2 : Form
    {
        public Form2(string[] data)
        {
            InitializeComponent();
            textBox1.Text = data[0];
            textBox2.Text = data[1];
            textBox3.Text = data[2];
            textBox4.Text = data[3];
            textBox5.Text = data[4];

        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Point lastPoint;
        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataBase db = new DataBase();

            MySqlCommand commandActive = new MySqlCommand("UPDATE `active` SET `type` = @aT WHERE `active`.`name` = @aN", db.getConnection());
            commandActive.Parameters.Add("@aN", MySqlDbType.VarChar).Value = textBox1.Text;
            commandActive.Parameters.Add("@aT", MySqlDbType.VarChar).Value = textBox2.Text;

            MySqlCommand commandMoney = new MySqlCommand("UPDATE `money` SET `Amount` = @mA, `Location` = @mL WHERE `money`.`name` = @aN", db.getConnection());
            commandMoney.Parameters.Add("@aN", MySqlDbType.VarChar).Value = textBox1.Text;
            commandMoney.Parameters.Add("@mA", MySqlDbType.VarChar).Value = textBox3.Text;
            commandMoney.Parameters.Add("@mL", MySqlDbType.VarChar).Value = textBox4.Text;

            MySqlCommand commandDescription = new MySqlCommand("UPDATE `Description` SET `Description` = @dD WHERE `Description`.`name` = @aN", db.getConnection());
            commandDescription.Parameters.Add("@aN", MySqlDbType.VarChar).Value = textBox1.Text;
            commandDescription.Parameters.Add("@dD", MySqlDbType.VarChar).Value = textBox5.Text;

            db.openConnection();

            commandActive.ExecuteNonQuery();
            commandMoney.ExecuteNonQuery();
            commandDescription.ExecuteNonQuery();

            db.closeConnection();

            MessageBox.Show("Данные успешно изменены");
            Form1 listPCForm = new Form1();
            listPCForm.Show();
            this.Hide();

        }
    }
}
