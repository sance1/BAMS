using System;
using System.Collections.Generic;
using BAMS.Models;
using Microsoft.Extensions.Configuration;

namespace BAMS.Helpers
{
    public class MenuConfiguration
    {
        public static List<MenuConfig> GetMenu(IConfiguration _config, string menu, int roleId)
        {
            var config = _config[$"MenuConfiguration:{menu}:{roleId}"] ?? _config[$"MenuConfiguration:{menu}:Default"];
            var split = config.Split(",");
            var list = new List<MenuConfig>();
            foreach (var value in split)
            {
                var menuConfig = new MenuConfig();
                var splitVal = value.Split("_");
                menuConfig.Key = splitVal[0];
                menuConfig.Name = splitVal[0];
                if (splitVal.Length > 1)
                {
                    menuConfig.Name = splitVal[1];
                }

                if (menuConfig.Name.Contains(":"))
                {
                    var getClass = menuConfig.Name.Split(":");
                    menuConfig.Name = getClass[0];
                    menuConfig.Class = getClass[1];
                }
                
                if (menuConfig.Key.Contains(":"))
                {
                    var getClass = menuConfig.Key.Split(":");
                    menuConfig.Key = getClass[0];
                    menuConfig.Class = getClass[1];
                }
                list.Add(menuConfig);
            }
            return list;
        }
    }
}