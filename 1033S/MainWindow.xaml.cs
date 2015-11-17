using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Xml;

namespace _1033S {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.MenuBar_Init();
            this.Server_Init();
        }

        #region Server
        private void Server_Init() {
            this.droneServer = new _1033C.Drones.MyDroneServer();
            this.droneServer.DroneStatusChanged += DroneServer_DroneStatusChanged;
#pragma warning removethis
            this.droneServer.Start();
        }

        private void DroneServer_DroneStatusChanged( object sender, _1033C.Drones.MyDrone e ) {
            if ( e.State == _1033C.Drones.MyDroneState.Online ) {
                this.Dispatcher.Invoke( new Action( () => {
                    this.MenuItem_AddDroneMenuItem( "Drohne #" + e.UID );
                } ) );
            } else if ( e.State == _1033C.Drones.MyDroneState.Offline ) {
                this.Dispatcher.Invoke( new Action( () => {
                    this.MenuItem_RemoveDroneMenuItem( "Drohne #" + e.UID );
                } ) );
            }
        }

        private _1033C.Drones.MyDroneServer droneServer;

        #endregion


        #region MenuBar
        public const int SubMenuHeight = 34;

        SubMenu visibleMenu;
        Storyboard expandMenus, contractMenus;

        private void MenuBar_Init() {
            this.visibleMenu = SubMenu.None;
            this.expandMenus = ( Storyboard ) TryFindResource( "ExpandSubMenus" );
            this.contractMenus = ( Storyboard ) TryFindResource( "ContractSubMenus" );
        }


        #region MainMenu

        void InitAnimation( SubMenu currentMenu, SubMenu requestedMenu ) {
            DoubleAnimation animation;

            if ( requestedMenu == currentMenu ) requestedMenu = SubMenu.None;

            switch ( requestedMenu ) {
                case SubMenu.Server:
                    if ( visibleMenu == SubMenu.None ) {
                        Grid.SetZIndex( ServerSubMenu, 1 );
                        Grid.SetZIndex( PackageSubMenu, 0 );
                        this.expandMenus.Begin();
                    } else {
                        animation = new DoubleAnimation( 0, 1, TimeSpan.FromMilliseconds( 250 ) );
                        ServerSubMenu.BeginAnimation( Menu.OpacityProperty, animation );
                        Grid.SetZIndex( ServerSubMenu, 1 );
                        Grid.SetZIndex( PackageSubMenu, 0 );
                    }
                    break;
                case SubMenu.Packages:
                    if ( visibleMenu == SubMenu.None ) {
                        Grid.SetZIndex( PackageSubMenu, 1 );
                        Grid.SetZIndex( ServerSubMenu, 0 );
                        this.expandMenus.Begin();
                    } else {
                        animation = new DoubleAnimation( 0, 1, TimeSpan.FromMilliseconds( 250 ) );
                        PackageSubMenu.BeginAnimation( Menu.OpacityProperty, animation );
                        Grid.SetZIndex( PackageSubMenu, 1 );
                        Grid.SetZIndex( ServerSubMenu, 0 );
                    }
                    break;
                default:
                    contractMenus.Begin();
                    break;
            }

            visibleMenu = requestedMenu;
        }

        private void MenuItem_Server_Click( object sender, RoutedEventArgs e ) {
            InitAnimation( this.visibleMenu, SubMenu.Server );
        }
        private void MenuItem_Packages_Click( object sender, RoutedEventArgs e ) {
            InitAnimation( this.visibleMenu, SubMenu.Packages );
        }
        private void MenuItem_PickupCam_Click( object sender, RoutedEventArgs e ) {
            this.InitAnimation( this.visibleMenu, SubMenu.None );
            this.Title = "1033S PickupCam";
        }

        #region CustomDroneMenuItems
        private _1033C.Drones.MyDrone droneInformation;

        private void MenuItem_AddDroneMenuItem( string text ) {
            MenuItem nItem = new MenuItem() {
                Padding = new Thickness( 10 )
            };
            nItem.Header = new StackPanel {
                Orientation = Orientation.Horizontal,
                Children = {
                    new Image() {
                        Source = Properties.Resources.drone.ToBitmapImage(),
                        Margin = new Thickness(0, 0, 10, 0)
                    },
                    new TextBlock() {
                        Text = text,
                        FontSize = 14.0,
                        Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255))
                    }
                }
            };
            nItem.MouseEnter += NItem_MouseEnter;
            nItem.MouseLeave += NItem_MouseLeave;
            this.MainMenu.Items.Add( nItem );
        }

        private void MenuItem_RemoveDroneMenuItem( string identifier ) {
            foreach ( MenuItem item in this.MainMenu.Items ) {
                if ( ( ( TextBlock ) ( ( StackPanel ) item.Header ).Children[1] ).Text == identifier ) {
                    this.MainMenu.Items.Remove( item );
                    return;
                }
            }
        }

        private void NItem_MouseLeave( object sender, System.Windows.Input.MouseEventArgs e ) {
            this.DroneInformationPanel.Visibility = Visibility.Collapsed;
        }

        private void NItem_MouseEnter( object sender, System.Windows.Input.MouseEventArgs e ) {
            MenuItem item = ( MenuItem ) sender;
            var text = ( ( TextBlock ) ( ( StackPanel ) item.Header ).Children[1] ).Text;
            var id = ulong.Parse( text.Split( '#' )[1] );
            this.droneInformation = this.droneServer.GetDroneByUID( id );

            this.DroneInformationPanel.DataContext = this.droneInformation;
            this.DroneInformationPanel.Margin = new Thickness( item.TransformToAncestor( this ).Transform( new Point( 0, 0 ) ).X, 0, 0, 0 );
            this.DroneInformationPanel.Visibility = Visibility.Visible;
        }
        #endregion

        #endregion
        #region SubMenu_Server
        private void ServerSubMenuItem_Drone_Click( object sender, RoutedEventArgs e ) {
            InitAnimation( this.visibleMenu, SubMenu.None );
            this.Title = "1033S Server\\Drohnen";
        }

        private void ServerSubMenuItem_SQL_Click( object sender, RoutedEventArgs e ) {
            InitAnimation( this.visibleMenu, SubMenu.None );
            this.Title = "1033S Server\\SQL";
        }

        private void ServerSubMenuItem_Location_Click( object sender, RoutedEventArgs e ) {
            InitAnimation( this.visibleMenu, SubMenu.None );
            this.Title = "1033S Server\\Location";
        }
        #endregion
        #region SubMenu_Package
        private void PackageSubMenuItem_Register_Click( object sender, RoutedEventArgs e ) {
            InitAnimation( this.visibleMenu, SubMenu.None );
            this.Title = "1033S Pakete\\Registrieren";
        }

        private void PackageSubMenuItem_Ready_Click( object sender, RoutedEventArgs e ) {
            InitAnimation( this.visibleMenu, SubMenu.None );
            this.Title = "1033S Pakete\\Versandbereit";
        }

        private void PackageSubMenuItem_Delivery_Click( object sender, RoutedEventArgs e ) {
            InitAnimation( this.visibleMenu, SubMenu.None );
            this.Title = "1033S Pakete\\Unterwegs";
        }

        private void PackageSubMenuItem_Closed_Click( object sender, RoutedEventArgs e ) {
            InitAnimation( this.visibleMenu, SubMenu.None );
            this.Title = "1033S Pakete\\Abgeschlossen";
        }
        #endregion
        #endregion

    }

    enum SubMenu {
        None,
        Server,
        Packages
    }

}
