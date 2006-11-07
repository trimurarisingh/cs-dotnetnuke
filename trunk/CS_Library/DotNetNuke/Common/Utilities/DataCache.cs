#region DotNetNuke License
// DotNetNukeŽ - http://www.dotnetnuke.com
// Copyright (c) 2002-2006
// by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Collections;
using System.Web.Caching;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Cache;

namespace DotNetNuke.Common.Utilities
{
    public class DataCache
    {
        public static bool CachePersistenceEnabled
        {
            get
            {
                if( Config.GetSetting( "EnableCachePersistence" ) != null )
                {
                    if( Config.GetSetting( "EnableCachePersistence" ) == "true" )
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public static object GetCache( string CacheKey )
        {
            return CachingProvider.Instance().GetItem( CacheKey );
        }

        public static object GetPersistentCacheItem( string CacheKey, Type objType )
        {
            return CachingProvider.Instance().GetPersistentCacheItem( CacheKey, objType );
        }

        [Obsolete( "This method is obsolete. Use the new specific methods: ClearHostCache, ClearPortalCache, ClearTabCache." )]
        public static void ClearCoreCache( CoreCacheType Type, int ID, bool Cascade )
        {
            switch( Type )
            {
                case CoreCacheType.Host:

                    ClearHostCache( Cascade );
                    break;
                case CoreCacheType.Portal:

                    ClearPortalCache( ID, Cascade );
                    break;
                case CoreCacheType.Tab:

                    TabInfo objTab;
                    TabController objTabs = new TabController();
                    objTab = objTabs.GetTab( ID );
                    if( objTab != null )
                    {
                        ClearTabCache( ID, objTab.PortalID );
                    }
                    break;
            }
        }

        public static void ClearHostCache( bool Cascade )
        {
            RemoveCache( "GetHostSettings" );

            RemoveCache( "GetPortalByAlias" );

            RemoveCache( "CSS" );

            RemoveCache( "Folders:-1" );

            if( Cascade )
            {
                PortalController objPortals = new PortalController();
                PortalInfo objPortal;
                ArrayList arrPortals = objPortals.GetPortals();

                int intIndex;
                for( intIndex = 0; intIndex <= arrPortals.Count - 1; intIndex++ )
                {
                    objPortal = (PortalInfo)arrPortals[intIndex];
                    ClearPortalCache( objPortal.PortalID, Cascade );
                }
            }
        }

        public static void ClearPortalCache( int PortalId, bool Cascade )
        {
            ArrayList arrTabs = (ArrayList)GetCache( "GetTabs" + PortalId.ToString() );

            RemovePersistentCacheItem( "GetPortalSettings" + PortalId.ToString() );

            RemoveCache( "GetTabs" + PortalId.ToString() );

            RemoveCache( "Folders:" + PortalId.ToString() );

            if( Cascade )
            {
                TabInfo objTab;

                if( arrTabs == null )
                {
                    TabController objTabs = new TabController();
                    arrTabs = objTabs.GetTabs( PortalId );
                }

                int intIndex;
                for( intIndex = 0; intIndex <= arrTabs.Count - 1; intIndex++ )
                {
                    objTab = (TabInfo)arrTabs[intIndex];
                    ClearTabCache( objTab.TabID );
                }

                RemoveCache( "GetTabPermissionsByPortal" + PortalId.ToString() );
            }
        }

        public static void ClearTabCache( int TabId, int PortalId )
        {
            ClearTabCache( TabId );

            RemoveCache( "GetTabPermissionsByPortal" + PortalId.ToString() );
        }

        private static void ClearTabCache( int TabId )
        {
            RemoveCache( "GetTab" + TabId.ToString() );

            RemoveCache( "GetPortalTabModules" + TabId.ToString() );
        }

        public static void RemoveCache( string CacheKey )
        {
            CachingProvider.Instance().Remove( CacheKey );
        }

        public static void RemovePersistentCacheItem( string CacheKey )
        {
            CachingProvider.Instance().RemovePersistentCacheItem( CacheKey );
        }

        public static void SetCache( string CacheKey, object objObject )
        {
            SetCache( CacheKey, objObject, false );
        }

        public static void SetCache( string CacheKey, object objObject, bool PersistAppRestart )
        {
            CachingProvider.Instance().Insert( CacheKey, objObject, PersistAppRestart );
        }

        public static void SetCache( string CacheKey, object objObject, CacheDependency objDependency )
        {
            SetCache( CacheKey, objObject, objDependency, false );
        }

        public static void SetCache( string CacheKey, object objObject, CacheDependency objDependency, bool PersistAppRestart )
        {
            CachingProvider.Instance().Insert( CacheKey, objObject, objDependency, PersistAppRestart );
        }

        public static void SetCache( string CacheKey, object objObject, CacheDependency objDependency, DateTime AbsoluteExpiration, TimeSpan SlidingExpiration )
        {
            SetCache( CacheKey, objObject, objDependency, AbsoluteExpiration, SlidingExpiration, false );
        }

        public static void SetCache( string CacheKey, object objObject, CacheDependency objDependency, DateTime AbsoluteExpiration, TimeSpan SlidingExpiration, bool PersistAppRestart )
        {
            CachingProvider.Instance().Insert( CacheKey, objObject, objDependency, AbsoluteExpiration, SlidingExpiration, PersistAppRestart );
        }

        public static void SetCache( string CacheKey, object objObject, TimeSpan SlidingExpiration )
        {
            SetCache( CacheKey, objObject, SlidingExpiration, false );
        }

        public static void SetCache( string CacheKey, object objObject, TimeSpan SlidingExpiration, bool PersistAppRestart )
        {
            CachingProvider.Instance().Insert( CacheKey, objObject, null, Cache.NoAbsoluteExpiration, SlidingExpiration, PersistAppRestart );
        }

        public static void SetCache( string CacheKey, object objObject, CacheDependency objDependency, DateTime AbsoluteExpiration, TimeSpan SlidingExpiration, CacheItemPriority Priority, CacheItemRemovedCallback OnRemoveCallback )
        {
            SetCache( CacheKey, objObject, objDependency, AbsoluteExpiration, SlidingExpiration, Priority, OnRemoveCallback, false );
        }

        public static void SetCache( string CacheKey, object objObject, CacheDependency objDependency, DateTime AbsoluteExpiration, TimeSpan SlidingExpiration, CacheItemPriority Priority, CacheItemRemovedCallback OnRemoveCallback, bool PersistAppRestart )
        {
            CachingProvider.Instance().Insert( CacheKey, objObject, null, Cache.NoAbsoluteExpiration, SlidingExpiration, PersistAppRestart );
        }

        public static void SetCache( string CacheKey, object objObject, DateTime AbsoluteExpiration )
        {
            SetCache( CacheKey, objObject, AbsoluteExpiration, false );
        }

        public static void SetCache( string CacheKey, object objObject, DateTime AbsoluteExpiration, bool PersistAppRestart )
        {
            CachingProvider.Instance().Insert( CacheKey, objObject, null, AbsoluteExpiration, Cache.NoSlidingExpiration, PersistAppRestart );
        }
    }
}