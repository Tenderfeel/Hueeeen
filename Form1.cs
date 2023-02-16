using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zeroconf;

namespace Hueeeen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Searching. Please wait...";

            Task.Run(async () => await SearchHueServices());
        }

        private async Task SearchHueServices()
        {
            Console.WriteLine("Start Multicast DNS");
            BeginInvoke(new Action(() => {
                button1.Enabled = false;
            }));

            try
            {
                var hueServices = await ZeroconfResolver.ResolveAsync("_hue._tcp.local.");

                BeginInvoke(new Action(() => {
                    label1.Text = $"{hueServices.Count} Bridge found";
                }));

                Console.WriteLine(hueServices.Count);

                foreach (var hueService in hueServices)
                {
                    var port = hueService.Services.FirstOrDefault(s => s.Key == "port").Value;
                    Console.WriteLine(hueService.DisplayName);
                    Console.WriteLine(hueService.IPAddress);
                    Console.WriteLine(port);

                    BeginInvoke(new Action(() => {
                        listBox1.Items.Add($"{hueService.DisplayName} ({hueService.IPAddress}:{port})");
                    }));
                }
            } catch(Exception ex)
            {
                BeginInvoke(new Action(() => {
                    label1.Text = $"{ex.Message}";
                }));
            }

            BeginInvoke(new Action(() => {
                button1.Enabled = true;
            }));

        }
    }
}
