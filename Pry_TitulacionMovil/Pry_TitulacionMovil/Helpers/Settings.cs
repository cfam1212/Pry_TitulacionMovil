namespace Pry_TitulacionMovil.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        const int userID = 0;
        static readonly int _intDefault = 0;

        public static int UserLogin
        {
            get
            {
                return AppSettings.GetValueOrDefault(userID.ToString(), _intDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(userID.ToString(), value);
            }
        }
    }
}
