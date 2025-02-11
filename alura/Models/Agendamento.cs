﻿using System;
using SQLite;

namespace alura.Models
{
    public class Agendamento
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Fone { get; set; }
        public string Email { get; set; }
        public string Modelo { get; set; }
        public decimal Preco { get; set; }
        public bool Confirmado { get; set;  }
        public TimeSpan HoraAgendamento { get; set; }

        public string DataFormatada
        {
            get
            {
                return DataAgendamento.Add(HoraAgendamento).ToString("dd/MM/yyyy HH:mm");
            }
        }
        DateTime dataAgendamento = DateTime.Today;
        public DateTime DataAgendamento
        {
            get
            {
                return dataAgendamento;
            }
            set
            {
                dataAgendamento = value;
            }
        }

        public Agendamento(string nome, string fone, string email, string modelo, decimal preco )
        {
            this.Nome = nome;
            this.Fone = fone;
            this.Email = email;
            this.Modelo = modelo;
            this.Preco = preco;
        }
        public Agendamento()
        {
        }
    }
}
