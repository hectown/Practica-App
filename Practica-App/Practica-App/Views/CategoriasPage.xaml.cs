using Practica_App.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Practica_App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriasPage : PopupPage
    {

        List<Model> list = new List<Model>();

        public CategoriasPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.White;



            list.Add(new Model() { Text = "Casas" });
            list.Add(new Model() { Text = "Autos" });
            list.Add(new Model() { Text = "Muebles" });
            list.Add(new Model() { Text = "Computación" });
            list.Add(new Model() { Text = "Joyas" });

            listView.ItemsSource = list;

        }

       
        private async void Button_Clicked(object sender, EventArgs e)
        {
        
            var result = list.Where(w => w.IsChecked == true).ToList();

            string s = "";

            int index = 0;
            foreach (var model in result)
            {
                s = s + model.Text;
                if (index < result.Count - 1)
                {
                    s = s + ",";
                }
                index++;
            }

            MessagingCenter.Send<App, List<Model>>((App)App.Current, "lstCategorias", result);
            MessagingCenter.Send<App, string>((App)App.Current, "Categorias", s);
            await Navigation.PopPopupAsync();

        }

    }
}