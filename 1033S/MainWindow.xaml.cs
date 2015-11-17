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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1033S {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.MenuBar_Init();
        }

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

            switch(requestedMenu) {
                case SubMenu.Server:
                    if(visibleMenu == SubMenu.None) {
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


        ///<summary>
        /// used to add menuitems to the main menu bar
        /// </summary>
        /// <TODO>
        /// maybe other yet unknown items (notifications?) as well 
        /// => should solve with some interface
        /// IMenuItem or sth.
        /// </TODO>
        private void MenuItem_AddMenuItem() {
            throw new NotImplementedException();
        }

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
