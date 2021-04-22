using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;

namespace CheckListApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            checkedListBox1.Items.Clear();
            LoadFile();
        }

        const string fileName = "list.dat";

        private void LoadFile()
        {
            if (File.Exists(fileName))
            {
                Dictionary<string, bool> list = new Dictionary<string, bool>();
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        while (fs.Length != fs.Position)
                        {
                            string json = reader.ReadString();
                            list = JsonSerializer.Deserialize<Dictionary<string, bool>>(json);
                        }
                    }
                }
                foreach (var item in list)
                {
                    checkedListBox1.Items.Add(item.Key, item.Value == true);
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ItemTextBox.Text))
            {
                MessageBox.Show("未輸入項目!", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                checkedListBox1.Items.Add(ItemTextBox.Text);
                ItemTextBox.Text = string.Empty;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.Items.Count != 0)
            {
                if (checkedListBox1.SelectedItem == null)
                {
                    MessageBox.Show("未選擇項目!", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    checkedListBox1.Items.RemoveAt(checkedListBox1.SelectedIndex);
                } 
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, bool> list = new Dictionary<string, bool>();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                string name = checkedListBox1.Items[i].ToString();
                bool value = checkedListBox1.GetItemChecked(i);
                list.Add(name, value);
            }

            if (File.Exists(fileName) || list.Count != 0)
            {
                SaveFile(list);
            }
        }

        private void SaveFile(Dictionary<string, bool> list)
        {
            var json = JsonSerializer.Serialize(list);
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                writer.Write(json);
            }
            MessageBox.Show("儲存成功", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
