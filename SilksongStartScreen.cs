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

namespace CustomTitle
{
    public class CustomTitle : Mod
    {
        public override string GetVersion() => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public CustomTitle() : base("Custom Title")
        {
            On.MenuStyleTitle.SetTitle += FixMenuTitle;
        }

        private void FixMenuTitle(On.MenuStyleTitle.orig_SetTitle orig, MenuStyleTitle self, int index)
        {
            MenuStyleTitle.TitleSpriteCollection spriteCollection =
                index < 0 || index >= self.TitleSprites.Length
                    ? self.DefaultTitleSprite
                    : self.TitleSprites[index];

            string modDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(modDir, "logo.png");

            byte[] fileData = File.ReadAllBytes(path);

            Texture2D RealTitle_texture = new Texture2D(2, 2);
            RealTitle_texture.LoadImage(fileData);

            var RealTitle = Sprite.Create(
                RealTitle_texture,
                new Rect(0, 0, RealTitle_texture.width, RealTitle_texture.height),
                new Vector2(0.5f, 0.5f),
                spriteCollection.Default.pixelsPerUnit,
                0,
                SpriteMeshType.FullRect
            );

            self.Title.sprite = RealTitle;
        }
    }
}
