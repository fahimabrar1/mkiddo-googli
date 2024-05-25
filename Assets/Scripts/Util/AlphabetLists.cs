using System.Collections.Generic;

public static class AlphabetLists
{


    // public static Dictionary<string, List<string>> AlphabetTypes = new Dictionary<string, List<string>>
    // {
    //     { "learn_bangla_soroborno", new List<string>
    // {
    //     "\u0985", "\u0986", "\u0987", "\u0988", "\u0989", "\u098A", "\u098B",
    //     "\u098F", "\u0990", "\u0993", "\u0994"
    // } },
    //     { "benjon_borno", new List<string>
    // {
    //     "\u0995", "\u0996", "\u0997", "\u0998", "\u0999", "\u099A", "\u099B",
    //     "\u099C", "\u099D", "\u099E", "\u099F", "\u09A0", "\u09A1", "\u09A2",
    //     "\u09A3", "\u09A4", "\u09A5", "\u09A6", "\u09A7", "\u09A8", "\u09AA",
    //     "\u09AB", "\u09AC", "\u09AD", "\u09AE", "\u09AF", "\u09B0", "\u09B2",
    //     "\u09B6", "\u09B7", "\u09B8", "\u09B9", "\u09DC", "\u09DD", "\u09DF",
    //     "\u09CE", "\u0982", "\u0983", "\u0981"
    // } },
    //     { "bangla_number", new List<string>
    // {
    //     "\u09E6", "\u09E7", "\u09E8", "\u09E9", "\u09EA", "\u09EB", "\u09EC", "\u09ED", "\u09EE", "\u09EF"
    // } },
    //     { "english_capital_letters", new List<string>
    // {
    //     "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
    //     "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
    // } },
    //     { "english_small_letters", new List<string>
    // {
    //     "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
    //     "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
    // } },
    //     { "english_number", new List<string>
    // {
    //     "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
    // } },
    //     { "arabic_letters", new List<string>
    // {
    //     "\u0627", // ا
    //     "\u0628", // ب
    //     "\u062A", // ت
    //     "\u062B", // ث
    //     "\u062C", // ج
    //     "\u062D", // ح
    //     "\u062E", // خ
    //     "\u062F", // د
    //     "\u0630", // ذ
    //     "\u0631", // ر
    //     "\u0632", // ز
    //     "\u0633", // س
    //     "\u0634", // ش
    //     "\u0635", // ص
    //     "\u0636", // ض
    //     "\u0637", // ط
    //     "\u0638", // ظ
    //     "\u0639", // ع
    //     "\u063A", // غ
    //     "\u0641", // ف
    //     "\u0642", // ق
    //     "\u0643", // ك
    //     "\u0644", // ل
    //     "\u0645", // م
    //     "\u0646", // ن
    //     "\u0647", // ه
    //     "\u0648", // و
    //     "\u064A"  // ي
    // } }
    // };


    public static Dictionary<string, List<string>> AlphabetTypes = new Dictionary<string, List<string>>
    {

        { "learn_bangla_soroborno", new List<string>
    {
        "অ", "আ", "ই", "ঈ", "উ", "ঊ", "ঋ", "এ", "ঐ", "ও", "ঔ"
    } },
        { "benjon_borno",      new List<string>
    {
        "ক", "খ", "গ", "ঘ", "ঙ", "চ", "ছ", "জ", "ঝ", "ঞ", "ট", "ঠ", "ড", "ঢ", "ণ",
        "ত", "থ", "দ", "ধ", "ন", "প", "ফ", "ব", "ভ", "ম", "য", "র", "ল", "শ", "ষ", "স",
        "হ", "ড়", "ঢ়", "য়", "ৎ", "ং", "ঃ", "ঁ"
    } },
        { "bangla_number", new List<string>
    {
        "\u09E6", "\u09E7", "\u09E8", "\u09E9", "\u09EA", "\u09EB", "\u09EC", "\u09ED", "\u09EE", "\u09EF"
    } },
        { "english_capital_letters", new List<string>
    {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
        "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
    } },
        { "english_small_letters", new List<string>
    {
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
        "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
    } },
        { "english_number", new List<string>
    {
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
    } },
        { "arabic_letters", new List<string>
    {
        "\u0627", // ا
        "\u0628", // ب
        "\u062A", // ت
        "\u062B", // ث
        "\u062C", // ج
        "\u062D", // ح
        "\u062E", // خ
        "\u062F", // د
        "\u0630", // ذ
        "\u0631", // ر
        "\u0632", // ز
        "\u0633", // س
        "\u0634", // ش
        "\u0635", // ص
        "\u0636", // ض
        "\u0637", // ط
        "\u0638", // ظ
        "\u0639", // ع
        "\u063A", // غ
        "\u0641", // ف
        "\u0642", // ق
        "\u0643", // ك
        "\u0644", // ل
        "\u0645", // م
        "\u0646", // ن
        "\u0647", // ه
        "\u0648", // و
        "\u064A"  // ي
    } }
    };


    public static Dictionary<string, string> AlphabetFontTypes = new Dictionary<string, string>
    {
        { "learn_bangla_soroborno", "bangla"},
        { "benjon_borno", "bangla"},
        { "bangla_number", "bangla"},
        { "english_capital_letters","english"},
        { "english_small_letters", "english"},
        { "english_number", "english"},
        { "arabic_letters", "arabic"}
    };

}
