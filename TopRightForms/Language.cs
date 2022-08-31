using System.Collections.Generic;

namespace topright
{
    public class Language
    {
        //Будет время добавлю все языки
        public static string LastWindowName;
        public static Dictionary<int, string> WindowNames = new Dictionary<int, string>()
        {
            [0] = "New notification",
            [1] = "Новое уведомление",
            [2] = "Нове сповіщення",
            [3] = "新通知", //not working
            [4] = "Yeni bildirim",
            [5] = "إعلام جديد", //not working
            [6] = ""
        };
        public static void LanguageSelect()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            if (culture.ToString() == "ru-RU")
                LastWindowName = WindowNames[1];
            else if (culture.ToString() == "uk-UA")
                LastWindowName = WindowNames[2];
            else if (culture.ToString() == "tr-TR")
                LastWindowName = WindowNames[4];

            else if (culture.ToString() == "zh-HK")     //not working
                LastWindowName = WindowNames[3];
            /*else if (culture.ToString() == "am-ET")
                LastWindowName = WindowNames[0];*/
            else if (culture.ToString() == "ar-DZ")    //not working
                LastWindowName = WindowNames[5];
            else
                LastWindowName = WindowNames[0];
            //MessageBox.Show(culture.ToString());
        }
    }
}
