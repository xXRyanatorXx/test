using System;
using System.Collections.Generic;
using NWH.VehiclePhysics2;
using PaintIn3D;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000161 RID: 353
public class CarProperties1 : MonoBehaviour
{
	// Token: 0x060007BC RID: 1980 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x0000245B File Offset: 0x0000065B
	private void Update()
	{
	}

	// Token: 0x04000CEA RID: 3306
	public string PartName;

	// Token: 0x04000CEB RID: 3307
	public string PartNameExtension;

	// Token: 0x04000CEC RID: 3308
	public string PrefabName;

	// Token: 0x04000CED RID: 3309
	public int Type;

	// Token: 0x04000CEE RID: 3310
	public int Type2;

	// Token: 0x04000CEF RID: 3311
	public int ObjectNumber;

	// Token: 0x04000CF0 RID: 3312
	public int ChildrenNumber;

	// Token: 0x04000CF1 RID: 3313
	public bool NumberPlate;

	// Token: 0x04000CF2 RID: 3314
	public Material One;

	// Token: 0x04000CF3 RID: 3315
	public Material Two;

	// Token: 0x04000CF4 RID: 3316
	public Material Three;

	// Token: 0x04000CF5 RID: 3317
	public Material Four;

	// Token: 0x04000CF6 RID: 3318
	public Material Five;

	// Token: 0x04000CF7 RID: 3319
	public Material Six;

	// Token: 0x04000CF8 RID: 3320
	public string Nr1;

	// Token: 0x04000CF9 RID: 3321
	public string Nr2;

	// Token: 0x04000CFA RID: 3322
	public string Nr3;

	// Token: 0x04000CFB RID: 3323
	public string Nr4;

	// Token: 0x04000CFC RID: 3324
	public string Nr5;

	// Token: 0x04000CFD RID: 3325
	public string Nr6;

	// Token: 0x04000CFE RID: 3326
	public bool WearWorking;

	// Token: 0x04000CFF RID: 3327
	public bool WearDriving;

	// Token: 0x04000D00 RID: 3328
	public bool WearByTime;

	// Token: 0x04000D01 RID: 3329
	public float WearSpeed;

	// Token: 0x04000D02 RID: 3330
	public bool triger;

	// Token: 0x04000D03 RID: 3331
	public Mesh Differentmesh;

	// Token: 0x04000D04 RID: 3332
	public bool tight;

	// Token: 0x04000D05 RID: 3333
	public int SavePosition;

	// Token: 0x04000D06 RID: 3334
	public int JunkSpawnChance;

	// Token: 0x04000D07 RID: 3335
	public string Owner;

	// Token: 0x04000D08 RID: 3336
	public bool SinglePart;

	// Token: 0x04000D09 RID: 3337
	public GameObject PREFAB;

	// Token: 0x04000D0A RID: 3338
	public bool InBarn;

	// Token: 0x04000D0B RID: 3339
	public float ConditionDebug;

	// Token: 0x04000D0C RID: 3340
	public Mesh NormalMesh;

	// Token: 0x04000D0D RID: 3341
	public Mesh RemovedDifferentMesh;

	// Token: 0x04000D0E RID: 3342
	public GameObject VisualObject;

	// Token: 0x04000D0F RID: 3343
	public MainCarProperties MainProperties;

	// Token: 0x04000D10 RID: 3344
	public VehicleController exp;

	// Token: 0x04000D11 RID: 3345
	public bool started;

	// Token: 0x04000D12 RID: 3346
	public bool Attached;

	// Token: 0x04000D13 RID: 3347
	public bool picked;

	// Token: 0x04000D14 RID: 3348
	public bool IsCopy;

	// Token: 0x04000D15 RID: 3349
	public P3dChangeCounter RustChangeCounter;

	// Token: 0x04000D16 RID: 3350
	public P3dPaintDecal RepairDecal;

	// Token: 0x04000D17 RID: 3351
	public P3dPaintDecal RustRepairDecal;

	// Token: 0x04000D18 RID: 3352
	public P3dPaintDecal RustRepairedDecal;

