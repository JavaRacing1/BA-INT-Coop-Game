using System;
using System.Collections.Generic;
using System.Linq;

using Godot;

namespace INTOnlineCoop.Script.Item
{
    /// <summary>
    /// Contains weapon information
    /// </summary>
    public partial class Item : RefCounted
    {
        /// <summary> Used when no item was chosen </summary>
        public static readonly Item None = new("None");

        /// <summary> Pistol for shooting normal bullets </summary>
        public static readonly Item Pistol = new("Pistol");

        /// <summary> Shotgun for shooting multiple bullets </summary>
        public static readonly Item Shotgun = new("Shotgun");

        /// <summary> Bazooka for rockets </summary>
        public static readonly Item Bazooka = new("Bazooka");

        /// <summary> Sniper for shooting long range bullets </summary>
        public static readonly Item Sniper = new("Sniper");

        /// <summary>
        /// Enumerable of all available items
        /// </summary>
        public static IEnumerable<Item> Values
        {
            get
            {
                yield return Pistol;
            }
        }

        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Scene of the item
        /// </summary>
        public PackedScene ItemScene { get; private set; }

        /// <summary>
        /// Scene of the bullet
        /// </summary>
        public PackedScene BulletScene { get; private set; }

        private Item(string name, bool hasBullet = true)
        {
            Name = name;
            ItemScene = GD.Load<PackedScene>($"res://scene/item/{name}Item.tscn");
            if (hasBullet)
            {
                BulletScene = GD.Load<PackedScene>($"res://scene/item/{name}Bullet.tscn");
            }
        }

        /// <summary>
        /// Returns the item object with given name
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <returns>Item object</returns>
        public static Item FromName(string name)
        {
            try
            {
                return Values.Single(item => item.Name == name);
            }
            catch (Exception)
            {
                GD.PrintErr($"Couldn't convert string {name} to Item!");
                return None;
            }
        }
    }
}