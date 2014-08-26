using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Data.Project.Internal
{
    public enum ItemCategory
    {
        // ./Resources/Content_6017.png
        Mod,

        // ./Resources/Image_24x.png
        Image,

        // ./Resources/Soundfile_461.png
        Sound,

        // ./Resources/Textfile_818_16x.png
        Text,

        Folder,
    }

    public class ItemExtensions
    {
        private static List<string> Mod = new List<string>();
        private static List<string> Image = new List<string>();
        private static List<string> Sound = new List<string>();
        private static List<string> Text = new List<string>();

        public static IEnumerable<string> ModExtensions
        {
            get { return Mod.AsReadOnly(); }
        }

        public static IEnumerable<string> ImageExtensions
        {
            get { return Image.AsReadOnly(); }
        }

        public static IEnumerable<string> SoundExtensions
        {
            get { return Sound.AsReadOnly(); }
        }

        public static IEnumerable<string> TextExtensions
        {
            get { return Text.AsReadOnly(); }
        }

        public static void RegisterExtension(ItemCategory category, string extension)
        {
            switch (category)
            {
                case ItemCategory.Mod:
                    Mod.Add(extension);
                    break;

                case ItemCategory.Image:
                    Image.Add(extension);
                    break;

                case ItemCategory.Sound:
                    Sound.Add(extension);
                    break;

                case ItemCategory.Text:
                    Text.Add(extension);
                    break;
            }
        }
    }
}
