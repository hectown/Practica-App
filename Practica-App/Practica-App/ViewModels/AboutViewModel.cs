using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Practica_App.Models;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Practica_App.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
       

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { SetProperty(ref nombre, value); }
        }

        private string correo;
        public string Correo
        {
            get { return correo; }
            set { SetProperty(ref correo, value); }
        }

        private string mensaje;
        public string Mensaje
        {
            get { return mensaje; }
            set { SetProperty(ref mensaje, value); }
        }


        private string resultado;
        public string Resultado
        {
            get { return resultado; }
            set { SetProperty(ref resultado, value); }
        }


        private bool resultadobool;
        public bool ResultadoBool
        {
            get { return resultadobool; }
            set { SetProperty(ref resultadobool, value); }
        }



        public AboutViewModel()
        {
            Title = "Contacto";
            //  OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            ResultadoBool = false;
            Resultado = "";
            IsBusy = false;
            OpenWebCommand = new Command(async () => await SendCorreo()); ;
        }

        public ICommand OpenWebCommand { get; }

        HttpClient client = new HttpClient(new HttpClientHandler() { ServerCertificateCustomValidationCallback = (o, cert, chain, errors) => true });



        public async Task<OperationResult<CorreoRequest>> SendCorreo()
        {

            OperationResult<CorreoRequest> result = new OperationResult<CorreoRequest> { Success = false, Message = string.Empty };

            if (Correo == null || Mensaje == null || Nombre == null)
            {
                ResultadoBool = true;
                Resultado = "No podrás continuar hasta que llenes todos los datos";
                result.Success = false;
                return result;
            }
            else
            {
                IsBusy = true;


                try
                {

                    CorreoRequest request = new CorreoRequest { Correo = Correo, Nombre = Nombre, Asunto = "Prueba Practica", Mensaje = Mensaje };


                    var response = await client.PostAsync("https://192.168.1.74:45456/api/Correo/Send", new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
                    string content = null;

                    if (response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<OperationResult<CorreoRequest>>(content);
                        ResultadoBool = true;
                        Resultado = "Mensaje enviado.";
                    }
                    else
                    {
                        result.ErrorType = ErrorType.Fatal;
                        result.Message = $"Error {response.StatusCode.ToString()}";
                        Resultado = result.Message;
                    }
                    IsBusy = false;

                }
                catch (TaskCanceledException ex)
                {
                    IsBusy = false;
                    ResultadoBool = true;
                    Resultado = ex.Message;
                }
                catch (Exception ex)
                {
                    ResultadoBool = true;
                    Resultado = ex.Message;
                    IsBusy = false;
                }
                return result;
            }
            
           

            
        }
    














    }
}