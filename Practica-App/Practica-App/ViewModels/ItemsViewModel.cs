using Practica_App.Models;
using Practica_App.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using MvvmHelpers;
using Plugin.Connectivity;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Windows.Input;
using System.Linq;

namespace Practica_App.ViewModels
{
    public class ItemsViewModel : MvvmHelpers.BaseViewModel
    {


        HttpClient client = new HttpClient(new HttpClientHandler() { ServerCertificateCustomValidationCallback = (o, cert, chain, errors) => true });
        public ObservableRangeCollection<Anuncio> Anuncios { get; } = new ObservableRangeCollection<Anuncio>();



        bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); this.OnPropertyChanged("IsRefreshing"); }
        }


        bool showCanLoadMore = false;
        public bool ShowCanLoadMore
        {
            get => showCanLoadMore;
            set => SetProperty(ref showCanLoadMore, value);
        }

        bool hasErrors = false;
        public bool HasErrors
        {
            get => hasErrors;
            set => SetProperty(ref hasErrors, value);
        }

        bool showBlankState = false;
        public bool ShowBlankState
        {
            get => showBlankState;
            set => SetProperty(ref showBlankState, value);
        }

        bool hasDataLoaded = false;
        public bool HasDataLoaded
        {
            get => hasDataLoaded;
            set => SetProperty(ref hasDataLoaded, value);
        }

        protected int pageNumber = 1;

        ICommand forceRefreshCommand;
        public ICommand ForceRefreshCommand =>
        forceRefreshCommand ?? (forceRefreshCommand = new Command(async () => await ExecuteForceRefreshCommandAsync()));

        async Task ExecuteForceRefreshCommandAsync()
        {
            pageNumber = 1;
            IsRefreshing = true;
            Anuncios.Clear();
            await ExecuteLoadItemsCommand(true);
            IsRefreshing = false;
        }


        public Command LoadItemsCommand { get; }
        public Command LoadItemsCategoriasCommand { get; }

        public ItemsViewModel()
        {
            Title = "Anuncios";
          
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

          
        }

       

        protected virtual async Task<bool> ExecuteLoadItemsCommand(bool force = false)
        {
            //if (IsBusy)
            //    return false;

            IsBusy = true;
            HasErrors = false;
            ShowBlankState = false;


            OperationResult<List<Anuncio>> result = new OperationResult<List<Anuncio>>() { Success = false, Message = string.Empty };
            try
            {
       
                    Anuncios.Clear();

                string url = Constantes.url+ "/api/Anuncios/Items";

                if (CrossConnectivity.Current.IsConnected)
                {
                    try
                    {
                        var response = await client.GetAsync(url);
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                var content = await response.Content.ReadAsStringAsync();
                                result = JsonConvert.DeserializeObject<OperationResult<List<Anuncio>>>(content);

                                IsBusy = false;
                                HasErrors = !result.Success;

                                if (result.Data != null)
                                {

                                    if (result.Data.Any())
                                    {
                                        CanLoadMore = result.Data.Count() == 5;
                                        Anuncios.AddRange(result.Data);
                                        pageNumber++;
                                    }
                                    else
                                    {
                                        CanLoadMore = false;
                                        result.Message = "No encontramos información registrada";
                                    }
                                    HasDataLoaded = true;


                                }
                                else
                                {
                                   
                                    CanLoadMore = false;
                                }

                                ShowBlankState = HasErrors || (IsNotBusy && Anuncios != null && !Anuncios.Any());
                                ShowCanLoadMore = IsNotBusy && CanLoadMore;

                                if (result == null)
                                {
                                  
                                    result.Message = "Hubo un error";
                                }
                                break;
                            default:
                                result.Message ="Hubo un error";
                                break;

                        }

                    }
                    catch (TaskCanceledException ex)
                    {


                        result.Message = ex.Message;
                    }
                    catch (Exception ex)
                    {
                       
                        result.Message = ex.Message;
                    }
                }
                else
                {
                    result.ErrorType = ErrorType.Connectivity;
                    result.Message = "Error de conectividad";
                }

     
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
            return true;
        }

        public void OnAppearing()
        {
            IsBusy = true;
           
        }

     
      

       
    }
}