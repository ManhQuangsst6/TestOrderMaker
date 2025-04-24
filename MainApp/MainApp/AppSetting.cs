using System.Configuration;

namespace TestOrderMaker
{
    public static class AppSetting
    {
        public static string ProjectFolder = ConfigurationManager.AppSettings["ProjectFolder"];
        public static string ProjectFileName = ConfigurationManager.AppSettings["ProjectFileName"];
    }
}