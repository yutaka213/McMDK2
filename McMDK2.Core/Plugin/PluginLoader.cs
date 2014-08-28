using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

using McMDK2.Plugin;

using McMDK2.Core.Converter;
using McMDK2.Core.Data.Project;
using McMDK2.Core.Plugin;
using McMDK2.Core.Plugin.Internal;
using McMDK2.Core.Plugin.Internal.UI;
using McMDK2.Core.Plugin.Internal.UI.Controls;

namespace McMDK2.Core.Plugin
{
    public class PluginLoader
    {
        public static void Load()
        {
            var asmPlugins = new AssemblyPluginLoader();
            if (asmPlugins.plugins != null)
            {
                foreach (var p in asmPlugins.plugins)
                {
                    PluginManager.Register(p);
                }
            }
            if (asmPlugins.templates != null)
            {
                foreach (var p in asmPlugins.templates)
                {
                    TemplateManager.Register(p);
                }
            }

            var xmlPlugins = new XmlBasePluginLoader();
            if (xmlPlugins.plugins != null)
            {
                foreach (var p in xmlPlugins.plugins)
                {
                    PluginManager.Register(p);
                }
            }
        }

        #region Assembly Base Plugin
        // Reference : https://github.com/fin-alice/Mystique/blob/master/Inscribe/Plugin/PluginLoader.cs
        private class AssemblyPluginLoader
        {
            [ImportMany()]
            public List<IPlugin> plugins = null;

            [ImportMany()]
            public List<ITemplate> templates = null;

            public AssemblyPluginLoader()
            {
                try
                {
                    var catalog = new DirectoryCatalog(Define.PluginDirectory);
                    var container = new CompositionContainer(catalog);
                    container.ComposeParts(this);
                }
                catch (Exception e)
                {
                    Define.GetLogger().Error("Cannot load plugin.", e);
                }
            }
        }

        #endregion

        #region Xml Base Plugin
        private class XmlBasePluginLoader
        {
            public List<IPlugin> plugins = new List<IPlugin>();

            public XmlBasePluginLoader()
            {
                string[] xmls = FileController.GetLists(Define.PluginDirectory, true);
                foreach (var xml in xmls)
                {
                    if (xml.EndsWith("template"))
                    {
                        continue;
                    }
                    if (!FileController.Exists(xml + "\\plugin.xml"))
                    {
                        continue;
                    }

                    var a = from b in XElement.Load(xml + "\\plugin.xml").Elements()
                            select new XmlBasePlugin
                            {
                                Name = b.Element("Name").Value,
                                Version = b.Element("Version").Value,
                                Author = b.Element("Author").Value,
                                Id = b.Element("Id").Value,
                                Dependents = b.Element("Dependents").Value,
                                IconPath = b.Element("IconPath").Value,
                                Description = b.Element("Description").Value,
                                XmlVersion = b.Element("XmlVersion").Value
                            };
                    XmlBasePlugin xmlPlugin = null;
                    foreach (var item in a)
                    {
                        xmlPlugin = item;
                    }

                    SerializeXML(xmlPlugin, xml);
                    plugins.Add(xmlPlugin);
                }
            }

            private void SerializeXML(XmlBasePlugin plugin, string dir)
            {
                if (!FileController.Exists(dir + "\\ui.xml"))
                {
                    return;
                }

                try
                {
                    var document = new XmlDocument();
                    document.Load(dir + "\\ui.xml");

                    XmlNode root = document.DocumentElement;
                    XmlNode node = root.ChildNodes[0];

                    var control = new UIControl();
                    control.Component = StringToObjectConverter.StringToComponents(((XmlElement)node).Name);

                    RecursiveSerializeXML(node, control, dir);

                    plugin.Controls.Add(control);
                }
                catch (Exception e)
                {
                    Define.GetLogger().Error("UI Load failed", e);
                }
            }

