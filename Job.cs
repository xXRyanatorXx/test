using System;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class Job : MonoBehaviour
{
	// Token: 0x04000A7D RID: 2685
	public bool PaintMatters;

	// Token: 0x04000A7E RID: 2686
	public float RewardModifier;

	// Token: 0x04000A7F RID: 2687
	public float MinCondition;

	// Token: 0x04000A80 RID: 2688
	public float MaxCondition;

	// Token: 0x04000A81 RID: 2689
	public string DescriptionKey;

	// Token: 0x04000A82 RID: 2690
	public string Description;

	// Token: 0x04000A83 RID: 2691
	public string MyNote;

	// Token: 0x04000A84 RID: 2692
	public float Reward;

	// Token: 0x04000A85 RID: 2693
	public bool CrashedCar;

	// Token: 0x04000A86 RID: 2694
	public bool WashCar;

	// Token: 0x04000A87 RID: 2695
	public bool FixAccident;

	// Token: 0x04000A88 RID: 2696
	public bool FixRust;

	// Token: 0x04000A89 RID: 2697
	public bool FixRustPart;

	// Token: 0x04000A8A RID: 2698
	public bool Repaint;

	// Token: 0x04000A8B RID: 2699
	public bool RepaintPart;

	// Token: 0x04000A8C RID: 2700
	public bool ChangeOil;

	// Token: 0x04000A8D RID: 2701
	public bool ChangeCoolant;

	// Token: 0x04000A8E RID: 2702
	public bool ChangeBrakeFluid;

	// Token: 0x04000A8F RID: 2703
	public bool FixAllCar;

	// Token: 0x04000A90 RID: 2704
	public bool ChangeBrakePads;

	// Token: 0x04000A91 RID: 2705
	public bool ChangeBrakeDiscs;

	// Token: 0x04000A92 RID: 2706
	public bool ChangeBrakeLines;

	// Token: 0x04000A93 RID: 2707
	public bool ChangeTires;

	// Token: 0x04000A94 RID: 2708
	public bool Openable;

	// Token: 0x04000A95 RID: 2709
	public bool Interior;

	// Token: 0x04000A96 RID: 2710
	public bool HandBrakeCable;

	// Token: 0x04000A97 RID: 2711
	public bool RealWheel;

	// Token: 0x04000A98 RID: 2712
	public bool Tire;

	// Token: 0x04000A99 RID: 2713
	public bool SuspensionPart;

	// Token: 0x04000A9A RID: 2714
	public bool Hub;

	// Token: 0x04000A9B RID: 2715
	public bool Spring;

	// Token: 0x04000A9C RID: 2716
	public bool Damper;

	// Token: 0x04000A9D RID: 2717
	public bool BrakePad;

	// Token: 0x04000A9E RID: 2718
	public bool BrakeDisc;

	// Token: 0x04000A9F RID: 2719
	public bool BrakeLine;

	// Token: 0x04000AA0 RID: 2720
	public bool MainBrakeLine;

	// Token: 0x04000AA1 RID: 2721
	public bool TieRod;

	// Token: 0x04000AA2 RID: 2722
	public bool SteeringWheel;

	// Token: 0x04000AA3 RID: 2723
	public bool WindowLift;

	// Token: 0x04000AA4 RID: 2724
	public bool WiperMotor;

	// Token: 0x04000AA5 RID: 2725
	public bool WiperBlade;

	// Token: 0x04000AA6 RID: 2726
	public bool Bulb;

	// Token: 0x04000AA7 RID: 2727
	public bool DriveShaft;

	// Token: 0x04000AA8 RID: 2728
	public bool Gearbox;

	// Token: 0x04000AA9 RID: 2729
	public bool Shifter;

	// Token: 0x04000AAA RID: 2730
	public bool Diff;

	// Token: 0x04000AAB RID: 2731
	public bool Flywheel;

	// Token: 0x04000AAC RID: 2732
	public bool Clutch;

	// Token: 0x04000AAD RID: 2733
	public bool ClutchCover;

	// Token: 0x04000AAE RID: 2734
	public bool FuelLine;

	// Token: 0x04000AAF RID: 2735
	public bool FuelTank;

	// Token: 0x04000AB0 RID: 2736
	public bool FuelPump;

	// Token: 0x04000AB1 RID: 2737
	public bool IgnitionCoil;

	// Token: 0x04000AB2 RID: 2738
	public bool SparkPlug;

	// Token: 0x04000AB3 RID: 2739
	public bool SparkWires;

	// Token: 0x04000AB4 RID: 2740
	public bool Distributor;

	// Token: 0x04000AB5 RID: 2741
	public bool Carburetor;

	// Token: 0x04000AB6 RID: 2742
	public bool Battery;

	// Token: 0x04000AB7 RID: 2743
	public bool BatteryWires;

	// Token: 0x04000AB8 RID: 2744
	public bool Alternator;

	// Token: 0x04000AB9 RID: 2745
	public bool Starter;

	// Token: 0x04000ABA RID: 2746
	public bool CrankshaftPulley;

	// Token: 0x04000ABB RID: 2747
	public bool AlternatorBelt;

	// Token: 0x04000ABC RID: 2748
	public bool EngineBlock;

	// Token: 0x04000ABD RID: 2749
	public bool EngineHead;

	// Token: 0x04000ABE RID: 2750
	public bool HeadGasket;

	// Token: 0x04000ABF RID: 2751
	public bool HeadCover;

	// Token: 0x04000AC0 RID: 2752
	public bool AirFilter;

	// Token: 0x04000AC1 RID: 2753
	public bool AirFilterCover;

	// Token: 0x04000AC2 RID: 2754
	public bool Crankshaft;

	// Token: 0x04000AC3 RID: 2755
	public bool EngineChain;

	// Token: 0x04000AC4 RID: 2756
	public bool Camshaft;

	// Token: 0x04000AC5 RID: 2757
	public bool Piston;

	// Token: 0x04000AC6 RID: 2758
	public bool Radiator;

	// Token: 0x04000AC7 RID: 2759
	public bool RadiatorFan;

	// Token: 0x04000AC8 RID: 2760
	public bool RadiatorGT;

	// Token: 0x04000AC9 RID: 2761
	public bool RadiatorFanGT;

	// Token: 0x04000ACA RID: 2762
	public bool WaterPump;

	// Token: 0x04000ACB RID: 2763
	public bool WaterPumpBelt;

	// Token: 0x04000ACC RID: 2764
	public bool WaterHoseUpper;

	// Token: 0x04000ACD RID: 2765
	public bool WaterHoseLower;

	// Token: 0x04000ACE RID: 2766
	public bool OilPan;

	// Token: 0x04000ACF RID: 2767
	public bool OilFilter;

	// Token: 0x04000AD0 RID: 2768
	public bool Exhaust;

	// Token: 0x04000AD1 RID: 2769
	public bool ExhaustHeader;

	// Token: 0x04000AD2 RID: 2770
	public bool ExhaustManifold;

	// Token: 0x04000AD3 RID: 2771
	public bool Cluster;
}
