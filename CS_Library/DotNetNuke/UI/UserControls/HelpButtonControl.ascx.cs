using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;

namespace DotNetNuke.UI.UserControls
{
    /// <Summary>
    /// HelpButtonControl is a user control that provides all the server code to display
    /// field level help button.
    /// </Summary>
    public abstract class HelpButtonControl : UserControl
    {
        private string _ControlName; //Associated Edit Control for this Label
        private string _HelpKey; //Resource Key for the Help Text
        private string _ResourceKey; //Resource Key for the Label Text
        protected LinkButton cmdHelp;
        protected Image imgHelp;
        protected Label lblHelp;
        protected Panel pnlHelp;

        /// <Summary>
        /// ControlName is the Id of the control that is associated with the label
        /// </Summary>
        public string ControlName
        {
            get
            {
                return this._ControlName;
            }
            set
            {
                this._ControlName = value;
            }
        }

        /// <Summary>HelpKey is the Resource Key for the Help Text</Summary>
        public string HelpKey
        {
            get
            {
                return this._HelpKey;
            }
            set
            {
                this._HelpKey = value;
            }
        }

        /// <Summary>
        /// HelpText is value of the Help Text if no ResourceKey is provided
        /// </Summary>
        public string HelpText
        {
            get
            {
                return this.lblHelp.Text;
            }
            set
            {
                this.lblHelp.Text = value;
                this.imgHelp.AlternateText = value;
                //hide the help icon if the help text is ""
                if( value == "" )
                {
                    imgHelp.Visible = false;
                }
            }
        }

        /// <Summary>ResourceKey is the Resource Key for the Help Text</Summary>
        public string ResourceKey
        {
            get
            {
                return this._ResourceKey;
            }
            set
            {
                this._ResourceKey = value;
            }
        }

        public HelpButtonControl()
        {
            this.cmdHelp.Click += new EventHandler( this.cmdHelp_Click );
            base.Load += new EventHandler( this.Page_Load );
        }

        /// <Summary>
        /// GetLocalizedText gets the localized text for the provided key
        /// </Summary>
        /// <Param name="key">The resource key</Param>
        /// <Param name="ctl">The current control</Param>
        private string GetLocalizedText( string key, Control ctl )
        {
            //We need to find the parent module
            Control parentControl = ctl.Parent;
            string localizedText;

            if( parentControl is PortalModuleBase )
            {
                //We are at the Module Level so return key
                //Get Resource File Root from Parents LocalResourceFile Property
                PortalModuleBase ctrl;
                ctrl = (PortalModuleBase)parentControl;
                localizedText = Localization.GetString( key, ctrl.LocalResourceFile );
            }
            else if( parentControl is Address )
            {
                //We are in an Address Control so return key
                //Get Resource File Root from Parents LocalResourceFile Property
                Address ctrl;
                ctrl = (Address)parentControl;
                localizedText = Localization.GetString( key, ctrl.LocalResourceFile );
            }
            else
            {
                //Drill up to the next level
                localizedText = GetLocalizedText( key, parentControl );
            }

            return localizedText;
        }

        private void cmdHelp_Click( object sender, EventArgs e )
        {
            this.pnlHelp.Visible = true;
        }

        /// <Summary>Page_Load runs when the control is loaded</Summary>
        private void Page_Load( object sender, EventArgs e )
        {
            try
            {
                DNNClientAPI.EnableMinMax( cmdHelp, pnlHelp, true, DNNClientAPI.MinMaxPersistanceType.None );

                if( _HelpKey == "" )
                {
                    //Set Help Key to the Resource Key plus ".Help"
                    _HelpKey = _ResourceKey + ".Help";
                }
                string helpText = GetLocalizedText( _HelpKey, this );
                if( helpText != "" )
                {
                    this.HelpText = helpText;
                }
            }
            catch( Exception exc ) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException( this, exc );
            }
        }
    }
}