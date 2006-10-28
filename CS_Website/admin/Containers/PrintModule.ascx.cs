using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;

namespace DotNetNuke.UI.Containers
{
    /// Project	 : DotNetNuke
    /// Class	 : Containers.Icon
    ///
    /// <summary>
    /// Contains the attributes of an Icon.
    /// These are read into the PortalModuleBase collection as attributes for the icons within the module controls.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// 	[sun1]	        2/1/2004	Created
    /// 	[Nik Kalyani]	10/15/2004	Replaced public members with properties and removed
    ///                                 brackets from property names
    /// </history>
    public partial class PrintModule : ActionBase
    {
        // private members
        private string _printIcon;

        public string PrintIcon
        {
            get
            {
                return _printIcon;
            }
            set
            {
                _printIcon = value;
            }
        }

        private void InitializeComponent()
        {
        }

        protected void Page_Init( Object sender, EventArgs e )
        {
            //CODEGEN: This method call is required by the Web Form Designer
            //Do not modify it using the code editor.
            InitializeComponent();
        }

        protected void Page_Load( Object sender, EventArgs e )
        {
            try
            {
                UserInfo _UserInfo = UserController.GetCurrentUserInfo();

                foreach( ModuleAction action in this.MenuActions )
                {
                    if( action.CommandName == ModuleActionType.PrintModule )
                    {
                        if( action.Visible && PortalSecurity.HasNecessaryPermission( action.Secure, PortalSettings, ModuleConfiguration, _UserInfo.UserID.ToString() ) )
                        {
                            bool blnPreview = false;
                            if( Request.Cookies["_Tab_Admin_Preview" + PortalSettings.PortalId.ToString()] != null )
                            {
                                blnPreview = bool.Parse( Request.Cookies["_Tab_Admin_Preview" + PortalSettings.PortalId.ToString()].Value );
                            }
                            if( blnPreview == false || ( action.Secure == SecurityAccessLevel.Anonymous || action.Secure == SecurityAccessLevel.View ) )
                            {
                                if( PortalModule.ModuleConfiguration.DisplayPrint )
                                {
                                    ImageButton ModuleActionIcon = new ImageButton();
                                    if( PrintIcon != "" )
                                    {
                                        ModuleActionIcon.ImageUrl = PortalModule.ModuleConfiguration.ContainerPath.Substring( 0, ModuleConfiguration.ContainerPath.LastIndexOf( "/" ) + 1 ) + PrintIcon;
                                    }
                                    else
                                    {
                                        ModuleActionIcon.ImageUrl = "~/images/" + action.Icon;
                                    }
                                    ModuleActionIcon.ToolTip = action.Title;
                                    ModuleActionIcon.ID = "ico" + action.ID.ToString();
                                    ModuleActionIcon.CausesValidation = false;

                                    ModuleActionIcon.Click += new ImageClickEventHandler( IconAction_Click );

                                    this.Controls.Add( ModuleActionIcon );
                                }
                            }
                        }
                    }
                }

                // set visibility
                if( this.Controls.Count > 0 )
                {
                    this.Visible = true;
                }
                else
                {
                    this.Visible = false;
                }
            }
            catch( Exception exc ) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException( this, exc );
            }
        }

        private void IconAction_Click( object sender, ImageClickEventArgs e )
        {
            try
            {
                ProcessAction( ( (ImageButton)sender ).ID.Substring( 3 ) );
            }
            catch( Exception exc ) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException( this, exc );
            }
        }
    }
}