	// Token: 0x04000D19 RID: 3353
	public P3dPaintDecal PaintRemoveDecal;

	// Token: 0x04000D1A RID: 3354
	public P3dPaintDecal WashDecal;

	// Token: 0x04000D1B RID: 3355
	public bool Chromed;

	// Token: 0x04000D1C RID: 3356
	public Material ChromeMat;

	// Token: 0x04000D1D RID: 3357
	public GameObject unmountedWheel;

	// Token: 0x04000D1E RID: 3358
	public GameObject WheelController;

	// Token: 0x04000D1F RID: 3359
	public GameObject FRSuspensionPosition;

	// Token: 0x04000D20 RID: 3360
	public GameObject FLSuspensionPosition;

	// Token: 0x04000D21 RID: 3361
	public GameObject RRSuspensionPosition;

	// Token: 0x04000D22 RID: 3362
	public GameObject RLSuspensionPosition;

	// Token: 0x04000D23 RID: 3363
	public GameObject AudioParent;

	// Token: 0x04000D24 RID: 3364
	public GameObject MaterialParent;

	// Token: 0x04000D25 RID: 3365
	public bool HandBrake;

	// Token: 0x04000D26 RID: 3366
	public bool HandBrakeCable;

	// Token: 0x04000D27 RID: 3367
	public bool MeshRepairable;

	// Token: 0x04000D28 RID: 3368
	public bool Tintable;

	// Token: 0x04000D29 RID: 3369
	public int TintLevel;

	// Token: 0x04000D2A RID: 3370
	public bool CantRust;

	// Token: 0x04000D2B RID: 3371
	public bool Paintable;

	// Token: 0x04000D2C RID: 3372
	public bool Washable;

	// Token: 0x04000D2D RID: 3373
	public bool Fairable;

	// Token: 0x04000D2E RID: 3374
	public bool Interior;

	// Token: 0x04000D2F RID: 3375
	public int OriginalInterior;

	// Token: 0x04000D30 RID: 3376
	public GameObject StartOption1;

	// Token: 0x04000D31 RID: 3377
	public GameObject StartOption2;

	// Token: 0x04000D32 RID: 3378
	public GameObject StartOption3;

	// Token: 0x04000D33 RID: 3379
	public GameObject StartOption4;

	// Token: 0x04000D34 RID: 3380
	public GameObject StartOption5;

	// Token: 0x04000D35 RID: 3381
	public GameObject StartOption6;

	// Token: 0x04000D36 RID: 3382
	public GameObject StartOption7;

	// Token: 0x04000D37 RID: 3383
	public GameObject StartOption8;

	// Token: 0x04000D38 RID: 3384
	public GameObject StartOption9;

	// Token: 0x04000D39 RID: 3385
	public bool VisualWheel;

	// Token: 0x04000D3A RID: 3386
	public bool NonRotatingVisualWheel;

	// Token: 0x04000D3B RID: 3387
	public bool RealWheel;

	// Token: 0x04000D3C RID: 3388
	public bool Hub;

	// Token: 0x04000D3D RID: 3389
	public CarProperties tireObject;

	// Token: 0x04000D3E RID: 3390
	public bool Tire;

	// Token: 0x04000D3F RID: 3391
	public float TirePressure;

	// Token: 0x04000D40 RID: 3392
	public float TireType;

	// Token: 0x04000D41 RID: 3393
	public float forwSlipCoef;

	// Token: 0x04000D42 RID: 3394
	public float forwForcCoef;

	// Token: 0x04000D43 RID: 3395
	public float sideSlipCoef;

	// Token: 0x04000D44 RID: 3396
	public float sideForcCoef;

	// Token: 0x04000D45 RID: 3397
	public bool TireValve;

	// Token: 0x04000D46 RID: 3398
	public bool Spring;

	// Token: 0x04000D47 RID: 3399
	public bool Damper;

	// Token: 0x04000D48 RID: 3400
	public bool SuspensionPart;

	// Token: 0x04000D49 RID: 3401
	public float SpacerSize;

