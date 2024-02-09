using CsvHelper;
using ExpectativaMercadoMensais.Domain.Dtos;
using ExpectativaMercadoMensais.Domain.Entities;
using ExpectativaMercadoMensais.Domain.Interfaces;
using GalaSoft.MvvmLight.Command;
using LiveCharts;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using LiveCharts.Wpf;
using LiveCharts.Definitions.Series;

public class ExpectativaMercadoMensalViewModel : DelegatingHandler, INotifyPropertyChanged
{
    private readonly IExpectativaMercadoMensalAppService _expectativaMercadoMensalService;


    private ObservableCollection<string> _tiposIndicador;
    private string _tipoIndicadorSelecionado;
    private DateTime _dataInicialSelecionado;
    private DateTime _dataFinalSelecionado;

    private ObservableCollection<ExpectativaMercadoMensal> _expectativasMercadoMensal;
    private ICommand _buscarExpectativasMercadoMensalCommand;

    private readonly HttpClient _httpClient;

    public ExpectativaMercadoMensalViewModel(IExpectativaMercadoMensalAppService expectativaMercadoMensalService, HttpClient httpClient, IHttpClientFactory httpClientFactory)
    {
        _expectativaMercadoMensalService = expectativaMercadoMensalService;
        _httpClient = httpClient;
        _httpClient = httpClientFactory.CreateClient();

        LoadTiposIndicador();
        LoadExpectativasMercadoMensal();
        DataInicialSelecionado = DateTime.Now;
        DataFinalSelecionado = DateTime.Now;
        _tipoIndicadorSelecionado = string.Empty;

        ExportarCSVCommand = new RelayCommand(ExportarCSVButton_Click);


    }


    public ObservableCollection<string> TiposIndicador
    {
        get => _tiposIndicador;
        set
        {
            _tiposIndicador = value;
            OnPropertyChanged();
        }
    }

    public string TipoIndicadorSelecionado
    {
        get => _tipoIndicadorSelecionado;
        set
        {
            _tipoIndicadorSelecionado = value;
            OnPropertyChanged();
            LoadExpectativasMercadoMensal();
        }
    }

    public DateTime DataInicialSelecionado
    {
        get => _dataInicialSelecionado;
        set
        {
            _dataInicialSelecionado = value;
            var collectionView = CollectionViewSource.GetDefaultView(ExpectativasMercadoMensalCollection);
            collectionView.Refresh();
        }
    }

    public DateTime DataFinalSelecionado
    {
        get => _dataFinalSelecionado;
        set
        {
            _dataFinalSelecionado = value;
            var collectionView = CollectionViewSource.GetDefaultView(ExpectativasMercadoMensalCollection);
            collectionView.Refresh();
        }
    }

    public ObservableCollection<ExpectativaMercadoMensal> ExpectativasMercadoMensalCollection
    {
        get => _expectativasMercadoMensal;
        set
        {
            _expectativasMercadoMensal = value;
            OnPropertyChanged();
        }
    }

    public ICommand BuscarExpectativasMercadoMensalCommand => _buscarExpectativasMercadoMensalCommand ??= new RelayCommand(async () => await BuscarExpectativasMercadoMensal());

    private async void LoadTiposIndicador()
    {
        TiposIndicador = new ObservableCollection<string>(await _expectativaMercadoMensalService.GetAllTipoIndicador());
    }

    private async void LoadExpectativasMercadoMensal()
    {

        var espectativas = await _expectativaMercadoMensalService.GetExpectativasMercadoMensalAsync(TipoIndicadorSelecionado);

        var espectativasObserve = new ObservableCollection<ExpectativaMercadoMensal>(espectativas);

        ExpectativasMercadoMensalCollection = new ObservableCollection<ExpectativaMercadoMensal>(espectativasObserve);

        Valores = new ChartValues<ExpectativaMercadoMensal>(ExpectativasMercadoMensalCollection);

    }

    private async Task BuscarExpectativasMercadoMensal()
    {
        var espectativas = await _expectativaMercadoMensalService.GetExpectativasMercadoMensalAsync(TipoIndicadorSelecionado);

        var espectativasObserve = new ObservableCollection<ExpectativaMercadoMensal>(espectativas);

        ExpectativasMercadoMensalCollection = new ObservableCollection<ExpectativaMercadoMensal>(espectativasObserve);

        var collectionView = CollectionViewSource.GetDefaultView(ExpectativasMercadoMensalCollection);
        collectionView.Filter = FiltrarExpectativasMercadoMensal;

        var mediaX = ConverterEmChartValues(ExpectativasMercadoMensalCollection, e => e.Data.Ticks);
        var mediaY = ConverterEmChartValues(ExpectativasMercadoMensalCollection, e => e.Media);
        //LabelsX = new ObservableCollection<string>(mediaX.ToString());
        LabelsY = new ObservableCollection<string>(mediaY.Select(y => y.ToString("0.00")));

        SeriesCollection = new ObservableCollection<ISeriesView>
    {
        new LineSeries
        {
            Title = "Expectativas Mercado Mensal",
            Values = mediaY,
        }
    };

        
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool FiltrarExpectativasMercadoMensal(object item)
    {
        var expectativa = item as ExpectativaMercadoMensal;
        if (expectativa == null) return false;

        var dataInicial = DataInicialSelecionado;
        var dataFinal = DataFinalSelecionado;

        if (dataInicial == null && dataFinal == null) return true;
        if (dataInicial != null && dataFinal == null) return expectativa.Data >= dataInicial;
        if (dataInicial == null && dataFinal != null) return expectativa.Data <= dataFinal;

        return dataInicial <= expectativa.Data && expectativa.Data <= dataFinal;
    }

    private void DataInicialDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        var collectionView = CollectionViewSource.GetDefaultView(ExpectativasMercadoMensalCollection);
        collectionView.Refresh();
    }

    private void DataFinalDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        var collectionView = CollectionViewSource.GetDefaultView(ExpectativasMercadoMensalCollection);
        collectionView.Refresh();
    }

    public RelayCommand ExportarCSVCommand { get; private set; }

    private async void ExportarCSVButton_Click()
    {
        var saveFileDialog = new SaveFileDialog
        {
            DefaultExt = ".csv",
            Filter = "Arquivos CSV (*.csv)|*.csv"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            using (var streamWriter = new StreamWriter(saveFileDialog.FileName))
            {
                var csvWriter = new CsvWriter(streamWriter, CultureInfo.CurrentCulture);

                csvWriter.WriteHeader<ExpectativaMercadoMensal>();
                csvWriter.NextRecord();

                foreach (var item in ExpectativasMercadoMensalCollection)
                {
                    csvWriter.WriteRecord(item);
                    csvWriter.NextRecord();
                }
            }
        }
    }

    public ChartValues<ExpectativaMercadoMensal> Valores { get; set; }

    public ChartValues<double> ConverterEmChartValues(ObservableCollection<ExpectativaMercadoMensal> expectativasMercadoMensalCollection, Func<ExpectativaMercadoMensal, double> selector)
    {
        return new ChartValues<double>(expectativasMercadoMensalCollection.Select(selector));
    }

    public ObservableCollection<ISeriesView> SeriesCollection { get; set; }
    public ObservableCollection<string> LabelsX { get; set; }
    public ObservableCollection<string> LabelsY { get; set; }

}
