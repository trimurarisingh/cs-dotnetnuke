using System;
using System.ComponentModel;
using System.Web.UI;
using System.Xml;

namespace DotNetNuke.UI.WebControls
{
    public class MenuNode : DNNNode
    {
        private MenuNodeCollection m_objNodes;
        private DNNMenu m_objDNNMenu;

        public MenuNode()
        {
        }

        public MenuNode( string strText ) : base( strText )
        {
        }

        internal MenuNode( XmlNode objXmlNode, Control ctlOwner ) : base( objXmlNode )
        {
            m_objDNNMenu = (DNNMenu)ctlOwner;
        }

        internal MenuNode( Control ctlOwner )
        {
            m_objDNNMenu = (DNNMenu)ctlOwner;
        }

        [Browsable( true ), PersistenceMode( PersistenceMode.InnerProperty )]
        public MenuNodeCollection MenuNodes
        {
            get
            {
                if( m_objNodes == null )
                {
                    m_objNodes = new MenuNodeCollection( this.XmlNode, this.DNNMenu );
                }
                return m_objNodes;
            }
        }

        [Browsable( false )]
        public MenuNode Parent
        {
            get
            {
                return this.ParentNode;
            }
        }

        [Browsable( false )]
        public DNNMenu DNNMenu
        {
            get
            {
                return m_objDNNMenu;
            }
        }

        [Bindable( true ), DefaultValue( "" ), PersistenceMode( PersistenceMode.Attribute )]
        public string CssClassOver
        {
            get
            {
                return this.CSSClassHover;
            }
            set
            {
                this.CSSClassHover = value;
            }
        }

        [Bindable( true ), DefaultValue( -1 ), PersistenceMode( PersistenceMode.Attribute )]
        public int ImageIndex
        {
            get
            {
                if( this.CustomAttribute( "iIdx" ).Length > 0 )
                {
                    return Convert.ToInt32( this.CustomAttribute( "iIdx" ) );
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                this.SetCustomAttribute( "iIdx", value.ToString() );
            }
        }

        [Bindable( true ), DefaultValue( -1 ), PersistenceMode( PersistenceMode.Attribute )]
        public int UrlIndex
        {
            get
            {
                if( this.CustomAttribute( "uIdx" ).Length > 0 )
                {
                    return Convert.ToInt32( this.CustomAttribute( "uIdx" ) );
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                this.SetCustomAttribute( "uIdx", value.ToString() );
            }
        }

        //Public Property PopulateOnDemand() As Boolean
        //	Get
        //		Return CBool(Me.CustomAttribute("pond", False))
        //	End Get
        //	Set(ByVal Value As Boolean)
        //		Me.CustomAttribute("pond") = Value
        //	End Set
        //End Property

        public string LeftHTML
        {
            get
            {
                return this.CustomAttribute( "lhtml", "" );
            }
            set
            {
                this.SetCustomAttribute( "lhtml", value );
            }
        }

        public string RightHTML
        {
            get
            {
                return this.CustomAttribute( "rhtml", "" );
            }
            set
            {
                this.SetCustomAttribute( "rhtml", value );
            }
        }

        public new MenuNode ParentNode
        {
            get
            {
                if( this.XmlNode.ParentNode != null && this.XmlNode.ParentNode.NodeType != XmlNodeType.Document )
                {
                    return new MenuNode( this.XmlNode.ParentNode, m_objDNNMenu );
                }
                else
                {
                    return null;
                }
            }
        }

        private IMenuNodeWriter NodeWriter
        {
            get
            {
                if( m_objDNNMenu.IsDownLevel )
                {
                    return new MenuNodeWriter( m_objDNNMenu.IsCrawler, m_objDNNMenu.Orientation );
                }
                else
                {
                    return null; // New MenuNodeUpLevelWriter
                }
            }
        }

        public void Click()
        {
            this.Selected = !this.Selected;
            if( DNNMenu.IsDownLevel )
            {
                MenuNode objNode;
                foreach( MenuNode tempLoopVar_objNode in DNNMenu.SelectedMenuNodes )
                {
                    objNode = tempLoopVar_objNode;
                    if( objNode.Level > this.Level )
                    {
                        objNode.Selected = false;
                    }
                }
            }
            DNNMenu.OnNodeClick( new DNNMenuNodeClickEventArgs( this ) );
        }

        public virtual void Render( HtmlTextWriter writer )
        {
            NodeWriter.RenderNode( writer, this );
        }

        internal void SetDNNMenu( DNNMenu objMenu )
        {
            m_objDNNMenu = objMenu;
        }
    }
}