	// Token: 0x04000D4A RID: 3402
	public bool BrakePad;

	// Token: 0x04000D4B RID: 3403
	public bool BrakeDisc;

	// Token: 0x04000D4C RID: 3404
	public bool BrakeLine;

	// Token: 0x04000D4D RID: 3405
	public bool MainBrakeLine;

	// Token: 0x04000D4E RID: 3406
	public bool BrakeMaster;

	// Token: 0x04000D4F RID: 3407
	public FLUID BrakeFluid;

	// Token: 0x04000D50 RID: 3408
	public bool TieRod;

	// Token: 0x04000D51 RID: 3409
	public bool BrakePedal;

	// Token: 0x04000D52 RID: 3410
	public bool SteeringWheel;

	// Token: 0x04000D53 RID: 3411
	public bool SteeringBox;

	// Token: 0x04000D54 RID: 3412
	public bool IgnitionKey;

	// Token: 0x04000D55 RID: 3413
	public bool WindowLift;

	// Token: 0x04000D56 RID: 3414
	public bool WindowLiftFL;

	// Token: 0x04000D57 RID: 3415
	public bool WindowLiftFR;

	// Token: 0x04000D58 RID: 3416
	public bool WindowLiftRL;

	// Token: 0x04000D59 RID: 3417
	public bool WindowLiftRR;

	// Token: 0x04000D5A RID: 3418
	public bool WiperMotor;

	// Token: 0x04000D5B RID: 3419
	public bool WiperL;

	// Token: 0x04000D5C RID: 3420
	public bool WiperR;

	// Token: 0x04000D5D RID: 3421
	public bool WiperOnly;

	// Token: 0x04000D5E RID: 3422
	public bool WiperBlade;

	// Token: 0x04000D5F RID: 3423
	public bool TrailerHook;

	// Token: 0x04000D60 RID: 3424
	public bool BrakeLight;

	// Token: 0x04000D61 RID: 3425
	public bool ReverseLight;

	// Token: 0x04000D62 RID: 3426
	public bool HeadLightLow;

	// Token: 0x04000D63 RID: 3427
	public bool HeadLightHigh;

	// Token: 0x04000D64 RID: 3428
	public bool RunningLight;

	// Token: 0x04000D65 RID: 3429
	public bool RightLight;

	// Token: 0x04000D66 RID: 3430
	public bool LeftLight;

	// Token: 0x04000D67 RID: 3431
	public bool Bulb;

	// Token: 0x04000D68 RID: 3432
	public bool FL;

	// Token: 0x04000D69 RID: 3433
	public bool FR;

	// Token: 0x04000D6A RID: 3434
	public bool RL;

	// Token: 0x04000D6B RID: 3435
	public bool RR;

	// Token: 0x04000D6C RID: 3436
	public bool FRONT;

	// Token: 0x04000D6D RID: 3437
	public bool REAR;

	// Token: 0x04000D6E RID: 3438
	public bool Openable;

	// Token: 0x04000D6F RID: 3439
	public bool Cup;

	// Token: 0x04000D70 RID: 3440
	public bool CupOpen;

	// Token: 0x04000D71 RID: 3441
	public bool SolidAxle;

	// Token: 0x04000D72 RID: 3442
	public bool ThisIsAfPOSITION;

	// Token: 0x04000D73 RID: 3443
	public bool AffectsHandling;

	// Token: 0x04000D74 RID: 3444
	public bool AffectsWCOLrotationZ;

	// Token: 0x04000D75 RID: 3445
	public bool AffectsWCOLrotationY;

	// Token: 0x04000D76 RID: 3446
	public bool AffectsWCOLpositionX;

	// Token: 0x04000D77 RID: 3447
	public bool AffectsWCOLpositionZ;

	// Token: 0x04000D78 RID: 3448
	public bool AffectsFRSuspensionPosition;

	// Token: 0x04000D79 RID: 3449
	public bool AffectsFLSuspensionPosition;

	// Token: 0x04000D7A RID: 3450
	public bool AffectsRLSuspensionPositionX;

