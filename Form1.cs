using GMap.NET.MapProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Forms;


namespace ISS_Tracker
{
    public partial class Form1 : Form
    {

        public double Latitude { get; set; }
        public double Longitude { get; set; }

       


        public Form1()
        {
            InitializeComponent();
            GetCoordinates();
            GetISSCrew();
        }

      

            public void GetCoordinates()
            {

            // Get actual ISS position from open-notify.org API
            using (var wb = new WebClient())
                {

                    var res = wb.DownloadString("http://api.open-notify.org/iss-now.json");
                    dynamic data = JsonConvert.DeserializeObject(res);
                  
                    label1.Text = "Latitude " + data.iss_position.latitude;
                    label2.Text = "Longitude " + data.iss_position.longitude;

                    Latitude = data.iss_position.latitude;
                    Longitude = data.iss_position.longitude;
                    string latit = Latitude.ToString().Replace(",", ".");
                    string longit = Longitude.ToString().Replace(",", ".");
                    
                }
            // Open map with actual latitude and longitude
                    StringBuilder queryAddress = new StringBuilder();

                    queryAddress.Append("https://www.openstreetmap.org/?mlat=" + Latitude + "&mlon=" + Longitude + "#map=3/" + Latitude + "/" + Longitude);

                    webBrowser1.Navigate(queryAddress.ToString());
                    webBrowser1.ScriptErrorsSuppressed = true;

             }


        // Get actual number of people in space with names from open-notify.org API
        public void GetISSCrew()
            {


            string results;
            using (WebClient client = new WebClient())
            results = client.DownloadString("http://api.open-notify.org/astros.json");
            
            
            dynamic crewResults = JsonConvert.DeserializeObject(results);
            var number = crewResults.number;
            label4.Text = string.Format("There's {0} people in space: ", number);

            // Show all astronauts names on list
            foreach (dynamic person in crewResults.people)
            {
                string per = person.name;
                textBox1.AppendText(per);
                textBox1.AppendText(Environment.NewLine);
                textBox1.Multiline = true;
            }

        }

        // Add transparent background to textBox object with astronauts names
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.BackColor = this.BackColor;
        }

        // Refresh actual position button
        private void button1_Click(object sender, EventArgs e)
        {
            GetCoordinates();
        }

        // Refresh position on map every 60 seconds
        private void timer1_Tick(object sender, EventArgs e)
        {
            GetCoordinates();
        }
    }
}
