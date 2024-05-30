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

namespace KnowledgeTesting
{
	public partial class AddTasks : Form
	{
		private List<TextBox> questionTextBox=new List<TextBox>(); 
		private List <List<TextBox>> answerTextBox =new List<List<TextBox>>();
		private List<List<RadioButton>> radioButton=new List<List<RadioButton>>();
		private TextBox testTitleSet;
		private Form1 mainForm;

		TableLayoutPanel tableLayoutPanel;
		public AddTasks(Form1 form1)
		{
			mainForm=form1;
			InitializeComponent();
			InitializeQuizForm();
			this.FormClosed += CompletingTask_FormClosed;

		}

		private void CompletingTask_FormClosed(object sender, FormClosedEventArgs e)
		{
			mainForm.Show();
		}
		private void InitializeQuizForm()
		{
			// Создаем и настраиваем TableLayoutPanel
			tableLayoutPanel = new TableLayoutPanel();
			tableLayoutPanel.ColumnCount = 1;
			tableLayoutPanel.Dock = DockStyle.Fill;
			tableLayoutPanel.AutoScroll = true;
			tableLayoutPanel.Padding=new Padding(30,0,50,0);
			tableLayoutPanel.AutoSize = true;

			// Добавляем название теста
			Label testTitleLabel = new Label();
			testTitleLabel.Text = "Форма для створення тестового завдання";
			testTitleLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			testTitleLabel.TextAlign = ContentAlignment.MiddleCenter;
			testTitleLabel.Font = new Font(testTitleLabel.Font.FontFamily, 12, FontStyle.Bold);
			testTitleLabel.AutoSize = true;
			tableLayoutPanel.Controls.Add(testTitleLabel);


			Label testTitleLabelForSet = new Label();
			testTitleLabelForSet.Text = "Введіть назву тестового завдання";
			testTitleLabelForSet.TextAlign = ContentAlignment.MiddleCenter;
			testTitleLabelForSet.Font = new Font(testTitleLabel.Font.FontFamily, 8, FontStyle.Bold);
		    testTitleLabelForSet.Margin = new Padding(0, 20,0, 0);
			testTitleLabelForSet.AutoSize = true;
			tableLayoutPanel.Controls.Add(testTitleLabelForSet);
			
		   // Добавляем название теста
		    testTitleSet = new TextBox();
			testTitleSet.Text = "";
			testTitleSet.Font = new Font(testTitleLabelForSet.Font.FontFamily,12, FontStyle.Bold);
			testTitleSet.Dock = DockStyle.Bottom;
			tableLayoutPanel.Controls.Add(testTitleSet);

			// Создаем вопросы и варианты ответов
			for (int i = 0; i < 12; i++)
			{
				Label answerTitleLabel = new Label();
				answerTitleLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				answerTitleLabel.TextAlign = ContentAlignment.MiddleCenter;
				answerTitleLabel.Text = $"Введіть питання {i+1} та варіанти відповідей";
				answerTitleLabel.Font = new Font(testTitleLabel.Font.FontFamily, 8, FontStyle.Bold);
				answerTitleLabel.Margin = new Padding(0, 20, 0, 5);
				answerTitleLabel.AutoSize = true;
				tableLayoutPanel.Controls.Add(answerTitleLabel);

				questionTextBox.Add(new TextBox()
				{
					Multiline = true,
					Dock = DockStyle.Top,
					ScrollBars = ScrollBars.Vertical,
					Height = 50,
					Text = $"Питання {i + 1}"  // Пример текста для вопроса
				});

				tableLayoutPanel.Controls.Add(questionTextBox[i]);
				// Создаем панель для вариантов ответов
				Panel answerPanel = new Panel();
				answerPanel.Dock = DockStyle.Top;
				
				List<TextBox> textBoxRow = new List<TextBox>();
				List<RadioButton> radioButtonRow = new List<RadioButton>();

				// Создаем текстовые поля и радиокнопки для вариантов ответов
				for (int j = 0; j < 3; j++)
				{
					TextBox textBox = new TextBox()
					{
						Multiline = true,
						Dock = DockStyle.Bottom,
						ScrollBars = ScrollBars.Vertical,
						Text = $"Варіант відповіді {j + 1}"
					};
					
					// Создаем новый RadioButton
					RadioButton radio = new RadioButton()
					{
						Text = $"Правильна відповідь {j + 1}",
						Dock = DockStyle.Right
					};
					if(j == 0)
					{
						radio.Checked = true;
					}

					textBoxRow.Add(textBox);
					radioButtonRow.Add(radio);
				}
				answerTextBox.Add(textBoxRow);
				radioButton.Add(radioButtonRow);
				for (int j = 0; j < 3; j++)
				{
					answerPanel.Controls.Add(answerTextBox[i][j]);
					answerPanel.Controls.Add(radioButton[i][j]);
				}
				tableLayoutPanel.Controls.Add(answerPanel);
			}

			// Добавляем TableLayoutPanel на форму
			this.Controls.Add(tableLayoutPanel);
			Button submitButton = new Button();
			submitButton.Text = "Додати ";
			submitButton.Dock = DockStyle.Bottom; // Размещаем кнопку внизу
			submitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left; // Закрепляем кнопку справа и внизу
			submitButton.Margin = new Padding(200, 0, 200, 30);
			submitButton.Click += SubmitButton_Click;
			tableLayoutPanel.Controls.Add(submitButton);
		}
		
		
		public class ForQuestion
		{
			public String	QuestionText {  get; set; }

