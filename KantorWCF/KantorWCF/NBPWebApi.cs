using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace KantorWCF
{
    public class NBPWebApi
    {
        static XmlElement NBPXmlResponse(string curr, string table, string date = "")
        {
            string apinbp;
            if (date == "")
            {
                apinbp = "https://api.nbp.pl/api/exchangerates/rates/" + table + "/" + curr + "/?format=xml";
            }
            else
            {
                apinbp = "https://api.nbp.pl/api/exchangerates/rates/" + table + "/" + curr + "/" + date + "/?format=xml";
            }
            XmlDocument xmlresponse = new XmlDocument();
            xmlresponse.Load(apinbp);
            XmlElement root = xmlresponse.DocumentElement;
            return root;
        }

        public string GetCurrencyName(string name)
        {

            XmlNode currency = NBPXmlResponse(name, "a").SelectSingleNode("Currency");
            return currency.InnerText;

        }
        public string GetCurrencyRate(string name)
        {
            XmlNode rate = NBPXmlResponse(name, "a").SelectSingleNode("Rates/Rate/Mid");
            return rate.InnerText;
        }
        public double GetCurrencyBid(string name)
        {
            XmlNode bid = NBPXmlResponse(name, "c").SelectSingleNode("Rates/Rate/Bid");
            return XmlConvert.ToDouble(bid.InnerText);
        }
        public double GetCurrencyAsk(string name)
        {
            XmlNode ask = NBPXmlResponse(name, "c").SelectSingleNode("Rates/Rate/Ask");
            return XmlConvert.ToDouble(ask.InnerText);
        }
        public string GetCurrencyArchive(string name, string date)
        {
            XmlNode archive = NBPXmlResponse(name, "a", date).SelectSingleNode("Rates/Rate/Mid");
            return archive.InnerText;
        }

    }
}