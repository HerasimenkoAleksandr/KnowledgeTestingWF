using KnowledgeTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KnowledgeTesting
{
	public partial class Task : Form
	{

		private Dictionary<string, int> testNameToIdMap = new Dictionary<string, int>();
		private Form1 mainForm;
		private bool isButtonClicked = false;
		public int idName;
		public string selectedTestName;

		public Task(Form1 form1)
		{
			InitializeComponent();
			mainForm = form1;
			this.FormClosed += Task_FormClosed;
			loadTaskNamefromDb();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			selectedTestName = comboBox1.SelectedItem.ToString();
			

			// Получить соответствующий Id из словаря
			if (testNameToIdMap.TryGetValue(selectedTestName, out int selectedTestId))
			{
				idName = selectedTestId;
				// Здесь можно выполнить дополнительные действия с выбранным Id
			}

			isButtonClicked = true;
			if(!textBox1.Text.Contains("@") && textBox1.Text.Length<3)
			{
				MessageBox.Show("необхідно вказати Email");
				return;
			}
			// Создаем и открываем вторую форму (CompletingTask)
			СompletingTask completingTask = new СompletingTask(mainForm, textBox1.Text, selectedTestName, idName);
			completingTask.Show();
			this.Close();
		}


		private void loadTaskNamefromDb()
		{
			string connectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=AppKnowledgeTesting; Integrated Security=SSPI;";

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				try
				{
					// Открыть соединение
					conn.Open();

					// SQL-запрос для получения всех TestName и их Id
					string selectTaskNameQuery = "SELECT Id, TestName FROM TaskName";

					using (SqlCommand cmd = new SqlCommand(selectTaskNameQuery, conn))
					{
						using (SqlDataReader reader = cmd.ExecuteReader())
						{
							// Очистить существующие элементы в ComboBox и словарь
							comboBox1.Items.Clear();
							testNameToIdMap.Clear();

							// Добавить каждый TestName в ComboBox и заполнить словарь
							while (reader.Read())
							{
								int id = Convert.ToInt32(reader["Id"]);
								string testName = reader["TestName"].ToString();
								comboBox1.Items.Add(testName);
								testNameToIdMap.Add(testName, id);
							}
							if(comboBox1.Items.Count>0)
							{
								comboBox1.SelectedIndex = 0;
							}
						}
					}
				}
				catch (Exception ex)
				{
					// Обработка ошибок подключения или выполнения запроса
					MessageBox.Show("Ошибка: " + ex.Message);
				}
				finally
				{
					// Закрыть соединение
					if (conn != null)
					{
						conn.Close();
					}
				}
			}
		}
			private void Task_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (!isButtonClicked)
			{
				mainForm.Show();
			}
			
		}

	}
}

