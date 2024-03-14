namespace ModemChorus
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();


            CALL_NUMBER = System.Configuration.ConfigurationManager.AppSettings["call-number"];

            Application.Run(new frmModemControl());
        }

        public static String CALL_NUMBER;
    }
}