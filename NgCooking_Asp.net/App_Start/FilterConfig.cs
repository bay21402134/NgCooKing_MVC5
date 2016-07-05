using System.Web;
using System.Web.Mvc;

namespace NgCooking_Asp.net
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters( GlobalFilterCollection filters )
        {
            filters.Add( new HandleErrorAttribute() );
        }
    }
}
