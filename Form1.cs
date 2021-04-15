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

        public double latitude { get; set; }
        public double longitude { get; set; }
        public string people { get; set; }
        public string name { get; set; }
       public Form1()
        {
            InitializeComponent();
            GetCoordinates();
            GetISSCrew();
        }

      

            public void GetCoordinates()
            {


                using (var wb = new WebClient())
                {

                    var res = wb.DownloadString("http://api.open-notify.org/iss-now.json");
                    dynamic data = JsonConvert.DeserializeObject(res);
                  
                    label1.Text = "Latitude " + data.iss_position.latitude;
                    label2.Text = "Longitude " + data.iss_position.longitude;

                    latitude = data.iss_position.latitude;
                    longitude = data.iss_position.longitude;
                    string latit = latitude.ToString().Replace(",", ".");
                    string longit = longitude.ToString().Replace(",", ".");


                    StringBuilder queryAddress = new StringBuilder();
               
                    queryAddress.Append("https://www.openstreetmap.org/?mlat=" + latit + "&mlon=" + longit + "#map=3/"+latit + "/" + longit);
                
                    webBrowser1.Navigate(queryAddress.ToString());
                    webBrowser1.ScriptErrorsSuppressed = true;
            }
                
                
            }

            public void GetISSCrew()
            {


            string results;
            using (WebClient client = new WebClient())
            results = client.DownloadString("http://api.open-notify.org/astros.json");
            dynamic crewResults = JObject.Parse(results);

            var number = crewResults.number;
            label4.Text = number;

            var people = crewResults.people;
          
            foreach (var person in people)
            {
                label6.Text = person.name;
            }
        }

        
        


    }
}
