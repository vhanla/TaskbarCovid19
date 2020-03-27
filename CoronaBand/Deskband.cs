using CSDeskBand;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SimpleInjector;
using System;
using CSDeskBand.ContextMenu;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;

namespace CoronaBand
{
    [ComVisible(true)]
    [Guid("9BDAB284-01E7-4B4F-B0A5-7CD016EE83EF")]
    [CSDeskBandRegistration(Name = "Coronavirus")]
    public class Deskband : CSDeskBandWin
    {
        private UserControl1 _mainControl;
        private Container _container;
        private frmStats _frmStats;

        private Boolean showStats = false;
        public Deskband()
        {
            Options.MinHorizontalSize = new Size(100, 30);
            Options.ContextMenuItems = ContextMenuItems;
            InitDependencies();
            
            _mainControl = _container.GetInstance<UserControl1>();
            _frmStats = _container.GetInstance<frmStats>();

            _mainControl.MouseEnter += showPopup;
            _mainControl.MouseLeave += hidePopup;
            _mainControl.label1.MouseMove += moveMouse;
        }

        protected override Control Control =>  _mainControl;

        protected override void DeskbandOnClosed()
        {
            base.DeskbandOnClosed();
            _mainControl.Hide();
            _mainControl = null;
        }

        private List<DeskBandMenuItem> ContextMenuItems
        {
            get
            {
                var action = new DeskBandMenuAction("About CoronaBand");                
                var separator = new DeskBandMenuSeparator();
                var update = new DeskBandMenuAction("Clear");
                var stats = new DeskBandMenuAction("Show Stats");
                stats.Checked = false;
                var item1 = new DeskBandMenuAction("PE"); item1.Clicked += itemClicked;
                var item2 = new DeskBandMenuAction("MX"); item2.Clicked += itemClicked;
                var item3 = new DeskBandMenuAction("IT"); item3.Clicked += itemClicked;
                var item4 = new DeskBandMenuAction("VE"); item4.Clicked += itemClicked;
                var item5 = new DeskBandMenuAction("BO"); item5.Clicked += itemClicked;
                var item6 = new DeskBandMenuAction("CO"); item6.Clicked += itemClicked;
                var item7 = new DeskBandMenuAction("CL"); item7.Clicked += itemClicked;
                var item8 = new DeskBandMenuAction("BR"); item8.Clicked += itemClicked;
                var submenu = new DeskBandMenu("Countries")
                {
                    Items = {item1, item2, item3, item4, item5, item6, item7, item8}
                };
                action.Clicked += dlgABout;
                update.Clicked += updateData;
                stats.Clicked += ShowStats;
                return new List<DeskBandMenuItem>() { action, separator, stats, update, submenu };
            }
        }
        private static void dlgABout(Object sender, EventArgs e)
        {
            MessageBox.Show("CoronaBand v1.0\n\nShows Coronavirus updated data in your taskbar");
        }
        private void updateData(Object sedner, EventArgs e)
        {
            _frmStats.cartesianChart1.Series.Clear();
            _frmStats.cartesianChart1.Series.Add(new LineSeries { Title = "PE", Values = new ChartValues<int> { } });
            _frmStats.UpdateData("PE");
            _mainControl.UpdateData("PE");
        }
        private void ShowStats(Object sender, EventArgs e)
        {
            var menuItem = (DeskBandMenuAction)sender;
            menuItem.Checked = !menuItem.Checked;
            showStats = menuItem.Checked;
            if (!menuItem.Checked)
            {
                _frmStats.Hide();
            }
        }
        private void itemClicked(Object sedner, EventArgs e)
        {
            var menuItem = (DeskBandMenuAction)sedner;

            _frmStats.UpdateData(menuItem.Text, true);
            _mainControl.UpdateData(menuItem.Text);
        }
        private void InitDependencies()
        {
            try
            {
                _mainControl = new UserControl1();
                _frmStats = new frmStats();
                _container = new Container();
                _container.RegisterInstance(Options);
                _container.RegisterInstance(TaskbarInfo);

                _container.Verify();

            } catch (Exception)
            {
                throw;
            }
        }

        private void showPopup(Object sender, EventArgs e)
        {
            _frmStats.Left = (Cursor.Position.X) - (_frmStats.Width / 2);
            _frmStats.Top = Cursor.Position.Y - 32 - _frmStats.Height;
            if (showStats)
                _frmStats.Show();
        }
        private void hidePopup(Object sender, EventArgs e)
        {
            _frmStats.Hide();
        }

        private void moveMouse(Object sender, EventArgs e)
        {
            if (!_frmStats.Visible)
            {
                _frmStats.Left = (Cursor.Position.X) - (_frmStats.Width / 2);
                _frmStats.Top = Cursor.Position.Y - 32 - _frmStats.Height;

                if(showStats)
                    _frmStats.Show();
            }
        }
    }
}
