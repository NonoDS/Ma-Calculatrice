using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;

namespace WpfCalc.Properties
{
    public partial class ConfigSection : ConfigurationSection 
    {

        public static void Main(string[]args)
        {
            // Get current configuration file.
            System.Configuration.Configuration config =
                ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.None);

            // Get the MyUrls section.
            ConfigSection myConfigurationSection =
                config.GetSection("userSettings/mySection") as ConfigSection;
            foreach (string elt in config.Sections.Keys)
            {
                Console.Out.WriteLine(elt);
            }
            Console.ReadLine();

            //if (myConfigurationSection == null)
            //    Console.WriteLine("Failed to load ConfigurationSection.");
            //else
            //{
            //    Console.WriteLine("The application collection of app.config:");
            //    for (int i = 0; i < myConfigurationSection.Applications.Count; i++)
            //    {
            //        Console.WriteLine("  Name={0} URL={1} Port={2}",
            //            myConfigurationSection.Applications[i].Description,
            //            myConfigurationSection.Applications[i].Arguments,
            //            myConfigurationSection.Applications[i].Shortcut);
            //    }
            //}
            //Console.ReadLine();

        }

        [ConfigurationProperty("Applications", IsDefaultCollection = false)]
        public ApplicationCollection Applications
        {
            get
            {
                ApplicationCollection urlsCollection = (ApplicationCollection)base["Applications"];
                return urlsCollection;
            }
        }

        public class ApplicationCollection : ConfigurationElementCollection
        {

            public override ConfigurationElementCollectionType CollectionType
            {
                get
                {
                    return ConfigurationElementCollectionType.AddRemoveClearMap;
                }
            }

            protected override ConfigurationElement CreateNewElement()
            {
                return new ApplicationElement();
            }

            public ApplicationElement this[int index]
            {
                get
                {
                    return (ApplicationElement)BaseGet(index);
                }
                set
                {
                    if (BaseGet(index) != null)
                    {
                        BaseRemoveAt(index);
                    }
                    BaseAdd(index, value);
                }
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((ApplicationElement)element).Shortcut;
            }

            protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
            {
                XmlSerializer s = new XmlSerializer();

            }
        }

        public class ApplicationElement : ConfigurationElement
        {
            [ConfigurationProperty("Description", IsRequired = false, IsKey = false)]
            public string Description
            {
                get 
                {
                    return (string)this["Description"];
                }
            }

            [ConfigurationProperty("Shortcut", IsRequired = true, IsKey = true)]
            public string Shortcut
            {
                get 
                {
                    return (string)this["Shortcut"];
                }
            }

            [ConfigurationProperty("Arguments", IsRequired = true, IsKey = false)]
            public string Arguments
            {
                get
                {
                    return (string)this["Arguments"];
                }
            }
        }
    }
}

   //<!--<WpfCalc.Properties.ApplicationsConfiguration>
   //   <Applications>
   //     <add>
   //       <Description>Une description</Description>
   //       <Shortcut>Un raccourci</Shortcut>
   //       <Arguments>Les arguments</Arguments>
   //     </add>
   //     <add>
   //       <Description>Une description 2</Description>
   //       <Shortcut>Un raccourci 2</Shortcut>
   //       <Arguments>Les arguments 2</Arguments>
   //     </add>
   //   </Applications>
   // </WpfCalc.Properties.ApplicationsConfiguration>-->
