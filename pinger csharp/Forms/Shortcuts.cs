using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pinger_csharp
{
    public partial class Shortcuts : Form
    {
        public Shortcuts()
        {
            InitializeComponent();

            listView1.Columns.Add("i");
            listView1.Columns.Add("i*i");
            listView1.Columns.Add("i*i*i");

            listView1.View = System.Windows.Forms.View.Details;

            for (int i=0; i<50; i++)
            {
                ListViewItem aFooItem = new ListViewItem(i.ToString()); //Parent item
                ListViewItem.ListViewSubItem aSubFooItem1 = new ListViewItem.ListViewSubItem(aFooItem, (i * i).ToString()); //Creating subitems for the parent item
                ListViewItem.ListViewSubItem aSubFooItem2 = new ListViewItem.ListViewSubItem(aFooItem, (i * i * i).ToString());
                aFooItem.SubItems.Add(aSubFooItem1); //Associating these subitems to the parent item
                aFooItem.SubItems.Add(aSubFooItem2);
                listView1.Items.Add(aFooItem); //Adding the parent item to the listview control
            }

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(listView1.SelectedItems[0].ToString() +" " +listView1.SelectedItems[0].SubItems +" " + listView1.SelectedItems[0].SubItems[1].ToString());
        }
    }
}
