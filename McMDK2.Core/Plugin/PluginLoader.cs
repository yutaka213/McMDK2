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
using System.Windows.Media;

using McMDK2.Plugin;
using McMDK2.UI;
using McMDK2.UI.Controls;
using McMDK2.Core.Converter;
using McMDK2.Core.Plugin.Internal;

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
            if (asmPlugins.mods != null)
            {
                foreach (var p in asmPlugins.mods)
                {
                    ModItemManager.Register(p);
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

            [ImportMany()]
            public List<IMod> mods = null;

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
                    Define.GetLogger().Error("Cannot load plugin", e);
                }
            }
        }

        #endregion

        #region Xml Base Plugin
        private class XmlBasePluginLoader
        {
            public List<IPlugin> plugins = new List<IPlugin>();
            public List<IMod> mods = new List<IMod>();
            public List<ITemplate> templates = new List<ITemplate>();

            public XmlBasePluginLoader()
            {
                string[] xmls = FileController.GetLists(Define.PluginDirectory, true);
                foreach (var xml in xmls)
                {
                    // McMDK Basic Template Folder.
                    if (xml.EndsWith("template"))
                    {
                        continue;
                    }
                    if (!FileController.Exists(xml + "\\plugin.xml"))
                    {
                        continue;
                    }


                    // IPlugin.cs
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
                                XmlVersion = b.Element("XmlVersion") == null ? "1.0" : b.Element("XmlVersion").Value,
                                Type = b.Element("PluginType") == null ? "Mod" : b.Element("PluginType").Value
                            };
                    XmlBasePlugin xmlPlugin = null;
                    foreach (var item in a)
                    {
                        xmlPlugin = item;
                    }

                    // IMod.cs
                    #region MOD
                    if (xmlPlugin.Type == "Mod")
                    {
                        // ui.xml
                        SerializeXML(xmlPlugin, xml);

                        // mod.xml
                        var c = from d in XElement.Load(xml + "\\mod.xml").Elements()
                                select new XmlBaseMod
                                {
                                    Name = d.Element("Name") == null ? xmlPlugin.Name : d.Element("Name").Value,
                                    Version = d.Element("Version") == null ? xmlPlugin.Version : d.Element("Version").Value,
                                    Id = d.Element("Id") == null ? xmlPlugin.Id : d.Element("Id").Value,
                                    SourceFile = d.Element("SourceFile").Value,
                                    XmlVersion = d.Element("XmlVersion") == null ? "1.0" : d.Element("XmlVersion").Value
                                };
                        XmlBaseMod xmlMod = null;
                        foreach (var item in c)
                        {
                            xmlMod = item;
                        }

                        XmlBaseModView view = new XmlBaseModView(xmlPlugin.Controls);
                        xmlMod.View = view;
                        mods.Add(xmlMod);
                    }
                    #endregion

                    // ITemplate.cs
                    if (xmlPlugin.Type == "Template")
                    {
                        // template.xml
                        var c = from d in XElement.Load(xml + "\\template.xml").Elements()
                                select new XmlBaseTemplate
                                {
                                    Name = d.Element("Name") == null ? xmlPlugin.Name : d.Element("Name").Value,
                                    Id = d.Element("Id") == null ? xmlPlugin.Id : d.Element("Id").Value,
                                    IconPath = d.Element("IconPath").Value,
                                    Description = d.Element("Description").Value,
                                    TemplateFile = d.Element("TemplateFile").Value,
                                    XmlVersion = d.Element("XmlVersion") == null ? "1.0" : d.Element("XmlVersion").Value
                                };
                        XmlBaseTemplate xmlTemplate = null;
                        foreach (var item in c)
                        {
                            xmlTemplate = item;
                        }

                        templates.Add(xmlTemplate);
                    }
                    plugins.Add(xmlPlugin);
                }
            }

            private void SerializeXML(XmlBasePlugin plugin, string dir)
            {
                if (!FileController.Exists(dir + "//ui.xml"))
                {
                    return;
                }
                try
                {
                    var document = new XmlDocument();
                    document.Load(dir + "\\ui.xml");

                    XmlNode root = document.DocumentElement;
                    XmlNode node = root.ChildNodes[0];

                    var control = new UIControl(StringToObjectConverter.StringToComponents(((XmlElement)node).Name));

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
                    UIControl control = null;
                    XmlElement element = (XmlElement)node;

                    // Parse TOPLEVEL Contents.
                    switch (node.Name)
                    {
                        case "TextBlock":
                            control = new TextBlockControl
                            {
                                Background = StringToObjectConverter.StringToBrush(element.GetAttribute("Background")),
                                /* FontFamiy */
                                FontSize = StringToObjectConverter.StringTo<double>(element.GetAttribute("FontSize")),
                                FontStretch = StringToObjectConverter.StringToProperty(element.GetAttribute("FontStretch"), typeof(FontStretches)),
                                FontStyle = StringToObjectConverter.StringToProperty(element.GetAttribute("FontStyle"), typeof(FontStyles)),
                                FontWeight = StringToObjectConverter.StringToProperty(element.GetAttribute("FontWeight"), typeof(FontWeights)),
                                Foreground = StringToObjectConverter.StringToBrush(element.GetAttribute("Foreground")),
                                Padding = ParseMargin(element.GetAttribute("Padding")),
                                LineHeight = StringToObjectConverter.StringTo<double>(element.GetAttribute("LineHeight")),
                                Text = element.GetAttribute("Text"),
                                TextAlignment = StringToObjectConverter.StringToEnum(element.GetAttribute("TextAlignment"), typeof(TextAlignment)),
                                TextDecorations = StringToObjectConverter.StringToProperty(element.GetAttribute("TextDecorations"), typeof(TextDecorations)),
                                TextTrimming = StringToObjectConverter.StringToEnum(element.GetAttribute("TextTrimming"), typeof(TextTrimming)),
                                TextWrapping = StringToObjectConverter.StringToEnum(element.GetAttribute("TextWrapping"), typeof(TextWrapping))
                            };
                            break;

                        case "ComboBox":
                            control = new SelectableControl(GuiComponents.ComboBox)
                            {
                                // SelectableControl
                                ItemsSource = element.GetAttribute("ItemsSource"),
                                // EnterableControl
                                IsRequired = StringToObjectConverter.StringTo<bool>(element.GetAttribute("IsRequired")),
                                Default = element.GetAttribute("Default"),
                                // ContentControl
                                Content = element.GetAttribute("Content")
                            };
                            break;

                        case "TextBox":
                        case "CheckBox":
                            control = new EnterableControl(StringToObjectConverter.StringToComponents(node.Name))
                            {
                                // EnterableControl
                                IsRequired = StringToObjectConverter.StringTo<bool>(element.GetAttribute("IsRequired")),
                                Default = element.GetAttribute("Default"),
                                // ContentControl
                                Content = element.GetAttribute("Content")
                            };
                            break;

                        case "GroupBox":
                            control = new GroupBoxControl()
                            {
                                // GroupBoxControl
                                Header = element.GetAttribute("Header"),
                                // ContentControl
                                Content = element.GetAttribute("Content")
                            };
                            break;

                        // There controls has 'Content' property.
                        // Based on System.Windows.Controls.ContentControl
                        case "Image":
                        case "Label":
                            control = new McMDK2.UI.Controls.ContentControl(StringToObjectConverter.StringToComponents(node.Name))
                            {
                                // ContentControl
                                Content = element.GetAttribute("Content")
                            };
                            break;

                        case "Grid":
                        case "WrapPanel":
                        case "StackPanel":
                        case "DockPanel":
                        case "Canvas":
                        case "UniformGrid":
                        case "ScrollViewer":
                            control = new PanelControl(StringToObjectConverter.StringToComponents(node.Name));
                            break;

                        case "Separator":
                            control = new UIControlEx(GuiComponents.Separator);
                            break;

                        default:
                            control = new UIControl(StringToObjectConverter.StringToComponents(node.Name));
                            break;
                    }

                    // If control has McMDK2.UI.Controls.UIControlEx(Based on System.Windows.Controls.Control), Parse these properties.
                    if (control is UIControlEx)
                    {
                        ((UIControlEx)control).Background = StringToObjectConverter.StringToBrush(element.GetAttribute("Background"));
                        ((UIControlEx)control).BorderBrush = StringToObjectConverter.StringToBrush(element.GetAttribute("BorderBrush"));
                        ((UIControlEx)control).BorderThickess = ParseMargin(element.GetAttribute("BorderThickness"));
                        /* FontFamily */
                        ((UIControlEx)control).FontSize = StringToObjectConverter.StringTo<double>(element.GetAttribute("FontSize"));
                        ((UIControlEx)control).FontStretch = StringToObjectConverter.StringToProperty(element.GetAttribute("FontStretch"), typeof(FontStretches));
                        ((UIControlEx)control).FontWeight = StringToObjectConverter.StringToProperty(element.GetAttribute("FontWeight"), typeof(FontWeights));
                        ((UIControlEx)control).Foreground = StringToObjectConverter.StringToBrush(element.GetAttribute("Foreground"));
                        ((UIControlEx)control).Padding = ParseMargin(element.GetAttribute("Padding"));
                    }

                    // All controls has this properties.
                    // Based on System.Windows.UIElement and System.Windows.FrameworkElement.
                    control.IsEnabled = StringToObjectConverter.StringTo<bool>(element.GetAttribute("IsEnabled"), true);
                    control.IsVisible = StringToObjectConverter.StringTo<bool>(element.GetAttribute("IsVisible"), true);
                    control.Opacity = StringToObjectConverter.StringTo<double>(element.GetAttribute("Opacity"));
                    control.Visibility = StringToObjectConverter.StringToEnum(element.GetAttribute("Visibility"), typeof(Visibility));
                    control.Height = StringToObjectConverter.StringTo<double>(element.GetAttribute("Height"));
                    control.HorizontalAlignment = StringToObjectConverter.StringToEnum(element.GetAttribute("HorizontalAlignment"), typeof(HorizontalAlignment));
                    control.Margin = ParseMargin(element.GetAttribute("Margin"));
                    control.Name = element.GetAttribute("Name");
                    control.ToolTip = element.GetAttribute("Tooltip");
                    control.VerticalAlignment = StringToObjectConverter.StringToEnum(element.GetAttribute("VerticalAlignment"), typeof(VerticalAlignment));
                    control.Width = StringToObjectConverter.StringTo<double>(element.GetAttribute("Width"));

                    if (parentControl is PanelControl)
                    {
                        ((PanelControl)parentControl).Children.Add(control);
                    }
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
