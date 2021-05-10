using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;

namespace AutoRangos
{
    public class Config : IRocketPluginConfiguration
    {
        public List<Rangos> Rangos;
        public int codigo_animacion;
        public bool efecto_rayo_al_ganar_rango;
        public bool mandar_mensaje_privado_que_gano_rango;
        public bool mandar_mensaje_server_que_gano_rango;
        public string mensaje;
        public string icon = "https://i.imgur.com/D4Q8xah.png";
        public bool discord_webhook;
        public string webhook_url;
        public string webhook_image;
        public void LoadDefaults()
        {
            codigo_animacion = 1;
            mensaje = "{color=#5EFB6E}Ha Obtenido El Rango: {/color}";
            
            mandar_mensaje_privado_que_gano_rango = false;
            mandar_mensaje_server_que_gano_rango = true;
            efecto_rayo_al_ganar_rango = true;
            discord_webhook = false;
            Rangos = new List<Rangos>
                {
                new Rangos { Rango = "Rango1", Reputacion = -1000},
                new Rangos { Rango = "Rango2", Reputacion = -2000},
                new Rangos { Rango = "Rango3", Reputacion = -3000}
            };

            webhook_image = "URL IMAGE HERE";
            webhook_url = "WEBHOOK URL";



           
            

        }
    }



    public class Rangos
    {
        public Rangos() { }
        public string Rango;
        public int Reputacion;
    }
}
