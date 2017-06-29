using System;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BLEAdvertisementWatcher
{
    public sealed partial class MainPage : Page
    {
        BluetoothLEAdvertisementWatcher watcher;

        public MainPage()
        {
            this.InitializeComponent();
            imageChocola.Visibility = Visibility.Collapsed;
            watcher = new BluetoothLEAdvertisementWatcher();
            var manufacturerData = new BluetoothLEManufacturerData();
            manufacturerData.CompanyId = 0xFFFE;
            watcher.AdvertisementFilter.Advertisement.ManufacturerData.Add(manufacturerData);
            watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(2000);
            watcher.Received += OnAdvertisementReceived;
            watcher.Start();
        }

        void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            foreach (var item in args.Advertisement.GetManufacturerDataByCompanyId(0xFFFE))
            {
                using (var dataReader = DataReader.FromBuffer(item.Data))
                {
                    var length = dataReader.ReadInt32();
                    string huidigeKorting = dataReader.ReadString((uint)length);
                    if (huidigeKorting == "chocola") imageChocola.Visibility = Visibility.Visible;
                    else imageChocola.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
