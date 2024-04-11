using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace ColorShadesGenerator
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Constants and Fields

        private static readonly double[] Percentages =
            {0.0, 0.05, 0.1, 0.15, 0.2, 0.25, 0.3, 0.35, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.95, 0.98, 0.99, 1.0};

        private string? _rgbColorText;
        private string _createdResources = "";
        private string? _colorName;

        #endregion

        #region Constructors and Destructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            CreateShadesCommand = new DelegateCommand(CreateShades, () => ConvertFromString(RgbColorText) != null);
        }

        #endregion

        #region Public Properties

        public string? ColorName
        {
            get => _colorName;
            set
            {
                if (value == _colorName) return;
                _colorName = value;
                OnPropertyChanged();
            }
        }

        public string CreatedResources
        {
            get => _createdResources;
            set => SetField(ref _createdResources, value);
        }

        public DelegateCommand CreateShadesCommand { get; }

        public string? RgbColorText
        {
            get => _rgbColorText;
            set
            {
                SetField(ref _rgbColorText, value);
                CreateShadesCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Color> Shades { get; } = new();

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Private Methods

        private static Color? ConvertFromString(string? hexString)
        {
            if (string.IsNullOrEmpty(hexString)) return null;

            try
            {
                return (Color) ColorConverter.ConvertFromString(hexString);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        private void CreateShades()
        {
            if (ConvertFromString(RgbColorText) is not { } color) return;

            var hslColor = HslColor.FromColor(color);

            Shades.Clear();

            foreach (var shade in Percentages.Select(lum => new HslColor(hslColor.H, hslColor.S, lum).ToColor()))
                Shades.Add(shade);

            CreatedResources = string.Join(Environment.NewLine, Shades.Select(ToColorResourceString));
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private string ToColorResourceString(Color color)
        {
            var colorName = string.IsNullOrEmpty(ColorName) ? "Color" : ColorName!;
            var luminance = HslColor.FromColor(color).L * 100.0;
            
            return
                $"<Color x:Key=\"{colorName}.{luminance:F0}\">{color.ToString()}</Color>";
        }

        #endregion
    }
}