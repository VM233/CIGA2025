﻿#if UNITY_EDITOR
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.Editor
{
    public partial class HierarchyGeneralSetting
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            colorPresets ??= new();

            if (colorPresets.Count == 0)
            {
                colorPresets.Add(new()
                {
                    keyChar = "$",
                    textColor = ColorDefinitions.white,
                    backgroundColor = ColorDefinitions.orange
                });

                colorPresets.Add(new()
                {
                    keyChar = "^",
                    textColor = ColorDefinitions.white,
                    backgroundColor = ColorDefinitions.green
                });

                colorPresets.Add(new()
                {
                    keyChar = "#",
                    textColor = ColorDefinitions.deepSkyBlue,
                    backgroundColor = ColorDefinitions.yellow
                });

                colorPresets.Add(new()
                {
                    keyChar = "@",
                    textColor = ColorDefinitions.hotPink,
                    backgroundColor = ColorDefinitions.aqua
                });

                colorPresets.Add(new()
                {
                    keyChar = "/",
                    textColor = ColorDefinitions.white,
                    backgroundColor = ColorDefinitions.magenta
                });

                colorPresets.Add(new()
                {
                    keyChar = "%",
                    textColor = ColorDefinitions.white,
                    backgroundColor = ColorDefinitions.purple
                });

                colorPresets.Add(new()
                {
                    keyChar = "!",
                    textColor = ColorDefinitions.white,
                    backgroundColor = ColorDefinitions.red
                });

                colorPresets.Add(new()
                {
                    keyChar = "&",
                    textColor = ColorDefinitions.white,
                    backgroundColor = Color.black
                });

                colorPresets.Add(new()
                {
                    keyChar = "*",
                    textColor = ColorDefinitions.white,
                    backgroundColor = new(0, 1, 0.617f, 1)
                });

                this.EnforceSave();
            }
        }
    }
}
#endif