using System.Web.UI;
using DotNetNuke.Entities.Portals;
using DotNetNuke.UI.WebControls;

public class NavDataPageHierarchyData : IHierarchyData, INavigateUIData
{
    private DNNNode m_objNode;

    /// <Summary>Returns Node description</Summary>
    public virtual string Description
    {
        get
        {
            return this.GetSafeValue( this.m_objNode.ToolTip, "" );
        }
    }

    /// <Summary>
    /// Indicates whether the hierarchical data node that the IHierarchyData object represents has any child nodes.
    /// </Summary>
    public virtual bool HasChildren
    {
        get
        {
            return this.m_objNode.HasNodes;
        }
    }

    /// <Summary>Returns nodes image</Summary>
    public virtual string ImageUrl
    {
        get
        {
            if (m_objNode.Image.Length == 0 || m_objNode.Image.StartsWith("/"))
            {
                return m_objNode.Image;
            }
            else
            {
                return PortalController.GetCurrentPortalSettings().HomeDirectory + m_objNode.Image;
            }
        }
    }

    /// <Summary>
    /// Gets the hierarchical data node that the IHierarchyData object represents.
    /// </Summary>
    public virtual object Item
    {
        get
        {
            return this.m_objNode;
        }
    }

    /// <Summary>Returns node name</Summary>
    public virtual string Name
    {
        get
        {
            return this.GetSafeValue( this.m_objNode.Text, "" );
        }
    }

    /// <Summary>Returns node navigation url</Summary>
    public virtual string NavigateUrl
    {
        get
        {
            return this.GetSafeValue( this.m_objNode.NavigateURL, "" );
        }
    }

    /// <Summary>Gets the hierarchical path of the node.</Summary>
    public virtual string Path
    {
        get
        {
            return this.GetValuePath( this.m_objNode );
        }
    }

    /// <Summary>
    /// Gets the name of the type of Object contained in the Item property.
    /// </Summary>
    public virtual string Type
    {
        get
        {
            return "NavDataPageHierarchyData";
        }
    }

    /// <Summary>Returns value path of node</Summary>
    public virtual string Value
    {
        get
        {
            return this.GetValuePath( this.m_objNode );
        }
    }

    public NavDataPageHierarchyData( DNNNode obj )
    {
        this.m_objNode = null;
        this.m_objNode = obj;
    }

    /// <Summary>
    /// Gets an enumeration object that represents all the child nodes of the current hierarchical node.
    /// </Summary>
    public virtual IHierarchicalEnumerable GetChildren()
    {
        NavDataPageHierarchicalEnumerable objNodes = new NavDataPageHierarchicalEnumerable();

        if (m_objNode != null)
        {
            DNNNode objNode;
            foreach (DNNNode tempLoopVar_objNode in m_objNode.DNNNodes)
            {
                objNode = tempLoopVar_objNode;
                objNodes.Add(new NavDataPageHierarchyData(objNode));
            }
        }
        return objNodes;
    }

    /// <Summary>
    /// Gets an enumeration object that represents the parent node of the current hierarchical node.
    /// </Summary>
    public virtual IHierarchyData GetParent()
    {
        if( this.m_objNode == null )
        {
            return null;
        }
        else
        {
            return new NavDataPageHierarchyData( this.m_objNode.ParentNode );
        }
    }

    /// <Summary>
    /// Helper function to handle cases where property is null (Nothing)
    /// </Summary>
    /// <Param name="Value">Value to evaluate for null</Param>
    /// <Param name="Def">If null, return this default</Param>
    private string GetSafeValue( string Value, string Def )
    {
        if( Value == null )
        {
            return Def;
        }
        else
        {
            return Value;
        }
    }

    /// <Summary>
    /// Computes valuepath necessary for ASP.NET controls to guarantee uniqueness
    /// </Summary>
    /// <Returns>ValuePath</Returns>
    private string GetValuePath( DNNNode objNode )
    {
        DNNNode objParent = objNode.ParentNode;
        string strPath = GetSafeValue(objNode.Key, "");
        do
        {
            if (objParent == null || objParent.Level == -1)
            {
                break;
            }
            strPath = GetSafeValue(objParent.Key, "") + "\\" + strPath;
            objParent = objParent.ParentNode;
        }
        while (true);
        return strPath;
    }

    public override string ToString()
    {
        return this.m_objNode.Text;
    }
}