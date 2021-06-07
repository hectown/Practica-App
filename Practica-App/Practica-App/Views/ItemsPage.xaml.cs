using MvvmHelpers;
using Practica_App.Controls;
using Practica_App.Models;
using Practica_App.ViewModels;
using Practica_App.Views;
using System;
using System.Collections.Generic;
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

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            double value = args.NewValue;
   
              lstView.ItemsSource = FilterItem3(value);
        }


        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //var texto = SearchBar.Text;
            //lstView.ItemsSource = _viewModel.Anuncios.Where(
            //      x => x.Titulo.Contains(texto.ToLower()));

            lstView.ItemsSource = FilterItem1(e.NewTextValue);

        }


        IEnumerable<Anuncio> FilterItem3(double filter = 0)
        {

            if (filter==0)
                return Items;

            return Items.Where(x => x.Precio <= filter);



        }

        IEnumerable<Anuncio> FilterItem1(string filter = null)
        {

            if (string.IsNullOrEmpty(filter))
                return Items;

            return Items.Where(x => x.Titulo.Contains(filter.ToLower()) || x.Tipo.ToLower().Contains(filter.ToLower()));


          
        }


        //IEnumerable<Anuncio> FilterItem2(string filter = null)
        //{

        //    if (string.IsNullOrEmpty(filter))
        //        return Items; 
        //    return Items.Where( x => x.Tipo.Contains(filter));
        //}


        //void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var picker = (Picker)sender;
        //    int selectedIndex = picker.SelectedIndex;

        //    //if (selectedIndex != -1)
        //    //{
        //    //    var item = (string)picker.ItemsSource[selectedIndex];

        //    //    if(item == "Todas")
        //    //    {
        //    //        lstView.ItemsSource = _viewModel.Anuncios;
        //    //    }
        //    //    else
        //    //    {
        //    //        lstView.ItemsSource = _viewModel.Anuncios.Where(
        //    //                          x => x.Tipo.Contains(item)
        //    //                         );
        //    //    }



        //    //}

        //    lstView.ItemsSource = FilterItem2((string)picker.ItemsSource[selectedIndex]);


        //}


        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            
               _viewModel.LoadItemsCommand.Execute(false);

              Items = _viewModel.Anuncios;
              lstView.ItemsSource = Items;

        }
    }
}