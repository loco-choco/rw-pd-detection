using System.Linq;
using BepInEx;
using UnityEngine;
using RWCustom;
using Menu.Remix.MixedUI;

namespace PDDetection
{
    class PDDetectionOptions : OptionInterface
    {
	//public readonly Configurable<PDDetectionMod.HatMode> HatMode;
	private UIelement[] options_on_tab;

        public PDDetectionOptions(){
	    //HatMode = config.Bind("HatMode", PDDetectionMod.HatMode.ScavOnly);
        }

	public override void Initialize(){
	    OpTab options_tab = new OpTab(this, "Options"); 
	    options_on_tab = new UIelement[2]{
	      new OpLabel(10f, 550f, "Configurations", bigText: true),
	      new OpLabel(10f, 490f, "Change who can have the cowboy hat"),
	      //new OpComboBox(HatMode, new Vector2(10f, 460f), 200f, OpResourceSelector.GetEnumNames(null, typeof(PDDetectionMod.HatMode)).ToList()),
	    };
	    options_tab.AddItems(options_on_tab);

	    Tabs = new OpTab[1] { options_tab };
	}
    }
}
