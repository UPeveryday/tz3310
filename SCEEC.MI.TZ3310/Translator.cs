﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCEEC.MI.TZ3310
{
    public static class Translator
    {
        public static List<MeasurementItemStruct> getJobListMeasurementItems(JobList jobList)
        {
            List<MeasurementItemStruct> miList = new List<MeasurementItemStruct>();
            //miList.Add(MeasurementItemStruct.CreateInformation("添加使用用户信息模块"));//问题

            int TapNum = (jobList.Transformer.OLTC.TapNum - 1) / 2;
            int mulTapNum = (jobList.Transformer.OLTC.MulTapNum - 1) / 2;
            if (jobList.Transformer.OLTC.Contained == true)
                miList.Add(MeasurementItemStruct.CreateText("将变压器有载分接开关位置切换到额定分接(分接" + (TapNum + 1).ToString() + "B)位置;"));

            if (jobList.DCInsulation.Enabled || jobList.Bushing.DCInsulation)
            {
                miList.Add(MeasurementItemStruct.CreateText("使用绝缘电阻试验模块："));
                if (jobList.DCInsulation.HVEnabled)
                    miList.Add(MeasurementItemStruct.CreateDCInsulationMeasurementItem(WindingType.HV));
                if ((jobList.Bushing.DCInsulation) && (jobList.Transformer.Bushing.HVContained))
                {
                    if (jobList.Transformer.WindingConfig.HV == TransformerWindingConfigName.Yn)
                        miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.HV, WindingTerimal.O));
                    miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.HV, WindingTerimal.A));
                    miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.HV, WindingTerimal.B));
                    miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.HV, WindingTerimal.C));
                }
                if (jobList.DCInsulation.MVEnabled)
                    miList.Add(MeasurementItemStruct.CreateDCInsulationMeasurementItem(WindingType.MV));
                if ((jobList.Bushing.DCInsulation) && (jobList.Transformer.Bushing.MVContained))
                {
                    if (jobList.Transformer.WindingConfig.MV == TransformerWindingConfigName.Yn)
                        miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.MV, WindingTerimal.O));
                    miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.MV, WindingTerimal.A));
                    miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.MV, WindingTerimal.B));
                    miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.MV, WindingTerimal.C));
                }
                if (jobList.DCInsulation.LVEnabled)
                    miList.Add(MeasurementItemStruct.CreateDCInsulationMeasurementItem(WindingType.LV));
            }

            if (jobList.Capacitance.Enabled || jobList.Bushing.Capacitance)
            {
                miList.Add(MeasurementItemStruct.CreateText("使用电容量及介质损耗试验模块："));
                if (jobList.Capacitance.HVEnabled)
                    miList.Add(MeasurementItemStruct.CreateCapacitanceMeasurementItem(WindingType.HV));
                if ((jobList.Bushing.Capacitance) && (jobList.Transformer.Bushing.HVContained))
                {
                    if (jobList.Transformer.WindingConfig.HV == TransformerWindingConfigName.Yn)
                        miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.HV, WindingTerimal.O));
                    miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.HV, WindingTerimal.A));
                    miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.HV, WindingTerimal.B));
                    miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.HV, WindingTerimal.C));
                }
                if (jobList.Capacitance.MVEnabled)
                    miList.Add(MeasurementItemStruct.CreateCapacitanceMeasurementItem(WindingType.MV));
                if ((jobList.Bushing.Capacitance) && (jobList.Transformer.Bushing.MVContained))
                {
                    if (jobList.Transformer.WindingConfig.MV == TransformerWindingConfigName.Yn)
                        miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.MV, WindingTerimal.O));
                    miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.MV, WindingTerimal.A));
                    miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.MV, WindingTerimal.B));
                    miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.MV, WindingTerimal.C));
                }
                if (jobList.Capacitance.LVEnabled)
                    miList.Add(MeasurementItemStruct.CreateCapacitanceMeasurementItem(WindingType.LV));
            }

            if (jobList.DCResistance.Enabled)
            {
                //miList.Add(MeasurementItemStruct.CreateText("使用直流电阻试验模块："));
                if ((jobList.DCResistance.HVEnabled) && (!((jobList.OLTC.Enabled) && (jobList.Transformer.OLTC.WindingPosition == WindingType.HV) && (jobList.OLTC.DCResistance))))
                {
                    if ((jobList.Transformer.PhaseNum == 3))
                    {
                        if (jobList.Transformer.WindingConfig.HV == TransformerWindingConfigName.Yn)
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.HV));
                        }
                        else
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.HV, WindingTerimal.A, WindingTerimal.B));
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.HV, WindingTerimal.B, WindingTerimal.C));
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.HV, WindingTerimal.C, WindingTerimal.A));
                        }
                    }
                    else
                    {
                        miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.HV, WindingTerimal.A, WindingTerimal.O));
                    }
                }
                if ((jobList.DCResistance.MVEnabled) && (!((jobList.OLTC.Enabled) && (jobList.Transformer.OLTC.WindingPosition == WindingType.MV) && (jobList.OLTC.DCResistance))))
                {
                    if ((jobList.Transformer.PhaseNum == 3))
                    {
                        if (jobList.Transformer.WindingConfig.MV == TransformerWindingConfigName.Yn)
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.MV));
                        }
                        else
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.MV, WindingTerimal.A, WindingTerimal.B));
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.MV, WindingTerimal.B, WindingTerimal.C));
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.MV, WindingTerimal.C, WindingTerimal.A));
                        }
                    }
                    else
                    {
                        miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.MV, WindingTerimal.A, WindingTerimal.O));
                    }
                }
                if (jobList.DCResistance.LVEnabled)
                {
                    if ((jobList.Transformer.PhaseNum == 3))
                    {
                        if (jobList.Transformer.WindingConfig.LV == TransformerWindingConfigName.Yn)
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.LV));
                        }
                        else
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.LV, WindingTerimal.A, WindingTerimal.B));
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.LV, WindingTerimal.B, WindingTerimal.C));
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.LV, WindingTerimal.C, WindingTerimal.A));
                        }
                    }
                    else
                    {
                        miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.LV, WindingTerimal.A, WindingTerimal.O));
                    }
                }
            }

            if (jobList.OLTC.Enabled)
            {
                int range = jobList.OLTC.Range;
                int mulrange = jobList.OLTC.MulRange;
                int lowest = mulTapNum - mulrange + 1;
                int highest = TapNum + range + 1;
                miList.Add(MeasurementItemStruct.CreateText("使用直阻与有载分接试验模块："));

                int i = TapNum + 1;
                char j = (char)((int)'A' + (jobList.Transformer.OLTC.TapMainNum - 1) / 2);
                int k;
                string lastTapName = i.ToString();
                string currentTapName;

                if (jobList.Transformer.OLTC.TapMainNum > 1)
                {
                    for (k = (jobList.Transformer.OLTC.TapMainNum - 1) / 2; k > 0; k--)
                    {
                        j = (char)((int)'A' + k);
                        lastTapName = i.ToString() + j.ToString();
                        currentTapName = i.ToString() + ((char)(j - 1)).ToString();
                        if (jobList.OLTC.DCResistance == true)
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName));
                        }
                        if (jobList.OLTC.SwitchingCharacter)
                        {
                            if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                                miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.HV));
                            if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                                miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.MV));
                        }
                        lastTapName = currentTapName;
                    }
                }

                for (i = TapNum; i >= lowest; i--)
                {
                    currentTapName = i.ToString();
                    if (jobList.OLTC.DCResistance)
                    {
                        miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName));
                    }
                    if (jobList.OLTC.SwitchingCharacter)
                    {
                        if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                            miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.HV));
                        if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                            miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.MV));
                    }
                    lastTapName = currentTapName;
                }

                if (jobList.OLTC.DCResistance)
                {
                    miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName));
                }

                for (i = lowest + 1; i <= TapNum; i++)
                {
                    currentTapName = i.ToString();
                    //if (jobList.OLTC.DCResistance)
                    //{
                    //    miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName));
                    //}
                    if (jobList.OLTC.SwitchingCharacter)
                    {
                        if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                            miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.HV));
                        if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                            miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.MV));
                    }
                    lastTapName = currentTapName;
                }

                i = TapNum + 1;

                if (jobList.Transformer.OLTC.TapMainNum > 1)
                {
                    for (k = 1; k <= jobList.Transformer.OLTC.TapMainNum; k++)
                    {
                        j = (char)((int)'A' + k - 1);
                        currentTapName = i.ToString() + j.ToString();
                        //if (jobList.OLTC.DCResistance)
                        //{
                        //    miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName));
                        //}
                        if (jobList.OLTC.SwitchingCharacter)
                        {
                            if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                                miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.HV));
                            if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                                miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.MV));
                        }
                        lastTapName = currentTapName;
                    }
                }
                else
                {
                    currentTapName = i.ToString();
                    //if (jobList.OLTC.DCResistance)
                    //{
                    //    miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName));
                    //}
                    if (jobList.OLTC.SwitchingCharacter)
                    {
                        if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                            miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.HV));
                        if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                            miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.MV));
                    }
                    lastTapName = currentTapName;
                }

                for (i = TapNum + 2; i <= highest; i++)
                {
                    currentTapName = i.ToString();
                    if (jobList.OLTC.DCResistance)
                    {
                        miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName));
                    }
                    if (jobList.OLTC.SwitchingCharacter)
                    {
                        if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                            miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.HV));
                        if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                            miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.MV));
                    }
                    lastTapName = currentTapName;
                }

                if (jobList.OLTC.DCResistance)
                {
                    miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName));
                }

                for (i = highest - 1; i > (TapNum + 1); i--)
                {
                    currentTapName = i.ToString();
                    //if (jobList.OLTC.DCResistance)
                    //{
                    //    miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName));
                    //}
                    if (jobList.OLTC.SwitchingCharacter)
                    {
                        if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                            miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.HV));
                        if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                            miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.MV));
                    }
                    lastTapName = currentTapName;
                }

                i = TapNum + 1;

                if (jobList.Transformer.OLTC.TapMainNum > 1)
                {
                    for (k = jobList.Transformer.OLTC.TapMainNum; k > ((jobList.Transformer.OLTC.TapMainNum - 1) / 2); k--)
                    {
                        j = (char)((int)'A' + k - 1);
                        currentTapName = i.ToString() + j.ToString();
                        //if (jobList.OLTC.DCResistance)
                        //{
                        //    miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName));
                        //}
                        if (jobList.OLTC.SwitchingCharacter)
                        {
                            if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                                miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.HV));
                            if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                                miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.MV));
                        }
                        lastTapName = currentTapName;
                    }
                }
                else
                {
                    currentTapName = i.ToString();
                    //if (jobList.OLTC.DCResistance)
                    //{
                    //    miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName));
                    //}
                    if (jobList.OLTC.SwitchingCharacter)
                    {
                        if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                            miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.HV));
                        if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                            miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, lastTapName, currentTapName, jobList.Transformer.WindingConfig.MV));
                    }
                    lastTapName = currentTapName;
                }


            }

            if (jobList.LossDcresistance)
            {
                miList.Add(MeasurementItemStruct.CreateOtherMeasurementItem(1));
            }
            if (jobList.Shortcircuitimpedance)
            {
                miList.Add(MeasurementItemStruct.CreateOtherMeasurementItem(3));
            }
            if (jobList.CoreDCInsulation)
            {
                miList.Add(MeasurementItemStruct.CreateOtherMeasurementItem(0));
            }
            return miList;
        }

        //JobList2MeasurementItems  //getJobListMeasurementItems
        public static List<MeasurementItemStruct> JobList2MeasurementItems(JobList jobList)
        {
            List<MeasurementItemStruct> miList = new List<MeasurementItemStruct>();
            //miList.Add(MeasurementItemStruct.CreateInformation("添加使用用户信息模块"));//问题

            int TapNum = (jobList.Transformer.OLTC.TapNum - 1) / 2;
            int mulTapNum = (jobList.Transformer.OLTC.MulTapNum - 1) / 2;
            int TapLocation = mulTapNum + 1;
            int TapMainNum = jobList.Transformer.OLTC.TapMainNum;



            if (jobList.OLTC.Enabled)
            {
                int lowest = TapLocation - jobList.OLTC.MulRange;
                int highest = TapLocation + jobList.OLTC.Range;
                if (jobList.OLTC.SwitchingCharacter)
                {
                    miList.Add(MeasurementItemStruct.CreateText("将变压器有载分接开关位置切换到额定(分接" + lowest + ")位置;", lowest.ToString()));
                    for (int i = lowest; i < highest; i++)
                    {
                        getStartAndEndMessage(i, TapLocation, TapMainNum, jobList, miList);
                        if (i != TapLocation && i != TapLocation - 1 || TapMainNum <= 1)
                        {
                            if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                                miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, i.ToString(), (i + 1).ToString(), jobList.Transformer.WindingConfig.HV));
                            if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                                miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, i.ToString(), (i + 1).ToString(), jobList.Transformer.WindingConfig.MV));
                        }
                    }
                    for (int i = highest; i > lowest; i--)
                    {
                        getStartAndEndMessageReserver(i, TapLocation, TapMainNum, jobList, miList);
                        if (i != TapLocation && i != TapLocation + 1 || TapMainNum <= 1)
                        {
                            if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                                miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, i.ToString(), (i - 1).ToString(), jobList.Transformer.WindingConfig.HV));
                            if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                                miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, i.ToString(), (i - 1).ToString(), jobList.Transformer.WindingConfig.MV));
                        }
                    }
                }
                if (jobList.OLTC.DCResistance)
                {
                    miList.Add(MeasurementItemStruct.CreateText("分接位置直流电阻试验，请确认有载分接开关在(分接" + lowest + ")位置;", lowest.ToString()));
                    if (!OltcLocationIsDorYn(jobList))
                    {
                        for (int i = lowest; i <= highest; i++)
                        {
                            if (TapMainNum > 1 && i == TapLocation)
                            {
                                for (int j = 0; j < TapMainNum; j++)
                                {
                                    miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, i.ToString() + ((char)('A' + j)).ToString()));
                                }
                            }
                            else
                                miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, i.ToString()));
                        }
                    }
                    else
                    {
                        for (int p = 0; p < 3; p++)
                        {
                            for (int i = lowest; i <= highest; i++)
                            {
                                WindingTerimal firstTerminal = WindingTerimal.A;
                                WindingTerimal SecondTerminal = WindingTerimal.B;
                                if (p == 0)
                                {
                                    firstTerminal = WindingTerimal.A;
                                    SecondTerminal = WindingTerimal.B;
                                }
                                else if (p == 1)
                                {
                                    firstTerminal = WindingTerimal.B;
                                    SecondTerminal = WindingTerimal.C;
                                }
                                else
                                {
                                    firstTerminal = WindingTerimal.C;
                                    SecondTerminal = WindingTerimal.A;
                                }

                                if (TapMainNum > 1 && i == TapLocation)
                                {
                                    for (int j = 0; j < TapMainNum; j++)
                                    {
                                        miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, firstTerminal, SecondTerminal,i.ToString()+ ((char)('A' + j)).ToString()));
                                    }
                                }
                                else
                                    miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(jobList.Transformer.OLTC.WindingPosition, firstTerminal, SecondTerminal, i.ToString()));
                            }
                        }
                    }

                }

            }
            if (jobList.DCResistance.Enabled)
            {
                //miList.Add(MeasurementItemStruct.CreateText("使用直流电阻试验模块："));
                if ((jobList.DCResistance.HVEnabled) && (!((jobList.OLTC.Enabled) && (jobList.Transformer.OLTC.WindingPosition == WindingType.HV) && (jobList.OLTC.DCResistance))))
                {
                    if ((jobList.Transformer.PhaseNum == 3))
                    {
                        if (jobList.Transformer.WindingConfig.HV == TransformerWindingConfigName.Yn)
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.HV));
                        }
                        else
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.HV, WindingTerimal.A, WindingTerimal.B));
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.HV, WindingTerimal.B, WindingTerimal.C));
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.HV, WindingTerimal.C, WindingTerimal.A));
                        }
                    }
                    else
                    {
                        miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.HV, WindingTerimal.A, WindingTerimal.O));
                    }
                }
                if ((jobList.DCResistance.MVEnabled) && (!((jobList.OLTC.Enabled) && (jobList.Transformer.OLTC.WindingPosition == WindingType.MV) && (jobList.OLTC.DCResistance))))
                {
                    if ((jobList.Transformer.PhaseNum == 3))
                    {
                        if (jobList.Transformer.WindingConfig.MV == TransformerWindingConfigName.Yn)
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.MV));
                        }
                        else
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.MV, WindingTerimal.A, WindingTerimal.B));
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.MV, WindingTerimal.B, WindingTerimal.C));
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.MV, WindingTerimal.C, WindingTerimal.A));
                        }
                    }
                    else
                    {
                        miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.MV, WindingTerimal.A, WindingTerimal.O));
                    }
                }
                if (jobList.DCResistance.LVEnabled)
                {
                    if ((jobList.Transformer.PhaseNum == 3))
                    {
                        if (jobList.Transformer.WindingConfig.LV == TransformerWindingConfigName.Yn)
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.LV));
                        }
                        else
                        {
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.LV, WindingTerimal.A, WindingTerimal.B));
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.LV, WindingTerimal.B, WindingTerimal.C));
                            miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.LV, WindingTerimal.C, WindingTerimal.A));
                        }
                    }
                    else
                    {
                        miList.Add(MeasurementItemStruct.CreateDCResistanceMeasurementItem(WindingType.LV, WindingTerimal.A, WindingTerimal.O));
                    }
                }
            }


            if (jobList.DCInsulation.Enabled || jobList.Bushing.DCInsulation)
            {
                miList.Add(MeasurementItemStruct.CreateText("使用绝缘电阻试验模块："));
                if (jobList.DCInsulation.HVEnabled)
                    miList.Add(MeasurementItemStruct.CreateDCInsulationMeasurementItem(WindingType.HV));
                if ((jobList.Bushing.DCInsulation) && (jobList.Transformer.Bushing.HVContained))
                {
                    if (jobList.Transformer.WindingConfig.HV == TransformerWindingConfigName.Yn)
                        miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.HV, WindingTerimal.O));
                    miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.HV, WindingTerimal.A));
                    miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.HV, WindingTerimal.B));
                    miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.HV, WindingTerimal.C));
                }
                if (jobList.DCInsulation.MVEnabled)
                    miList.Add(MeasurementItemStruct.CreateDCInsulationMeasurementItem(WindingType.MV));
                if ((jobList.Bushing.DCInsulation) && (jobList.Transformer.Bushing.MVContained))
                {
                    if (jobList.Transformer.WindingConfig.MV == TransformerWindingConfigName.Yn)
                        miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.MV, WindingTerimal.O));
                    miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.MV, WindingTerimal.A));
                    miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.MV, WindingTerimal.B));
                    miList.Add(MeasurementItemStruct.CreateBushingDCInsulationMeasurementItem(WindingType.MV, WindingTerimal.C));
                }
                if (jobList.DCInsulation.LVEnabled)
                    miList.Add(MeasurementItemStruct.CreateDCInsulationMeasurementItem(WindingType.LV));
            }

            if (jobList.Capacitance.Enabled || jobList.Bushing.Capacitance)
            {
                miList.Add(MeasurementItemStruct.CreateText("使用电容量及介质损耗试验模块："));
                if (jobList.Capacitance.HVEnabled)
                    miList.Add(MeasurementItemStruct.CreateCapacitanceMeasurementItem(WindingType.HV));
                if ((jobList.Bushing.Capacitance) && (jobList.Transformer.Bushing.HVContained))
                {
                    if (jobList.Transformer.WindingConfig.HV == TransformerWindingConfigName.Yn)
                        miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.HV, WindingTerimal.O));
                    miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.HV, WindingTerimal.A));
                    miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.HV, WindingTerimal.B));
                    miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.HV, WindingTerimal.C));
                }
                if (jobList.Capacitance.MVEnabled)
                    miList.Add(MeasurementItemStruct.CreateCapacitanceMeasurementItem(WindingType.MV));
                if ((jobList.Bushing.Capacitance) && (jobList.Transformer.Bushing.MVContained))
                {
                    if (jobList.Transformer.WindingConfig.MV == TransformerWindingConfigName.Yn)
                        miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.MV, WindingTerimal.O));
                    miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.MV, WindingTerimal.A));
                    miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.MV, WindingTerimal.B));
                    miList.Add(MeasurementItemStruct.CreateBushingCapacitanceMeasurementItem(WindingType.MV, WindingTerimal.C));
                }
                if (jobList.Capacitance.LVEnabled)
                    miList.Add(MeasurementItemStruct.CreateCapacitanceMeasurementItem(WindingType.LV));
            }


            if (jobList.LossDcresistance)
            {
                miList.Add(MeasurementItemStruct.CreateOtherMeasurementItem(1));
            }
            if (jobList.Shortcircuitimpedance)
            {
                miList.Add(MeasurementItemStruct.CreateOtherMeasurementItem(3));
            }
            if (jobList.CoreDCInsulation)
            {
                miList.Add(MeasurementItemStruct.CreateOtherMeasurementItem(0));
            }
            return miList;
        }


        private static bool OltcLocationIsDorYn(JobList jobList)
        {
            if (jobList.Transformer.PhaseNum == 3)
            {
                if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                {
                    if (jobList.Transformer.WindingConfig.HV == TransformerWindingConfigName.Y || jobList.Transformer.WindingConfig.HV == TransformerWindingConfigName.D)
                        return true;
                    else
                        return false;
                }
                else if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                {
                    if (jobList.Transformer.WindingConfig.MV == TransformerWindingConfigName.Y || jobList.Transformer.WindingConfig.MV == TransformerWindingConfigName.D)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (jobList.Transformer.WindingConfig.LV == TransformerWindingConfigName.Y || jobList.Transformer.WindingConfig.LV == TransformerWindingConfigName.D)
                        return true;
                    else
                        return false;
                }
            }
            else
            {
                if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                {
                    if (jobList.Transformer.WindingConfig.HV == TransformerWindingConfigName.Y || jobList.Transformer.WindingConfig.HV == TransformerWindingConfigName.D)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (jobList.Transformer.WindingConfig.MV == TransformerWindingConfigName.Y || jobList.Transformer.WindingConfig.MV == TransformerWindingConfigName.D)
                        return true;
                    else
                        return false;
                }
            }

        }

        private static void getStartAndEndMessage(int i, int TapLocation, int TapMainNum, JobList jobList, List<MeasurementItemStruct> miList)
        {
            if (i == TapLocation && TapMainNum > 1)
            {
                char tapMessage = 'A';
                for (int j = 0; j <= TapMainNum; j++)
                {
                    string firstMessage = String.Empty;
                    string endMessage = String.Empty;
                    if (j == 0)
                    {
                        firstMessage = (i - 1).ToString();
                        endMessage = i.ToString() + tapMessage;
                    }
                    else if (j == TapMainNum)
                    {
                        firstMessage = i.ToString() + (char)('A' + j - 1);
                        endMessage = (i + 1).ToString();
                    }
                    else
                    {
                        firstMessage = i.ToString() + tapMessage;
                        tapMessage = (char)('A' + j);
                        endMessage = i.ToString() + tapMessage;
                    }
                    if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                        miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, firstMessage, endMessage, jobList.Transformer.WindingConfig.HV));
                    if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                        miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, firstMessage, endMessage, jobList.Transformer.WindingConfig.MV));

                }
            }
        }


        private static void getStartAndEndMessageReserver(int i, int TapLocation, int TapMainNum, JobList jobList, List<MeasurementItemStruct> miList)
        {
            if (i == TapLocation && TapMainNum > 1)
            {
                char tapMessage = (char)('A' + TapMainNum - 1);
                for (int j = 0; j <= TapMainNum; j++)
                {
                    string firstMessage = String.Empty;
                    string endMessage = String.Empty;
                    if (j == 0)
                    {
                        firstMessage = (i + 1).ToString();
                        endMessage = i.ToString() + (char)('A' + TapMainNum - 1);
                    }
                    else if (j == TapMainNum)
                    {
                        firstMessage = i.ToString() + 'A';
                        endMessage = (i - 1).ToString();
                    }
                    else
                    {
                        firstMessage = i.ToString() + tapMessage;
                        tapMessage = (char)(tapMessage - 1);
                        endMessage = i.ToString() + tapMessage;
                    }
                    if (jobList.Transformer.OLTC.WindingPosition == WindingType.HV)
                        miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, firstMessage, endMessage, jobList.Transformer.WindingConfig.HV));
                    if (jobList.Transformer.OLTC.WindingPosition == WindingType.MV)
                        miList.Add(MeasurementItemStruct.CreateOLTCSwitchingCharacterMeasurementItem(jobList.Transformer.OLTC.WindingPosition, firstMessage, endMessage, jobList.Transformer.WindingConfig.MV));

                }
            }
        }

    }
}
