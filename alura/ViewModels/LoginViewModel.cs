﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using alura.Models;
using Xamarin.Forms;

namespace alura.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand EntrarCommand { get; private set; }

        public LoginViewModel()
        {
            EntrarCommand = new Command(
                async () =>
                {
                    try
                    {
                        var loginService = new LoginService();
                        var resultado = await loginService.FazerLogin(new Login(usuario, senha));

                        if (resultado.IsSuccessStatusCode)
                        {
                            string resultContent = resultado.Content.ReadAsStringAsync().Result;
                            LoginResult resultadoLogin = JsonConvert.DeserializeObject<LoginResult>(resultContent);

                            MessagingCenter.Send<Usuario>(resultadoLogin.usuario, "SucessoLogin");
                        }
                        else
                            MessagingCenter.Send<LoginException>(new LoginException("Dados Incorretos"), "FalhaLogin");
                    }
                    catch (Exception exc)
                    {
                        MessagingCenter.Send<LoginException>(new 
                            LoginException("Erro de comunicação com o servidor.", exc), "FalhaLogin");
                    }
                },
            () =>
            {
                return !string.IsNullOrEmpty(usuario)
                    && !string.IsNullOrEmpty(senha);
            });
        }

        private string usuario;
        public string Usuario
        {
            get { return usuario; }
            set
            {
                usuario = value;
                ((Command)EntrarCommand).ChangeCanExecute();
            }
        }

        private string senha;
        public string Senha
        {
            get { return senha; }
            set
            {
                senha = value;
                ((Command)EntrarCommand).ChangeCanExecute();
            }
        }
    }

    public class LoginException : Exception
    {
        public LoginException(string message) : base(message) { }

        public LoginException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
