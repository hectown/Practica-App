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

namespace Practica_App.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {


        HttpClient client = new HttpClient(new HttpClientHandler() { ServerCertificateCustomValidationCallback = (o, cert, chain, errors) => true });
        public ObservableRangeCollection<Anuncio> Anuncios { get; } = new ObservableRangeCollection<Anuncio>();

        public Command LoadItemsCommand { get; }


        public ItemsViewModel()
        {
            Title = "Anuncios";
          
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

         
        }

        async Task<OperationResult<List<Anuncio>>> ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            OperationResult<List<Anuncio>> result = new OperationResult<List<Anuncio>>() { Success = false, Message = string.Empty };
            try
            {
                Anuncios.Clear();



               

                string url = "https://192.168.1.74:45456/api/Anuncios/Items";

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
                                if (result.Data != null)
                                {

                                    Anuncios.AddRange(result.Data);

                                
                                }
                        
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
            return result;
        }

        public void OnAppearing()
        {
            IsBusy = true;
           
        }

     
      

       
    }
}