	// Token: 0x04000D7B RID: 3451
	public bool AffectsRLSuspensionPositionZ;

	// Token: 0x04000D7C RID: 3452
	public bool AffectsRRSuspensionPositionX;

	// Token: 0x04000D7D RID: 3453
	public bool AffectsRRSuspensionPositionZ;

	// Token: 0x04000D7E RID: 3454
	public float RimR;

	// Token: 0x04000D7F RID: 3455
	public float radius;

	// Token: 0x04000D80 RID: 3456
	public float width;

	// Token: 0x04000D81 RID: 3457
	public float SpringLength;

	// Token: 0x04000D82 RID: 3458
	public float SpringForce;

	// Token: 0x04000D83 RID: 3459
	public float DamperBumpForce;

	// Token: 0x04000D84 RID: 3460
	public float DamperReboundForce;

	// Token: 0x04000D85 RID: 3461
	public CoilNut coilnut;

	// Token: 0x04000D86 RID: 3462
	public bool Gearbox;

	// Token: 0x04000D87 RID: 3463
	public bool Manual;

	// Token: 0x04000D88 RID: 3464
	public GearProfile TransmissionGearingProfile;

	// Token: 0x04000D89 RID: 3465
	public GearProfile TransmissionGearingbroken1;

	// Token: 0x04000D8A RID: 3466
	public GearProfile TransmissionGearingbroken2;

	// Token: 0x04000D8B RID: 3467
	public GearProfile TransmissionGearingbroken3;

	// Token: 0x04000D8C RID: 3468
	public bool Shifter;

	// Token: 0x04000D8D RID: 3469
	public Mesh Reverse;

	// Token: 0x04000D8E RID: 3470
	public Mesh N;

	// Token: 0x04000D8F RID: 3471
	public Mesh R;

	// Token: 0x04000D90 RID: 3472
	public Mesh P;

	// Token: 0x04000D91 RID: 3473
	public Mesh First;

	// Token: 0x04000D92 RID: 3474
	public Mesh Second;

	// Token: 0x04000D93 RID: 3475
	public Mesh Third;

	// Token: 0x04000D94 RID: 3476
	public Mesh Fourth;

	// Token: 0x04000D95 RID: 3477
	public Mesh Fifth;

	// Token: 0x04000D96 RID: 3478
	public bool Chain;

	// Token: 0x04000D97 RID: 3479
	public bool ChainSprocket;

	// Token: 0x04000D98 RID: 3480
	public bool WheelSprocket;

	// Token: 0x04000D99 RID: 3481
	public bool DriveShaft;

	// Token: 0x04000D9A RID: 3482
	public bool DriveShaftFront;

	// Token: 0x04000D9B RID: 3483
	public bool AxleFront;

	// Token: 0x04000D9C RID: 3484
	public bool AxleRear;

	// Token: 0x04000D9D RID: 3485
	public bool Diff;

	// Token: 0x04000D9E RID: 3486
	public float DiffRatio;

	// Token: 0x04000D9F RID: 3487
	public bool DiffLocked;

	// Token: 0x04000DA0 RID: 3488
	public bool DiffFront;

	// Token: 0x04000DA1 RID: 3489
	public bool TransferCase;

	// Token: 0x04000DA2 RID: 3490
	public bool Flywheel;

	// Token: 0x04000DA3 RID: 3491
	public bool Clutch;

	// Token: 0x04000DA4 RID: 3492
	public float ClutchSlipTorque;

	// Token: 0x04000DA5 RID: 3493
	public bool ClutchCover;

	// Token: 0x04000DA6 RID: 3494
	public bool ClutchPedal;

	// Token: 0x04000DA7 RID: 3495
	public bool GasPedal;

	// Token: 0x04000DA8 RID: 3496
	public bool FuelLine;

	// Token: 0x04000DA9 RID: 3497
	public bool FuelTank;

	// Token: 0x04000DAA RID: 3498
	public FLUID Fuel;

	// Token: 0x04000DAB RID: 3499
	public bool FuelPump;

