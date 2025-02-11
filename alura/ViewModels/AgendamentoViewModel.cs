﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using alura.Data;
using alura.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace alura.ViewModels
{
    public class AgendamentoViewModel : BaseViewModel
    {
        public Agendamento Agendamento { get; set; }

        private string modelo;
        public string Modelo
        {
            get { return modelo; }
            set { this.modelo = value; }
        }

        private decimal preco;
        public decimal Preco
        {
            get
            {
                return preco;
            }
            set
            {
                this.preco = value;
            }
        }

        public string Nome
        {
            get { return Agendamento.Nome; }
            set
            {
                Agendamento.Nome = value;
                OnPropertyChanged();
                ((Command)AgendamentoCommand).ChangeCanExecute();
            }
        }

        public string Fone { get { return Agendamento.Fone; } 
            set 
            { 
                Agendamento.Fone = value;
                OnPropertyChanged();
                ((Command)AgendamentoCommand).ChangeCanExecute();
            }
        }
        public string Email { get { return Agendamento.Email; } 
            set 
            { 
                Agendamento.Email = value;
                OnPropertyChanged();
                ((Command)AgendamentoCommand).ChangeCanExecute();
            } 
        }
        public TimeSpan HoraAgendamento { get { return Agendamento.HoraAgendamento; } set { Agendamento.HoraAgendamento = value; } }
        public const string URL_POST_AGENDAMENTO = "http://aluracar.herokuapp.com/salvaragendamento";

        public DateTime DataAgendamento
        {
            get
            {
                return Agendamento.DataAgendamento;
            }
            set
            {
                Agendamento.DataAgendamento = value;
            }
        }

        public AgendamentoViewModel(Veiculo veiculo, Usuario usuario)
        {
            this.Agendamento = new Agendamento(usuario.nome, usuario.telefone, usuario.email, veiculo.Nome, veiculo.Preco);


            AgendamentoCommand = new Command(() =>
            {
                MessagingCenter.Send<Agendamento>(this.Agendamento, "MostrarAgendamento");
            },
                () =>
                {
                    return !string.IsNullOrEmpty(this.Nome);
                }
            );
        }

        public async void SalvarAgendamento()
        {
            HttpClient cliente = new HttpClient();
            var dataHoraAgendamento = new DateTime
            (
                DataAgendamento.Year, DataAgendamento.Month, DataAgendamento.Day,
                HoraAgendamento.Hours, DataAgendamento.Minute, DataAgendamento.Second
            );

            var json = JsonConvert.SerializeObject(new
            {
                nome = Nome,
                fone = Fone,
                email = Email,
                carro = Modelo,
                preco = Preco,
                dataAgendamento = dataHoraAgendamento

            });
            var conteudo = new StringContent(json, Encoding.UTF8, "application/json");
            var resposta = await cliente.PostAsync(URL_POST_AGENDAMENTO, conteudo);

            SalvarAgendamentoDB();
            this.Agendamento.Confirmado = resposta.IsSuccessStatusCode;
            if (this.Agendamento.Confirmado)
            {
                MessagingCenter.Send<Agendamento>(this.Agendamento, "AgendamentoSalvo");
            }
            else
            {
                MessagingCenter.Send<ArgumentException>(new ArgumentException(), "AgendamentoNaoSalvo");
            }

        }


        public void SalvarAgendamentoDB()
        {
            using (var conexao = DependencyService.Get<ISQLite>().PegarConexao())
            {
                AgentamentoDAO dao = new AgentamentoDAO(conexao);
                dao.Salvar(new Agendamento(Nome, Fone, Email, Modelo, Preco));
            }
        }

        public ICommand AgendamentoCommand { get; set;  }

    }
}
