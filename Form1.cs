using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameDev
{
    public partial class Form1 : Form
    {

        public String conString = @"Data Source=LAPTOP-GTKGDTGS\NIKITASERVER;Initial Catalog=P1_Game_Studio;Integrated Security=True";

        private SqlConnection connection;
        private SqlCommand cmd;
        private DataSet dataSet;
        private SqlDataAdapter dataAdapter;
        public Form1()
        {
            InitializeComponent();
            birthDatePicker.MaxDate = new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day);
            releaseDatePicker.MaxDate = new DateTime(DateTime.Now.Year + 20, DateTime.Now.Month, DateTime.Now.Day);
            GetTables();
            GetAvaliable(false);
        }

        private void GetTables()
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter("SELECT * FROM dbo.Genre", connection);
                dataSet = new DataSet();
                DataTable dt = dataSet.Tables.Add("dbo.Genre");
                dataAdapter.Fill(dt);
                Genre_dataGridView.DataSource = dataSet.Tables["dbo.Genre"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter("SELECT * FROM dbo.Position", connection);
                DataTable dt = dataSet.Tables.Add("dbo.Position");
                dataAdapter.Fill(dt);
                Position_dataGridView.DataSource = dataSet.Tables["dbo.Position"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter("SELECT * FROM dbo.Department", connection);
                DataTable dt = dataSet.Tables.Add("dbo.Department");
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter("SELECT ID_Worker, CONCAT(Last_Name, ' ', First_Name, ' ', Middle_Name) AS FIO, Date_Of_Birth, ID_Position, ID_Department FROM dbo.Worker", connection);
                DataTable dt = dataSet.Tables.Add("dbo.Worker");
                dataAdapter.Fill(dt);
                Worker_dataGridView.DataSource = dataSet.Tables["dbo.Worker"];

                PositionComboBox.DataSource = dataSet.Tables["dbo.Position"];
                PositionComboBox.DisplayMember = "Position_Name";
                PositionComboBox.ValueMember = "ID_Position";

                DepartmentComboBox.DataSource = dataSet.Tables["dbo.Department"];
                DepartmentComboBox.DisplayMember = "Department_Name";
                DepartmentComboBox.ValueMember = "ID_Department";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter("SELECT * FROM dbo.Department", connection);
                DataTable dt = dataSet.Tables["dbo.Department"];
                dataAdapter.Update(dt);
                Department_dataGridView.DataSource = dataSet.Tables["dbo.Department"];

                TeamLeadComboBox.DataSource = dataSet.Tables["dbo.Worker"];
                TeamLeadComboBox.DisplayMember = "FIO";
                TeamLeadComboBox.ValueMember = "ID_Worker";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter("SELECT * FROM dbo.Design_Document", connection);
                DataTable dt = dataSet.Tables.Add("dbo.Design_Document");
                dataAdapter.Fill(dt);
                DesignDoc_dataGridView.DataSource = dataSet.Tables["dbo.Design_Document"];

                MainAuthorComboBox.DataSource = dataSet.Tables["dbo.Worker"];
                MainAuthorComboBox.DisplayMember = "FIO";
                MainAuthorComboBox.ValueMember = "ID_Worker";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter("SELECT ID_Game, Game_Name, Price, Release_Date, ID_Genre, ID_Document FROM dbo.Game", connection);
                DataTable dt = dataSet.Tables.Add("dbo.Game");
                dataAdapter.Fill(dt);
                Game_dataGridView.DataSource = dataSet.Tables["dbo.Game"];

                GenreComboBox.DataSource = dataSet.Tables["dbo.Genre"];
                GenreComboBox.DisplayMember = "Genre_Name";
                GenreComboBox.ValueMember = "ID_Genre";

                DesignDocComboBox.DataSource = dataSet.Tables["dbo.Design_Document"];
                DesignDocComboBox.DisplayMember = "Main_Idea";
                DesignDocComboBox.ValueMember = "ID_Document";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void GetAvaliable(bool status)
        {
            if (status == false)
                this.BackColor = Color.Red;
            if (status == true)
                this.BackColor = Color.LimeGreen;
            tabControl1.TabPages[1].Enabled = status;
            tabControl1.TabPages[2].Enabled = status;
            tabControl1.TabPages[3].Enabled = status;
            tabControl1.TabPages[4].Enabled = status;
            tabControl1.TabPages[5].Enabled = status;
            tabControl1.TabPages[6].Enabled = status;

            Genre_dataGridView.Visible = status;
            Position_dataGridView.Visible = status;
            Department_dataGridView.Visible = status;
            Worker_dataGridView.Visible = status;
            DesignDoc_dataGridView.Visible = status;
            Game_dataGridView.Visible = status;
        }
        //100% complete
        #region Checkers Code
        private bool GenreCheck(string name, string description, int status)
        {
            if (name.Length > 18 || string.IsNullOrEmpty(name) || name.Length < 3)
            {
                MessageBox.Show("Неверно введено название жанра");
                return false;
            }
            if (string.IsNullOrEmpty(description) || description.Length < 4)
            {
                MessageBox.Show("Неверно введено описание жанра");
                return false;
            }
            if (status == 1)
            {
                connection = new SqlConnection(conString);
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM dbo.Genre WHERE Genre_Name= '{name}'", connection);
                    int i = Convert.ToInt32(command.ExecuteScalar());
                    if (i != 0)
                    {
                        MessageBox.Show("Такой жанр уже существует");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return true;
        }
        private bool PositionCheck(string name, int salary, int status)
        {
            if (name.Length > 20 || string.IsNullOrEmpty(name) || name.Length < 5)
            {
                MessageBox.Show("Неверно введено название должности");
                return false;
            }
            if (salary < 11500 || salary > 650000)
            {
                MessageBox.Show("Неверно введена З/П");
                return false;
            }
            if (status == 1)
            {
                connection = new SqlConnection(conString);
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM dbo.Position WHERE Position_Name = '{name}'", connection);
                    int i = Convert.ToInt32(command.ExecuteScalar());
                    if (i != 0)
                    {
                        MessageBox.Show("Такая должность уже существует");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return true;
        }
        private bool WorkerCheck(string firstName, string lastName, string MiddleName)
        {
            if (firstName.Length > 15 || string.IsNullOrEmpty(firstName) || firstName.Length < 2)
            {
                MessageBox.Show("Неверно введено имя");
                return false;
            }
            if (lastName.Length > 20 || string.IsNullOrEmpty(lastName) || lastName.Length < 2)
            {
                MessageBox.Show("Неверно введена фамилия");
                return false;
            }
            if ((MiddleName.Length > 20 || MiddleName.Length < 2) && !string.IsNullOrEmpty(MiddleName))
            {
                MessageBox.Show("Неверно введено отчество");
                return false;
            }
            return true;
        }
        private bool DepartmentCheck(string name, int id, int status)
        {
            if (name.Length > 30 || string.IsNullOrEmpty(name) || name.Length < 6)
            {
                MessageBox.Show("Неверно введено название отдела");
                return false;
            }

            if (status == 1)
            {
                connection = new SqlConnection(conString);
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM dbo.Department WHERE Department_Name= '{name}'", connection);
                    SqlCommand command2 = new SqlCommand($"SELECT COUNT(*) FROM dbo.Department WHERE ID_Team_Lead= '{id}'", connection);
                    int i = Convert.ToInt32(command.ExecuteScalar());
                    if (i != 0)
                    {
                        MessageBox.Show("Такой отдел уже существует");
                        return false;
                    }
                    int j = Convert.ToInt32(command2.ExecuteScalar());
                    if (j != 0)
                    {
                        MessageBox.Show("Этот человек уже отвечает за отдел");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return true;
        }
        private bool DesignDocCheck(string idea)
        {
            if (idea.Length < 10 || string.IsNullOrEmpty(idea))
            {
                MessageBox.Show("Слишком короткое описание концепта");
                return false;
            }
            return true;
        }
        private bool GameCheck(string name, int docID, int status)
        {
            if (name.Length > 20 || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Невозможное название для игры");
                return false;
            }
            if (status == 1)
            {
                connection = new SqlConnection(conString);
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM dbo.Game WHERE Game_Name= '{name}'", connection);
                    SqlCommand command2 = new SqlCommand($"SELECT COUNT(*) FROM dbo.Game WHERE ID_Document= '{docID}'", connection);
                    int i = Convert.ToInt32(command.ExecuteScalar());
                    if (i != 0)
                    {
                        MessageBox.Show("Такая игра уже есть");
                        return false;
                    }
                    int j = Convert.ToInt32(command2.ExecuteScalar());
                    if (j != 0)
                    {
                        MessageBox.Show("Этот документ уже используется");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return true;
        }
        private bool RegCheck(string login, string password)
        {
            if (login.Length < 5 || login.Length > 13 || string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Неверно указан логин");
                return false;
            }
            if (password.Length < 6 || password.Length > 25 || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Неверно указан пароль");
                return false;
            }

            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM dbo.Users WHERE User_Login = '{login}'", connection);
                SqlCommand command2 = new SqlCommand($"SELECT COUNT(*) FROM dbo.Users WHERE User_Password = '{password}'", connection);
                int i = Convert.ToInt32(command.ExecuteScalar());
                if (i != 0)
                {
                    MessageBox.Show("Такой пользователь уже существует");
                    return false;
                }
                int j = Convert.ToInt32(command2.ExecuteScalar());
                if (j != 0)
                {
                    MessageBox.Show("Такой пароль уже используется");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
        private bool LoginCheck(string login, string password)
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT ID_User FROM dbo.Users WHERE User_Login = '{login}'", connection);
                SqlCommand command2 = new SqlCommand($"SELECT ID_User FROM dbo.Users WHERE User_Password = '{password}'", connection);
                int i = Convert.ToInt32(command.ExecuteScalar());
                int j = Convert.ToInt32(command2.ExecuteScalar());
                if (i == 0 || j == 0 || (i != j))
                {
                    MessageBox.Show("Неверный логин или пароль");
                    return false;
                }
                if (i == j)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return true;
        }
        #endregion
        //100% complete
        #region Genre Code

        private void GenreInsertButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (GenreCheck(GenreNameBox.Text, GenreDescriptionBox.Text, 1))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("INSERT INTO dbo.Genre (Genre_Name, Genre_Description) VALUES ('{0}', '{1}')", GenreNameBox.Text, GenreDescriptionBox.Text), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }

        private void Genre_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GenreNameBox.Text = Genre_dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                GenreDescriptionBox.Text = Genre_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }

        private void GenreDeleteButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                cmd = new SqlCommand(String.Format("DELETE FROM dbo.Genre WHERE ID_Genre={0}", Genre_dataGridView.SelectedRows[0].Cells[0].Value), connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }

        private void GenreUpdateButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (GenreCheck(GenreNameBox.Text, GenreDescriptionBox.Text, 2))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("UPDATE dbo.Genre SET Genre_Name='{0}', Genre_Description='{1}' WHERE ID_Genre={2}", GenreNameBox.Text, GenreDescriptionBox.Text, Genre_dataGridView.SelectedRows[0].Cells[0].Value), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }
        #endregion
        //100% complete
        #region Position Code

        private void PositionInsertButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (PositionCheck(PositionNameBox.Text, Convert.ToInt32(PositionSalaryBox.Text), 1))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("INSERT INTO dbo.Position (Position_Name, Position_Salary) VALUES ('{0}', '{1}')", PositionNameBox.Text, PositionSalaryBox.Text), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }

        private void Position_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                PositionNameBox.Text = Position_dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                PositionSalaryBox.Text = Position_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }

        private void PositionDeleteButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                cmd = new SqlCommand(String.Format("DELETE FROM dbo.Position WHERE ID_Position={0}", Position_dataGridView.SelectedRows[0].Cells[0].Value), connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }

        private void PositionUpdateButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (PositionCheck(PositionNameBox.Text, Convert.ToInt32(PositionSalaryBox.Text), 2))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("UPDATE dbo.Position SET Position_Name='{0}', Position_Salary='{1}' WHERE ID_Position={2}", PositionNameBox.Text, PositionSalaryBox.Text, Position_dataGridView.SelectedRows[0].Cells[0].Value), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }
        #endregion 
        //100% complete
        #region Worker Code

        private void WorkerInsertButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (WorkerCheck(FirstNameBox.Text, LastNameBox.Text, PatronymicBox.Text))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("INSERT INTO dbo.Worker (First_Name, Last_Name, Middle_Name, Date_Of_Birth, ID_Position, ID_Department) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", FirstNameBox.Text, LastNameBox.Text, PatronymicBox.Text, birthDatePicker.Value, Convert.ToInt32(PositionComboBox.SelectedValue), Convert.ToInt32(DepartmentComboBox.SelectedValue)), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }

        private void Worker_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                FirstNameBox.Text = null;
                LastNameBox.Text = null;
                PatronymicBox.Text = null;
                string FullName = Worker_dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                int spaceCount = 0;
                for (int i = 0; i < FullName.Length; i++)
                {
                    if (spaceCount == 0 && !char.IsWhiteSpace(FullName[i]))
                    {
                        LastNameBox.Text += FullName[i];
                    }
                    if (spaceCount == 1 && !char.IsWhiteSpace(FullName[i]))
                    {
                        FirstNameBox.Text += FullName[i];
                    }
                    if (spaceCount == 2 && !char.IsWhiteSpace(FullName[i]))
                    {
                        PatronymicBox.Text += FullName[i];
                    }
                    if (char.IsWhiteSpace(FullName[i]))
                    {
                        spaceCount++;
                    }
                }
                birthDatePicker.Text = Worker_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                PositionComboBox.SelectedValue = Worker_dataGridView.Rows[e.RowIndex].Cells[3].Value;
                DepartmentComboBox.SelectedValue = Worker_dataGridView.Rows[e.RowIndex].Cells[4].Value;
            }
        }

        private void WorkerDeleteButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                cmd = new SqlCommand(String.Format("DELETE FROM dbo.Worker WHERE ID_Worker={0}", Worker_dataGridView.SelectedRows[0].Cells[0].Value), connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }

        private void WorkerUpdateButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (WorkerCheck(FirstNameBox.Text, LastNameBox.Text, PatronymicBox.Text))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("UPDATE dbo.Worker SET First_Name='{0}', Last_Name='{1}', Middle_Name='{2}', Date_Of_Birth='{3}', ID_Position='{4}', ID_Department='{5}'  WHERE ID_Worker={6}", FirstNameBox.Text, LastNameBox.Text, PatronymicBox.Text, birthDatePicker.Value.ToString(), Convert.ToInt32(PositionComboBox.SelectedValue), Convert.ToInt32(DepartmentComboBox.SelectedValue), Worker_dataGridView.SelectedRows[0].Cells[0].Value), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }
        #endregion
        //100% complete
        #region Department Code

        private void DepartmentInsertButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (DepartmentCheck(DepartmentNameBox.Text, Convert.ToInt32(TeamLeadComboBox.SelectedValue), 1))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("INSERT INTO dbo.Department (Department_Name, ID_Team_Lead) VALUES ('{0}', '{1}')", DepartmentNameBox.Text, Convert.ToInt32(TeamLeadComboBox.SelectedValue)), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }

        private void Department_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DepartmentNameBox.Text = Department_dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                TeamLeadComboBox.SelectedValue = Department_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }

        private void DepartmentDeleteButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                cmd = new SqlCommand(String.Format("DELETE FROM dbo.Department WHERE ID_Department={0}", Department_dataGridView.SelectedRows[0].Cells[0].Value), connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }

        private void DepartmentUpdateButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (DepartmentCheck(DepartmentNameBox.Text, Convert.ToInt32(TeamLeadComboBox.SelectedValue), 2))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("UPDATE dbo.Department SET Department_Name='{0}', ID_Team_Lead='{1}' WHERE ID_Department={2}", DepartmentNameBox.Text, Convert.ToInt32(TeamLeadComboBox.SelectedValue), Department_dataGridView.SelectedRows[0].Cells[0].Value), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }
        #endregion
        //100% complete
        #region DesignDoc Code

        private void DesignDocInsertButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (DesignDocCheck(ConceptBox.Text))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("INSERT INTO dbo.Design_Document (Main_Idea, ID_Main_Author) VALUES ('{0}', '{1}')", ConceptBox.Text, Convert.ToInt32(MainAuthorComboBox.SelectedValue)), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }

        private void DesignDoc_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                ConceptBox.Text = DesignDoc_dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                MainAuthorComboBox.SelectedValue = DesignDoc_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }

        private void DesignDocDeleteButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                cmd = new SqlCommand(String.Format("DELETE FROM dbo.Design_Document WHERE ID_Document={0}", DesignDoc_dataGridView.SelectedRows[0].Cells[0].Value), connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }

        private void DesignDocUpdateButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (DesignDocCheck(ConceptBox.Text))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("UPDATE dbo.Design_Document SET Main_Idea='{0}', ID_Main_Author='{1}' WHERE ID_Document={2}", ConceptBox.Text, Convert.ToInt32(MainAuthorComboBox.SelectedValue), DesignDoc_dataGridView.SelectedRows[0].Cells[0].Value), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }
        #endregion
        //100% complete
        #region Game Code

        private void GameInsertButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (GameCheck(GameNameBox.Text, Convert.ToInt32(DesignDocComboBox.SelectedValue), 1))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("INSERT INTO dbo.Game (ID_Document, Game_Name, Price, ID_Genre, Release_Date) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", Convert.ToInt32(DesignDocComboBox.SelectedValue), GameNameBox.Text, GamePriceBox.Text, Convert.ToInt32(GenreComboBox.SelectedValue), releaseDatePicker.Value), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }

        private void Game_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GameNameBox.Text = Game_dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                GamePriceBox.Text = Game_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                releaseDatePicker.Text = Game_dataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                GenreComboBox.SelectedValue = Game_dataGridView.Rows[e.RowIndex].Cells[4].Value;
                DesignDocComboBox.SelectedValue = Game_dataGridView.Rows[e.RowIndex].Cells[5].Value;
            }
        }

        private void GameDeleteButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                cmd = new SqlCommand(String.Format("DELETE FROM dbo.Game WHERE ID_Game={0}", Game_dataGridView.SelectedRows[0].Cells[0].Value), connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }

        private void GameUpdateButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (GameCheck(GameNameBox.Text, Convert.ToInt32(DesignDocComboBox.SelectedValue), 2))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("UPDATE dbo.Game SET ID_Document='{0}', Game_Name='{1}', Price='{2}', ID_Genre='{3}', Release_Date='{4}' WHERE ID_Game={5}", Convert.ToInt32(DesignDocComboBox.SelectedValue), GameNameBox.Text, GamePriceBox.Text, Convert.ToInt32(GenreComboBox.SelectedValue), releaseDatePicker.Value, Game_dataGridView.SelectedRows[0].Cells[0].Value), connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            GetTables();
        }
        #endregion
        //100% complete
        #region Login Code

        private void LoginButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (LoginCheck(loginBox.Text, passwordBox.Text))
                {
                    MessageBox.Show($"Добро пожаловать в систему, {loginBox.Text}");
                    GetAvaliable(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void RegistrationButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                if (RegCheck(loginBox.Text, passwordBox.Text))
                {
                    connection.Open();
                    cmd = new SqlCommand(String.Format("INSERT INTO dbo.Users (User_Login, User_Password) VALUES ('{0}', '{1}')", loginBox.Text, passwordBox.Text), connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Вы успешно зарегестрировались");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion
        //100% complete
        #region Filters Code
        private void FilterGameButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter($"SELECT ID_Game, Game_Name, Price, Release_Date, ID_Genre, ID_Document FROM dbo.Game WHERE Game_Name LIKE '%{FilterGameBox.Text}%'", connection);
                DataTable dt = dataSet.Tables["dbo.Game"];
                dt.Rows.Clear();
                dataAdapter.Fill(dt);
                Genre_dataGridView.DataSource = dataSet.Tables["dbo.Game"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void FilterWorkersButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter($"SELECT ID_Worker, CONCAT(Last_Name, ' ', First_Name, ' ', Middle_Name) AS FIO, Date_Of_Birth, ID_Position, ID_Department FROM dbo.Worker WHERE Last_Name LIKE '%{FilterFIOBox.Text}%'", connection);
                DataTable dt = dataSet.Tables["dbo.Worker"];
                dt.Rows.Clear();
                dataAdapter.Fill(dt);
                Genre_dataGridView.DataSource = dataSet.Tables["dbo.Worker"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void FilterPositionButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);

            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.Position WHERE Position_Name LIKE '%{FilterPositionBox.Text}%'", connection);
                DataTable dt = dataSet.Tables["dbo.Position"];
                dt.Rows.Clear();
                dataAdapter.Fill(dt);
                Genre_dataGridView.DataSource = dataSet.Tables["dbo.Position"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void FilterGenreButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.Genre WHERE Genre_Name LIKE '%{FilterGenreBox.Text}%'", connection);
                DataTable dt = dataSet.Tables["dbo.Genre"];
                dt.Rows.Clear();
                dataAdapter.Fill(dt);
                Genre_dataGridView.DataSource = dataSet.Tables["dbo.Genre"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void FilterDepartmentButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.Department WHERE Department_Name LIKE '%{FilterDepartmentBox.Text}%'", connection);
                DataTable dt = dataSet.Tables["dbo.Department"];
                dt.Rows.Clear();
                dataAdapter.Fill(dt);
                Genre_dataGridView.DataSource = dataSet.Tables["dbo.Department"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void FilterDesignDocButton_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(conString);
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.Design_Document WHERE Main_Idea LIKE '%{FilterDesignDocBox.Text}%'", connection);
                DataTable dt = dataSet.Tables["dbo.Design_Document"];
                dt.Rows.Clear();
                dataAdapter.Fill(dt);
                Genre_dataGridView.DataSource = dataSet.Tables["dbo.Design_Document"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            GetTables();
        }
    }
}