	// Token: 0x04000DAC RID: 3500
	public bool IgnitionCoil;

	// Token: 0x04000DAD RID: 3501
	public bool SparkPlug;

	// Token: 0x04000DAE RID: 3502
	public bool SparkWires;

	// Token: 0x04000DAF RID: 3503
	public bool Distributor;

	// Token: 0x04000DB0 RID: 3504
	public bool Carburetor;

	// Token: 0x04000DB1 RID: 3505
	public bool Turbo;

	// Token: 0x04000DB2 RID: 3506
	public bool TurboPipe;

	// Token: 0x04000DB3 RID: 3507
	public bool Battery;

	// Token: 0x04000DB4 RID: 3508
	public float BatteryCharge;

	// Token: 0x04000DB5 RID: 3509
	public bool BatteryWires;

	// Token: 0x04000DB6 RID: 3510
	public bool Alternator;

	// Token: 0x04000DB7 RID: 3511
	public bool AlternatorPulley;

	// Token: 0x04000DB8 RID: 3512
	public bool Starter;

	// Token: 0x04000DB9 RID: 3513
	public bool CamshaftSprocket;

	// Token: 0x04000DBA RID: 3514
	public bool CrankshaftSprocket;

	// Token: 0x04000DBB RID: 3515
	public bool HarmonicBalancer;

	// Token: 0x04000DBC RID: 3516
	public bool CrankshaftPulley;

	// Token: 0x04000DBD RID: 3517
	public bool AlternatorBelt;

	// Token: 0x04000DBE RID: 3518
	public bool BlowerPulley;

	// Token: 0x04000DBF RID: 3519
	public bool Blower;

	// Token: 0x04000DC0 RID: 3520
	public bool BlowerBelt;

	// Token: 0x04000DC1 RID: 3521
	public bool GlowPlug;

	// Token: 0x04000DC2 RID: 3522
	public bool FuelFilter;

	// Token: 0x04000DC3 RID: 3523
	public bool Injector;

	// Token: 0x04000DC4 RID: 3524
	public bool FuelHoses;

	// Token: 0x04000DC5 RID: 3525
	public bool GlowPlugRelay;

	// Token: 0x04000DC6 RID: 3526
	public bool DieselEngine;

	// Token: 0x04000DC7 RID: 3527
	public bool EngineBlock;

	// Token: 0x04000DC8 RID: 3528
	public bool AirCooled;

	// Token: 0x04000DC9 RID: 3529
	public bool DoubleHeads;

	// Token: 0x04000DCA RID: 3530
	public float EngineDisplacement;

	// Token: 0x04000DCB RID: 3531
	public float Power;

	// Token: 0x04000DCC RID: 3532
	public float EngineCylinderCount;

	// Token: 0x04000DCD RID: 3533
	public bool EngineHead;

	// Token: 0x04000DCE RID: 3534
	public bool EngineHead2;

	// Token: 0x04000DCF RID: 3535
	public bool HeadGasket;

	// Token: 0x04000DD0 RID: 3536
	public bool HeadGasket2;

	// Token: 0x04000DD1 RID: 3537
	public bool HeadCover;

	// Token: 0x04000DD2 RID: 3538
	public bool HeadCover2;

	// Token: 0x04000DD3 RID: 3539
	public bool Rockers;

	// Token: 0x04000DD4 RID: 3540
	public bool Rockers2;

	// Token: 0x04000DD5 RID: 3541
	public AudioClip EngineIdling;

	// Token: 0x04000DD6 RID: 3542
	public AudioClip EngineRunning;

	// Token: 0x04000DD7 RID: 3543
	public AudioClip EngineRunningNoExhaust;

	// Token: 0x04000DD8 RID: 3544
	public bool AirFilter;

	// Token: 0x04000DD9 RID: 3545
	public bool AirFilterCover;

	// Token: 0x04000DDA RID: 3546
	public bool Crankshaft;

	// Token: 0x04000DDB RID: 3547
	public bool EngineChain;

	// Token: 0x04000DDC RID: 3548
	public bool Camshaft;

