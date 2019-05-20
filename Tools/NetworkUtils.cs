using System;
using System.Linq;
using Xamarin.Essentials;

namespace Tools
{
    public enum NetworkStatus
    {
        NotConnected = 0,
        Wifi = 1,
        MobileData = 2,
        Ethernet = 3,
        Unknown = 4
    }

    public class NetworkUtils
    {
        public event Action<object> OnNetworkConnected;
        public event Action<object> OnNetworkDisonnected;

        public NetworkUtils()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            var profiles = e.ConnectionProfiles;

            if (access == NetworkAccess.Internet) OnNetworkConnected?.Invoke(sender);

            else OnNetworkDisonnected?.Invoke(sender);
        }

        public static NetworkStatus NetworkStatus
        {
            get
            {
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    var profiles = Connectivity.ConnectionProfiles;
                    if (profiles.Contains(ConnectionProfile.WiFi))
                    {
                        return NetworkStatus.Wifi;
                    }

                    if (profiles.Contains(ConnectionProfile.Cellular))
                    {
                        return NetworkStatus.MobileData;
                    }

                    if (profiles.Contains(ConnectionProfile.Ethernet))
                    {
                        return NetworkStatus.Ethernet;
                    }

                    return NetworkStatus.Unknown;
                }

                return NetworkStatus.NotConnected;
            }
        }
    }
}
