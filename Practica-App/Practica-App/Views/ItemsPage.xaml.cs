using MvvmHelpers;
using Practica_App.Controls;
using Practica_App.Models;
using Practica_App.ViewModels;
using Practica_App.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Practica_App.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ObservableRangeCollection<Anuncio> Items { get; set; }

        public List<Model> lstCategorias { get; set; }

        public string filtro1 { get; set; }
        public string filtro2 { get; set; }

        public double filtro3 { get; set; }


        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();

       
            //MessagingCenter.Subscribe<App, string>(App.Current, "Categorias", (snd, arg) =>
            //{
            //    filtro2 = arg as String;
            //});


        }



        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            double value = args.NewValue;

            filtro3 = value;
            lstView.ItemsSource = Filtro(filtro3, filtro2, filtro1);
            // lstView.ItemsSource = FilterItem3(value);
        }


        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //var texto = SearchBar.Text;
            //lstView.ItemsSource = _viewModel.Anuncios.Where(
            //      x => x.Titulo.Contains(texto.ToLower()));

            // lstView.ItemsSource = FilterItem1(e.NewTextValue);

            filtro1 = e.NewTextValue;
            lstView.ItemsSource= Filtro(filtro3, filtro2, filtro1);
        }


        IEnumerable<Anuncio> Filtro(double filter3 = 0, string filtro2 = "", string filter1= "")
        {

            //if (filter==0)
            //    return Items;

            //return Items.Where(x => x.Precio <= filter);



            if (string.IsNullOrEmpty(filter1) && string.IsNullOrEmpty(filtro2) && filter3 == 0)
            {
                return Items;
            }
            else if (string.IsNullOrEmpty(filter1))
            {
                filter1 = "";
            }
            else if (string.IsNullOrEmpty(filtro2))
            {
                filtro2 = "";
            }



            var split = filtro2.Split(',');

            return Items.Where(x => x.Precio >= filter3 && split.Any(p=> x.Tipo.Contains(p))  && x.Titulo.ToLower().Contains(filter1.ToLower()));


        }







        async void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstView.SelectedItem = null;

            //try
            //{
            //    if (e.SelectedItem != null)
            //    {
            //        var saldo = e.SelectedItem as Ingresos;

            //        await Navigation.PushAsync(new IngresosPage());

            //    }
            //    lstView.SelectedItem = null;
            //}
            //catch (Exception ex)
            //{
            //    var m = ex.Message;
            //}
        }

       
        private async void Entry_Focused(object sender, FocusEventArgs e)
        {
            await Navigation.PushPopupAsync(new CategoriasPage());

            

        }


  

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            
               _viewModel.LoadItemsCommand.Execute(false);

            


            Items = _viewModel.Anuncios;
            lstView.ItemsSource = Items;

            MessagingCenter.Subscribe<App, string>(App.Current, "Categorias", (snd, arg) =>
            {
                filtro2 = arg as String;
                entry.Text = filtro2;
                lstView.ItemsSource = Filtro(filtro3, filtro2, filtro1);
            });
            //MessagingCenter.Subscribe<App, List<Model>>(App.Current, "lstCategorias", (snd, arg) =>
            //{
            //    lstCategorias = arg as List<Model>;
            
            //    lstView.ItemsSource = Filtro(filtro3, lstCategorias, filtro1);
            //});


        }


    }
}