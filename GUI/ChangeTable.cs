using BLL;
using DAL;
using GUI.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class ChangeTable : Form
    {
        public ChangeTable()
        {
            InitializeComponent();
        }

        public ChangeTable(Order order)
        {
            InitializeComponent();
            this.LoadDataTabControl1(order);
            this.LoadDataTabControl2();
        }

        private void LoadDataTabControl1(Order order)
        {
            this.tabControl.Controls.Clear();

            var t = new TabPage();
            t.Location = new Point(4, 22);
            t.Name = "Tabpage";
            t.Padding = new Padding(3);
            t.Size = new Size(597, 257);
            t.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            t.Text = "Table of Order 1";
            t.UseVisualStyleBackColor = true;
            t.AutoScroll = true;

            this.tabControl.Controls.Add(t);

            // add layout
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.AutoScroll = true;
            t.Controls.Add(flowLayoutPanel);

            // add menu item
            //TableBLL tableBLL = new TableBLL();
            //List<DAL.Table> tables = tableBLL.ListTablesIsNotOrder(area);


            var listTable = order.OrderTables.ToList();
            for (int i = 0; i < order.OrderTables.Count; i++)
            {
                TableControl tableControl = new TableControl(listTable[i].Table, true);
                flowLayoutPanel.Controls.Add(tableControl);
            }
        }

        private void LoadDataTabControl2()
        {
            AreaBLL areaBLL = new AreaBLL();
            List<DAL.Area> areas = areaBLL.ListArea();

            this.tabControl1.Controls.Clear();

            foreach (DAL.Area area in areas)
            {
                var t = new TabPage();
                t.Location = new Point(4, 22);
                t.Name = area.Name;
                t.Padding = new Padding(3);
                t.Size = new Size(597, 257);
                t.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                t.Text = area.Name;
                t.UseVisualStyleBackColor = true;
                t.AutoScroll = true;
                
                this.tabControl1.Controls.Add(t);

                // add layout
                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                flowLayoutPanel.Dock = DockStyle.Fill;
                flowLayoutPanel.AutoScroll = true;
                t.Controls.Add(flowLayoutPanel);

                // add menu item
                TableBLL tableBLL = new TableBLL();
                List<DAL.Table> tables = tableBLL.ListTablesIsNotOrder(area);
                for (int i = 0; i < tables.Count; i++)
                {
                    TableControl tableControl = new TableControl(tables[i], true);
                    tableControl.Tag = area;
                    flowLayoutPanel.Controls.Add(tableControl);
                }

            }
        }
    }
}
