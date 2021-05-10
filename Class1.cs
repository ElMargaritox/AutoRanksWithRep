using System;
using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Plugins;
using SDG;
using SDG.Unturned;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.API.Serialisation;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using Rocket.Core.Utils;

namespace AutoRangos
{
    public class Plugin : RocketPlugin<Config>
    {
        public List<Rangos> ConfigGrupos;
        public static Plugin Instance;
        public static string funcion;

        protected override void Load()
        {

            
            Instance = this;
            ConfigGrupos = Configuration.Instance.Rangos;
            

            Logger.LogWarning("------------------------------------");
            Logger.LogWarning("              RANGOS                ");
            Logger.LogWarning("------------------------------------");
            foreach (Rangos item in Configuration.Instance.Rangos)
            {
                Logger.Log("Rango: " + item.Rango + "Reputacion: " + item.Reputacion);

                RocketPermissionsGroup Group = R.Permissions.GetGroup(item.Rango);
                if(Group == null)
                {
                    Logger.LogError("No Existe El Grupo " + item.Rango + " En Permissions");
                    Logger.LogWarning("Creando Grupos...");
                    Logger.Log("Creando..." + item.Rango); Thread.Sleep(500);
                    RocketPermissionsGroup Ola = new RocketPermissionsGroup(item.Rango, "EnvyHosting", "", new List<string>(), new List<Permission>(), "white"); Ola.Prefix = ""; Ola.Suffix = "";
                    switch (R.Permissions.AddGroup(Ola))
                    {
                        case Rocket.API.RocketPermissionsProviderResult.Success:
                            Logger.Log("Se Ha Creado El Grupo " + item.Rango + " Correctamente");
                            Permission essentialperm = new Permission("essentials.kit." + item.Rango);
                            Permission aviperm = new Permission("avi.kit." + item.Rango);
                            Ola.Permissions.Add(essentialperm);
                            Ola.Permissions.Add(aviperm);
                            R.Permissions.SaveGroup(Ola);
                            break;
                        case Rocket.API.RocketPermissionsProviderResult.UnspecifiedError:
                            Logger.LogError("Error Grave"); Instance.Unload();
                            break;
                        case Rocket.API.RocketPermissionsProviderResult.DuplicateEntry:
                            Logger.Log("El Grupo " + item.Rango + " Ya Fue Creado");
                            break;
 
                    }
                }
            }

            Logger.LogWarning("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            Logger.Log("Plugin Creado Por @Margarita#8172", ConsoleColor.Green);
            Logger.Log($"Version Del Plugin: {Assembly.GetName().Version}", ConsoleColor.Cyan);
            Logger.Log($"Nombre Del Plugin: {Assembly.GetName().Name}", ConsoleColor.Cyan);
            Logger.LogWarning("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");Thread.Sleep(500);




                Logger.Log("El Plugin Ha Cargado Correctamente", ConsoleColor.Yellow);
                UnturnedPlayerEvents.OnPlayerUpdateGesture += OnPlayerUpdateGesture;
                U.Events.OnPlayerConnected += OnPlayerConnected;
                U.Events.OnPlayerDisconnected += OnPlayerDisconnected;

                try
                {
                    System.IO.Directory.CreateDirectory(Path.Combine(Directory, "Datos/"));
                }
                catch (Exception x)
                {

                    Logger.LogError("ERROR RARO:" + x);
                }
               

            try
            {
                switch (Configuration.Instance.codigo_animacion)
                {
                    case 1:
                        funcion = "Levantar Las Manos";
                        break;
                    case 2:
                        funcion = "Golpearse La Cara";
                        break;
                    case 3:
                        funcion = "Hola";
                        break;
                    case 4:
                        funcion = "Saludar";
                        break;
                }
            }
            catch (Exception)
            {

                Logger.LogError("Error Encontrado En _CODIGO_ANIMACION_ [1|2|3|4] unicamente.");
                Instance.Unload();
                base.Unload();
            }



        }



        public void apagadoforzoso()
        {
            Instance.Unload();
            Instance.UnloadPlugin();
            base.Unload();
            base.UnloadPlugin();
        }
        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            string currentPath = Path.Combine(Plugin.Instance.Directory, "Datos/");
            string playerStatFile = currentPath + player.CSteamID + ".xml";


            XmlDocument playerDoc = new XmlDocument();
            playerDoc.Load(playerStatFile);
            XmlElement playerPvPRoot = playerDoc.DocumentElement;


            foreach (XmlNode node in playerPvPRoot)
            {

                try
                {
                    node["ReputacionNew"].InnerText = player.Reputation + ""; playerDoc.Save(playerStatFile);



                }
                catch (Exception x)
                {

                    Logger.LogError("Error Extraño:" + x);
                }
                // int reputacion_new = int.Parse(node["ReputacionNew"].InnerText);
                
               


   
            }
        }


