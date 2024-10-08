using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private int _columnToSort = -1;
        private List<ListViewItem> allData = new List<ListViewItem>();
        private System.Data.SqlClient.SortOrder _sortOrder;
        private List<ListViewItem> filteredData = new List<ListViewItem>();
        public Form1()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.Columns.Add("Заголовок", 100);
            listView1.Columns.Add("Текст", 300);
            listView1.Columns.Add("Теги", 50);
            listView1.ColumnClick += listView1_ColumnClick;
        }
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != _columnToSort)
            {
                _columnToSort = e.Column;
                _sortOrder = System.Data.SqlClient.SortOrder.Ascending;
            }
            else
            {
                _sortOrder = _sortOrder == System.Data.SqlClient.SortOrder.Ascending ? System.Data.SqlClient.SortOrder.Descending : System.Data.SqlClient.SortOrder.Ascending;
            }

            listView1.ListViewItemSorter = new ListViewItemComparer(e.Column, _sortOrder);
            listView1.Sort();
        }

        private class ListViewItemComparer : IComparer
        {
            private int _column;
            private System.Data.SqlClient.SortOrder _sortOrder;

            public ListViewItemComparer(int column, System.Data.SqlClient.SortOrder sortOrder)
            {
                _column = column;
                _sortOrder = sortOrder;
            }

            public int Compare(object x, object y)
            {
                int result = string.Compare(((ListViewItem)x).SubItems[_column].Text, ((ListViewItem)y).SubItems[_column].Text);

                return _sortOrder == System.Data.SqlClient.SortOrder.Ascending ? result : -result;
            }
        }
        private void RefreshListView(List<ListViewItem> data)
        {
            listView1.Items.Clear();
            foreach (var item in data)
            {
                listView1.Items.Add(item);

            }
        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox8.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                RefreshListView(allData);
            }
            else
            {
                filteredData = allData
                    .Where(item =>
                        item.SubItems[0].Text.ToLower().Contains(searchText) ||
                        item.SubItems[1].Text.ToLower().Contains(searchText) ||
                        item.SubItems[2].Text.ToLower().Contains(searchText))
                    .ToList(); 

                RefreshListView(filteredData);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem item3 = new ListViewItem(new[]
               {
                    textBox4.Text,
                    textBox5.Text,
                    textBox6.Text
                });
            listView1.Items.Add(item3);
            allData = new List<ListViewItem>();
            foreach (ListViewItem item in listView1.Items)
            {
                allData.Add(item);
            }
    }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    listView1.Items.Remove(item);
                }
            }
            else
            {
                MessageBox.Show("Элемент не выбран");
            }
        }
private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
