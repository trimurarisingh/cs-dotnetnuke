using System;
using System.Collections;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Vendors;

namespace DotNetNuke.Modules.Admin.Vendors
{
    public partial class BannerOptions : PortalModuleBase
    {
        //*******************************************************
        //
        // The Page_Load event handler on this User Control is used to
        // obtain a DataReader of banner information from the Banners
        // table, and then databind the results to a templated DataList
        // server control.  It uses the DotNetNuke.BannerDB()
        // data component to encapsulate all data functionality.
        //
        //*******************************************************'
        protected void Page_Load( Object sender, EventArgs e )
        {
            try
            {
                if( ! Page.IsPostBack )
                {
                    // Obtain banner information from the Banners table and bind to the list control
                    BannerTypeController objBannerTypes = new BannerTypeController();

                    cboType.DataSource = objBannerTypes.GetBannerTypes();
                    cboType.DataBind();
                    cboType.Items.Insert( 0, new ListItem( Localization.GetString( "AllTypes", LocalResourceFile ), "-1" ) );

                    if( ModuleId > 0 )
                    {
                        // Get settings from the database
                        Hashtable settings = PortalSettings.GetModuleSettings( ModuleId );

                        if( optSource.Items.FindByValue( Convert.ToString( settings["bannersource"] ) ) != null )
                        {
                            optSource.Items.FindByValue( Convert.ToString( settings["bannersource"] ) ).Selected = true;
                        }
                        else
                        {
                            optSource.Items.FindByValue( "L" ).Selected = true;
                        }
                        if( cboType.Items.FindByValue( Convert.ToString( settings["bannertype"] ) ) != null )
                        {
                            cboType.Items.FindByValue( Convert.ToString( settings["bannertype"] ) ).Selected = true;
                        }
                        txtGroup.Text = Convert.ToString( settings["bannergroup"] );
                        if( optOrientation.Items.FindByValue( Convert.ToString( settings["orientation"] ) ) != null )
                        {
                            optOrientation.Items.FindByValue( Convert.ToString( settings["orientation"] ) ).Selected = true;
                        }
                        else
                        {
                            optOrientation.Items.FindByValue( "V" ).Selected = true;
                        }
                        if( Convert.ToString( settings["bannercount"] ) != "" )
                        {
                            txtCount.Text = Convert.ToString( settings["bannercount"] );
                        }
                        else
                        {
                            txtCount.Text = "1";
                        }
                        if( Convert.ToString( settings["border"] ) != "" )
                        {
                            txtBorder.Text = Convert.ToString( settings["border"] );
                        }
                        else
                        {
                            txtBorder.Text = "0";
                        }
                        if( Convert.ToString( settings["padding"] ) != "" )
                        {
                            txtPadding.Text = Convert.ToString( settings["padding"] );
                        }
                        else
                        {
                            txtPadding.Text = "4";
                        }
                        txtBorderColor.Text = Convert.ToString( settings["bordercolor"] );
                        txtRowHeight.Text = Convert.ToString( settings["rowheight"] );
                        txtColWidth.Text = Convert.ToString( settings["colwidth"] );
                    }
                }
            }
            catch( Exception exc ) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException( this, exc );
            }
        }

        protected void cmdUpdate_Click( object sender, EventArgs e )
        {
            try
            {
                if( Page.IsValid )
                {
                    // Update settings in the database
                    ModuleController objModules = new ModuleController();

                    if( optSource.SelectedItem != null )
                    {
                        objModules.UpdateModuleSetting( ModuleId, "bannersource", optSource.SelectedItem.Value );
                    }
                    if( cboType.SelectedItem != null )
                    {
                        objModules.UpdateModuleSetting( ModuleId, "bannertype", cboType.SelectedItem.Value );
                    }
                    objModules.UpdateModuleSetting( ModuleId, "bannergroup", txtGroup.Text );
                    if( optOrientation.SelectedItem != null )
                    {
                        objModules.UpdateModuleSetting( ModuleId, "orientation", optOrientation.SelectedItem.Value );
                    }
                    objModules.UpdateModuleSetting( ModuleId, "bannercount", txtCount.Text );
                    objModules.UpdateModuleSetting( ModuleId, "border", txtBorder.Text );
                    objModules.UpdateModuleSetting( ModuleId, "bordercolor", txtBorderColor.Text );
                    objModules.UpdateModuleSetting( ModuleId, "rowheight", txtRowHeight.Text );
                    objModules.UpdateModuleSetting( ModuleId, "colwidth", txtColWidth.Text );
                    objModules.UpdateModuleSetting( ModuleId, "padding", txtPadding.Text );

                    // Redirect back to the portal home page
                    Response.Redirect( Globals.NavigateURL(), true );
                }
            }
            catch( Exception exc ) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException( this, exc );
            }
        }

        protected void cmdCancel_Click( object sender, EventArgs e )
        {
            try
            {
                Response.Redirect( Globals.NavigateURL(), true );
            }
            catch( Exception exc ) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException( this, exc );
            }
        }
    }
}