        #region listas
        public class Thumbnail
        {
            [JsonProperty("url")]
            public string url { get; set; }
        }

        public class Footer
        {
            [JsonProperty("text")]
            public string text { get; set; }
            [JsonProperty("icon_url")]
            public string iconurl { get; set; }
        }

        public class Author
        {
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("url")]
            public string url { get; set; }
            [JsonProperty("icon_url")]
            public string iconurl { get; set; }
        }
        #endregion

        public void llamardiscord_ex(string steamurl, string steamnombre, string steamico, string nombrerango, int reputacionrango)
        {
            TaskDispatcher.QueueOnMainThread(() =>
            {

                const string colorGreen = "80E61F";

                WebRequest wr = (HttpWebRequest)WebRequest.Create(Configuration.Instance.webhook_url);
                wr.ContentType = "application/json";
                wr.Method = "POST";

                using (var sw = new StreamWriter(wr.GetRequestStream()))
                {

                    string json = JsonConvert.SerializeObject(new
                    {
                        username = "[AutoRangos]",
                        avatar_url = Configuration.Instance.webhook_image,
                        embeds = new[]
                    {

                        new
                        {
                            description=" \n \n" +
                            "ᴀᴄᴀʙᴀ ᴅᴇ ꜱᴜʙɪʀ ᴀʟ ʀᴀɴɢᴏ:" + " **" + nombrerango + "** \n \n"  +
                            "ℭ𝔬𝔫 𝔘𝔫𝔞 ℜ𝔢𝔭𝔲𝔱𝔞𝔠𝔦𝔬𝔫 𝔇𝔢: " + "**" + reputacionrango + "** \n \n" +
                            "𝙃𝙤𝙧𝙖: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " - " + DateTime.Now.Date.Day + "/" + DateTime.Now.Date.Month + "/" + DateTime.Now.Date.Year + "     🕐 \n \n",
                            author = new Author
                            {
                                name = steamnombre,
                                url =  steamurl,
                                iconurl = steamico,
                            },
                            thumbnail = new Thumbnail
                            {
                                url = "https://static.thenounproject.com/png/84093-200.png"
                            },
                            footer = new Footer
                            {
                                text = "7w7",
                                iconurl = "https://i.pinimg.com/originals/ef/8f/b5/ef8fb588c8da75dd0e1f0a5bc1c0aa62.jpg"
                            },
                            color= int.Parse(colorGreen, System.Globalization.NumberStyles.HexNumber)
                        },


                    }
                    });

                    sw.Write(json);
                }
                var response = (HttpWebResponse)wr.GetResponse();
            });


        }

      

        private void OnPlayerConnected(UnturnedPlayer player)
{




  string currentPath = Path.Combine(Plugin.Instance.Directory,"Datos/");


  string statFile = currentPath + player.CSteamID + ".xml";


  if (!System.IO.File.Exists(statFile))
  {
      XmlTextWriter writer = new XmlTextWriter(statFile, System.Text.Encoding.UTF8);
      writer.WriteStartDocument(true);
      writer.Formatting = System.Xml.Formatting.Indented;
      writer.Indentation = 2;
      writer.WriteStartElement("EnvyHosting");
      writer.WriteStartElement("YourReputation");
      Utilidad.GenerarReputacion(player.Reputation + "", writer);
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndDocument();
      writer.Close();
  }
      string playerStatFile = currentPath + player.CSteamID + ".xml";
      XmlDocument playerDoc = new XmlDocument();
      playerDoc.Load(playerStatFile);
      XmlElement playerPvPRoot = playerDoc.DocumentElement;


      foreach (XmlNode node in playerPvPRoot)
      {
          try
          {
              player.Reputation = 0 - player.Reputation;



              player.Reputation = int.Parse(node["ReputacionNew"].InnerText); break;

          }
          catch (Exception x)
          {

              Logger.LogError("ERROR: " + x);
          }
      }
    }


