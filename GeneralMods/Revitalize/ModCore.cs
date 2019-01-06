using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using PyTK.Extensions;
using PyTK.Types;
using Revitalize.Framework.Crafting;
using Revitalize.Framework.Environment;
using Revitalize.Framework.Graphics;
using Revitalize.Framework.Graphics.Animations;
using Revitalize.Framework.Illuminate;
using Revitalize.Framework.Objects;
using Revitalize.Framework.Player;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;

namespace Revitalize
{
    // TODO:
    //  -Multiple Lights On Object
    //  -Illumination Colors
    //  Furniture:
    //      -rugs
    //      -tables
    //      -lamps
    //      -chairs
    //      -dressers/other storage containers
    //      -fun interactables
    //      -More crafting tables
    //  -Machines
    //      !=Energy
    //      -Furnace
    //      -Seed Maker
    //      -Stone Quarry
    //  -Materials
    //      -Tin/Bronze/Alluminum/Silver?Platinum/Etc
    //  -Crafting Menu
    //  -Item Grab Menu (Extendable)
    //  -Gift Boxes
    //  Magic!
    //      -Alchemy Bags
    //      -Transmutation
    //      -Effect Crystals
    //      -Spell books
    //      -Potions!
    //      -Magic Meter
    //      -Connected chests much like Project EE2 from MC
    //
    //
    //
    //  -Bigger chests
    //
    //  Festivals
    //      -Firework festival?
    //  Stargazing???
    //      -Moon Phases+DarkerNight
    //  Bigger/Better Museum?
    //  More Crops?
    //  More Food?
    // 
    //  Equippables!
    //      -accessories that provide buffs/regen/friendship
    //      -braclets/rings/broaches....more crafting for these???
    //      
    //  Music???
    //      -IDK maybe add in instruments???
    //      
    //  More buildings????
    //  
    //  More Animals???
    //  
    //  Readable Books?
    //  
    //  Custom NPCs for shops???
    //  
    //  Frisbee Minigame?
    //  
    //  HorseRace Minigame/Betting?
    //  
    //  Locations:
    //      -Small Island Home?
    //      
    //  More crops
    //
    //  More monsters
    //  -boss fights
    //
    //  More dungeons??


    public class ModCore : Mod
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;

        public static Dictionary<string, CustomObject> customObjects;

        public static PlayerInfo playerInfo;

        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = this.Monitor;

            this.createDirectories();
            this.initailizeComponents();

            ModHelper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded;
            ModHelper.Events.GameLoop.TimeChanged += this.GameLoop_TimeChanged;
            ModHelper.Events.GameLoop.UpdateTicked += this.GameLoop_UpdateTicked;
            playerInfo = new PlayerInfo();
        }

        private void createDirectories()
        {
            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Configs"));
        }

        private void initailizeComponents()
        {
            DarkerNight.InitializeConfig();
        }

        private void GameLoop_UpdateTicked(object sender, StardewModdingAPI.Events.UpdateTickedEventArgs e)
        {
            DarkerNight.SetDarkerColor();
            playerInfo.update();
        }

        private void GameLoop_TimeChanged(object sender, StardewModdingAPI.Events.TimeChangedEventArgs e)
        {
            DarkerNight.CalculateDarkerNightColor();
        }

        private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
        {
            MultiTiledComponent obj = new MultiTiledComponent(new BasicItemInformation("CoreObjectTest", "YAY FUN!", "Omegasis.Revitalize.MultiTiledComponent", Color.White, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.TEST1", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", Game1.objectSpriteSheet, Color.White, 0, true, typeof(MultiTiledComponent), null, new AnimationManager(new Texture2DExtended(Game1.objectSpriteSheet), new Animation(new Rectangle(0, 0, 16, 16))), Color.Red, true, null, null));
            MultiTiledComponent obj2 = new MultiTiledComponent(new BasicItemInformation("CoreObjectTest2", "SomeFun", "Omegasis.Revitalize.MultiTiledComponent", Color.White, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.TEST1", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", Game1.objectSpriteSheet, Color.White, 0, true, typeof(MultiTiledComponent), null, new AnimationManager(new Texture2DExtended(Game1.objectSpriteSheet), new Animation(new Rectangle(0, 16, 16, 16))), Color.Red, false, null, null));
            MultiTiledComponent obj3 = new MultiTiledComponent(new BasicItemInformation("CoreObjectTest3", "NoFun", "Omegasis.Revitalize.MultiTiledComponent", Color.White, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.TEST1", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", Game1.objectSpriteSheet, Color.White, 0, true, typeof(MultiTiledComponent), null, new AnimationManager(new Texture2DExtended(Game1.objectSpriteSheet), new Animation(new Rectangle(0, 32, 16, 16))), Color.Red, false, null, null));


            obj.info.lightManager.addLight(new Vector2(Game1.tileSize), new LightSource(4, new Vector2(0, 0), 2.5f, Color.Orange.Invert()), obj);

            MultiTiledObject bigObject = new MultiTiledObject(new BasicItemInformation("MultiTest", "A really big object", "Omegasis.Revitalize.MultiTiledObject", Color.Blue, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.BigTiledTest", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", Game1.objectSpriteSheet, Color.White, 0, true, typeof(MultiTiledObject), null, new AnimationManager(), Color.White, false, null, null));
            bigObject.addComponent(new Vector2(0, 0), obj);
            bigObject.addComponent(new Vector2(1, 0), obj2);
            bigObject.addComponent(new Vector2(2, 0), obj3);

            Recipe pie = new Recipe(new Dictionary<Item, int>()
            {
                [bigObject] = 1
            }, new KeyValuePair<Item, int>(new Furniture(3, Vector2.Zero), 1),new StatCost(100,50,0,0));


            new InventoryItem(bigObject, 100, 1).addToNPCShop("Gus");
            Game1.player.addItemToInventory(bigObject);

            if (pie.PlayerCanCraft())
            {
                pie.craft();
            }
        }

        public static void log(object message)
        {
            ModMonitor.Log(message.ToString());
        }
    }
}