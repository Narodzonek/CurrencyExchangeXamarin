using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KantorApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void UserRegistryBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Registry());
        }

        async void UserLoginBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Login());
        }

        private void PrivacyPolicyBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}
