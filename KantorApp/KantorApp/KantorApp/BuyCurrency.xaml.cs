﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KantorApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuyCurrency : ContentPage
    {
        public BuyCurrency()
        {
            InitializeComponent();
        }

        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void AcceptTransaction_Clicked(object sender, EventArgs e)
        {
            decimal pln = decimal.Parse(PlnAmount.Text);
            string currency = CurrencySign.Text.ToString();
            WCFKantor.UserDetails user = new WCFKantor.UserDetails() { UserEmail = Login.emailUser.ToString() };
            WCFKantor.Service1Client client = new WCFKantor.Service1Client();
            Dictionary<string, string> userData = client.PobierzDane(user);
            string getExchange = client.GetExchangeRate(currency);
            int a = getExchange.LastIndexOf("i");
            getExchange = getExchange.Substring(a+2);
            decimal exchange = decimal.Parse(getExchange, CultureInfo.InvariantCulture);
            decimal score = decimal.Parse((pln / exchange).ToString("N4"));
            WCFKantor.Transactions transaction = new WCFKantor.Transactions() { UserID = Int32.Parse(userData["ID"]), Type = "KUP", Pln = pln, Currency = currency, Exchange = exchange, Score = score};
            string result = client.SetTransakcje(transaction);
            CheckLabel.Text = result;
            client.CloseAsync();
            
        }
    }
}