            private void RecursiveSerializeXML(XmlNode parentNode, UIControl parentControl, string dir)
            {
                foreach (XmlNode node in parentNode.ChildNodes)
                {
                    if (node is XmlComment)
                    {
                        continue;
                    }
                    UIControl control = new UIControl();

                    switch (node.Name)
                    {
                        case "TextBlock":
                        case "TextBox":
                        case "Label":
                        case "CheckBox":
                        case "GroupBox":
                            var textcontrol = new TextControl();
                            textcontrol.FontSize = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("FontSize"));
                            textcontrol.FontStretch = (FontStretch)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("FontStretch"), typeof(FontStretches), FontStretches.Normal);
                            textcontrol.FontStyle = (FontStyle)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("FontStyle"), typeof(FontStyles), FontStyles.Normal);
                            textcontrol.FontWeight = (FontWeight)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("FontWeight"), typeof(FontWeights), FontWeights.Normal);
                            textcontrol.Text = (((XmlElement)node).GetAttribute("Text"));
                            textcontrol.TextAlignment = (TextAlignment?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("TextAlignment"), typeof(TextAlignment));
                            textcontrol.TextDecoration = (TextDecoration)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("TextDecoration"), typeof(TextDecorations), null);
                            textcontrol.TextWrapping = (TextWrapping?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("TextWrapping"), typeof(TextWrapping));
                            control = textcontrol;
                            break;

                        case "ComboBox":
                            var selectionControl = new SelectionControl();
                            selectionControl.FontSize = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("FontSize"));
                            selectionControl.FontStretch = (FontStretch)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("FontStretch"), typeof(FontStretches), FontStretches.Normal);
                            selectionControl.FontStyle = (FontStyle)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("FontStyle"), typeof(FontStyles), FontStyles.Normal);
                            selectionControl.FontWeight = (FontWeight)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("FontWeight"), typeof(FontWeights), FontWeights.Normal);
                            selectionControl.Text = (((XmlElement)node).GetAttribute("Text"));
                            selectionControl.TextAlignment = (TextAlignment?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("TextAlignment"), typeof(TextAlignment));
                            selectionControl.TextDecoration = (TextDecoration)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("TextDecoration"), typeof(TextDecorations), null);
                            selectionControl.TextWrapping = (TextWrapping?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("TextWrapping"), typeof(TextWrapping));
                            selectionControl.ItemsSource = (((XmlElement)node).GetAttribute("ItemsSource"));
                            control = selectionControl;

                            break;

                        case "Image":
                            var imagecontrol = new ImageControl();
                            imagecontrol.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(dir + ((XmlElement)node).GetAttribute("ImageSource")));
                            control = imagecontrol;
                            break;

                        default:
                            control = new UIControl();
                            break;
                    }
                    control.Background = StringToObjectConverter.StringToBrush(((XmlElement)node).GetAttribute("Background"), new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = (byte)0xFF, R = (byte)0xFF, G = (byte)0xFF, B = (byte)0xFF }));
                    control.Component = StringToObjectConverter.StringToComponents(((XmlElement)node).Name);
                    control.Foreground = StringToObjectConverter.StringToBrush(((XmlElement)node).GetAttribute("Foreground"), new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = (byte)0xFF, R = (byte)0x00, G = (byte)0x00, B = (byte)0x00 }));
                    control.Height = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("Height"));
                    control.HorizontalAlignment = (HorizontalAlignment?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("HorizontalAlignment"), typeof(HorizontalAlignment));
                    control.IsEnabled = StringToObjectConverter.StringTo<bool>(((XmlElement)node).GetAttribute("IsEnabled"), true);
                    control.IsVisible = StringToObjectConverter.StringTo<bool>(((XmlElement)node).GetAttribute("IsVisible"));
                    control.Margin = ParseMargin(((XmlElement)node).GetAttribute("Margin"));
                    control.Name = (((XmlElement)node).GetAttribute("Name"));
                    control.Opacity = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("Opacity"));
                    control.ToolTip = (((XmlElement)node).GetAttribute("ToolTip"));
                    control.VerticalAlignment = (VerticalAlignment?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("VerticalAlignment"), typeof(VerticalAlignment));
                    control.Visibility = (Visibility?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("Visibility"), typeof(Visibility));
                    control.Width = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("Width"));
                    control.IsRequired = StringToObjectConverter.StringTo<bool>(((XmlElement)node).GetAttribute("IsRequired"), false);
                    parentControl.Children.Add(control);
                    RecursiveSerializeXML(node, control, dir);
                }
            }

            private static Thickness? ParseMargin(string margin)
            {
                if (String.IsNullOrEmpty(margin))
                {
                    return null;
                }
                string[] s = margin.Split(',');
                if (s.Length == 1)
                {
                    return new Thickness(int.Parse(s[0]));
                }
                else if (s.Length == 4)
                {
                    return new Thickness(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3]));
                }
                return null;
            }
        }

        #endregion
    }
}
