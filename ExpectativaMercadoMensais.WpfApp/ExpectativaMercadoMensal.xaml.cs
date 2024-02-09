using ExpectativaMercadoMensais.Application.Services;
using ExpectativaMercadoMensais.Domain.Entities;
using ExpectativaMercadoMensais.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExpectativaMercadoMensais.WpfApp
{
    /// <summary>
    /// Lógica interna para ExpectativaMercadoMensal.xaml
    /// </summary>
    public partial class ExpectativaMercadoMensal : Window
    {
        private readonly ExpectativaMercadoMensalViewModel _expectativaMercadoMensalViewModel;
        public ExpectativaMercadoMensal()
        {
            InitializeComponent();
        }

    }
}
