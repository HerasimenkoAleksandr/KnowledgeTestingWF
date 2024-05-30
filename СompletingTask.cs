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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KnowledgeTesting
{
	public partial class СompletingTask : Form
	{
		private Form1 mainForm;
		private String taskNameFromDb;
		private int IdNameFromDb;
		private String EmailForm1;
		private List<List<RadioButton>> radioButton = new List<List<RadioButton>>();
		private List<List<bool>> IsSelectRadioButton = new List<List<bool>>();


		public СompletingTask(Form1 form1, String Email, String taskName, int idName)
		{
			InitializeComponent();
			taskNameFromDb = taskName;
			IdNameFromDb = idName;
			EmailForm1 = Email;
			loadTaskNamefromDb();
			mainForm = form1;
			this.FormClosed += CompletingTask_FormClosed;
			
		}

		private void CompletingTask_FormClosed(object sender, FormClosedEventArgs e)
		{
				mainForm.Show();
		}

		/// <summary>
		private void loadTaskNamefromDb()
		{
			// Создаем и настраиваем TableLayoutPanel
			TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
			tableLayoutPanel.ColumnCount = 1;
			tableLayoutPanel.RowCount = 13; // 12 вопросов + название теста
			tableLayoutPanel.Dock = DockStyle.Fill;
			tableLayoutPanel.AutoScroll = true;

			// Создаем и настраиваем название теста
			Label testTitleLabel = new Label();
			testTitleLabel.Text = taskNameFromDb;
			testTitleLabel.TextAlign = ContentAlignment.MiddleCenter;
			testTitleLabel.Font = new Font(testTitleLabel.Font.FontFamily, 14, FontStyle.Bold);
			testTitleLabel.Dock = DockStyle.Fill;



			// Добавляем название теста в первую строку TableLayoutPanel
			tableLayoutPanel.Controls.Add(testTitleLabel, 0, 0);

			string connectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=AppKnowledgeTesting; Integrated Security=SSPI;";

			using (SqlConnection conn = new SqlConnection(connectionString))
			{

				try
				{
					// Открыть соединение
					conn.Open();

					// SQL-запрос для получения всех TestName и их Id
					string selectQuestionQuery = "SELECT QuestionText, Answer1Text, Answer1IsCorrect, Answer2Text, Answer2IsCorrect, Answer3Text, Answer3IsCorrect " +
							 "FROM Question " +
							 "WHERE FormNameId = @idName"; ;

					using (SqlCommand cmd = new SqlCommand(selectQuestionQuery, conn))
					{
						cmd.Parameters.AddWithValue("@idName", IdNameFromDb);
						using (SqlDataReader reader = cmd.ExecuteReader())
						{
							// Очистить существующие элементы в ComboBox и словарь

							// Добавить каждый TestName в ComboBox и заполнить словарь
							int i = 1;
						
							while (reader.Read())
							{
								List<RadioButton> radioButtonRow = new List<RadioButton>();
								List<bool> IsSelectRadioButtonRow = new List<bool>();
								
							   Label questionLabel = new Label();
								questionLabel.Text =$"Питання {i}: {reader["QuestionText"].ToString()}";
								questionLabel.Dock = DockStyle.Fill;
								questionLabel.Margin = new Padding(50, 30, 50, 0);

								// Создаем FlowLayoutPanel для размещения вариантов ответов
								FlowLayoutPanel answerPanel = new FlowLayoutPanel();
								answerPanel.Dock = DockStyle.Fill;
								answerPanel.FlowDirection = FlowDirection.TopDown;
								answerPanel.AutoSize = true;
								answerPanel.Margin = new Padding(100, 5, 50, 5);

								// Создаем радио-кнопки для вариантов ответов
								RadioButton answer1 = new RadioButton();
								answer1.Text = reader["Answer1Text"].ToString();
								radioButtonRow.Add(answer1);
								bool res1 = Convert.ToBoolean(reader["Answer1IsCorrect"]);
								IsSelectRadioButtonRow.Add(res1);

								RadioButton answer2 = new RadioButton();
								answer2.Text = reader["Answer2Text"].ToString();
								radioButtonRow.Add(answer2);
								bool res2 = Convert.ToBoolean(reader["Answer2IsCorrect"]);
								IsSelectRadioButtonRow.Add(res2);
								RadioButton answer3 = new RadioButton();
								answer3.Text = reader["Answer3Text"].ToString();
								radioButtonRow.Add(answer3);
								bool res3 = Convert.ToBoolean(reader["Answer3IsCorrect"]);
								IsSelectRadioButtonRow.Add(res3);
								answer1.AutoSize = true;
								answer2.AutoSize = true;
								answer3.AutoSize = true;

								// Добавляем радио-кнопки в панель ответов
								answerPanel.Controls.Add(answer1);
								answerPanel.Controls.Add(answer2);
								answerPanel.Controls.Add(answer3);
								radioButton.Add(radioButtonRow);
								IsSelectRadioButton.Add(IsSelectRadioButtonRow);

								// Добавляем вопрос и варианты ответов в TableLayoutPanel
								tableLayoutPanel.Controls.Add(questionLabel, 0, i * 2 - 1); // вопрос
								tableLayoutPanel.Controls.Add(answerPanel, 0, i * 2); // варианты ответов
								i++;
							}
							// Добавляем TableLayoutPanel на форму
							this.Controls.Add(tableLayoutPanel);
                            System.Windows.Forms.Button submitButton = new System.Windows.Forms.Button();
							submitButton.Text = "Перевірити";
							submitButton.Dock = DockStyle.Bottom; // Размещаем кнопку внизу
							submitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left; // Закрепляем кнопку справа и внизу
							submitButton.Margin = new Padding(200, 0, 200, 30);
							submitButton.Click += SubmitButton_Click;

							// Добавляем кнопку "Отправить" под TableLayoutPanel
							tableLayoutPanel.Controls.Add(submitButton);

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


		private void SubmitButton_Click(object sender, EventArgs e)
		{
			int result = 0;
			List<int> comment = new List<int>();

			for(int i = 0; i<IsSelectRadioButton.Count; i++)
			{
				for(int j = 0; j<3; j++)
				{
					if(radioButton[i][j].Checked==true)
					{
						if (radioButton[i][j].Checked == IsSelectRadioButton[i][j])
						{
							result++;
							comment.Add(i + 1);
							continue;
						}
					}
				}
			}
			
			
			double roundedPercentage = Math.Round(((double)result / 12 * 100), 1);

			String commentFull = string.Join(", ", comment);

			MessageBox.Show($"Ваш результат {roundedPercentage}%. Правильно відповіли на питання: {commentFull}"); 
			this.Close();

		}
	}
}
