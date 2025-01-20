using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using BepInEx;
using UnityEngine;
#pragma warning disable CS0618 //Do not remove the following line.
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace PDDetection
{
    [BepInPlugin("locochoco.pddetection", "PDDetection", "0.1.0")] // (GUID, mod name, mod version)
    public class PDDetectionMod : BaseUnityPlugin
    {
	private static PDDetectionOptions options;

	//private Dictionary<GraphicsModule, List<PDDetection>> hatsOfGraphics = new Dictionary<GraphicsModule, List<PDDetection>>();
        public void OnEnable(){
	    Logger.LogInfo("Enabled!");
	    //On.GraphicsModule.InitiateSprites += GraphicsModuleOnInitiateSprites;
	    //On.GraphicsModule.DrawSprites += GraphicsModuleOnDrawSprites;
	    //On.PhysicalObject.DisposeGraphicsModule += PhysicalObjectOnDisposeGraphicsModule;
	    On.RainWorldGame.ctor += RainWorldGamector;
	    On.RainWorld.OnModsInit += RainWorldOnModsInit;
	    On.RainWorld.PostModsInit += RainWorldPostModsInit;
	    On.HUD.HUD.InitSinglePlayerHud += HUDInitSinglePlayerHud;
        }
	private void HUDInitSinglePlayerHud(On.HUD.HUD.orig_InitSinglePlayerHud orig, HUD.HUD self, RoomCamera cam){
	  self.AddPart(new PDThreatIndicator(self, self.fContainers[1], cam));
	  self.AddPart(new PDCreatureIndicator(self, self.fContainers[1], cam));
	  orig(self, cam);
	}
	private void RainWorldGamector(On.RainWorldGame.orig_ctor orig, RainWorldGame self, ProcessManager manager){
	    orig(self, manager);
	    ReadSettings();
	}
	private void RainWorldPostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld self){
	    orig(self);
	    ReadSettings();
	}
	private void RainWorldOnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self){
	    orig(self);
	    InitOptions();
	    LoadResources();
	}
	private void InitOptions(){
	    if(options == null) options = new PDDetectionOptions();
	    MachineConnector.SetRegisteredOI("locochoco.pddetection", options);
	}
	private void LoadResources(){
	    Futile.atlasManager.ActuallyLoadAtlasOrImage("pddetection-warning", "sprites/warning", "");
	    Futile.atlasManager.ActuallyLoadAtlasOrImage("pddetection-danger", "sprites/danger", "");
	    Futile.atlasManager.ActuallyLoadAtlasOrImage("pddetection-warning-pointer", "sprites/warning-pointer", "");
	    Futile.atlasManager.ActuallyLoadAtlasOrImage("pddetection-danger-pointer", "sprites/danger-pointer", "");
	    Logger.LogInfo("Adding PDDetection sprite to atlas!");
	}
	private void ReadSettings(){
	    //hatMode = options.HatMode.Value;
	}
	/*
        private void GraphicsModuleOnInitiateSprites(On.GraphicsModule.orig_InitiateSprites orig, GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam){
            orig(self, sLeaser, rCam);
	    Logger.LogInfo("Initting Sprites");
	    if(hatsOfGraphics.TryGetValue(self, out var hats)){
	    	Logger.LogInfo("New room!");
	        InitiateHatOnNewRoom(hats, rCam);
		return;
	    }
	    Logger.LogInfo("New hat!");
	    AddHatToGraphics(self);
        }
        private void GraphicsModuleOnDrawSprites(On.GraphicsModule.orig_DrawSprites orig, GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos){
            orig(self, sLeaser, rCam, timeStacker, camPos);
	    if(hatsOfGraphics.TryGetValue(self, out var hats)){
	    	//Logger.LogInfo("Moving Sprite!"); 
	        hats.ForEach(hat => hat.MoveWithParentSprite(sLeaser));
	    }
        }
        private void PhysicalObjectOnDisposeGraphicsModule(On.PhysicalObject.orig_DisposeGraphicsModule orig, PhysicalObject self){
	    //after orig, self.graphicsModule will be disposed, so we need to remove it from the dictionary now
	    Logger.LogInfo("Disposing Sprites!");
	    if(self.graphicsModule != null) hatsOfGraphics.Remove(self.graphicsModule);
            orig(self);
        }
	private void InitiateHatOnNewRoom(List<PDDetection> hats, RoomCamera rCam){
	    hats.ForEach(hat => rCam.room.AddObject(hat));
	}
	private void AddHatToGraphics(GraphicsModule graphics){
	    if(graphics == null)
	        return;
	    Logger.LogInfo("Adding hat to someone!");
	    List<PDDetection> hats = new List<PDDetection>();
	    if(hatMode != HatMode.ScavOnly && graphics is PlayerGraphics){
	    	Logger.LogInfo("Adding hat to player!");
		hats.Add(new PDDetection(graphics, 3, 0f, 5f, false));
		hatsOfGraphics.Add(graphics, hats);
	    }
	    else if(hatMode != HatMode.PlayerOnly && graphics is ScavengerGraphics scavGraph){
	    	Logger.LogInfo("Adding hat to scav!");
		
		hats.Add(new PDDetection(graphics, scavGraph.HeadSprite, 180f, 7f, false));
		hatsOfGraphics.Add(graphics, hats);
	    }
	}*/
    }
}
