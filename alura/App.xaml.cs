﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using alura.Models;
using alura.Views;
using Xamarin.Forms;

namespace alura
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoginView();
        }

        protected override void OnStart()
        {
            MessagingCenter.Subscribe<Usuario>(this, "SucessoLogin",
                (usuario) =>
                {
                    MainPage = new MasterDetailView(usuario);
                });
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
