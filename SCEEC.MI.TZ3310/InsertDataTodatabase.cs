﻿using SCEEC.Numerics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SCEEC.MI.TZ3310
{
    public static class InsertDataTodatabase
    {
        public static string TestInstruments = "TZ3500";//仪器名称
                                                        // public static string InstrumentNumber = "370001";
        public static string InstrumentNumber = "370001";
        private static void CreateSampleInformation(string ResultName)
        {
            var tws = WorkingSets.local.getTestResults(ResultName);
            DataRow row = WorkingSets.local.Sample_Information.NewRow();
            row["TestCode"] = tws.job.Information.GetHashCode();
            row["StationName"] = tws.Transformer.Location;
            row["TestSite"] = WorkingSets.local.getLocation(tws.Transformer.Location).address;
            row["ExperimentalProperties"] = "例行";
            row["PrincipalUnit"] = "无";
            row["NameOfDevice"] = tws.Transformer.SerialNo;
            row["TestDate"] = tws.job.Information.testingTime;
            row["Tester"] = tws.job.Information.tester;
            row["TestUnit"] = tws.job.Information.testingAgency;
            row["Auditor"] = tws.job.Information.auditor;
            row["Approver"] = tws.job.Information.approver;
            row["PersonInCharge"] = tws.job.Information.principal;
            row["TestWeather"] = tws.job.Information.weather;
            row["Temperature"] = tws.job.Information.temperature;
            row["Humidity"] = tws.job.Information.humidity;
            row["DateOfReport"] = DateTime.Now;
            WorkingSets.local.Sample_Information.Rows.Add(row);
            WorkingSets.local.SaveCreateLocateDatabase();
            //OperationNumber
        }
        public static void CreateParameterInformation(string ResultName)
        {
            var tws = WorkingSets.local.getTestResults(ResultName);
            DataRow row = WorkingSets.local.Parameter_Information.NewRow();
            row["TestCode"] = tws.job.Information.GetHashCode();
            row["FactoryName"] = tws.Transformer.Manufacturer;
            row["Model"] = "370001";
            row["DateOfProduction"] = tws.Transformer.ProductionYear;
            row["ExitNumber"] = tws.Transformer.SerialNo;
            row["RatedVoltage"] = tws.job.Transformer.VoltageRating.HV;
            row["RatedCapacitance"] = tws.job.Transformer.PowerRating.HV;
            row["PhaseNumber"] = tws.Transformer.PhaseNum;
            row["VoltageCombination"] = tws.job.Transformer.VoltageRating.HV + "/" + tws.job.Transformer.VoltageRating.MV + "/" +
                tws.job.Transformer.VoltageRating.LV;
            row["CapacityCombination"] = tws.job.Transformer.PowerRating.HV + "/" + tws.job.Transformer.PowerRating.MV + "/" +
                tws.job.Transformer.PowerRating.LV;
            row["DifferentWiring"] = tws.job.Transformer.WindingConfig.HV + "/" + tws.job.Transformer.WindingConfig.LV
                + "/" + tws.job.Transformer.WindingConfig.LVLabel + "/" + tws.job.Transformer.WindingConfig.MV + "/" +
                tws.job.Transformer.WindingConfig.MVLabel;//连结组标号
            row["Model"] = tws.Transformer.SerialNo;
            row["PhaseNumber"] = tws.Transformer.PhaseNum;
            //  row["RatedCapacitance"] = tws.job.Information.testingName;
            //  row["VoltageCombination"] = tws.job.Information.testingAgency;
            //row["DifferentWiring"] = tws.job.Information.auditor; 
            //row["CapacityCombination"] = tws.job.Information.approver;
            //row["CurrentCombination"] = tws.job.Information.principal;
            row["ImpedanceHighAlignment"] = tws.Transformer.transformermessage.Impedancevoltagehv;
            row["ImpedanceHighToLow"] = tws.Transformer.transformermessage.Impedancevoltagemv;
            row["ImpedanceMediumToLow"] = tws.Transformer.transformermessage.Impedancevoltagelv;
            row["LoadHighAlignment"] = tws.Transformer.transformermessage.Theloadlosshv;
            row["LoadHighToLow"] = tws.Transformer.transformermessage.Theloadlossmv;
            row["LoadMediumToLow"] = tws.Transformer.transformermessage.Theloadlosslv;
            row["No_LoadLoss"] = tws.Transformer.transformermessage.Noloadloss;
            row["No_LoadCurrent"] = tws.Transformer.transformermessage.Noloadcurrent;
            WorkingSets.local.Parameter_Information.Rows.Add(row);
            if (!WorkingSets.local.SaveCreateLocateDatabase())
            {
                string err = WorkingSets.local.LocalSQLClient.ErrorText;
                err = err;
            }
        }
        private static void CreateTestResult(string ResultName)
        {
            var tws = WorkingSets.local.getTestResultsbyid(ResultName);
            if (tws.MeasurementItems != null)
            {
                int ms = tws.MeasurementItems.Length;
                if (tws.Transformer.PhaseNum == 3)
                {
                    // CreateTableHead(ResultName);
                    #region 数据 行
                    //兆欧表
                    DataRow rowDCInsulation = WorkingSets.local.Insulationresistance_Threewinding.NewRow();
                    DataRow rowDCInsulationAbs = WorkingSets.local.Insulationresistance_Threewinding.NewRow();
                    DataRow rowDCInsulationMax = WorkingSets.local.Insulationresistance_Threewinding.NewRow();
                    DataRow rowResultDCInsulation = WorkingSets.local.Insulationresistance_Threewindingresults.NewRow();
                    //介损
                    DataRow CapacitanceRowCapacitance = WorkingSets.local.Dielectriclossandcapacitance_Threewinding.NewRow();
                    DataRow CnRowCapacitance = WorkingSets.local.Dielectriclossandcapacitance_Threewinding.NewRow();
                    DataRow rowResultCapacitance = WorkingSets.local.Dielectriclossandcapacitance_Threewindingresults.NewRow();
                    //套管介损
                    DataRow CapacitanceRowBushingCapacitance = WorkingSets.local.Casingtest_Commonbody.NewRow();
                    DataRow CnRowBushingCapacitance = WorkingSets.local.Casingtest_Commonbody.NewRow();
                    DataRow rowResultBushingCapacitance = WorkingSets.local.Casingtest_Commonbodyresults.NewRow();
                    //套管兆欧表
                    DataRow rowBushingDCInsulation = WorkingSets.local.Casingtest_Commonbody.NewRow();
                    DataRow rowResultBushingDCInsulation = WorkingSets.local.Casingtest_Commonbodyresults.NewRow();
                    #endregion
                    DataRow rowHighResult = WorkingSets.local.Dcresistor_Highpressureresults.NewRow();
                    DataRow rowMResult = WorkingSets.local.Dcresistor_Mediumvoltageresults.NewRow();
                    DataRow rowLowResult = WorkingSets.local.Dcresistor_Lowpressureresults.NewRow();

                    for (int i = 0; i < ms; i++)
                    {
                        if (tws.MeasurementItems[i].Result != null)
                        {
                            if (tws.MeasurementItems[i].Result.Function == MeasurementFunction.DCInsulation)
                            {

                                if (tws.MeasurementItems[i].Winding == WindingType.HV)
                                {
                                    rowDCInsulation["High_CentreLowPressure"] = tws.MeasurementItems[i].Result.values[1].OriginText;
                                    rowDCInsulationAbs["High_CentreLowPressure"] = tws.MeasurementItems[i].Result.values[2].OriginText;
                                    rowDCInsulationMax["High_CentreLowPressure"] = tws.MeasurementItems[i].Result.values[3].OriginText;
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.MV)
                                {
                                    rowDCInsulation["Centre_HighLowPressure"] = tws.MeasurementItems[i].Result.values[1].OriginText;
                                    rowDCInsulationAbs["Centre_HighLowPressure"] = tws.MeasurementItems[i].Result.values[2].OriginText;
                                    rowDCInsulationMax["Centre_HighLowPressure"] = tws.MeasurementItems[i].Result.values[3].OriginText;
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.LV)
                                {
                                    rowDCInsulation["Low_HighMediumVoltage"] = tws.MeasurementItems[i].Result.values[1].OriginText;
                                    rowDCInsulationAbs["Low_HighMediumVoltage"] = tws.MeasurementItems[i].Result.values[2].OriginText;
                                    rowDCInsulationMax["Low_HighMediumVoltage"] = tws.MeasurementItems[i].Result.values[3].OriginText;
                                }
                                rowDCInsulation["TestCode"] = tws.job.Information.GetHashCode();
                                rowDCInsulationAbs["TestCode"] = tws.job.Information.GetHashCode();
                                rowDCInsulationMax["TestCode"] = tws.job.Information.GetHashCode();
                            }
                            if (tws.MeasurementItems[i].Result.Function == MeasurementFunction.Capacitance)
                            {

                                if (tws.MeasurementItems[i].Winding == WindingType.HV)
                                {
                                    CnRowCapacitance["TestCode"] = tws.job.Information.GetHashCode();

                                    if (tws.MeasurementItems[i].Result.values[3] == null)
                                        CnRowCapacitance["HighPressure_CentreLowPressure"] = tws.MeasurementItems[i].Result.values[3].OriginText;
                                    else if (tws.MeasurementItems[i].Result.values[3].PhysicalVariableType == Numerics.Quantities.QuantityName.Length)
                                        CnRowCapacitance["HighPressure_CentreLowPressure"] = SCEEC.Numerics.NumericsConverter.Value2Text((double)tws.MeasurementItems[i].Result.values[3].value / 1000, 4, -5, "", "", percentage: true, usePrefix: false).Trim();
                                    else
                                        CnRowCapacitance["HighPressure_CentreLowPressure"] = SCEEC.Numerics.NumericsConverter.Value2Text((double)tws.MeasurementItems[i].Result.values[3].value, 4, -5, "", "", percentage: true, usePrefix: false).Trim();

                                    CapacitanceRowCapacitance["HighPressure_CentreLowPressure"] = tws.MeasurementItems[i].Result.values[2].ToString(); //兆欧表套管
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.MV)
                                {
                                    CnRowCapacitance["TestCode"] = tws.job.Information.GetHashCode();

                                    if (tws.MeasurementItems[i].Result.values[3] == null)
                                        CnRowCapacitance["MediumVoltage_HighLowPressure"] = tws.MeasurementItems[i].Result.values[3].OriginText;
                                    else if (tws.MeasurementItems[i].Result.values[3].PhysicalVariableType == Numerics.Quantities.QuantityName.Length)
                                        CnRowCapacitance["MediumVoltage_HighLowPressure"] = SCEEC.Numerics.NumericsConverter.Value2Text((double)tws.MeasurementItems[i].Result.values[3].value / 1000, 4, -5, "", "", percentage: true, usePrefix: false).Trim();
                                    else
                                        CnRowCapacitance["MediumVoltage_HighLowPressure"] = SCEEC.Numerics.NumericsConverter.Value2Text((double)tws.MeasurementItems[i].Result.values[3].value, 4, -5, "", "", percentage: true, usePrefix: false).Trim();


                                    CapacitanceRowCapacitance["MediumVoltage_HighLowPressure"] = tws.MeasurementItems[i].Result.values[2].ToString();
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.LV)
                                {
                                    if (tws.MeasurementItems[i].Result.values[3] == null)
                                        CnRowCapacitance["LowPressure_HighMediumVoltage"] = tws.MeasurementItems[i].Result.values[3].OriginText;
                                    else if (tws.MeasurementItems[i].Result.values[3].PhysicalVariableType == Numerics.Quantities.QuantityName.Length)
                                        CnRowCapacitance["LowPressure_HighMediumVoltage"] = SCEEC.Numerics.NumericsConverter.Value2Text((double)tws.MeasurementItems[i].Result.values[3].value / 1000, 4, -5, "", "", percentage: true, usePrefix: false).Trim();
                                    else
                                        CnRowCapacitance["LowPressure_HighMediumVoltage"] = SCEEC.Numerics.NumericsConverter.Value2Text((double)tws.MeasurementItems[i].Result.values[3].value, 4, -5, "", "", percentage: true, usePrefix: false).Trim();

                                    CnRowCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                                    CapacitanceRowCapacitance["LowPressure_HighMediumVoltage"] = tws.MeasurementItems[i].Result.values[2].ToString();

                                }
                            }
                            if (tws.MeasurementItems[i].Result.Function == MeasurementFunction.DCResistance)
                            {
                                if (tws.MeasurementItems[i].Winding == WindingType.HV)
                                {
                                    var data = tws.MeasurementItems[i].Result.values;
                                    DataRow rowHigh = WorkingSets.local.Dcresistor_Highpressure.NewRow();
                                    rowHigh["A0"] = data[2].OriginText;
                                    rowHigh["A0_75"] = ChangeValueToNeed.DcResistans_To_75(data[2]);
                                    rowHigh["B0"] = data[5].OriginText;
                                    rowHigh["B0_75"] = ChangeValueToNeed.DcResistans_To_75(data[5]);
                                    rowHigh["C0"] = data[8].OriginText;
                                    rowHigh["C0_75"] = ChangeValueToNeed.DcResistans_To_75(data[8]);
                                    rowHigh["TestCode"] = tws.job.Information.GetHashCode();
                                    if (isNotPhyNull(new PhysicalVariable[] { data[5], data[2] }))
                                    {
                                        rowHigh["A0_B0MutualDifference"] = ChangeValueToNeed.MutualDifference((double)data[2].value * 310 / 255, Convert.ToDouble(data[5].value) * 310 / 255);
                                    }
                                    if (tws.MeasurementItems[i].TapLabel != null)
                                        rowHigh["DCResistorHighPressure"] = tws.MeasurementItems[i].TapLabel[0];
                                    if (isNotPhyNull(new PhysicalVariable[] { data[5], data[8] }))
                                    {
                                        rowHigh["B0_C0MutualDifference"] = ChangeValueToNeed.MutualDifference((double)data[5].value * 310 / 255, Convert.ToDouble(data[8].value) * 310 / 255);
                                    }
                                    if (isNotPhyNull(new PhysicalVariable[] { data[2], data[8] }))
                                    {
                                        rowHigh["C0_A0MutualDifference"] = ChangeValueToNeed.MutualDifference((double)data[8].value * 310 / 255, Convert.ToDouble(data[2].value) * 310 / 255);
                                    }
                                    if (isNotPhyNull(new PhysicalVariable[] { data[5], data[2], data[8] }))
                                    {

                                        rowHigh["UnbalanceRate"] = ChangeValueToNeed.UnBalance(Convert.ToDouble(data[2].value * 310 / 255),
                                     Convert.ToDouble(data[5].value * 310 / 255),
                                     Convert.ToDouble(data[8].value * 310 / 255));
                                    }
                                    if (isNotPhyNull(new PhysicalVariable[] { data[2] }))
                                    {
                                        rowHigh["MaximumMutualDifference"] = ChangeValueToNeed.MaxMutualDifference((double)data[2].value * 310 / 255, (double)data[2].value * 310 / 255,
                                 (double)data[2].value * 310 / 255);
                                    }

                                    WorkingSets.local.Dcresistor_Highpressure.Rows.Add(rowHigh);
                                    WorkingSets.local.SaveCreateLocateDatabase();
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.MV)
                                {
                                    DataRow rowM = WorkingSets.local.Dcresistor_Mediumvoltage.NewRow();
                                    if (tws.MeasurementItems[i].WindingConfig == TransformerWindingConfigName.Yn)
                                    {
                                        if (tws.MeasurementItems[i].TapLabel != null)
                                            rowM["DCResistorHighPressure"] = tws.MeasurementItems[i].TapLabel[0];
                                        rowM["Am0m"] = tws.MeasurementItems[i].Result.values[2].OriginText;
                                        rowM["Am0m_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[2]);
                                        rowM["Bm0m"] = tws.MeasurementItems[i].Result.values[5].OriginText;
                                        rowM["Bm0m_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[5]);
                                        rowM["Cm0m"] = tws.MeasurementItems[i].Result.values[8].OriginText;
                                        rowM["Cm0m_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[8]);
                                        rowM["TestCode"] = tws.job.Information.GetHashCode();
                                        rowM["Am_BmMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255, (double)(tws.MeasurementItems[i].Result.values[5].value * 310 / 255));
                                        rowM["Bm_CmMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[5].value * 310 / 255, (double)(tws.MeasurementItems[i].Result.values[8].value * 310 / 255));
                                        rowM["Cm_AmMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[8].value * 310 / 255, (double)(tws.MeasurementItems[i].Result.values[2].value * 310 / 255));
                                        rowM["MaximumMutualDifference"] = ChangeValueToNeed.MaxMutualDifference((double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255, (double)tws.MeasurementItems[i].Result.values[5].value * 310 / 255,
                                       (double)tws.MeasurementItems[i].Result.values[8].value * 310 / 255);

                                    }
                                    if (tws.MeasurementItems[i].WindingConfig != TransformerWindingConfigName.Yn)
                                    {
                                        if (tws.MeasurementItems[i].Terimal != null)
                                        {
                                            if (tws.MeasurementItems[i].TapLabel != null)
                                                rowM["DCResistorHighPressure"] = tws.MeasurementItems[i].TapLabel[0];
                                            rowM["Am0m"] = tws.MeasurementItems[i].Result.values[2].OriginText;
                                            rowM["Am0m_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[2]);
                                            rowM["Bm0m"] = tws.MeasurementItems[i + 1].Result.values[5].OriginText;
                                            rowM["Bm0m_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i + 1].Result.values[5]);
                                            rowM["Cm0m"] = tws.MeasurementItems[i + 2].Result.values[8].OriginText;
                                            rowM["Cm0m_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i + 2].Result.values[8]);
                                            rowM["UnbalanceRate"] = ChangeValueToNeed.UnBalance(Convert.ToDouble(tws.MeasurementItems[i].Result.values[2].value * 310 / 255),
                                                Convert.ToDouble(tws.MeasurementItems[i + 1].Result.values[5].value * 310 / 255),
                                                Convert.ToDouble(tws.MeasurementItems[i + 2].Result.values[8].value * 310 / 255));
                                            rowM["Am_BmMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255, Convert.ToDouble(tws.MeasurementItems[i].Result.values[5].value * 310 / 255));
                                            rowM["Bm_CmMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i + 1].Result.values[5].value * 310 / 255, Convert.ToDouble(tws.MeasurementItems[i + 1].Result.values[8].value * 310 / 255));
                                            rowM["Cm_AmMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i + 2].Result.values[8].value * 310 / 255, Convert.ToDouble(tws.MeasurementItems[i + 2].Result.values[2].value * 310 / 255));
                                            rowM["TestCode"] = tws.job.Information.GetHashCode();
                                            rowM["MaximumMutualDifference"] = ChangeValueToNeed.MaxMutualDifference((double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255, (double)tws.MeasurementItems[i + 1].Result.values[5].value * 310 / 255,
                                                                         (double)tws.MeasurementItems[i + 2].Result.values[8].value * 310 / 255);
                                            i += 2;
                                        }
                                    }
                                    WorkingSets.local.Dcresistor_Mediumvoltage.Rows.Add(rowM);
                                    WorkingSets.local.SaveCreateLocateDatabase();
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.LV)
                                {
                                    DataRow rowLow = WorkingSets.local.Dcresistor_Lowpressure.NewRow();
                                    var data = tws.MeasurementItems[i].Result.values;

                                    if (tws.MeasurementItems[i].WindingConfig == TransformerWindingConfigName.Yn)
                                    {
                                        if (tws.MeasurementItems.Length > (i + 2) && tws.MeasurementItems[i + 1] != null && tws.MeasurementItems[i + 2] != null)
                                        {
                                            var data1 = tws.MeasurementItems[i + 1].Result.values;
                                            var data2 = tws.MeasurementItems[i + 2].Result.values;
                                            Numerics.PhysicalVariable[] sumData = { data[0], data[1], data[2], data1[3], data1[4], data1[5], data2[6], data2[7], data2[8] };
                                            if (sumData.Any(p => p != null && p.value != null))
                                            {
                                                if (tws.MeasurementItems[i].TapLabel != null)
                                                    rowLow["DCResistorHighPressure"] = tws.MeasurementItems[i].TapLabel[0];
                                                rowLow["a-b"] = sumData[2].OriginText;
                                                rowLow["a-b_75"] = ChangeValueToNeed.DcResistans_To_75(sumData[2]);
                                                rowLow["b-c"] = sumData[5].OriginText;
                                                rowLow["b-c_75"] = ChangeValueToNeed.DcResistans_To_75(sumData[5]);
                                                rowLow["c-a"] = sumData[8].OriginText;
                                                rowLow["c-a_75"] = ChangeValueToNeed.DcResistans_To_75(sumData[8]);
                                                rowLow["TestCode"] = tws.job.Information.GetHashCode();
                                                rowLow["unbalance"] = ChangeValueToNeed.UnBalance(Convert.ToDouble(sumData[2].value) * 310 / 255,
                                                    Convert.ToDouble(sumData[5].value) * 310 / 255,
                                                    Convert.ToDouble(sumData[8].value) * 310 / 255);
                                                rowLow["ax-byMutualDifference"] = ChangeValueToNeed.MutualDifference((double)sumData[2].value * 310 / 255, (double)(sumData[5].value * 310 / 255));
                                                rowLow["by-czMutualDifference"] = ChangeValueToNeed.MutualDifference((double)sumData[5].value * 310 / 255, (double)(sumData[8].value * 310 / 255));
                                                rowLow["cz-axMutualDifference"] = ChangeValueToNeed.MutualDifference((double)sumData[8].value * 310 / 255, (double)(sumData[2].value * 310 / 255));
                                                rowLow["max"] = ChangeValueToNeed.MaxMutualDifference((double)sumData[2].value * 310 / 255, (double)sumData[5].value * 310 / 255,
                                               (double)sumData[8].value * 310 / 255);
                                            }
                                        }
                                    }
                                    if (tws.MeasurementItems[i].WindingConfig != TransformerWindingConfigName.Yn)
                                    {

                                        if (tws.MeasurementItems[i].Terimal != null)
                                        {
                                            if (tws.MeasurementItems[i].TapLabel != null)
                                                rowLow["DCResistorHighPressure"] = tws.MeasurementItems[i].TapLabel[0];
                                            rowLow["a-b"] = tws.MeasurementItems[i].Result.values[2].OriginText;
                                            rowLow["a-b_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[2]);
                                            rowLow["b-c"] = tws.MeasurementItems[i + 1].Result.values[5].OriginText;
                                            rowLow["b-c_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i + 1].Result.values[5]);
                                            if (tws.MeasurementItems[i + 2].Result != null)
                                            {
                                                rowLow["c-a"] = tws.MeasurementItems[i + 2].Result.values[8].OriginText;
                                                rowLow["c-a_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i + 2].Result.values[8]);

                                            }
                                            else
                                            {
                                                rowLow["c-a"] = "0";
                                                rowLow["c-a_75"] = "0";
                                            }
                                            if (tws.MeasurementItems[i].Result != null && tws.MeasurementItems[i + 1].Result != null && tws.MeasurementItems[i + 2].Result != null)
                                            {
                                                rowLow["TestCode"] = tws.job.Information.GetHashCode();
                                                rowLow["unbalance"] = ChangeValueToNeed.UnBalance(Convert.ToDouble(tws.MeasurementItems[i].Result.values[2].value) * 310 / 255,
                                                    Convert.ToDouble(tws.MeasurementItems[i + 1].Result.values[5].value) * 310 / 255,
                                                    Convert.ToDouble(tws.MeasurementItems[i + 2].Result.values[8].value) * 310 / 255);
                                                rowLow["ax-byMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255, (double)(tws.MeasurementItems[i].Result.values[2].value * 310 / 255));
                                                rowLow["by-czMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i + 1].Result.values[5].value * 310 / 255, (double)(tws.MeasurementItems[i + 1].Result.values[5].value * 310 / 255));
                                                rowLow["cz-axMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i + 2].Result.values[8].value * 310 / 255, (double)(tws.MeasurementItems[i + 2].Result.values[8].value * 310 / 255));
                                                rowLow["max"] = ChangeValueToNeed.MaxMutualDifference((double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255, (double)tws.MeasurementItems[i + 1].Result.values[5].value * 310 / 255,
                                               (double)tws.MeasurementItems[i + 2].Result.values[8].value * 310 / 255);
                                            }

                                            i += 2;
                                        }
                                    }

                                    WorkingSets.local.Dcresistor_Lowpressure.Rows.Add(rowLow);
                                    WorkingSets.local.SaveCreateLocateDatabase();

                                }
                            }
                            if (tws.MeasurementItems[i].Result != null && tws.MeasurementItems[i].Result.Function == MeasurementFunction.BushingCapacitance)
                            {
                                if (tws.MeasurementItems[i].Winding == WindingType.HV)
                                {
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.O)
                                    {

                                        CapacitanceRowBushingCapacitance["O"] = ((double)tws.MeasurementItems[i].Result.values[3].value * 100).ToString("N3") + "%";
                                        //  CapacitanceRowBushingCapacitance["O"] = tws.MeasurementItems[i].Result.values[3];
                                        CnRowBushingCapacitance["O"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.A)
                                    {
                                        CapacitanceRowBushingCapacitance["A"] = ((double)tws.MeasurementItems[i].Result.values[3].value * 100).ToString("N3") + "%";

                                        CnRowBushingCapacitance["A"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.B)
                                    {
                                        CapacitanceRowBushingCapacitance["B"] = ((double)tws.MeasurementItems[i].Result.values[3].value * 100).ToString("N3") + "%";

                                        CnRowBushingCapacitance["B"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.C)
                                    {
                                        CapacitanceRowBushingCapacitance["C"] = ((double)tws.MeasurementItems[i].Result.values[3].value * 100).ToString("N3") + "%";

                                        CnRowBushingCapacitance["C"] = tws.MeasurementItems[i].Result.values[2];
                                    }


                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.MV)
                                {
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.O)
                                    {
                                        CapacitanceRowBushingCapacitance["Om"] = ((double)tws.MeasurementItems[i].Result.values[3].value * 100).ToString("N3") + "%";
                                        CnRowBushingCapacitance["Om"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.A)
                                    {
                                        CapacitanceRowBushingCapacitance["Am"] = ((double)tws.MeasurementItems[i].Result.values[3].value * 100).ToString("N3") + "%";

                                        CnRowBushingCapacitance["Am"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.B)
                                    {
                                        CapacitanceRowBushingCapacitance["Bm"] = ((double)tws.MeasurementItems[i].Result.values[3].value * 100).ToString("N3") + "%";

                                        CnRowBushingCapacitance["Bm"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.C)
                                    {
                                        CapacitanceRowBushingCapacitance["Cm"] = ((double)tws.MeasurementItems[i].Result.values[3].value * 100).ToString("N3") + "%";

                                        CnRowBushingCapacitance["Cm"] = tws.MeasurementItems[i].Result.values[2];
                                    }

                                }
                            }
                            if (tws.MeasurementItems[i].Result!=null&&tws.MeasurementItems[i].Result.Function == MeasurementFunction.BushingDCInsulation)
                            {
                                if (tws.MeasurementItems[i].Winding == WindingType.HV)
                                {
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.O)
                                        rowBushingDCInsulation["O"] = tws.MeasurementItems[i].Result.values[1];
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.A)
                                        rowBushingDCInsulation["A"] = tws.MeasurementItems[i].Result.values[1];
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.B)
                                        rowBushingDCInsulation["B"] = tws.MeasurementItems[i].Result.values[1];
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.C)
                                        rowBushingDCInsulation["C"] = tws.MeasurementItems[i].Result.values[1];
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.MV)
                                {
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.O)
                                        rowBushingDCInsulation["Om"] = tws.MeasurementItems[i].Result.values[1];
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.A)
                                        rowBushingDCInsulation["Am"] = tws.MeasurementItems[i].Result.values[1];
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.B)
                                        rowBushingDCInsulation["Bm"] = tws.MeasurementItems[i].Result.values[1];
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.C)
                                        rowBushingDCInsulation["Cm"] = tws.MeasurementItems[i].Result.values[1];
                                }
                            }
                        }

                    }
                    #region 数据行
                    WorkingSets.local.Dcresistor_Lowpressureresults.Rows.Add(rowLowResult);
                    WorkingSets.local.Dcresistor_Mediumvoltageresults.Rows.Add(rowMResult);
                    WorkingSets.local.Dcresistor_Highpressureresults.Rows.Add(rowHighResult);
                    rowLowResult["Temperature"] = tws.job.Information.temperature;
                    rowLowResult["Humidity"] = tws.job.Information.humidity;
                    rowLowResult["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowLowResult["InstrumentNumber"] = InstrumentNumber;
                    rowLowResult["TestCode"] = tws.job.Information.GetHashCode();

                    rowMResult["TestInstruments"] = TestInstruments;
                    rowLowResult["TestInstruments"] = TestInstruments;
                    rowHighResult["TestInstruments"] = TestInstruments;
                    rowResultBushingDCInsulation["TestInstruments"] = TestInstruments;
                    rowResultBushingCapacitance["TestInstruments"] = TestInstruments;

                    rowMResult["InstrumentNumber"] = "370001";
                    rowLowResult["InstrumentNumber"] = "370001";
                    rowHighResult["InstrumentNumber"] = "370001";
                    rowResultBushingDCInsulation["InstrumentNumber"] = "370001";
                    rowResultBushingCapacitance["InstrumentNumber"] = "370001";

                    rowMResult["ProjectConclusions"] = "";
                    rowLowResult["ProjectConclusions"] = "";
                    rowHighResult["ProjectConclusions"] = "";
                    rowResultBushingDCInsulation["ProjectConclusions"] = "";
                    rowResultBushingCapacitance["ProjectConclusions"] = "";


                    rowMResult["TestCode"] = tws.job.Information.GetHashCode();
                    rowMResult["Temperature"] = tws.job.Information.temperature;
                    rowMResult["Humidity"] = tws.job.Information.humidity;
                    rowMResult["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowMResult["InstrumentNumber"] = InstrumentNumber;
                    rowHighResult["TestCode"] = tws.job.Information.GetHashCode();
                    rowHighResult["Temperature"] = tws.job.Information.temperature;
                    rowHighResult["Humidity"] = tws.job.Information.humidity;
                    rowHighResult["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowHighResult["InstrumentNumber"] = InstrumentNumber;
                    CapacitanceRowCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                    CnRowCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                    CapacitanceRowCapacitance["DielectricLossAndCapacitance"] = "绕组电容";
                    CnRowCapacitance["DielectricLossAndCapacitance"] = "绕组介损";
                    rowResultCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                    rowResultCapacitance["Temperature"] = tws.job.Information.temperature;
                    rowResultCapacitance["Humidity"] = tws.job.Information.humidity;
                    rowResultCapacitance["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowResultCapacitance["InstrumentNumber"] = InstrumentNumber;
                    rowDCInsulation["WindingInsulationEsistance"] = "绝缘电阻";
                    rowDCInsulationAbs["WindingInsulationEsistance"] = "吸收比";
                    rowDCInsulationMax["WindingInsulationEsistance"] = "极化指数";
                    rowResultDCInsulation["TestCode"] = tws.job.Information.GetHashCode();
                    rowResultDCInsulation["Temperature"] = tws.job.Information.temperature;
                    rowResultDCInsulation["Humidity"] = tws.job.Information.humidity;
                    rowResultDCInsulation["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowResultDCInsulation["TestInstruments"] = TestInstruments;
                    rowResultDCInsulation["InstrumentNumber"] = InstrumentNumber;
                    CapacitanceRowBushingCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                    CnRowBushingCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                    rowResultBushingCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                    rowResultBushingCapacitance["Temperature"] = tws.job.Information.temperature;
                    rowResultBushingCapacitance["Humidity"] = tws.job.Information.humidity;
                    rowResultBushingCapacitance["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowResultBushingCapacitance["InstrumentNumber"] = InstrumentNumber;
                    CapacitanceRowBushingCapacitance["CasingTest"] = "套管介损";
                    CnRowBushingCapacitance["CasingTest"] = "实测电容";
                    rowBushingDCInsulation["CasingTest"] = "套管绝缘（M）";
                    rowBushingDCInsulation["TestCode"] = tws.job.Information.GetHashCode();
                    rowResultBushingDCInsulation["TestCode"] = tws.job.Information.GetHashCode();
                    rowResultBushingDCInsulation["Temperature"] = tws.job.Information.temperature;
                    rowResultBushingDCInsulation["Humidity"] = tws.job.Information.humidity;
                    rowResultBushingDCInsulation["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowResultBushingDCInsulation["InstrumentNumber"] = InstrumentNumber;
                    //兆欧表
                    WorkingSets.local.Insulationresistance_Threewinding.Rows.Add(rowDCInsulation);
                    WorkingSets.local.Insulationresistance_Threewinding.Rows.Add(rowDCInsulationAbs);
                    WorkingSets.local.Insulationresistance_Threewinding.Rows.Add(rowDCInsulationMax);
                    WorkingSets.local.Insulationresistance_Threewindingresults.Rows.Add(rowResultDCInsulation);
                    //介损
                    WorkingSets.local.Dielectriclossandcapacitance_Threewinding.Rows.Add(CapacitanceRowCapacitance);
                    WorkingSets.local.Dielectriclossandcapacitance_Threewinding.Rows.Add(CnRowCapacitance);
                    WorkingSets.local.Dielectriclossandcapacitance_Threewindingresults.Rows.Add(rowResultCapacitance);
                    //套管介损
                    WorkingSets.local.Casingtest_Commonbodyresults.Rows.Add(rowResultBushingCapacitance);
                    WorkingSets.local.Casingtest_Commonbody.Rows.Add(CapacitanceRowBushingCapacitance);
                    WorkingSets.local.Casingtest_Commonbody.Rows.Add(CnRowBushingCapacitance);
                    //套管兆欧表
                    // WorkingSets.local.Casingtest_Commonbodyresults.Rows.Add(rowResultBushingDCInsulation);
                    WorkingSets.local.Casingtest_Commonbody.Rows.Add(rowBushingDCInsulation);
                    WorkingSets.local.SaveCreateLocateDatabase();
                    #endregion
                }
                if (tws.Transformer.PhaseNum == 2)
                {
                    // CreateTableHead(ResultName);
                    #region 数据 行
                    //兆欧表
                    DataRow rowDCInsulation = WorkingSets.local.Insulationresistance_Threewinding.NewRow();
                    DataRow rowDCInsulationAbs = WorkingSets.local.Insulationresistance_Threewinding.NewRow();
                    DataRow rowDCInsulationMax = WorkingSets.local.Insulationresistance_Threewinding.NewRow();
                    DataRow rowResultDCInsulation = WorkingSets.local.Insulationresistance_Threewindingresults.NewRow();
                    //介损
                    DataRow CapacitanceRowCapacitance = WorkingSets.local.Dielectriclossandcapacitance_Threewinding.NewRow();
                    DataRow CnRowCapacitance = WorkingSets.local.Dielectriclossandcapacitance_Threewinding.NewRow();
                    DataRow rowResultCapacitance = WorkingSets.local.Dielectriclossandcapacitance_Threewindingresults.NewRow();
                    //套管介损
                    DataRow CapacitanceRowBushingCapacitance = WorkingSets.local.Casingtest_Commonbody.NewRow();
                    DataRow CnRowBushingCapacitance = WorkingSets.local.Casingtest_Commonbody.NewRow();
                    DataRow rowResultBushingCapacitance = WorkingSets.local.Casingtest_Commonbodyresults.NewRow();
                    //套管兆欧表
                    DataRow rowBushingDCInsulation = WorkingSets.local.Casingtest_Commonbody.NewRow();
                    DataRow rowResultBushingDCInsulation = WorkingSets.local.Casingtest_Commonbodyresults.NewRow();

                    DataRow rowHighResult = WorkingSets.local.Dcresistor_Highpressureresults.NewRow();
                    DataRow rowMResult = WorkingSets.local.Dcresistor_Mediumvoltageresults.NewRow();
                    DataRow rowLowResult = WorkingSets.local.Dcresistor_Lowpressureresults.NewRow();

                    #endregion

                    for (int i = 0; i < ms; i++)
                    {
                        if (tws.MeasurementItems[i].Result != null)
                        {
                            if (tws.MeasurementItems[i].Result.Function == MeasurementFunction.DCInsulation)
                            {

                                if (tws.MeasurementItems[i].Winding == WindingType.HV)
                                {
                                    rowDCInsulation["High_CentreLowPressure"] = tws.MeasurementItems[i].Result.values[1];
                                    rowDCInsulationAbs["High_CentreLowPressure"] = tws.MeasurementItems[i].Result.values[2];
                                    rowDCInsulationMax["High_CentreLowPressure"] = tws.MeasurementItems[i].Result.values[3];
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.MV)
                                {
                                    rowDCInsulation["Centre_HighLowPressure"] = tws.MeasurementItems[i].Result.values[1];
                                    rowDCInsulationAbs["Centre_HighLowPressure"] = tws.MeasurementItems[i].Result.values[2];
                                    rowDCInsulationMax["Centre_HighLowPressure"] = tws.MeasurementItems[i].Result.values[3];
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.LV)
                                {
                                    rowDCInsulation["Low_HighMediumVoltage"] = tws.MeasurementItems[i].Result.values[1];
                                    rowDCInsulationAbs["Low_HighMediumVoltage"] = tws.MeasurementItems[i].Result.values[2];
                                    rowDCInsulationMax["Low_HighMediumVoltage"] = tws.MeasurementItems[i].Result.values[3];
                                }
                                rowDCInsulation["TestCode"] = tws.job.Information.GetHashCode();
                                rowDCInsulationAbs["TestCode"] = tws.job.Information.GetHashCode();
                                rowDCInsulationMax["TestCode"] = tws.job.Information.GetHashCode();
                            }
                            if (tws.MeasurementItems[i].Result.Function == MeasurementFunction.Capacitance)
                            {

                                if (tws.MeasurementItems[i].Winding == WindingType.HV)
                                {
                                    CapacitanceRowCapacitance["HighPressure_CentreLowPressure"] = tws.MeasurementItems[i].Result.values[3]; //兆欧表套管
                                    CnRowCapacitance["HighPressure_CentreLowPressure"] = tws.MeasurementItems[i].Result.values[2];//兆欧表套管
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.MV)
                                {
                                    CapacitanceRowCapacitance["MediumVoltage_HighLowPressure"] = tws.MeasurementItems[i].Result.values[3];//兆欧表套管
                                    CnRowCapacitance["MediumVoltage_HighLowPressure"] = tws.MeasurementItems[i].Result.values[2];//兆欧表套管
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.LV)
                                {
                                    CnRowCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                                    CapacitanceRowCapacitance["LowPressure_HighMediumVoltage"] = tws.MeasurementItems[i].Result.values[3];//兆欧表套管
                                    CnRowCapacitance["LowPressure_HighMediumVoltage"] = tws.MeasurementItems[i].Result.values[2];//兆欧表套管
                                }



                            }
                            if (tws.MeasurementItems[i].Result.Function == MeasurementFunction.DCResistance)
                            {
                                if (tws.MeasurementItems[i].Winding == WindingType.HV)
                                {
                                    DataRow rowHigh = WorkingSets.local.Dcresistor_Highpressure.NewRow();
                                    rowHigh["A0"] = tws.MeasurementItems[i].Result.values[2].OriginText;
                                    rowHigh["A0_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[2]);
                                    rowHigh["B0"] = tws.MeasurementItems[i].Result.values[5].OriginText;
                                    rowHigh["B0_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[5]);
                                    rowHigh["C0"] = tws.MeasurementItems[i].Result.values[8].OriginText;
                                    rowHigh["C0_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[8]);
                                    rowHigh["TestCode"] = tws.job.Information.GetHashCode();
                                    try { rowHigh["A0_B0MutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255, Convert.ToDouble(tws.MeasurementItems[i].Result.values[5].value) * 310 / 255); } catch { }
                                    try { rowHigh["B0_C0MutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[5].value * 310 / 255, Convert.ToDouble(tws.MeasurementItems[i].Result.values[8].value) * 310 / 255); } catch { }
                                    try { rowHigh["C0_A0MutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[8].value * 310 / 255, Convert.ToDouble(tws.MeasurementItems[i].Result.values[2].value) * 310 / 255); } catch { }
                                    rowHigh["UnbalanceRate"] = ChangeValueToNeed.UnBalance(Convert.ToDouble(tws.MeasurementItems[i].Result.values[2].value * 310 / 255),
                                        Convert.ToDouble(tws.MeasurementItems[i].Result.values[5].value * 310 / 255),
                                        Convert.ToDouble(tws.MeasurementItems[i].Result.values[8].value * 310 / 255));
                                    rowHigh["MaximumMutualDifference"] = ChangeValueToNeed.MaxMutualDifference((double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255, (double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255,
                                      (double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255);

                                    WorkingSets.local.Dcresistor_Highpressure.Rows.Add(rowHigh);
                                    WorkingSets.local.SaveCreateLocateDatabase();
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.MV)
                                {
                                    DataRow rowM = WorkingSets.local.Dcresistor_Mediumvoltage.NewRow();
                                    rowM["Am0m"] = tws.MeasurementItems[i].Result.values[2];
                                    rowM["Am0m_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[2]);
                                    rowM["Bm0m"] = tws.MeasurementItems[i].Result.values[5];
                                    rowM["Bm0m_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[5]);
                                    rowM["Cm0m"] = tws.MeasurementItems[i].Result.values[8];
                                    rowM["Cm0m_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[8]);
                                    rowM["UnbalanceRate"] = ChangeValueToNeed.UnBalance(Convert.ToDouble(tws.MeasurementItems[i].Result.values[2].value * 310 / 255),
                                        Convert.ToDouble(tws.MeasurementItems[i].Result.values[5].value * 310 / 255),
                                        Convert.ToDouble(tws.MeasurementItems[i].Result.values[8].value * 310 / 255));
                                    try { rowM["Am_BmMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255, Convert.ToDouble(tws.MeasurementItems[i].Result.values[5].value * 310 / 255)); } catch { }
                                    try { rowM["Bm_CmMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[5].value * 310 / 255, Convert.ToDouble(tws.MeasurementItems[i].Result.values[8].value * 310 / 255)); } catch { }
                                    try { rowM["Cm_AmMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[8].value * 310 / 255, Convert.ToDouble(tws.MeasurementItems[i].Result.values[2].value * 310 / 255)); } catch { }
                                    rowM["TestCode"] = tws.job.Information.GetHashCode();
                                    rowM["MaximumMutualDifference"] = ChangeValueToNeed.MaxMutualDifference((double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255, (double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255,
                                       (double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255);

                                    WorkingSets.local.Dcresistor_Mediumvoltage.Rows.Add(rowM);

                                    WorkingSets.local.SaveCreateLocateDatabase();
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.LV)
                                {
                                    DataRow rowLow = WorkingSets.local.Dcresistor_Lowpressure.NewRow();
                                    rowLow["a-b"] = tws.MeasurementItems[i].Result.values[2];
                                    rowLow["a-b_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[2]);
                                    rowLow["b-c"] = tws.MeasurementItems[i].Result.values[5];
                                    rowLow["a-b_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[5]);
                                    rowLow["c-a"] = tws.MeasurementItems[i].Result.values[8];
                                    rowLow["a-b_75"] = ChangeValueToNeed.DcResistans_To_75(tws.MeasurementItems[i].Result.values[8]);
                                    rowLow["TestCode"] = tws.job.Information.GetHashCode();
                                    rowLow["unbalance"] = ChangeValueToNeed.UnBalance(Convert.ToDouble(tws.MeasurementItems[i].Result.values[2].value) * 310 / 255,
                                        Convert.ToDouble(tws.MeasurementItems[i].Result.values[5].value) * 310 / 255,
                                        Convert.ToDouble(tws.MeasurementItems[i].Result.values[8].value) * 310 / 255);
                                    rowLow["ax-byMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255, (double)(tws.MeasurementItems[i].Result.values[5].value * 310 / 255));
                                    rowLow["by-czMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[5].value * 310 / 255, (double)(tws.MeasurementItems[i].Result.values[8].value * 310 / 255));
                                    rowLow["cz-axMutualDifference"] = ChangeValueToNeed.MutualDifference((double)tws.MeasurementItems[i].Result.values[8].value * 310 / 255, (double)(tws.MeasurementItems[i].Result.values[2].value * 310 / 255));
                                    rowLow["max"] = ChangeValueToNeed.MaxMutualDifference((double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255, (double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255,
                                        (double)tws.MeasurementItems[i].Result.values[2].value * 310 / 255);
                                    WorkingSets.local.Dcresistor_Lowpressure.Rows.Add(rowLow);
                                    WorkingSets.local.SaveCreateLocateDatabase();
                                }
                            }
                            if (tws.MeasurementItems[i].Result.Function == MeasurementFunction.BushingCapacitance)
                            {
                                if (tws.MeasurementItems[i].Winding == WindingType.HV)
                                {
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.O)
                                    {
                                        CapacitanceRowBushingCapacitance["O"] = tws.MeasurementItems[i].Result.values[3];
                                        CnRowBushingCapacitance["O"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.A)
                                    {
                                        CapacitanceRowBushingCapacitance["A"] = tws.MeasurementItems[i].Result.values[3];
                                        CnRowBushingCapacitance["A"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.B)
                                    {
                                        CapacitanceRowBushingCapacitance["B"] = tws.MeasurementItems[i].Result.values[3];
                                        CnRowBushingCapacitance["B"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.C)
                                    {
                                        CapacitanceRowBushingCapacitance["C"] = tws.MeasurementItems[i].Result.values[3];
                                        CnRowBushingCapacitance["C"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.LV)
                                {
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.O)
                                    {
                                        CapacitanceRowBushingCapacitance["Om"] = tws.MeasurementItems[i].Result.values[3];
                                        CnRowBushingCapacitance["Om"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.A)
                                    {
                                        CapacitanceRowBushingCapacitance["Am"] = tws.MeasurementItems[i].Result.values[3];
                                        CnRowBushingCapacitance["Am"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.B)
                                    {
                                        CapacitanceRowBushingCapacitance["Bm"] = tws.MeasurementItems[i].Result.values[3];
                                        CnRowBushingCapacitance["Bm"] = tws.MeasurementItems[i].Result.values[2];
                                    }
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.C)
                                    {
                                        CapacitanceRowBushingCapacitance["Cm"] = tws.MeasurementItems[i].Result.values[3];
                                        CnRowBushingCapacitance["Cm"] = tws.MeasurementItems[i].Result.values[2];
                                    }

                                }
                            }
                            if (tws.MeasurementItems[i].Result.Function == MeasurementFunction.BushingDCInsulation)
                            {
                                if (tws.MeasurementItems[i].Winding == WindingType.HV)
                                {
                                    if (tws.MeasurementItems[i].Winding == WindingTerimal.O)
                                        rowBushingDCInsulation["O"] = tws.MeasurementItems[i].Result.values[1];
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.A)
                                        rowBushingDCInsulation["A"] = tws.MeasurementItems[i].Result.values[1];
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.B)
                                        rowBushingDCInsulation["B"] = tws.MeasurementItems[i].Result.values[1];
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.C)
                                        rowBushingDCInsulation["C"] = tws.MeasurementItems[i].Result.values[1];
                                }
                                if (tws.MeasurementItems[i].Winding == WindingType.LV)
                                {
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.O)
                                        rowBushingDCInsulation["Om"] = tws.MeasurementItems[i].Result.values[1];
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.A)
                                        rowBushingDCInsulation["Am"] = tws.MeasurementItems[i].Result.values[1];
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.B)
                                        rowBushingDCInsulation["Bm"] = tws.MeasurementItems[i].Result.values[1];
                                    if (tws.MeasurementItems[i].Terimal[0] == WindingTerimal.C)
                                        rowBushingDCInsulation["Cm"] = tws.MeasurementItems[i].Result.values[1];
                                }
                            }
                            if (tws.MeasurementItems[i].Result.Function == MeasurementFunction.OLTCSwitchingCharacter)
                            {
                                int j = 0;
                                DataRow OLTERowOLTCSwitchingCharacter = WorkingSets.local.Tapchangertest.NewRow();
                                DataRow rowResultOLTCSwitchingCharacter = WorkingSets.local.Tapchangertestresults.NewRow();
                                OLTERowOLTCSwitchingCharacter["TestCode"] = tws.job.Information.GetHashCode();
                                OLTERowOLTCSwitchingCharacter["SwitchingTime"] = tws.MeasurementItems[i].Result.recordTime;
                                OLTERowOLTCSwitchingCharacter["testchart"] = System.Convert.ToBase64String(Array2Bytes(tws.MeasurementItems[i].Result.waves));
                                OLTERowOLTCSwitchingCharacter["waveformname"] = "WaveFormImage.png" + j.ToString(); j++;
                                rowResultOLTCSwitchingCharacter["TestCode"] = tws.job.Information.GetHashCode();
                                rowResultOLTCSwitchingCharacter["Temperature"] = tws.job.Information.temperature;
                                rowResultOLTCSwitchingCharacter["Humidity"] = tws.job.Information.humidity;
                                rowResultOLTCSwitchingCharacter["OilTemperature"] = tws.job.Information.oilTemperature;
                                rowResultOLTCSwitchingCharacter["InstrumentNumber"] = InstrumentNumber;
                                WorkingSets.local.Tapchangertestresults.Rows.Add(rowResultOLTCSwitchingCharacter);
                                WorkingSets.local.Tapchangertest.Rows.Add(OLTERowOLTCSwitchingCharacter);
                                WorkingSets.local.SaveCreateLocateDatabase();
                            }
                        }

                    }
                    #region 数据行
                    WorkingSets.local.Dcresistor_Lowpressureresults.Rows.Add(rowLowResult);
                    WorkingSets.local.Dcresistor_Mediumvoltageresults.Rows.Add(rowMResult);
                    WorkingSets.local.Dcresistor_Highpressureresults.Rows.Add(rowHighResult);
                    rowLowResult["Temperature"] = tws.job.Information.temperature;
                    rowLowResult["Humidity"] = tws.job.Information.humidity;
                    rowLowResult["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowLowResult["InstrumentNumber"] = InstrumentNumber;
                    rowLowResult["TestCode"] = tws.job.Information.GetHashCode();

                    rowMResult["TestInstruments"] = TestInstruments;
                    rowLowResult["TestInstruments"] = TestInstruments;
                    rowHighResult["TestInstruments"] = TestInstruments;
                    rowResultBushingDCInsulation["TestInstruments"] = TestInstruments;
                    rowResultBushingCapacitance["TestInstruments"] = TestInstruments;


                    rowMResult["TestCode"] = tws.job.Information.GetHashCode();
                    rowMResult["Temperature"] = tws.job.Information.temperature;
                    rowMResult["Humidity"] = tws.job.Information.humidity;
                    rowMResult["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowMResult["InstrumentNumber"] = InstrumentNumber;
                    rowHighResult["TestCode"] = tws.job.Information.GetHashCode();
                    rowHighResult["Temperature"] = tws.job.Information.temperature;
                    rowHighResult["Humidity"] = tws.job.Information.humidity;
                    rowHighResult["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowHighResult["InstrumentNumber"] = InstrumentNumber;
                    CapacitanceRowCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                    CnRowCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                    CapacitanceRowCapacitance["DielectricLossAndCapacitance"] = "绕组介损";
                    CnRowCapacitance["DielectricLossAndCapacitance"] = "绕组电容";
                    rowResultCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                    rowResultCapacitance["Temperature"] = tws.job.Information.temperature;
                    rowResultCapacitance["Humidity"] = tws.job.Information.humidity;
                    rowResultCapacitance["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowResultCapacitance["InstrumentNumber"] = InstrumentNumber;
                    rowDCInsulation["WindingInsulationEsistance"] = "绝缘电阻";
                    rowDCInsulationAbs["WindingInsulationEsistance"] = "吸收比";
                    rowDCInsulationMax["WindingInsulationEsistance"] = "极化指数";
                    rowResultDCInsulation["TestCode"] = tws.job.Information.GetHashCode();
                    rowResultDCInsulation["Temperature"] = tws.job.Information.temperature;
                    rowResultDCInsulation["Humidity"] = tws.job.Information.humidity;
                    rowResultDCInsulation["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowResultDCInsulation["TestInstruments"] = TestInstruments;
                    rowResultDCInsulation["InstrumentNumber"] = InstrumentNumber;
                    CapacitanceRowBushingCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                    CnRowBushingCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                    rowResultBushingCapacitance["TestCode"] = tws.job.Information.GetHashCode();
                    rowResultBushingCapacitance["Temperature"] = tws.job.Information.temperature;
                    rowResultBushingCapacitance["Humidity"] = tws.job.Information.humidity;
                    rowResultBushingCapacitance["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowResultBushingCapacitance["InstrumentNumber"] = InstrumentNumber;
                    CapacitanceRowBushingCapacitance["CasingTest"] = "套管介损";
                    CnRowBushingCapacitance["CasingTest"] = "实测电容";
                    rowBushingDCInsulation["CasingTest"] = "套管绝缘（M）";
                    rowBushingDCInsulation["TestCode"] = tws.job.Information.GetHashCode();
                    rowResultBushingDCInsulation["TestCode"] = tws.job.Information.GetHashCode();
                    rowResultBushingDCInsulation["Temperature"] = tws.job.Information.temperature;
                    rowResultBushingDCInsulation["Humidity"] = tws.job.Information.humidity;
                    rowResultBushingDCInsulation["OilTemperature"] = tws.job.Information.oilTemperature;
                    rowResultBushingDCInsulation["InstrumentNumber"] = InstrumentNumber;
                    //兆欧表
                    WorkingSets.local.Insulationresistance_Threewinding.Rows.Add(rowDCInsulation);
                    WorkingSets.local.Insulationresistance_Threewinding.Rows.Add(rowDCInsulationAbs);
                    WorkingSets.local.Insulationresistance_Threewinding.Rows.Add(rowDCInsulationMax);
                    WorkingSets.local.Insulationresistance_Threewindingresults.Rows.Add(rowResultDCInsulation);
                    //介损
                    WorkingSets.local.Dielectriclossandcapacitance_Threewinding.Rows.Add(CapacitanceRowCapacitance);
                    WorkingSets.local.Dielectriclossandcapacitance_Threewinding.Rows.Add(CnRowCapacitance);
                    WorkingSets.local.Dielectriclossandcapacitance_Threewindingresults.Rows.Add(rowResultCapacitance);
                    //套管介损
                    WorkingSets.local.Casingtest_Commonbodyresults.Rows.Add(rowResultBushingCapacitance);
                    WorkingSets.local.Casingtest_Commonbody.Rows.Add(CapacitanceRowBushingCapacitance);
                    WorkingSets.local.Casingtest_Commonbody.Rows.Add(CnRowBushingCapacitance);
                    //套管兆欧表
                    // WorkingSets.local.Casingtest_Commonbodyresults.Rows.Add(rowResultBushingDCInsulation);
                    WorkingSets.local.Casingtest_Commonbody.Rows.Add(rowBushingDCInsulation);
                    WorkingSets.local.SaveCreateLocateDatabase();
                    #endregion

                }
            }

        }

        private static bool isNotPhyNull(PhysicalVariable[] physicalVariable)
        {
            foreach (PhysicalVariable item in physicalVariable)
            {
                if (item == null || item.value == null)
                    return false;
            }
            return true;
        }
        public static void CreateWaveResult(OverResult or, int testcode)
        {

            DataRow rowtap = WorkingSets.local.Tapchangertest.NewRow();
            //   rowtap["TestCode"] = WorkingSets.local.Tapchangertest.Rows[0]["TestCode"];
            rowtap["TestCode"] = testcode.ToString();
            rowtap["waveformname"] = or.WaveName + ".jpg";
            rowtap["taplabelnum"] = or.OLTcNum[0].ToString() + "-" + or.OLTcNum[1].ToString();
            rowtap["Aovertime"] = or.AOverTime.ToString() + "ms";
            rowtap["Aoverresistanceone"] = or.AOverResistanceOne.ToString() + "mΩ"; ;
            rowtap["Aoverresistancetwo"] = or.AOverResistanceTwo.ToString() + "mΩ"; ;
            rowtap["Bovertime"] = or.BOverTime.ToString() + "ms";
            rowtap["Boverresistanceone"] = or.BOverResistanceOne.ToString() + "mΩ"; ;
            rowtap["Boverresistancetwo"] = or.BOverResistanceTwo.ToString() + "mΩ"; ;
            rowtap["Covertime"] = or.COverTime.ToString() + "ms";
            rowtap["Coverresistanceone"] = or.COverResistanceOne.ToString() + "mΩ"; ;
            rowtap["Coverresistancetwo"] = or.COverResistanceTwo.ToString() + "mΩ"; ;
            rowtap["SwitchingTime"] = DateTime.Now;
            WorkingSets.local.Tapchangertest.Rows.Add(rowtap);
            WorkingSets.local.SaveCreateLocateDatabase();
        }
        internal static byte[] Array2Bytes(short[] array)
        {

            if (array.Length < 1) throw new NullReferenceException("转换数组为空");
            Type type = array[0].GetType();
            List<byte> bytesCollection = new List<byte>();
            for (int i = 0; i < array.Length; i++)
            {
                byte[] bytes = BitConverter.GetBytes(array[i]);
                bytesCollection.AddRange(bytes);
            }
            return bytesCollection.ToArray();
        }
        //创建表头 
        public static void CreateTableHead(string ResultName, string location = "湖南")
        {
            var tws = WorkingSets.local.getTestResults(ResultName);
            DataRow rowDCInsulation = WorkingSets.local.Insulationresistance_Threewinding.NewRow();
            rowDCInsulation["TestCode"] = tws.job.Information.GetHashCode();
            rowDCInsulation["WindingInsulationEsistance"] = "绕组绝缘电阻";
            rowDCInsulation["High_CentreLowPressure"] = "高压对中压及地";
            rowDCInsulation["Centre_HighLowPressure"] = "中压对高压及地";
            rowDCInsulation["Low_HighMediumVoltage"] = "低压对高中压及地";
            DataRow rowCapacitance = WorkingSets.local.Dielectriclossandcapacitance_Threewinding.NewRow();
            rowCapacitance["TestCode"] = tws.job.Information.GetHashCode();
            rowCapacitance["DielectricLossAndCapacitance"] = "绕组介损及电容";
            //rowCapacitance["HighLowPressure_MediumVoltage"] = "高低压对中压及地";
            // rowCapacitance["LowMediumVoltage_HighPressure"] = "低中压对高压及地"; 
            rowCapacitance["HighPressure_CentreLowPressure"] = "高压对中低压及地";
            rowCapacitance["MediumVoltage_HighLowPressure"] = "中压对高低压及地";
            rowCapacitance["LowPressure_HighMediumVoltage"] = "低压对高中压及地";
            //rowCapacitance["HighMediumVoltage_LowPressure"] = "高中压对低压及地"; 
            rowCapacitance["HighCentreLowPressure"] = "高中低压及地";
            DataRow rowHigh = WorkingSets.local.Dcresistor_Highpressure.NewRow();
            rowHigh["TestCode"] = tws.job.Information.GetHashCode();
            rowHigh["DCResistorHighPressure"] = "绕组直流电阻（高压绕组-相（Ω））";
            rowHigh["A0"] = "A0";
            rowHigh["A0_75"] = "A0(75℃)";
            rowHigh["B0"] = "B0";
            rowHigh["B0_75"] = "B0(75℃)";
            rowHigh["C0"] = "C0";
            rowHigh["C0_75"] = "C0(75℃)";
            rowHigh["A0_B0MutualDifference"] = "A0,B0互差（%）";
            rowHigh["B0_C0MutualDifference"] = "B0,C0互差（%）";
            rowHigh["C0_A0MutualDifference"] = "C0,A0互差（%）";
            rowHigh["MaximumMutualDifference"] = "最大互差（%）";
            rowHigh["UnbalanceRate"] = "不平衡率（%）";
            rowHigh["MaximumInitialValueDifference"] = "最大初值差（%）";
            DataRow rowM = WorkingSets.local.Dcresistor_Mediumvoltage.NewRow();
            rowM["TestCode"] = tws.job.Information.GetHashCode();
            rowM["DCResistorMediumVoltage"] = "绕组直流电阻（中压绕组-相（Ω））";
            rowM["Am0m"] = "Am0m";
            rowM["Am0m_75"] = "Am0m(75℃)";
            rowM["Bm0m"] = "Bm0m";
            rowM["Bm0m_75"] = "Bm0m(75℃)";
            rowM["Cm0m"] = "Cm0m";
            rowM["Cm0m_75"] = "Cm0m(75℃)";
            rowM["Am_BmMutualDifference"] = "AmBm互差（%）";
            rowM["Bm_CmMutualDifference"] = "BmCm互差（%）";
            rowM["Cm_AmMutualDifference"] = "CmAm互差（%）";
            rowM["MaximumMutualDifference"] = "最大互差互差（%）";
            rowM["UnbalanceRate"] = "不平衡率（%）";
            rowM["MaximumInitialValueDifference"] = "最大初值差（%）";
            DataRow rowLow = WorkingSets.local.Dcresistor_Lowpressure.NewRow();
            rowLow["TestCode"] = tws.job.Information.GetHashCode();
            rowLow["DCResistorLowPressure"] = "绕组直流电阻（低压绕组-相（Ω））";
            rowLow["a-b"] = "a-b";
            rowLow["a-b_75"] = "a-b(75℃)";
            rowLow["b-c"] = "b-c";
            rowLow["b-c_75"] = "b-c(75℃)";
            rowLow["c-a"] = "c-a";
            rowLow["c-a_75"] = "c-a(75℃)";
            rowLow["a-x"] = "a-x";
            rowLow["a-x_75"] = "a-x(75℃)";
            rowLow["b-y"] = "b-y";
            rowLow["b-y_75"] = "b-y(75℃)";
            rowLow["c-z"] = "c-z";
            rowLow["c-z_75"] = "c-z(75℃)";
            rowLow["ax-byMutualDifference"] = "ax-by互差（%）";
            rowLow["by-czMutualDifference"] = "by-cz（%）";
            rowLow["cz-axMutualDifference"] = "cz-ax互差（%）";
            rowLow["max"] = "最大互差（%）";
            rowLow["unbalance"] = "不平衡率（%）";
            rowLow["maxdiffer"] = "最大初差值（%）";
            DataRow CapacitanceRowBushingCapacitance = WorkingSets.local.Casingtest_Commonbody.NewRow();
            CapacitanceRowBushingCapacitance["TestCode"] = tws.job.Information.GetHashCode();
            CapacitanceRowBushingCapacitance["CasingTest"] = "试验";
            CapacitanceRowBushingCapacitance["A"] = "A";
            CapacitanceRowBushingCapacitance["Am"] = "Am";
            CapacitanceRowBushingCapacitance["B"] = "B";
            CapacitanceRowBushingCapacitance["Bm"] = "Bm";
            CapacitanceRowBushingCapacitance["C"] = "C";
            CapacitanceRowBushingCapacitance["Cm"] = "Cm";
            CapacitanceRowBushingCapacitance["O"] = "O";
            CapacitanceRowBushingCapacitance["Om"] = "Om";
            //DataRow OLTERowOLTCSwitchingCharacter = WorkingSets.local.Tapchangertest.NewRow();
            //OLTERowOLTCSwitchingCharacter["TestCode"] = tws.job.Information.GetHashCode();
            //OLTERowOLTCSwitchingCharacter["TapchangerTest"] = "分接开关试验";
            //OLTERowOLTCSwitchingCharacter["ActionCharacteristics"] = "分接开关试验";
            //OLTERowOLTCSwitchingCharacter["SwitchingTime"] = DateTime.Now;
            //OLTERowOLTCSwitchingCharacter["testchart"] = "分接开关试验";
            //OLTERowOLTCSwitchingCharacter["waveformname"] = "波形名称";
            //OLTERowOLTCSwitchingCharacter["taplabelnum"] = "分接位置";

            //OLTERowOLTCSwitchingCharacter["overtime"] = "过度时间";
            //OLTERowOLTCSwitchingCharacter["overresistanceone"] = "过度电阻1";
            //OLTERowOLTCSwitchingCharacter["overresistancetwo"] = "过度电阻2";
            WorkingSets.local.Insulationresistance_Threewinding.Rows.Add(rowDCInsulation);
            WorkingSets.local.Dielectriclossandcapacitance_Threewinding.Rows.Add(rowCapacitance);
            WorkingSets.local.Dcresistor_Highpressure.Rows.Add(rowHigh);
            WorkingSets.local.Dcresistor_Mediumvoltage.Rows.Add(rowM);
            WorkingSets.local.Dcresistor_Lowpressure.Rows.Add(rowLow);
            WorkingSets.local.Casingtest_Commonbody.Rows.Add(CapacitanceRowBushingCapacitance);
            // WorkingSets.local.Tapchangertest.Rows.Add(OLTERowOLTCSwitchingCharacter);
            WorkingSets.local.SaveCreateLocateDatabase();

        }



        public static void ShowExport(string ResultName)
        {
            var tws = WorkingSets.local.getTestResults(ResultName);
            HNReport.DoReport.Run(HNReport.ReportOperator.Previev, tws.job.Information.GetHashCode().ToString(), "配电变压器");

        }
        /// <summary>
        /// 处理波形数据库问题
        /// </summary>
        /// <param name="resulname"></param>
        public static void Deelwavedatabase(string resulname)
        {
            //var tws = WorkingSets.local.getTestResults(resulname);
            WorkingSets.local.deletetap();
            DataRow OLTERowOLTCSwitchingCharacter = WorkingSets.local.Tapchangertest.NewRow();
            // OLTERowOLTCSwitchingCharacter["TestCode"] = tws.job.Information.GetHashCode();
            //OLTERowOLTCSwitchingCharacter["TestCode"] = 88888888;
            OLTERowOLTCSwitchingCharacter["TapchangerTest"] = "分接开关试验";
            OLTERowOLTCSwitchingCharacter["ActionCharacteristics"] = "分接开关试验";
            OLTERowOLTCSwitchingCharacter["SwitchingTime"] = DateTime.Now;
            OLTERowOLTCSwitchingCharacter["testchart"] = "分接开关试验";
            OLTERowOLTCSwitchingCharacter["waveformname"] = "波形名称";
            OLTERowOLTCSwitchingCharacter["taplabelnum"] = "分接位置";
            OLTERowOLTCSwitchingCharacter["Aovertime"] = "A相过度时间";
            OLTERowOLTCSwitchingCharacter["Aoverresistanceone"] = "A相过度电阻1";
            OLTERowOLTCSwitchingCharacter["Aoverresistancetwo"] = "A相过度电阻2";
            OLTERowOLTCSwitchingCharacter["Bovertime"] = "B相过度时间";
            OLTERowOLTCSwitchingCharacter["Boverresistanceone"] = "B相过度电阻1";
            OLTERowOLTCSwitchingCharacter["Boverresistancetwo"] = "B相过度电阻2";
            OLTERowOLTCSwitchingCharacter["Covertime"] = "C相过度时间";
            OLTERowOLTCSwitchingCharacter["Coverresistanceone"] = "C相过度电阻1";
            OLTERowOLTCSwitchingCharacter["Coverresistancetwo"] = "C相过度电阻2";
            WorkingSets.local.Tapchangertest.Rows.Add(OLTERowOLTCSwitchingCharacter);
            WorkingSets.local.updatatap();


        }
        public static void UpdataDatabase(string ResultName)
        {
            WorkingSets.local.DeleteAllExportTable(ResultName);//删除原表数据
            CreateTableHead(ResultName);//创建新表头，表头哈希代码不同需要新的表头检索
            CreateParameterInformation(ResultName);
            CreateSampleInformation(ResultName);
            CreateTestResult(ResultName);
        }

    }

    public class OverResult
    {
        public float AOverTime { get; set; }
        public float AOverResistanceOne { get; set; }
        public float AOverResistanceTwo { get; set; }
        public float BOverTime { get; set; }
        public float BOverResistanceOne { get; set; }
        public float BOverResistanceTwo { get; set; }
        public float COverTime { get; set; }
        public float COverResistanceOne { get; set; }
        public float COverResistanceTwo { get; set; }
        public int[] OLTcNum { get; set; }
        public string WaveName { get; set; }
    }
}
