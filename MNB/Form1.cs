using MNB.Entities;
using MNB.MNBServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace MNB
{
    public partial class Form1 : Form
    {
        BindingList<RateData> _rates = new BindingList<RateData>();
        BindingList<string> currencies = new BindingList<string>();
        public Form1()
        {
            InitializeComponent();
            loadCurrencyxml(GetCurrencies);
            cbValuta.DataSource = currencies;
            RefreshData();
        }

        private void RefreshData()
        {
            if (cbValuta.SelectedItem == null) return;
            _rates.Clear();
            LoadXML(GetRates());

            dataGridView1.DataSource = _rates;

            MakeChart();
        }

        private void MakeChart()
        {
            chartRateData.DataSource = _rates;
            Series sorozatok = chartRateData.Series[0];
            sorozatok.ChartType = SeriesChartType.Line;
            sorozatok.XValueMember = "Data";
            sorozatok.YValueMembers = "Value";

            var jelmagyarazat = chartRateData.Legends[0];
            jelmagyarazat.Enabled = false;

            var diagramterulet = chartRateData.ChartAreas[0];
            diagramterulet.AxisY.IsStartedFromZero = false;
            diagramterulet.AxisY.MajorGrid.Enabled = false;
            diagramterulet.AxisX.MajorGrid.Enabled = false;
                

        }

        private void LoadXML(string xmlstring )
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlstring);
            foreach (XmlElement item in xml.DocumentElement)
            {
                RateData r = new RateData();
                r.Date = DateTime.Parse(item.GetAttribute("date"));
                var ChildElement = (XmlElement)item.ChildNodes[0];
                if (ChildElement != null)
                {
                    r.Currency = ChildElement.GetAttribute("curr");
                    decimal unit = decimal.Parse(ChildElement.GetAttribute("unit"));
                    r.Value = decimal.Parse(ChildElement.InnerText);
                    if (unit != 0)
                        r.Value = r.Value / unit;
                    _rates.Add(r);
                }

                
            }
        }
        private void loadCurrencyxml(string xmlstring)
        {
            currencies.Clear();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlstring);
            foreach (XmlElement item in xml.DocumentElement.ChildNodes[0])
            {
                string s = item.InnerText;
                currencies.Add(s);
            }
        }

        private string GetRates()
        {
           
            var MNBServiceReference = new MNBArfolyamServiceSoapClient();
            GetExchangeRatesRequestBody req = new GetExchangeRatesRequestBody();
            req.currencyNames = cbValuta.SelectedItem.ToString();
            req.startDate = tolPicker.Value.ToString("yyyy-MM-dd");
            req.endDate = igPicker.Value.ToString("yyyy-MM-dd");
            var response = MNBServiceReference.GetExchangeRates(req);
            return response.GetExchangeRatesResult;

        }
        private string GetCurrencies()
        {
            var MNBServiceReference = new MNBArfolyamServiceSoapClient();
            GetCurrenciesRequestBody req = new GetCurrenciesRequestBody();
            var resp = MNBServiceReference.GetCurrencies(req);
            string result = resp.GetCurrenciesResult;
            File.WriteAllText("currency.xml",result);
            return resp.GetCurrenciesResult;
        }
        private void paramChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
