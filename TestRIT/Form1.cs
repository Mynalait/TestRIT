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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadData();
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

        private void LoadData()
        {
            try
            {
                DataBase db = new DataBase();
                db.openConnection();
                MySqlCommand commandPC = new MySqlCommand("SELECT active.name, active.type, money.amount, money.location, description.description  FROM active INNER JOIN money ON active.name = money.name INNER JOIN description ON active.name = description.name; ", db.getConnection());


                MySqlDataReader reader = commandPC.ExecuteReader();
                List<string[]> data = new List<string[]>();

                while (reader.Read())
                {
                    data.Add(new string[5]);

                    data[data.Count - 1][0] = reader[0].ToString();
                    data[data.Count - 1][1] = reader[1].ToString();
                    data[data.Count - 1][2] = reader[2].ToString();
                    data[data.Count - 1][3] = reader[3].ToString();
                    data[data.Count - 1][4] = reader[4].ToString();
                }

                reader.Close();

                db.closeConnection();
                foreach (string[] s in data)
                    dataGridView1.Rows.Add(s);

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    DataGridViewLinkCell linkedCell = new DataGridViewLinkCell();
                    dataGridView1[7, i] = linkedCell;
                    dataGridView1[7, i].Value = "Удалить";
                    DataGridViewLinkCell linkedCell1 = new DataGridViewLinkCell();
                    dataGridView1[5, i] = linkedCell1;
                    dataGridView1[5, i].Value = "Добавить";
                    DataGridViewLinkCell linkedCell2 = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkedCell2;
                    dataGridView1[6, i].Value = "Изменить";
                }
            


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public string[] agenstvo = new string[5];
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)

        {
            if (e.ColumnIndex == 7)
            {
                string task = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();

                if (task == "Удалить")
                {
                    if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int rowIndex = e.RowIndex;
                        

                        DataBase db = new DataBase();

                        MySqlCommand command = new MySqlCommand("DELETE FROM active WHERE active.Name = @aN", db.getConnection());
                        command.Parameters.Add("@aN", MySqlDbType.VarChar).Value = dataGridView1[columnIndex: 0, rowIndex].Value.ToString();

                        dataGridView1.Rows.RemoveAt(rowIndex);

                        db.openConnection();

                        command.ExecuteNonQuery();

                        db.closeConnection();
                    }
                }
            }
            if (e.ColumnIndex == 5)
            {
                string task = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();

                if (task == "Добавить")
                {
                    if (MessageBox.Show("Добавить эту строку в базу данных?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int rowIndex = e.RowIndex;


                        DataBase db = new DataBase();

                        MySqlCommand commandActive = new MySqlCommand("INSERT INTO `active` (`Name`, `Type`) VALUES (@aN, @aT)", db.getConnection());
                        commandActive.Parameters.Add("@aN", MySqlDbType.VarChar).Value = dataGridView1[columnIndex: 0, rowIndex].Value.ToString();
                        commandActive.Parameters.Add("@aT", MySqlDbType.VarChar).Value = dataGridView1[columnIndex: 1, rowIndex].Value.ToString();

                        MySqlCommand commandMoney = new MySqlCommand("INSERT INTO `money` (`Name`, `Amount`, `Location`) VALUES (@aN, @mA, @mL)", db.getConnection());
                        commandMoney.Parameters.Add("@aN", MySqlDbType.VarChar).Value = dataGridView1[columnIndex: 0, rowIndex].Value.ToString();
                        commandMoney.Parameters.Add("@mA", MySqlDbType.VarChar).Value = dataGridView1[columnIndex: 2, rowIndex].Value.ToString();
                        commandMoney.Parameters.Add("@mL", MySqlDbType.VarChar).Value = dataGridView1[columnIndex: 3, rowIndex].Value.ToString();

                        MySqlCommand commandDescription = new MySqlCommand("INSERT INTO `description` (`Name`, `Description`) VALUES (@aN, @dD)", db.getConnection());
                        commandDescription.Parameters.Add("@aN", MySqlDbType.VarChar).Value = dataGridView1[columnIndex: 0, rowIndex].Value.ToString();
                        commandDescription.Parameters.Add("@dD", MySqlDbType.VarChar).Value = dataGridView1[columnIndex: 4, rowIndex].Value.ToString();

                        db.openConnection();

                        commandActive.ExecuteNonQuery();
                        commandMoney.ExecuteNonQuery();
                        commandDescription.ExecuteNonQuery();

                        db.closeConnection();

                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            DataGridViewLinkCell linkedCell = new DataGridViewLinkCell();
                            dataGridView1[7, i] = linkedCell;
                            dataGridView1[7, i].Value = "Удалить";
                            DataGridViewLinkCell linkedCell1 = new DataGridViewLinkCell();
                            dataGridView1[5, i] = linkedCell1;
                            dataGridView1[5, i].Value = "Добавить";
                            DataGridViewLinkCell linkedCell2 = new DataGridViewLinkCell();
                            dataGridView1[6, i] = linkedCell2;
                            dataGridView1[6, i].Value = "Изменить";
                        }

                    }
                }
            }

            if (e.ColumnIndex == 6)
            {
                string task = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

                if (task == "Изменить")
                {
                    if (MessageBox.Show("Изменить эту строку?", "Редактирование", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        for (int i = 0; i < dataGridView1.ColumnCount - 3; i++)
                        {
                            agenstvo[i] = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[i].Value.ToString();
                        }
                        Form2 f = new Form2(agenstvo);
                        f.Owner = this;
                        f.Show();
                        this.Hide();


                    }

                }
            
            }

        }

    }
}
