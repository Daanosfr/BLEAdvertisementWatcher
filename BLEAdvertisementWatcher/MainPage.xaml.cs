using System;
using System.Diagnostics;
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

            watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(500);

            watcher.Received += OnAdvertisementReceived;
            watcher.Start();
            Debug.WriteLine("watching");
        }

        void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            foreach (var item in args.Advertisement.GetManufacturerDataByCompanyId(0xFFFE))
            {
                Debug.WriteLine("gevonden");
                using (var dataReader = DataReader.FromBuffer(item.Data))
                {
                    var length = dataReader.ReadInt32();
                    string huidigeKorting = dataReader.ReadString((uint)length);
                    Debug.WriteLine(huidigeKorting);
                    if (huidigeKorting == "melk")
                    {
                        imageChocola.Visibility = Visibility.Visible;
                    }
                }
            }
        }
    }
}
