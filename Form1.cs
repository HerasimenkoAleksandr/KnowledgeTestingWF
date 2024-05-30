using KnowledgeTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KnowledgeTesting
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();


		}
		private void Window_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.Show();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Hide();

			// Создаем и открываем форму Task
			Task taskForm = new Task(this);
			taskForm.Show();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Hide();

			// Создаем и открываем вторую форму (AddTasks)
			AddTasks addTask = new AddTasks(this);
			addTask.FormClosed += Window_FormClosed;
			addTask.Show();
		}
	}
}