	// Token: 0x04000DDD RID: 3549
	public bool Piston;

	// Token: 0x04000DDE RID: 3550
	public bool Radiator;

	// Token: 0x04000DDF RID: 3551
	public bool RadiatorFan;

	// Token: 0x04000DE0 RID: 3552
	public bool RadiatorGT;

	// Token: 0x04000DE1 RID: 3553
	public bool RadiatorFanGT;

	// Token: 0x04000DE2 RID: 3554
	public bool WaterPumpPulley;

	// Token: 0x04000DE3 RID: 3555
	public bool WaterPump;

	// Token: 0x04000DE4 RID: 3556
	public bool WaterPumpBelt;

	// Token: 0x04000DE5 RID: 3557
	public bool WaterHoseUpper;

	// Token: 0x04000DE6 RID: 3558
	public bool WaterHoseLower;

	// Token: 0x04000DE7 RID: 3559
	public bool ThermostatHousing;

	// Token: 0x04000DE8 RID: 3560
	public FLUID Coolant;

	// Token: 0x04000DE9 RID: 3561
	public bool OilPan;

	// Token: 0x04000DEA RID: 3562
	public bool OilFilter;

	// Token: 0x04000DEB RID: 3563
	public FLUID EngineOil;

	// Token: 0x04000DEC RID: 3564
	public bool Exhaust;

	// Token: 0x04000DED RID: 3565
	public bool ExhaustHeader;

	// Token: 0x04000DEE RID: 3566
	public bool ExhaustManifold;

	// Token: 0x04000DEF RID: 3567
	public bool ExhaustSmoke;

	// Token: 0x04000DF0 RID: 3568
	public bool ExhaustHeaderSmoke;

	// Token: 0x04000DF1 RID: 3569
	public bool ExhaustManifoldSmoke;

	// Token: 0x04000DF2 RID: 3570
	public bool HeadSmoke;

	// Token: 0x04000DF3 RID: 3571
	public bool Exhaust2;

	// Token: 0x04000DF4 RID: 3572
	public bool ExhaustHeader2;

	// Token: 0x04000DF5 RID: 3573
	public bool ExhaustManifold2;

	// Token: 0x04000DF6 RID: 3574
	public bool ExhaustSmoke2;

	// Token: 0x04000DF7 RID: 3575
	public bool ExhaustHeaderSmoke2;

	// Token: 0x04000DF8 RID: 3576
	public bool ExhaustManifoldSmoke2;

	// Token: 0x04000DF9 RID: 3577
	public bool HeadSmoke2;

	// Token: 0x04000DFA RID: 3578
	public bool Cluster;

	// Token: 0x04000DFB RID: 3579
	public bool AnalogRpmGauge;

	// Token: 0x04000DFC RID: 3580
	public bool DigitalRpmGauge;

	// Token: 0x04000DFD RID: 3581
	public bool AnalogSpeedGauge;

	// Token: 0x04000DFE RID: 3582
	public bool DigitalSpeedGauge;

	// Token: 0x04000DFF RID: 3583
	public bool TempGauge;

	// Token: 0x04000E00 RID: 3584
	public bool FuelGauge;

	// Token: 0x04000E01 RID: 3585
	public bool BatteryGauge;

	// Token: 0x04000E02 RID: 3586
	public bool Clock;

	// Token: 0x04000E03 RID: 3587
	public Text MileageText;

	// Token: 0x04000E04 RID: 3588
	public float ClusterMileage;

	// Token: 0x04000E05 RID: 3589
	public GameObject ClusterL;

	// Token: 0x04000E06 RID: 3590
	public GameObject ClusterR;

	// Token: 0x04000E07 RID: 3591
	public GameObject ClusterBat;

	// Token: 0x04000E08 RID: 3592
	public GameObject ClusterHigh;

	// Token: 0x04000E09 RID: 3593
	public GameObject ClusterABS;

	// Token: 0x04000E0A RID: 3594
	public GameObject ClusterCheck;

	// Token: 0x04000E0B RID: 3595
	public GameObject ClusterGlowPlugs;

