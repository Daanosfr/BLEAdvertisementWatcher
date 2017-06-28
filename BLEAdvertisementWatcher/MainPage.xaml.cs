using System.Diagnostics;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BLEAdvertisementWatcher
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            BluetoothLEAdvertisementWatcher watcher = new BluetoothLEAdvertisementWatcher();

            var manufacturerData = new BluetoothLEManufacturerData();
            manufacturerData.CompanyId = 0xFFFE;

            watcher.AdvertisementFilter.Advertisement.ManufacturerData.Add(manufacturerData);

            watcher.Received += OnAdvertisementReceived;
            watcher.Start();
            Debug.WriteLine("watching");
        }

        void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            foreach (var item in args.Advertisement.GetManufacturerDataByCompanyId(0xFFFE))
            {
                Debug.WriteLine("received");

                using (var dataReader = DataReader.FromBuffer(item.Data))
                {
                    var length = dataReader.ReadInt32();
                    Debug.WriteLine(length);
                    string huidigeKorting = dataReader.ReadString((uint)length);
                    Debug.WriteLine(huidigeKorting);
                }
            }
        }
    }
}
