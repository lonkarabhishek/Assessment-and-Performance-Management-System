using System.Web;
using System.Web.Mvc;

namespace Mini_Tekstac_Question_Paper_Generation
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

    }
}
