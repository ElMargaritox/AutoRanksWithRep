using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AutoRangos
{
    public class Utilidad
    {
        public static Utilidad Instance;

        public static void GenerarReputacion(string reputacion_new, XmlTextWriter writer)
        {
            writer.WriteStartElement("ReputacionNew");
            writer.WriteString(reputacion_new);
            writer.WriteEndElement();
        }

        public static void LeerReputacion(UnturnedPlayer target)
        {
            string currentPath = System.IO.Directory.GetCurrentDirectory();
            string filePath = currentPath + "\\Plugins\\AutoRangos\\Datos\\";
            string targetStatFile = filePath + target.CSteamID + ".xml";

            XmlDocument targetDoc = new XmlDocument();
            targetDoc.Load(targetStatFile);
            XmlElement targetPvPRoot = targetDoc.DocumentElement;
            XmlNodeList targetPvPNodes = targetPvPRoot.SelectNodes("/Datos");

            foreach (XmlNode node in targetPvPNodes)
            {
                int reputacion_new = int.Parse(node["ReputacionNew"].InnerText);
                int reputacion_old = int.Parse(node["ReputacionOld"].InnerText);
                UnturnedChat.Say(target, target.CharacterName + "Reputacion Guardada:" + reputacion_new + "Reputacion Vieja: " + reputacion_old);
            }
        }
    }


}