        private void OnPlayerUpdateGesture(UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
        {
            if (gesture == UnturnedPlayerEvents.PlayerGesture.SurrenderStart & Configuration.Instance.codigo_animacion == 1)
            {
                RevisarReputacion(player, player.Reputation); return;
            }

            if (gesture == UnturnedPlayerEvents.PlayerGesture.Facepalm & Configuration.Instance.codigo_animacion == 2)
            {
                RevisarReputacion(player, player.Reputation); return;
            } 

            if (gesture == UnturnedPlayerEvents.PlayerGesture.Wave & Configuration.Instance.codigo_animacion == 3)
            {
                RevisarReputacion(player, player.Reputation); return;
            }

            if (gesture == UnturnedPlayerEvents.PlayerGesture.Salute & Configuration.Instance.codigo_animacion == 4)
            {
                RevisarReputacion(player, player.Reputation); return;
            }
        }


        public void RevisarReputacion(UnturnedPlayer player, int reputacion)
        {
            if (reputacion > 0) { UnturnedChat.Say(player, Translate("reputacion_positiva"), Color.green); return; } else
            {
                Geimer(player); return;
            }

        }


        public void Geimer(UnturnedPlayer player)
        {






            foreach (Rangos item in Configuration.Instance.Rangos)
            {


                   var result = R.Permissions.AddPlayerToGroup(item.Rango, player);

                    switch (result)
                    {
                        case Rocket.API.RocketPermissionsProviderResult.Success:
    
                            if(player.Reputation <= item.Reputacion) 
                            {

                            if (Configuration.Instance.mandar_mensaje_privado_que_gano_rango == true) { UnturnedChat.Say(player, Translate("avanzar_rango", item.Rango, item.Reputacion)); }
                            if (Configuration.Instance.mandar_mensaje_server_que_gano_rango == true) { ChatManager.serverSendMessage("<color=#F70D1A>「AUTO RANGO」</color> " + player.CharacterName + " " + Instance.Configuration.Instance.mensaje.Replace('{', '<').Replace('}', '>') + " " +  item.Rango, Color.white, null, null, EChatMode.GLOBAL, Instance.Configuration.Instance.icon, true); }

                            if (Configuration.Instance.efecto_rayo_al_ganar_rango == true)
                            {
                                EffectManager.sendEffect(147, 30, player.Position);
                            }




                            if (Configuration.Instance.discord_webhook)
                            {
                                llamardiscord_ex("https://steamcommunity.com/profiles/" + player.SteamProfile.SteamID64, player.CharacterName, player.SteamProfile.AvatarFull.ToString(), item.Rango, item.Reputacion);
                            }


                                
                            }
                            else
                            {
                                R.Permissions.RemovePlayerFromGroup(item.Rango, player); break;
                            }
                        break;

                           
                        case Rocket.API.RocketPermissionsProviderResult.DuplicateEntry:
                        break;

                    }
            }
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){

                     { "reputacion_positiva", "¡Tu Reputacion Es Positiva! No Podras Avanzar De Rango"},
                     { "avanzar_rango", "¡Has Avanzado Al Rango! {0} Con [{1}]"},
                     { "Mensaje_Recompensa", "¡Puedes Avanzar De Rango! Solo Has La Accion: {0} "}

                };
            }
        }

        protected override void Unload()
        {
            base.Unload();
            UnturnedPlayerEvents.OnPlayerUpdateGesture -= OnPlayerUpdateGesture;
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
            ConfigGrupos = null;
        }
    }
}
