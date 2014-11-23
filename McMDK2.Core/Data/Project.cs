using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using McMDK2.Core.Extensions;
using McMDK2.Core.Migrations;
using Newtonsoft.Json;


namespace McMDK2.Core.Data
{
    public class Project : ICloneable, IMigratable
    {
        public Project()
        {
            this.UserProperties = new Dictionary<string, object>();
            this.ProjectSettings = new Dictionary<string, object>();
            this.Items = new ObservableCollection<ProjectItem>();
            this.Version = Define.ProjectXmlVersion;
        }

        public string Name { set; get; }

        public string Id { set; get; }

        public string Path { set; get; }

        /// <summary>
        /// Version can use 1.0.0 only currently available.
        /// </summary>
        [XmlIgnore]
        public string Version { set; get; }

        [XmlIgnore]
        public Dictionary<string, object> UserProperties { private set; get; }

        [XmlIgnore]
        public Dictionary<string, object> ProjectSettings { private set; get; }

        [XmlIgnore]
        [JsonIgnore]
        public ObservableCollection<ProjectItem> Items { set; get; }

        public void Save()
        {
            using (var sw = new StreamWriter(System.IO.Path.Combine(this.Path, this.Name + ".mmproj")))
            {
                var xws = new XmlWriterSettings();
                xws.Encoding = Encoding.UTF8;
                xws.Indent = true;
                xws.IndentChars = "  ";

                using (var xw = XmlWriter.Create(sw, xws))
                {
                    xw.WriteStartElement("Project");
                    xw.WriteStartElement("Items");

                    foreach (var item in this.Items)
                    {
                        RecursiveWrite(item, xw);
                    }

                    xw.WriteEndElement();
                    xw.WriteEndElement();
                }
            }
            var json = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            using (var sw = new StreamWriter(System.IO.Path.Combine(this.Path, "project.mdk")))
            {
                sw.WriteLine(json);
            }
        }

        private void RecursiveWrite(ProjectItem item, XmlWriter xw)
        {
            if (item.Children.Count == 0)
            {
                xw.WriteStartElement("Content");
                xw.WriteAttributeString("Include", item.FilePath.Replace(this.Path + "\\", ""));
                xw.WriteAttributeString("Id", item.Id);
                xw.WriteEndElement();
                return;
            }
            foreach (var innerItem in item.Children)
            {
                RecursiveWrite(innerItem, xw);
            }
        }

        public object Clone()
        {
            var project = new Project();
            project.Id = this.Id;
            project.Items = this.Items.Clone();
            project.Name = this.Name;
            project.Path = this.Path;
            project.ProjectSettings = new Dictionary<string, object>(this.ProjectSettings);
            project.UserProperties = new Dictionary<string, object>(this.UserProperties);
            project.Version = this.Version;
            return project;
        }

        public void Migrate()
        {

        }
    }
}
