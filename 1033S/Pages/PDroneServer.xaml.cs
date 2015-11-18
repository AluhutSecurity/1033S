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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1033S.Pages {
    /// <summary>
    /// Interaction logic for PDroneServer.xaml
    /// </summary>
    public partial class PDroneServer : UserControl {
        public PDroneServer( _1033C.Drones.MyDroneServer server ) {
            InitializeComponent();
            this.ServerParameter = new _1033C.Utils.MyServerParameter( _1033C.Defines.DEFAULT_DRONESERVER_LISTENERIP, _1033C.Defines.DEFAULT_DRONESERVER_LOCALPORT );
            this.Server = server;
            this.DroneServerStackPanel.DataContext = this.ServerParameter;
        }

        public _1033C.Utils.MyServerParameter ServerParameter { get; }
        private _1033C.Drones.MyDroneServer Server { get; }

        private void DroneServer_StartStopButton_Click( object sender, RoutedEventArgs e ) {
            if ( ( string )( ( Button )sender ).Content == "Start" ) {
                if(this.ServerParameter.LocalIP != null ) {
                    this.Server.LocalEndPoint = this.ServerParameter.LocalIP;
                }
                this.Server.Port = this.ServerParameter.Port;

                this.Server.Start();
                ( ( Button )sender ).Content = "Stop";
            } else {
                this.Server.Stop();
                ( ( Button )sender ).Content = "Start";
            }
        }
    }
}
