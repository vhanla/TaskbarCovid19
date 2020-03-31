using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CoronaBand
{
    public partial class UserControl1: UserControl
    {
        private IRestClient client = new RestClient("https://coronavirus-tracker-api.herokuapp.com/");


        private String confirmed, deaths, recovered, country;
        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            UpdateData("PE", "Peru (PE)");  
        }

        public void UpdateData(string countrycode, string countrytext)
        {
            this.label1.Text = "Loading...";
            var request = new RestRequest("v2/locations?country_code="+countrycode, Method.GET);

            client.ExecuteAsync(
                request,
                response =>
                {
                    this.label1.Text = "";
                    JObject output = (JObject)JsonConvert.DeserializeObject(response.Content);
                    var latestData = output["latest"];
                    this.country = countrytext;
                    this.confirmed = latestData["confirmed"].ToString();
                    this.recovered = latestData["recovered"].ToString();
                    this.deaths = latestData["deaths"].ToString();
                    this.label1.Text = "Confirmed: " + this.confirmed;
                }
            );

        }
        private void UserControl1_MouseHover(object sender, EventArgs e)
        {
            new ToolTip().Show("Country: "+country+"\nConfirmed: "+confirmed+"\nDeaths: "+deaths+"\nRecovered: "+recovered,
                this, Cursor.Position.X - this.Location.X, Cursor.Position.Y - this.Location.Y, 1500);
        }

        private void UserControl1_MouseLeave(object sender, EventArgs e)
        {
            //frm2.Hide();
        }

        private void UserControl1_MouseEnter(object sender, EventArgs e)
        {

            //frm2.Left = (Cursor.Position.X)-(frm2.Width / 2);
            //frm2.Top = Cursor.Position.Y - 32 - frm2.Height;
            //frm2.Show();
            
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (!frm2.Visible)
            //{
            //    frm2.Show();
            //}
        }
    }
}
