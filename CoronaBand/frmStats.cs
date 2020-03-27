using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.WinForms;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace CoronaBand
{
    internal enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
        ACCENT_INVALID_STATE = 99
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    internal enum WindowCompositionAttribute
    {
        WCA_ACCENT_POLICY = 19
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }
    public partial class frmStats : Form
    {
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private IRestClient rest = new RestClient("https://coronavirus-tracker-api.herokuapp.com/");

        public frmStats()
        {
            InitializeComponent();

            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "PE",
                    Values = new ChartValues<int> { 3, 5, 2, 3, 5}
                }
            };
        }

        internal void EnableBlur()
        {
            //var windowHelper = new WindowInteropHelper(this.Handle);

            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND;
            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(this.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        internal void UpdateData(string country_code, Boolean add = false)
        {
            cartesianChart1.LegendLocation = LegendLocation.Top;
            int seriesIdx = 0;
            if (add)
            {
                seriesIdx = cartesianChart1.Series.Count;
                cartesianChart1.Series.Add(new LineSeries { Title = country_code , Values = new ChartValues<int> { } });
            }
            var req = new RestRequest("v2/locations?country_code="+country_code+"&timelines=true", Method.GET);
            //this.textBox1.Text = "Updating...";
            cartesianChart1.Series[seriesIdx].Values.Clear();
            this.rest.ExecuteAsync(
                req, 
                response =>
                {
                    JObject output = (JObject)JsonConvert.DeserializeObject(response.Content);
                    var locations = output["locations"];
                    var timelines = locations[0]["timelines"];
                    var confirmed = timelines["confirmed"];
                    var timeline = confirmed["timeline"];
                    //this.textBox1.Text = timeline.ToString();
                    //listBox1.Items.Clear();
                    foreach (int d in timeline)
                    {
                        cartesianChart1.Series[seriesIdx].Values.Add(d);
                        //listBox1.Items.Add(d);
                    }
                }
            );

        }
        private void frmStats_Load(object sender, EventArgs e)
        {
            //EnableBlur();
            UpdateData("PE");
        }

        private void cartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateData("PE");
        }
    }
}
