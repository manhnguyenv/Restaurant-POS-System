﻿using BLL;
using DAL;
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
    public partial class SeatSetup : Form
    {
        private List<System.Windows.Forms.TabPage> tabPages;

        public SeatSetup()
        {
            InitializeComponent();
            this.LoadData();
        }

        public void LoadData()
        {
            int currIndex = this.tabControl.SelectedIndex;
            this.tabControl.Controls.Clear();

            AreaBLL areaBLL = new AreaBLL();
            List<Area> listArea = areaBLL.ListArea();
            this.tabPages = new List<System.Windows.Forms.TabPage>();
            foreach (Area area in listArea)
            {
                var t = new System.Windows.Forms.TabPage();
                t.Location = new System.Drawing.Point(4, 22);
                t.Name = area.Name;
                t.Padding = new System.Windows.Forms.Padding(3);
                t.Size = new System.Drawing.Size(597, 257);
                t.Text = area.Name;
                t.UseVisualStyleBackColor = true;
                t.AutoScroll = true;

                // add tables
                TableBLL tableBLL = new TableBLL();
                List<Table> listTable = tableBLL.ListTablesByArea(area);
                for (int i = 0; i < listTable.Count; i++)
                {
                    var tt = new TableControl(listTable[i].Name);
                    t.Controls.Add(tt);
                }

                // store
                this.tabPages.Add(t);
                this.tabControl.Controls.Add(t);
            }
            this.tabControl.SelectedIndex = Math.Min(listArea.Count, currIndex);

            this.UpdateControlPosition();
        }

        private void UpdateControlPosition()
        {
            int tableWidth = 150;
            int tableHeight = 70;
            int minPadding = 6;

            for(int i = 0; i < this.tabPages.Count; i++)
            {
                Control.ControlCollection controls = this.tabPages[i].Controls;

                int cols = (this.tabPages[i].Width - minPadding) / (tableWidth + minPadding);
                int rows = (this.tabPages[i].Height - minPadding) / (tableHeight + minPadding);

                // calculate paddingHorizontal
                int containerWidth = (tableWidth + minPadding) * Math.Min(cols, controls.Count) - minPadding;
                int paddingHorizontal = (this.tabPages[i].Width - containerWidth) / 2;

                for (int j = 0; j < controls.Count; j++)
                {
                    int x = j % cols;
                    int y = j / cols;
                    controls[j].Size = new System.Drawing.Size(tableWidth, tableHeight);
                    controls[j].Location = new System.Drawing.Point(paddingHorizontal + (tableWidth + minPadding) * x, minPadding + (tableHeight + minPadding) * y);
                }
            }
        }

        private void SeatSetup_Resize(object sender, EventArgs e)
        {
            this.UpdateControlPosition();
        }

        private void btnAddArea_Click(object sender, EventArgs e)
        {
            AddAreaDialog addAreaDialog = new AddAreaDialog();
            DialogResult dr=addAreaDialog.ShowDialog();
            if(dr==DialogResult.OK)
            {
                AreaBLL areaBLL = new AreaBLL();
                Area area = areaBLL.CreateArea(addAreaDialog.AreaName);

                // add table
                if (addAreaDialog.IsAddTable)
                {
                    TableBLL tableBLL = new TableBLL();
                    for (var i = addAreaDialog.TableFrom; i <= addAreaDialog.TableTo; i++)
                    {
                        tableBLL.CreateTable(new Table { Name = "Bàn " + i, AreaID = area.ID });
                    }
                }

                this.LoadData();
            }
        }

        private void tabControl_TabIndexChanged(object sender, EventArgs e)
        {
            this.UpdateControlPosition();
        }
    }
}
