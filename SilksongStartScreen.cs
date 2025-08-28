using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UObject = UnityEngine.Object;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Newtonsoft.Json;

namespace CustomTitle
{
    public class CustomTitle : Mod
    {
        public override string GetVersion() => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        readonly string modDataDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data");

        public CustomTitle() : base("Custom Title")
        {
            On.MenuStyleTitle.SetTitle += FixMenuTitle;
        }

        private void FixMenuTitle(On.MenuStyleTitle.orig_SetTitle orig, MenuStyleTitle self, int index)
        {
            byte[] fileData;

            try 
            {
                fileData = File.ReadAllBytes(GetImagePath());
            }
            catch (Exception ex)
            {
                Log("Failed to load image. " + ex.Message);
                Log("Switching to default menu title");
                orig(self, index);
                return;
            }

            Texture2D RealTitle_texture = new Texture2D(2, 2);
            RealTitle_texture.LoadImage(fileData);

            var RealTitle = Sprite.Create(
                RealTitle_texture,
                new Rect(0, 0, RealTitle_texture.width, RealTitle_texture.height),
                new Vector2(0.5f, 0.5f),
                RealTitle_texture.height / 8,
                0,
                SpriteMeshType.FullRect
            );

            self.Title.sprite = RealTitle;
        }

        private string GetImagePath()
        {
            Log(Application.persistentDataPath);
            string path = Path.Combine(modDataDir, "data.json");
            string json = File.ReadAllText(path);
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            return Path.Combine(modDataDir, dict[GetActiveMenuStyle()]);
        }

        private string GetActiveMenuStyle()
        {
            return MenuStyles.Instance.styles[MenuStyles.Instance.CurrentStyle].styleObject.name;
        }
    }
}