			List<ForAnswer> Answers { get; set; }
		}

		public class ForAnswer
		{
			String AnswerText { get; set; }

			bool AnswerIsCorrect { get; set; }
			
		}

		
		private void SubmitButton_Click(object sender, EventArgs e)
		{

			if (testTitleSet.Text!=null) 
			{ 
				addDBTitile();
				addDBQuestions();
				MessageBox.Show("Завдання успішно додано");
				this.Close();
				return;
			}
			else
			{
				MessageBox.Show("Назву тестового завдання необхідно додати");
				return;
			}
			for (int i = 0; i<12; i++)
			{
				if (questionTextBox[i].Text.Contains("Питання") && questionTextBox[i].Text.Contains(""))
				{
					MessageBox.Show("Заповніть усі поля для питань");
					return;
				}
				
				for (int j = 0; j < 3; j++)
				{
					if (answerTextBox[i][j].Text.Contains("Варіант відповіді") && questionTextBox[i].Text.Contains(""))
					{
						MessageBox.Show("Заповніть усі варіанти відповідей");
						return;
					}
				}
			}

			this.Close();
		}



		private void addDBTitile()
		{
			String connectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=AppKnowledgeTesting; Integrated Security=SSPI;";


			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				try
				{
					//открыть соединение
					conn.Open();
					string insertStringQuest = "INSERT INTO TaskName (TestName) VALUES (@TestName)";
					SqlCommand cmd = new SqlCommand(insertStringQuest, conn);
					cmd.Parameters.AddWithValue("@TestName", testTitleSet.Text);
					cmd.ExecuteNonQuery();
					Console.WriteLine("Данные успешно добавлены в базу данных.");
				}
				catch (Exception ex)
				{
					// Обработка ошибок подключения или выполнения запроса
					Console.WriteLine("Error: " + ex.Message);
				}

				finally
				{
					// закрыть соединение
					if (conn != null)
					{
						conn.Close();
					}
				}
			}
		
		}

		private void addDBQuestions()
		{
			string connectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=AppKnowledgeTesting; Integrated Security=SSPI;";
			int formNameId;
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				try
				{
					// Открыть соединение
					conn.Open();

					// Получить Id из TaskName по TestName
					string selectTaskNameQuery = "SELECT Id FROM TaskName WHERE TestName = @TestName";
					using (SqlCommand selectTaskNameCmd = new SqlCommand(selectTaskNameQuery, conn))
					{
						selectTaskNameCmd.Parameters.AddWithValue("@TestName", testTitleSet.Text);

						object result = selectTaskNameCmd.ExecuteScalar();
						if (result != null)
						{
							formNameId = (int)result;
						}
						else
						{
							MessageBox.Show("Не удалось найти тест с именем: " + testTitleSet.Text);
							return;
						}
					}
					for (int i = 0; i < 12; i++)
					{
						string insertQuestionQuery2 = "INSERT INTO Question (FormNameId, QuestionText, Answer1Text, Answer1IsCorrect, Answer2Text, " +
							"Answer2IsCorrect, Answer3Text, Answer3IsCorrect) " +
							"VALUES (@FormNameId, @QuestionText, @Answer1Text, @Answer1IsCorrect, @Answer2Text, @Answer2IsCorrect, @Answer3Text, @Answer3IsCorrect)";

						using (SqlCommand insertQuestionCmd = new SqlCommand(insertQuestionQuery2, conn))
						{
							// Добавляем параметры к команде
							insertQuestionCmd.Parameters.AddWithValue("@FormNameId", formNameId);
							insertQuestionCmd.Parameters.AddWithValue("@QuestionText", questionTextBox[i].Text ?? (object)DBNull.Value);
							insertQuestionCmd.Parameters.AddWithValue("@Answer1Text", answerTextBox[i][0].Text ?? (object)DBNull.Value);
							insertQuestionCmd.Parameters.AddWithValue("@Answer1IsCorrect", radioButton[i][0].Checked);
							insertQuestionCmd.Parameters.AddWithValue("@Answer2Text", answerTextBox[i][1].Text ?? (object)DBNull.Value);
							insertQuestionCmd.Parameters.AddWithValue("@Answer2IsCorrect", radioButton[i][1].Checked);
							insertQuestionCmd.Parameters.AddWithValue("@Answer3Text", answerTextBox[i][2].Text ?? (object)DBNull.Value);
							insertQuestionCmd.Parameters.AddWithValue("@Answer3IsCorrect", radioButton[i][2].Checked);

							// Выполняем запрос
							insertQuestionCmd.ExecuteNonQuery();
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

	}

}