	// Token: 0x04000E0C RID: 3596
	public float FluidSize;

	// Token: 0x04000E0D RID: 3597
	public float DieselPercent;

	// Token: 0x04000E0E RID: 3598
	public float FluidCondition;

	// Token: 0x04000E0F RID: 3599
	public bool DMGdisplacepart;

	// Token: 0x04000E10 RID: 3600
	public bool DMGdeformMesh;

	// Token: 0x04000E11 RID: 3601
	public bool DMGRemovablepart;

	// Token: 0x04000E12 RID: 3602
	public bool DMGAnyDamag;

	// Token: 0x04000E13 RID: 3603
	public bool Damaged;

	// Token: 0x04000E14 RID: 3604
	public bool Ruined;

	// Token: 0x04000E15 RID: 3605
	public bool MeshDamaged;

	// Token: 0x04000E16 RID: 3606
	public bool MeshLittleDamaged;

	// Token: 0x04000E17 RID: 3607
	public float DamageReceived;

	// Token: 0x04000E18 RID: 3608
	public Mesh RemovedMesh;

	// Token: 0x04000E19 RID: 3609
	public Mesh WornMesh;

	// Token: 0x04000E1A RID: 3610
	public Mesh RuinedMesh;

	// Token: 0x04000E1B RID: 3611
	public Mesh damagedMesh;

	// Token: 0x04000E1C RID: 3612
	public Material WornMaterial;

	// Token: 0x04000E1D RID: 3613
	public Material RuinedMaterial;

	// Token: 0x04000E1E RID: 3614
	public Material OldMaterial;

	// Token: 0x04000E1F RID: 3615
	public bool PartIsOld;

	// Token: 0x04000E20 RID: 3616
	public int BodyMatNumber;

	// Token: 0x04000E21 RID: 3617
	public bool ConditionMatters;

	// Token: 0x04000E22 RID: 3618
	public float Condition;

	// Token: 0x04000E23 RID: 3619
	public float RealCondition;

	// Token: 0x04000E24 RID: 3620
	public float CleanRatio;

	// Token: 0x04000E25 RID: 3621
	public float NoRustRatio;

	// Token: 0x04000E26 RID: 3622
	public float PaintRatio;

	// Token: 0x04000E27 RID: 3623
	public bool PaintIsSet;

	// Token: 0x04000E28 RID: 3624
	private float StartX;

	// Token: 0x04000E29 RID: 3625
	private float StartY;

	// Token: 0x04000E2A RID: 3626
	private float StartZ;

	// Token: 0x04000E2B RID: 3627
	private float StartXr;

	// Token: 0x04000E2C RID: 3628
	private float StartYr;

	// Token: 0x04000E2D RID: 3629
	private float StartZr;

	// Token: 0x04000E2E RID: 3630
	public bool RepairSpot;

	// Token: 0x04000E2F RID: 3631
	public LiftHandle BikeStand;

	// Token: 0x04000E30 RID: 3632
	public int x;

	// Token: 0x04000E31 RID: 3633
	private Mesh mesh;

	// Token: 0x04000E32 RID: 3634
	private Vector3[] vertices;

	// Token: 0x04000E33 RID: 3635
	public Vector3[] initialvertices;

	// Token: 0x04000E34 RID: 3636
	public Vector3[] Damagedvertices;

	// Token: 0x04000E35 RID: 3637
	public CarProperties ChildDamag;

	// Token: 0x04000E36 RID: 3638
	public string Texture1;

	// Token: 0x04000E37 RID: 3639
	public string Texture2;

	// Token: 0x04000E38 RID: 3640
	public string Texture3;

	// Token: 0x04000E39 RID: 3641
	public string Texture4;

	// Token: 0x04000E3A RID: 3642
	public List<string> Loose = new List<string>();

	// Token: 0x04000E3B RID: 3643
	public float tightnuts;

	// Token: 0x04000E3C RID: 3644
	public float fixedwelds;

	// Token: 0x04000E3D RID: 3645
	public float fixedImportantBolts;
}
